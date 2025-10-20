using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Application.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.API.Controllers
{
    [ApiController]
    public class ChatUsersController(
        IChatUserRepository chatUserRepository,
        ChatUserMapper chatUserMapper
        ) : ControllerBase
    {
        [Authorize]
        [HttpGet("chat-user/get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var chatUsers = chatUserRepository.GetAll();
                return Ok(chatUsers);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("chat-user/get-by-id")]
        public async Task<IActionResult> GetById([FromQuery]int id)
        {
            try
            {
                var chatUser = await chatUserRepository.GetByIdAsync(id);
                if (chatUser is null)
                    return NotFound("There is no chatUser with this id.");
                return Ok(chatUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("chat-user/add")]
        public async Task<IActionResult> Add([FromBody]AddChatUserDto dto)
        {
            try
            {
                var chatUser = chatUserMapper.AddChatUserDtoToChatUser(dto);
                if (chatUser is null)
                    return BadRequest("Please fill up the info.");

                await chatUserRepository.AddAsync(chatUser);
                await chatUserRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("chat-user/update")]
        public async Task<IActionResult> Update([FromBody]UpdateChatUserDto dto)
        {
            try
            {
                var chatUser = await chatUserRepository.GetByIdAsync(dto.Id);
                if (chatUser is null)
                    return NotFound("There is no chatUser with this id.");

                chatUser.ChatId = dto.ChatId;
                chatUser.UserId = dto.UserId;
                chatUserRepository.Update(chatUser);
                await chatUserRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("chat-user/remove")]
        public async Task<IActionResult> Remove([FromQuery]int id)
        {
            try
            {
                var chatUser = await chatUserRepository.GetByIdAsync(id);
                if(chatUser is null)
                    return NotFound("There is no chatUser with this id.");

                chatUserRepository.Delete(chatUser);
                await chatUserRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
