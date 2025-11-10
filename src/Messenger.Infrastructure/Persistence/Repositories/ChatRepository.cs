using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class ChatRepository(MessengerDbContext context) : Repository<Chat>(context), IChatRepository
    {
        public async Task<IList<ChatDto>> GetAllChatsWithUserRole(int userId)
        {
            var chatsDto = await context.Chats
                .Include(c => c.ChatUsers.Where(cu => cu.UserId == userId && cu.IsDeleted == false))
                .Where(c => c.IsDeleted == false)
                .Select(c => new ChatDto
                {
                    Name = c.Name,
                    CreatedAt = c.CreatedAt,
                    ChatImageUrl = c.ChatImageUrl,
                    ChatUsers = c.ChatUsers.ToList(),
                    CurrentUserRole = c.ChatUsers.First().Role.ToString(),
                    Type = c.Type
                })
                .ToListAsync();

            return chatsDto;
        }

        public async Task<ChatDto?> GetChatByIdWithUserRole(int chatId, int userId)
        {
            var chat = await context.Chats
                .Include(c => c.ChatUsers)
                .FirstOrDefaultAsync(c => c.Id == chatId && !c.IsDeleted);

            if (chat == null)
                return null;

            var currentUser = chat.ChatUsers
                .FirstOrDefault(cu => cu.UserId == userId && !cu.IsDeleted);

            var chatDto = new ChatDto
            {
                Name = chat.Name,
                CreatedAt = chat.CreatedAt,
                ChatImageUrl = chat.ChatImageUrl,
                ChatUsers = chat.ChatUsers.Where(cu => cu.UserId == userId && !cu.IsDeleted).ToList(),
                CurrentUserRole = chat.ChatUsers.First().Role.ToString(),
                Type = chat.Type
            };

            return chatDto;
        }

        public async Task<GlobalSearchResponseDto> GlobalSearchAsync(string searchText, int userId)
        {
            var response = new GlobalSearchResponseDto();

            response.Users = await context.Users
                .Where(u => u.IsDeleted == false && (u.Username.Contains(searchText) || u.Email.Contains(searchText)))
                .ToListAsync();

            response.Chats = await context.Chats
                .Where(c => ((int)c.Type == (int)ChatType.PublicGroup || (int)c.Type == (int)ChatType.PublicChannel) && c.Name == searchText && c.IsDeleted == false)
                .ToListAsync();

            response.UserExistingChats = await context.Chats.Include(c => c.ChatUsers.Where(cu => cu.UserId == userId && cu.IsDeleted == false))
                .Where(c => c.Name == searchText && c.IsDeleted == false)
                .ToListAsync();

            return response;
        }

        public async Task<Chat?> OneOnOne(int firstUserId, int secondUserId)
        {
            var chat = await context.Chats?.Include(c => c.ChatUsers.Where(cu => (cu.UserId == firstUserId || cu.UserId == secondUserId) && cu.IsDeleted == false))
                .FirstOrDefaultAsync(c => ((int)c.Type == (int)ChatType.Private) && c.IsDeleted == false);

            if (chat is null)
            {
                var newChat = new Chat
                {
                    Name = "New private chat",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                };

                await context.Chats.AddAsync(newChat);
                await context.SaveChangesAsync();

                var firstChatUser = new ChatUser
                {
                    ChatId = newChat.Id,
                    UserId = firstUserId,
                    IsDeleted = false
                };

                var secondChatUser = new ChatUser
                {
                    ChatId = newChat.Id,
                    UserId = secondUserId,
                    IsDeleted = false
                };

                await context.ChatUsers.AddAsync(firstChatUser);
                await context.ChatUsers.AddAsync(secondChatUser);
                await context.SaveChangesAsync();

                return newChat;
            }

            return chat;
        }
    }
}
