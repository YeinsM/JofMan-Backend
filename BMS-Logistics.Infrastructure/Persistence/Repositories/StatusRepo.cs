using BMS_Logistics.Application.Exceptions;
using BMS_Logistics.Application.Interfaces;
using BMS_Logistics.Domain.Entities;
using BMS_Logistics.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BMS_Logistics.Infrastructure.Persistence.Repositories
{
    public class StatusRepo : IGeneric<Status>
    {
        private readonly DataContext _context;
        private readonly ICurrentUserContext _currentUser;

        public StatusRepo(DataContext context, ICurrentUserContext currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<Status>> GetAll()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<Status> GetById(int id)
        {
            var result = await _context.Statuses.FindAsync(id);

            return result == null ? throw new NotFoundException("Estado") : result;
        }

        public async Task<bool> Add(Status entity)
        {
            int userId = _currentUser.UserId;

            var creatorUserName = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(creatorUserName))
                throw new NotFoundException("Usuario");

            // Validation to check if a status with the same table and description already exists
            bool statusExists = await _context.Statuses
                .AnyAsync(u => u.Table == entity.Table && u.Description == entity.Description);

            if (statusExists)
                throw new BadRequestException("El estado ya existe.");

            var status = new Status(entity.Code!, entity.Table!, entity.Description!)
            {
                CreatedBy = creatorUserName,
                CreatedAt = DateTime.UtcNow
            };

            _context.Statuses.Add(status);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Status entity)
        {
            int userId = _currentUser.UserId;

            var creatorUserName = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(creatorUserName))
                throw new NotFoundException("Usuario");

            var existingStatus = await _context.Statuses.FindAsync(entity.Id);

            if (existingStatus == null)
                return false;

            existingStatus.Update(entity.Code!, entity.Table!, entity.Description!);
            existingStatus.UpdatedBy = _currentUser.UserName;
            existingStatus.UpdatedAt = DateTime.UtcNow;

            _context.Statuses.Update(existingStatus);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var existingStatus = await _context.Statuses.FindAsync(id);

            if (existingStatus == null)
                return false;

            //existingStatus.Status = 0;
            existingStatus.UpdatedAt = DateTime.Now;
            //existingUser.del = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        // Method implemented but not used
        public async Task<IEnumerable<Status>> GetAll(int id)
        {
            return await _context.Statuses.ToListAsync();
        }
    }
}
