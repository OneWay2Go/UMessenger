using Messenger.Application.DTOs;
using Messenger.Application.Interfaces;
using Messenger.Application.Mapper;
using Messenger.Application.Services;
using Messenger.Infrastructure.Persistence;
using Messenger.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Messenger.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MessengerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Repositories and services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatUserRepository, ChatUserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<UserMapper>();
            services.AddScoped<RefreshTokenMapper>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ChatMapper>();
            services.AddScoped<ChatUserMapper>();
            services.AddScoped<MessageMapper>();

            // Add SignalR
            services.AddSignalR();

            // Json configuration
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            return services;
        }
    }
}
