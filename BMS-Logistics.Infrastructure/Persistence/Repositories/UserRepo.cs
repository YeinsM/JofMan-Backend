using BMS_Logistics.Application.Common.Authentication;
using BMS_Logistics.Application.DTOs;
using BMS_Logistics.Application.Exceptions;
using BMS_Logistics.Application.Interfaces;
using BMS_Logistics.Application.Responses;
using BMS_Logistics.Domain.Entities;
using BMS_Logistics.Domain.Enums;
using BMS_Logistics.Infrastructure.Helpers;
using BMS_Logistics.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BMS_Logistics.Infrastructure.Persistence.Repositories
{
    public class UserRepo : IGeneric<User>, IUser
    {
        private readonly DataContext _context;
        private readonly ICurrentUserContext _currentUser;
        private readonly GenericJwtConfig _token;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserRepo(DataContext context, ICurrentUserContext currentUser, GenericJwtConfig token, IConfiguration configuration)
        {
            _context = context;
            _currentUser = currentUser;
            _token = token;
            _configuration = configuration;
        }

        // Login Method
        public async Task<LoginResponse> Login(string userName, string password)
        {
            password = PasswordHelper.Encrypt(password!);

            int lockedStatusId = await GetLockedStatus();

            // Search for the user by userName and password
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName!.Trim() == userName);

            if (existingUser == null)
                return new LoginResponse { Success = false, Message = "NotFound" };

            if (existingUser.StatusId == lockedStatusId || existingUser.Attempts >= 3)
            {
                existingUser.StatusId = lockedStatusId;
                await _context.SaveChangesAsync();
                return new LoginResponse { Success = false, Message = "Locked" };
            }

            if (existingUser.Password != password)
            {
                existingUser.Attempts += 1;

                if (existingUser.Attempts >= 3)
                    existingUser.StatusId = lockedStatusId;

                await _context.SaveChangesAsync();
                return new LoginResponse { Success = false, Message = $"Fail,{existingUser.Attempts}" };
            }

            existingUser.Attempts = 0; // Reset attempts on successful login
            var user = new UserDto
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                SurName = existingUser.SurName,
                UserName = existingUser.UserName,
                Email = existingUser.Email,
                StatusId = existingUser.StatusId,
                RoleId = existingUser.RoleId
            };

            string token = _token.GetToken(existingUser, _configuration);
            string refreshToken = _token.GenerateRefreshToken();

            await SaveRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            await _context.SaveChangesAsync();
            return new LoginResponse { Success = true, Message = "Correct", Token = token, RefreshToken = refreshToken };
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                 .Include(u => u.Status)
                 .AsNoTracking()
                  .OrderByDescending(u => u.CreatedAt)
                 .ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users
                .Include(u => u.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.Id == id);
        }

        public async Task<User?> GetByUserName(string userName)
        {
            return await _context.Users
                .Include(u => u.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.UserName == userName);
        }

        public async Task<string?> GetByUserRole(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.RoleId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Add(User entity)
        {
            //string currentUser = _currentUser.UserName!;

            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == entity.Email);

            if (emailExists)
                throw new BadRequestException("El correo electrónico ya está registrado.");

            // Initialize the required properties in the received entity
            entity.Password = PasswordHelper.Encrypt("Bacc12345");
            entity.ExpirationDate = DateTime.UtcNow.AddDays(45);
            entity.StatusId = await StatusHelper.GetStatusId(Tables.USERS, Statuses.ACTIVE, _context);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = "jsusana"; //currentUser;

            await _context.Users.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(User entity)
        {
            _context.Users.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var existing = await _context.Users.FindAsync(id);

            if (existing == null)
                throw new NotFoundException("Usuario");

            existing.StatusId = await StatusHelper.GetStatusId(Tables.USERS, Statuses.DELETED, _context);
            return await _context.SaveChangesAsync() > 0;
        }

        // Method implemented but not used
        public async Task<IEnumerable<User>> GetAll(int id)
        {
            return await _context.Users.ToListAsync();
        }

        private async Task<int> GetLockedStatus()
        {
            return await StatusHelper.GetStatusId(Tables.USERS, Statuses.BLOCKED, _context);
        }

        public async Task<LoginResponse> ForgetPassword(string userName)
        {
            var user = await _context.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();

            if (user == null)
                return new LoginResponse { Success = false, Message = "NotFound" };

            if (user.BlockLogin)
                return new LoginResponse { Success = false, Message = "Blocked" };

            string code = CodeGeneratorHelper.GenerateCode();
            string newPassword = PasswordHelper.Encrypt(code!).Trim();

            user.Password = newPassword;
            user.ExpirationDate = DateTime.Now.AddDays(45);

            try
            {
                await _context.SaveChangesAsync();

                // Construir el cuerpo del correo
                string body = _emailService.CreateForgetPasswordEmail(
                    new UserDto
                    {
                        Name = user.Name,
                        SurName = user.SurName,
                        Email = user.Email
                    },
                    code
                );

                // Enviar correo
                _emailService.Send(
                    user.Email!,
                    null,
                    "Recuperación de contraseña",
                    body
                );
            }

            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return new LoginResponse { Success = true, Message = "Correct" };
        }

        public async Task<LoginResponse> ChangePassword(ChangePasswordDto userData)
        {
            int userId = _currentUser.UserId;

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return new LoginResponse { Success = false, Message = "NotFound" };

            string newPassword = PasswordHelper.Encrypt(userData.Password!).Trim();
            user.Password = newPassword;
            user.ExpirationDate = DateTime.Now.AddDays(45);

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return new LoginResponse { Success = true, Message = "Correct" };
        }

        public async Task<RefreshToken> SaveRefreshToken(int userId, string refreshToken, DateTime expiryDate)
        {
            var tokenEntity = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                ExpiryDate = expiryDate,
                Used = false
            };

            _context.RefreshTokens.Add(tokenEntity);

            return tokenEntity;
        }

        public async Task<User?> ValidateRefreshToken(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.Used && rt.ExpiryDate > DateTime.UtcNow);

            if (tokenEntity == null) return null;

            // Mark token as used
            tokenEntity.Used = true;
            _context.RefreshTokens.Update(tokenEntity);
            await _context.SaveChangesAsync();

            return await _context.Users.FindAsync(tokenEntity.UserId);
        }

        public async Task<LoginResponse> GenerateTokenForUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // 1. Generar JWT
            var jwtToken = _token.GetToken(user, _configuration);

            // 2. Generar refresh token
            var refreshToken = _token.GenerateRefreshToken();

            // 3. Guardar refresh token en la base de datos
            var expiryDate = DateTime.UtcNow.AddDays(2);
            await SaveRefreshToken(user.Id, refreshToken, expiryDate);

            // 🔹 4. Retornar resultado
            return new LoginResponse
            {
                Success = true,
                Token = jwtToken,
                RefreshToken = refreshToken,
                Message = "Token generado correctamente"
            };
        }
    }
}
