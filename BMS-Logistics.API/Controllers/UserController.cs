using BMS_Logistics.Application.DTOs;
using BMS_Logistics.Application.Interfaces;
using BMS_Logistics.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS_Logistics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IGeneric<User> _genericRepository;
        private readonly IUser _userRepository;


        /// <summary>
        /// Constructor del controlador de usuarios
        /// </summary>
        /// <param name="genericRep">Repositorio genérico de usuarios</param>
        /// <param name="userRep">Repositorio nativo de usuarios</param>
        public UserController(IGeneric<User> genericRep, IUser userRep)
        {
            _genericRepository = genericRep;
            _userRepository = userRep;
        }

        /// <summary>
        /// Inicia sesión con sus credenciales para poder acceder al sistema
        /// </summary>
        /// <param name="userCredentials">
        /// <b>Cuerpo de Solicitud:</b> JSON con las credenciales del usuario.<br/>
        /// <b>Campos Requeridos:</b> Username y Password. <br/>
        /// <b>Ejemplo:</b><br/>
        /// <code>
        /// {
        ///    "username": "aperez",
        ///    "password": "***************"
        /// }
        /// </code>
        /// </param>
        /// <returns></returns>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login(UserCredentialsDto userCredentials)
        {
            var loginResult = await _userRepository.Login(userCredentials.Username!, userCredentials.Password!);

            if (loginResult.Message == "NotFound")
                return BadRequest(new { message = "El usuario es incorrecto para la compañia seleccionada, revise e intente nuevamente." });

            if (loginResult.Message == "Locked")
                return BadRequest(new { message = "El usuario está bloqueado, comuníquese con seguridad de la información." });

            if (loginResult.Message!.Contains("Fail"))
            {
                var parts = loginResult.Message.Split(',');
                if (int.TryParse(parts[^1], out int trysToLogin))
                {
                    if (trysToLogin < 3)
                        return BadRequest(new { message = $"La contraseña es incorrecta. Intento {trysToLogin} de 3 intentos" });
                    else
                        return BadRequest(new { message = "El usuario ha sido bloqueado por exceder el número de intentos permitidos." });
                }
            }

            return Ok(loginResult);
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        /// <response code="200">Retorna la lista de usuarios correctamente</response>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _genericRepository.GetAll();

            return Ok(users);
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="user">Datos del usuario a crear</param>
        /// <returns>Usuario creado</returns>
        /// <response code="201">Usuario creado correctamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        public async Task<ActionResult<UserDto>> Post(User user)
        {
            var create = new UserDto
            {
                Name = user.Name,
                SurName = user.SurName,
                UserName = user.UserName,
                Email = user.Email,
                RoleId = user.RoleId               
            };

            var users = await _genericRepository.Add(user);

            return Ok(users);
        }
    }
}
