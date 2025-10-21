using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Application.Mapper;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Application.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly MessageMapper _messageMapper;

        public MessageHub(IMessageRepository messageRepository, MessageMapper messageMapper)
        {
            _messageRepository = messageRepository;
            _messageMapper = messageMapper;
        }

        public async Task SendMessage(AddMessageDto messageDto)
        {
            var message = _messageMapper.AddMessageDtoToMessage(messageDto);
            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();

            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", message);
        }

        public async Task AddToGroup(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task RemoveFromGroup(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }
    }
}
