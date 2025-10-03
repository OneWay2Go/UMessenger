using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Messenger.Application.Mapper
{
    [Mapper]
    public partial class UserMapper(IUserRepository userRepository)
    {
        public partial User AddUserDtoToUser(AddUserDTO dto);

        public partial AddUserDTO UserToAddUserDto(User user);

        public void UpdateUserDtoToUser(UpdateUserDto dto, ref User user)
        {
            if (dto == null)
                return;

            if (dto.Username != null || dto.Username != "")
            {
                user.Username = dto.Username;
            }

            if (dto.DisplayName != null || dto.DisplayName != "")
            {
                user.DisplayName = dto.DisplayName;
            }

            if (dto.Bio != null || dto.Bio != "")
            {
                user.Bio = dto.Bio;
            }

            if (dto.ProfileImageUrl != null || dto.ProfileImageUrl != "")
            {
                user.ProfileImageUrl = dto.ProfileImageUrl;
            }
        }
    }
}
