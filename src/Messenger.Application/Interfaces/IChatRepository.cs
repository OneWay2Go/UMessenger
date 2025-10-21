using Messenger.Application.DTOs;
using Messenger.Domain.Entities;

namespace Messenger.Application.Interfaces
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<GlobalSearchResponseDto> GlobalSearchAsync(string searchText, int userId);

        Task<Chat?> OneOnOne(int firstUserId, int secondUserId);
    }
}
