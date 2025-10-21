using Messenger.Application.DTOs;
using Messenger.Application.Enums;
using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Persistence.Repositories
{
    public class ChatRepository(MessengerDbContext context) : Repository<Chat>(context), IChatRepository
    {
        public async Task<GlobalSearchResponseDto> GlobalSearchAsync(string searchText, int userId) // searchText can be username, email, public groups and channels name, user's existing chats
        {
            var response = new GlobalSearchResponseDto();

            response.Users = await context.Users.Where(u => u.Username == searchText || u.Email == searchText).ToListAsync();

            response.Chats = await context.Chats
                .Where(c => ((int)c.Type == (int)ChatType.PublicGroup || (int)c.Type == (int)ChatType.PublicChannel) && c.Name == searchText)
                .ToListAsync();

            response.UserExistingChats = await context.Chats.Include(c => c.ChatUsers.Where(cu => cu.UserId == userId))
                .Where(c => c.Name == searchText)
                .ToListAsync();

            return response;
        }

        public async Task<Chat?> OneOnOne(int firstUserId, int secondUserId)
        {
            return await context.Chats?.Include(c => c.ChatUsers.Where(cu => cu.UserId == firstUserId || cu.UserId == secondUserId))
                .FirstOrDefaultAsync(c => (int)c.Type == (int)ChatType.Private);
        }
    }
}
