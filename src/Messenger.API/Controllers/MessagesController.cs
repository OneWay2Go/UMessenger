using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Application.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Messenger.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.API.Controllers
{
    [ApiController]
    public class MessagesController(
        IMessageRepository messageRepository,
        MessageMapper messageMapper,
        IHubContext<MessageHub> hubContext
        ) : ControllerBase
    {
        [Authorize]
        [HttpGet("message/get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var messages = messageRepository.GetAll()
                    .Where(m => m.IsDeleted != true);

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("message/get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var message = await messageRepository.GetByIdAsync(id);
                if (message is null)
                    return NotFound("There is no message with this id.");

                if (message.IsDeleted == true)
                    return NotFound("There is no message with this id.");

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("message/get-by-chat/{chatId}")]
        public async Task<IActionResult> GetByChatId([FromRoute] int chatId)
        {
            try
            {
                var messages = await messageRepository.GetByChatIdAsync(chatId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpPut("message/update-user-check")]
        public async Task<IActionResult> UpdateWithUserCheck([FromBody]UpdateMessageDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var message = await messageRepository.GetByIdAsync(dto.Id);
                if (message is null)
                    return NotFound("There is no message with this id.");

                await messageRepository.UpdateWithUserCheckAsync(dto, userId);
                
                await hubContext.Clients.Group(message.ChatId.ToString()).SendAsync("OnMessageEdited", message.Id, dto.Content);

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("message/remove-user-check")]
        public async Task<IActionResult> RemoveWithUserCheck([FromQuery]int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var message = await messageRepository.GetByIdAsync(id);
                if (message is null)
                    return NotFound("There is no message with this id.");

                await messageRepository.DeleteWithUserCheckAsync(id, userId);

                await hubContext.Clients.Group(message.ChatId.ToString()).SendAsync("OnMessageDeleted", message.ChatId, message.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("message/remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            try
            {
                var message = await messageRepository.GetByIdAsync(id);
                if (message is null)
                    return NotFound("There is no chat with this id.");

                message.IsDeleted = true;

                messageRepository.Update(message);
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
