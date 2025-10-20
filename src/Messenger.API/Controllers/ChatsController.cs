using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Application.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.API.Controllers
{
    [ApiController]
    public class ChatsController(
        IChatRepository chatRepository,
        ChatMapper chatMapper
        ) : ControllerBase
    {
        [Authorize]
        [HttpGet("chat/get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var chats = chatRepository.GetAll();
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("chat/get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var chat = await chatRepository.GetByIdAsync(id);
                if (chat is null)
                    return NotFound("There is no chat with this id.");
                return Ok(chat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("chat/add")]
        public async Task<IActionResult> Add([FromBody]AddChatDto dto)
        {
            try
            {
                var chat = chatMapper.AddChatDtoToChat(dto);
                if (chat is null)
                    return BadRequest("Please fill up the info.");

                await chatRepository.AddAsync(chat);
                await chatRepository.SaveChangesAsync();

                return Ok(chat.Id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("chat/update")]
        public async Task<IActionResult> Update([FromBody]UpdateChatDto dto)
        {
            try
            {
                var chat = await chatRepository.GetByIdAsync(dto.Id);
                if(chat is null)
                    return NotFound("There is no chat with this id.");
                
                chat.Name = dto.Name;
                chatRepository.Update(chat);
                await chatRepository.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("chat/remove")]
        public async Task<IActionResult> Remove([FromQuery]int id)
        {
            try
            {
                var chat = await chatRepository.GetByIdAsync(id);
                if (chat is null)
                    return NotFound("There is no chat with this id.");

                chatRepository.Delete(chat);
                await chatRepository.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
