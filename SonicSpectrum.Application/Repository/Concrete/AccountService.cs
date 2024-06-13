using Microsoft.AspNetCore.Identity;
using SonicSpectrum.Application.Models;
using SonicSpectrum.Application.Repository.Abstract;
using SonicSpectrum.Application.Services;
using SonicSpectrum.Domain.Entities;
using SonicSpectrum.Persistence.Data;

namespace SonicSpectrum.Application.Repository.Concrete
{
    public class AccountService(AppDbContext _context, UserManager<User> _userManager) : IAccountService
    {
        public async Task<OperationResult> ChangeNickname(string email, string newNickname)
        {
            var result = new OperationResult();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "User not found.";
                    return result;
                }

                user.UserName = newNickname;
                var identityResult = await _userManager.UpdateAsync(user);

                if (!identityResult.Succeeded)
                {
                    result.Success = false;
                    result.ErrorMessage = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                    return result;
                }

                await transaction.CommitAsync();

                result.Success = true;
                result.Message = "Nickname updated successfully";
                return result;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
                return result;
            }
        }

        public async Task<OperationResult> DeleteAccount(string userId)
        {
            var result = new OperationResult();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "User not found.";
                    return result;
                }

                if (!string.IsNullOrEmpty(user.ImageUrl)) await UploadFileHelper.DeleteFile(userId, "userphoto");
                

                var identityResult = await _userManager.DeleteAsync(user);

                if (!identityResult.Succeeded)
                {
                    result.Success = false;
                    result.ErrorMessage = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                    return result;
                }

                await transaction.CommitAsync();

                result.Success = true;
                result.Message = "Account deleted successfully.";
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
                return result;
            }
        }
    }
}
