using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Application.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.API.Controllers
{
    [ApiController]
    public class MessagesController(
        IMessageRepository messageRepository,
        MessageMapper messageMapper
        ) : ControllerBase
    {
        [HttpGet("message/get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var chats = messageRepository.GetAll();
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("message/get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var message = await messageRepository.GetByIdAsync(id);
                if (message is null)
                    return NotFound("There is no chat with this id.");
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("message/add")]
        public async Task<IActionResult> Add([FromBody]AddMessageDto dto)
        {
            try
            {
                var message = messageMapper.AddMessageDtoToMessage(dto);
                if (message is null)
                    return BadRequest("Please fill up the info.");

                await messageRepository.AddAsync(message);
                await messageRepository.SaveChangesAsync();

                return Ok(message.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("message/update")]
        public async Task<IActionResult> Update([FromBody]UpdateMessageDto dto)
        {
            try
            {
                var message = await messageRepository.GetByIdAsync(dto.Id);
                if (message is null)
                    return NotFound("There is no chat with this id.");

                message.Content = dto.Content;
                messageRepository.Update(message);
                await messageRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("message/remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            try
            {
                var message = await messageRepository.GetByIdAsync(id);
                if (message is null)
                    return NotFound("There is no chat with this id.");

                messageRepository.Delete(message);
                await messageRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
