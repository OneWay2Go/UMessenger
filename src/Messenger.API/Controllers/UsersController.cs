using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Application.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.API.Controllers
{
    [ApiController]
    public class UsersController(
        IUserRepository userRepository,
        UserMapper userMapper,
        IAuthService authService,
        IRefreshTokenRepository refreshTokenRepository,
        RefreshTokenMapper refreshTokenMapper
        ) : ControllerBase
    {
        [HttpPost("user/register")]
        public async Task<IActionResult> Register([FromBody]AddUserDTO dto)
        {
            try
            {
                var user = userMapper.AddUserDtoToUser(dto);
                if (user == null)
                    return BadRequest("Please fill up the information.");

                await userRepository.AddAsync(user);
                await userRepository.SaveChangesAsync();

                return Ok("Registered successfully.");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("user/log-in")]
        public async Task<IActionResult> LogIn([FromBody]AddUserDTO dto)
        {
            try
            {
                var user = await userRepository.GetByEmailAsync(dto.Email);
                if (user == null)
                    return NotFound("Incorrect email or password.");

                if (user.Password != dto.Password)
                    return BadRequest("Incorrect email or password.");

                string jwt = authService.CreateJwt(user);

                var refreshTokenDto = new AddRefreshTokenDto
                {
                    Token = authService.GenerateRefreshToken(),
                    ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
                    UserId = user.Id,
                };
                var refreshToken = refreshTokenMapper.DtoToEntity(refreshTokenDto);
                
                await refreshTokenRepository.AddAsync(refreshToken);
                await refreshTokenRepository.SaveChangesAsync();

                return Ok(new { accessToken = jwt, refreshToken = refreshToken.Token});
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("user/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody]string refreshToken)
        {
            try
            {
                var RefreshToken = await refreshTokenRepository.GetByRefreshToken(refreshToken);

                var accessToken = authService.CreateJwt(RefreshToken.User);

                RefreshToken.Token = authService.GenerateRefreshToken();
                RefreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

                refreshTokenRepository.Update(RefreshToken);
                await refreshTokenRepository.SaveChangesAsync();

                return Ok(new { accessToken = accessToken, refreshToken = RefreshToken.Token });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("user/revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromQuery]int userId)
        {
            try
            {
                var deleted = await refreshTokenRepository.DeleteRefreshTokensByUserId(userId);
                if (deleted)
                    return NoContent();
                return BadRequest("You are definitely doing something wrong, because how you get that bad request bro...");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("user/email-verification")]
        public async Task<IActionResult> EmailVerification()
        {
            throw new NotImplementedException();
        }

        [HttpGet("user/get-all")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = userRepository.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/get-by-id/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute]int id)
        {
            try
            {
                var user = await userRepository.GetByIdAsync(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("user/remove/{id}")]
        public async Task<IActionResult> RemoveUser([FromRoute]int id)
        {
            try
            {
                var user = await userRepository.GetByIdAsync(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                userRepository.Delete(user);
                await userRepository.SaveChangesAsync();

                return Ok("User deleted successfully.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("user/add")]
        public async Task<IActionResult> AddUser([FromBody]AddUserDTO dto)
        {
            try
            {
                var user = userMapper.AddUserDtoToUser(dto);

                await userRepository.AddAsync(user);
                await userRepository.SaveChangesAsync();

                return Ok("User added successfully.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("user/update/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute]int id, [FromBody]UpdateUserDto dto)
        {
            try
            {
                var user = await userRepository.GetByIdAsync(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                userMapper.UpdateUserDtoToUser(dto, ref user);

                userRepository.Update(user);
                await userRepository.SaveChangesAsync();

                return Ok("User updated successfully.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
