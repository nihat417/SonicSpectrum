using SonicSpectrum.Application.Models;

namespace SonicSpectrum.Application.Repository.Abstract
{
    public interface IAccountService
    {
        Task<OperationResult> ChangeNickname(string email, string newNickname);
        Task<OperationResult> DeleteAccount(string userId);
    }
}
