using Microsoft.AspNetCore.Identity;
using SonicSpectrum.Domain.Entities;

namespace SonicSpectrum.Application.Repository.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthService AuthService { get; }
        IEmailService EmailService { get; }
        IMusicSettingService MusicSettingService { get; }
        UserManager<User> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        Task<int> SaveChangesAsync();
    }
}
