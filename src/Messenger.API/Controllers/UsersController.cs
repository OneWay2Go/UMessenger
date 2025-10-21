﻿using Messenger.Application.DTOs;
﻿using Messenger.Application.Interfaces;
﻿using Messenger.Application.Mapper;
﻿using Microsoft.AspNetCore.Authorization;
﻿using Microsoft.AspNetCore.Mvc;
﻿
﻿namespace Messenger.API.Controllers
﻿{
﻿    [ApiController]
﻿    public class UsersController(
﻿        IUserRepository userRepository,
﻿        UserMapper userMapper,
﻿        IAuthService authService,
﻿        IRefreshTokenRepository refreshTokenRepository,
﻿        RefreshTokenMapper refreshTokenMapper,
﻿        IEmailService emailService
﻿        ) : ControllerBase
﻿    {
﻿        /// <summary>
﻿        /// Registers a new user.
﻿        /// </summary>
﻿        /// <param name="dto">The user registration data.</param>
﻿        /// <returns>A confirmation message.</returns>
﻿        [HttpPost("user/register")]
﻿        public async Task<IActionResult> Register([FromBody]AddUserDTO dto)
﻿        {
﻿            try
﻿            {
﻿                var user = userMapper.AddUserDtoToUser(dto);
﻿                if (user == null)
﻿                    return BadRequest("Please fill up the information.");
﻿
﻿                await userRepository.AddAsync(user);
﻿                await userRepository.SaveChangesAsync();
﻿
﻿                return Ok("Registered successfully.");
﻿            }
﻿            catch (Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿
﻿        /// <summary>
﻿        /// Logs in a user and returns JWT and refresh tokens.
﻿        /// </summary>
﻿        /// <param name="dto">The user login data.</param>
﻿        /// <returns>An object containing the access token and refresh token.</returns>
﻿        [HttpPost("user/log-in")]
﻿        public async Task<IActionResult> LogIn([FromBody]AddUserDTO dto)
﻿        {
﻿            try
﻿            {
﻿                var user = await userRepository.GetByEmailAsync(dto.Email);
﻿                if (user == null)
﻿                    return NotFound("Incorrect email or password.");
﻿
﻿                if (user.Password != dto.Password)
﻿                    return BadRequest("Incorrect email or password.");
﻿
﻿                string jwt = authService.CreateJwt(user);
﻿
﻿                var refreshTokenDto = new AddRefreshTokenDto
﻿                {
﻿                    Token = authService.GenerateRefreshToken(),
﻿                    ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
﻿                    UserId = user.Id,
﻿                };
﻿                var refreshToken = refreshTokenMapper.DtoToEntity(refreshTokenDto);
﻿
﻿                await refreshTokenRepository.AddAsync(refreshToken);
﻿                await refreshTokenRepository.SaveChangesAsync();
﻿
﻿                return Ok(new { user, accessToken = jwt, refreshToken = refreshToken.Token });
﻿            }
﻿            catch(Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿
﻿        /// <summary>
﻿        /// Refreshes an access token using a refresh token.
﻿        /// </summary>
﻿        /// <param name="refreshToken">The refresh token.</param>
﻿        /// <returns>A new access token and refresh token.</returns>
﻿        [HttpPost("user/refresh-token")]
﻿        public async Task<IActionResult> RefreshToken([FromHeader]string refreshToken)
﻿        {
﻿            try
﻿            {
﻿                var RefreshToken = await refreshTokenRepository.GetByRefreshToken(refreshToken);
﻿
﻿                var accessToken = authService.CreateJwt(RefreshToken.User);
﻿
﻿                RefreshToken.Token = authService.GenerateRefreshToken();
﻿                RefreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);
﻿
﻿                refreshTokenRepository.Update(RefreshToken);
﻿                await refreshTokenRepository.SaveChangesAsync();
﻿
﻿                return Ok(new { accessToken = accessToken, refreshToken = RefreshToken.Token });
﻿            }
﻿            catch(Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿
﻿        /// <summary>
﻿        /// Revokes all refresh tokens for a user. (Requires authorization)
﻿        /// </summary>
﻿        /// <param name="userId">The ID of the user.</param>
﻿        /// <returns>No content if successful.</returns>
﻿        [Authorize]
﻿        [HttpDelete("user/revoke-refresh-token")]
﻿        public async Task<IActionResult> RevokeRefreshToken([FromQuery]int userId)
﻿        {
﻿            try
﻿            {
﻿                var deleted = await refreshTokenRepository.DeleteRefreshTokensByUserId(userId);
﻿                if (deleted)
﻿                    return NoContent();
﻿                return BadRequest("You are definitely doing something wrong, because how you get that bad request bro...");
﻿            }
﻿            catch(Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿
﻿        /// <summary>
﻿        /// Sends a confirmation code to the user's email.
﻿        /// </summary>
﻿        /// <param name="dto">The email address of the user.</param>
﻿        /// <returns>A confirmation message.</returns>
﻿        [HttpPost("user/email/send-code")]
﻿        public async Task<IActionResult> SendCodeAsync([FromBody]SendCodeDto dto)
﻿        {
﻿            try
﻿            {
﻿                var user = await userRepository.GetByEmailAsync(dto.Email);
﻿                if (user == null)
﻿                    return NotFound("Invalid email, please check it again.");
﻿
﻿                var rand = new Random();
﻿                user.EmailConfirmationCode = rand.Next(1000, 9999);
﻿                userRepository.Update(user);
﻿                await userRepository.SaveChangesAsync();
﻿
﻿                await emailService.SendEmailCodeAsync(user);
﻿
﻿                return Ok("Email code send successfuly.");
﻿            }
﻿            catch(Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿
﻿        /// <summary>
﻿        /// Verifies the email confirmation code.
﻿        /// </summary>
﻿        /// <param name="dto">The user's email and the confirmation code.</param>
﻿        /// <returns>A confirmation message.</returns>
﻿        [HttpPut("user/email/verify-code")]
﻿        public async Task<IActionResult> VerifyCodeAsync([FromBody]VerifyCodeDto dto)
﻿        {
﻿            try
﻿            {
﻿                var user = await userRepository.GetByEmailAsync(dto.Email);
﻿                if (user == null)
﻿                    return NotFound("Invalid email, please check it again.");
﻿
﻿                var isSuccess = await emailService.VerifyEmailCodeAsync(user, dto.Code);
﻿                if (!isSuccess)
﻿                    return BadRequest("Invalid code or code did not sent, please try again.");
﻿
﻿                user.IsEmailConfirmed = true;
﻿                userRepository.Update(user);
﻿                await userRepository.SaveChangesAsync();
﻿
﻿                return Ok("Verified successfully.");
﻿            }
﻿            catch(Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿
﻿        /// <summary>
﻿        /// Gets all users.
﻿        /// </summary>
﻿        /// <returns>A list of all users.</returns>
                [Authorize]
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
        
                [Authorize]
                [HttpGet("user/search")]
                public async Task<IActionResult> SearchUsers([FromQuery] string query)
                {
                    try
                    {
                        var users = await userRepository.SearchUsersAsync(query);
                        return Ok(users);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }﻿
﻿        /// <summary>
﻿        /// Gets a user by their ID.
﻿        /// </summary>
﻿        /// <param name="id">The ID of the user.</param>
﻿        /// <returns>The user object.</returns>
        [Authorize]
﻿        [HttpGet("user/get-by-id/{id}")]
﻿        public async Task<IActionResult> GetUserById([FromRoute]int id)
﻿        {
﻿            try
﻿            {
﻿                var user = await userRepository.GetByIdAsync(id);
﻿                if (user == null)
﻿                    return NotFound($"User with ID {id} not found.");
﻿
﻿                return Ok(user);
﻿            }
﻿            catch (Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿
﻿        /// <summary>
﻿        /// Removes a user by their ID.
﻿        /// </summary>
﻿        /// <param name="id">The ID of the user to remove.</param>
﻿        /// <returns>A confirmation message.</returns>
        [Authorize]
﻿        [HttpDelete("user/remove/{id}")]
﻿        public async Task<IActionResult> RemoveUser([FromRoute]int id)
﻿        {
﻿            try
﻿            {
﻿                var user = await userRepository.GetByIdAsync(id);
﻿                if (user == null)
﻿                    return NotFound($"User with ID {id} not found.");
﻿
﻿                userRepository.Delete(user);
﻿                await userRepository.SaveChangesAsync();
﻿
﻿                return Ok("User deleted successfully.");
﻿            }
﻿            catch(Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿
﻿        /// <summary>
﻿        /// Adds a new user.
﻿        /// </summary>
﻿        /// <param name="dto">The data for the new user.</param>
﻿        /// <returns>A confirmation message.</returns>
        [Authorize]
﻿        [HttpPost("user/add")]
﻿        public async Task<IActionResult> AddUser([FromBody]AddUserDTO dto)
﻿        {
﻿            try
﻿            {
﻿                var user = userMapper.AddUserDtoToUser(dto);
﻿
﻿                await userRepository.AddAsync(user);
﻿                await userRepository.SaveChangesAsync();
﻿
﻿                return Ok("User added successfully.");
﻿            }
﻿            catch(Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿
﻿        /// <summary>
﻿        /// Updates a user's information.
﻿        /// </summary>
﻿        /// <param name="id">The ID of the user to update.</param>
﻿        /// <param name="dto">The new user data.</param>
﻿        /// <returns>A confirmation message.</returns>
        [Authorize]
﻿        [HttpPut("user/update/{id}")]
﻿        public async Task<IActionResult> UpdateUser([FromRoute]int id, [FromBody]UpdateUserDto dto)
﻿        {
﻿            try
﻿            {
﻿                var user = await userRepository.GetByIdAsync(id);
﻿                if (user == null)
﻿                    return NotFound($"User with ID {id} not found.");
﻿
﻿                userMapper.UpdateUserDtoToUser(dto, ref user);
﻿
﻿                userRepository.Update(user);
﻿                await userRepository.SaveChangesAsync();
﻿
﻿                return Ok("User updated successfully.");
﻿            }
﻿            catch(Exception ex)
﻿            {
﻿                return BadRequest(ex.Message);
﻿            }
﻿        }
﻿    }
﻿}
﻿
