using TaskManagmentSystem.API.Entities;

namespace TaskManagmentSystem.API.Interfaces.Repositories
{
    public interface IOtpVerificationRepository
    {
        #region Core CRUD Operations

        Task<int> AddAsync(OtpVerification otp);
        Task<OtpVerification?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(OtpVerification otp);
        Task<bool> DeleteAsync(int id);

        #endregion

        #region OTP Specific Operations

        Task<OtpVerification?> GetLatestValidOtpAsync(string email);
        Task<bool> MarkAsUsedAsync(int otpId);
        Task<bool> MarkAsUsedByEmailAndCodeAsync(string email, string otpCode);
        Task<bool> AnyActiveOtpAsync(string email);
        Task DeleteExpiredOtpsAsync();
        Task DeleteAllForEmailAsync(string email);

        #endregion
    }
}