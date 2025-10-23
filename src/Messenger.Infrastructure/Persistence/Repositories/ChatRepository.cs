using Messenger.Application.DTOs;
using Messenger.Application.Enums;
using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class ChatRepository(MessengerDbContext context) : Repository<Chat>(context), IChatRepository
    {
        public async Task<IList<Chat>> GetAllChatsWithUserRole(int userId)
        {
            var chats = await context.Chats
                .Include(c => c.ChatUsers.Where(cu => cu.UserId == userId && cu.IsDeleted == false))
                .Where(c => c.IsDeleted == false)
                .ToListAsync();

            return chats;
        }

        public async Task<Chat?> GetChatByIdWithUserRole(int chatId, int userId)
        {
            var chat = await context.Chats
                .Include(c => c.ChatUsers.Where(cu => (cu.UserId == userId) && cu.IsDeleted == false))
                .FirstOrDefaultAsync(c => (c.Id == chatId) && c.IsDeleted == false);

            return chat;
        }

        public async Task<GlobalSearchResponseDto> GlobalSearchAsync(string searchText, int userId)
        {
            var response = new GlobalSearchResponseDto();

            response.Users = await context.Users
                .Where(u => u.IsDeleted == false && (u.Username == searchText || u.Email == searchText))
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
            return await context.Chats?.Include(c => c.ChatUsers.Where(cu => (cu.UserId == firstUserId || cu.UserId == secondUserId) && cu.IsDeleted == false))
                .FirstOrDefaultAsync(c => ((int)c.Type == (int)ChatType.Private) && c.IsDeleted == false);
        }
    }
}
