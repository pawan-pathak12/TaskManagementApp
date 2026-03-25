using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Repositories;

namespace TaskManagmentSystem.API.Repositories
{
    public class OtpVerificationRepository : IOtpVerificationRepository
    {
        private readonly ApplicationDbContext _context;

        public OtpVerificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Core CRUD Operations

        public async Task<int> AddAsync(OtpVerification otp)
        {
            otp.CreatedAt = DateTime.UtcNow;
            if (otp.ExpiryTime == default)
                otp.ExpiryTime = DateTime.UtcNow.AddMinutes(5);

            await _context.OtpVerifications.AddAsync(otp);
            await _context.SaveChangesAsync();

            return otp.Id;
        }

        public async Task<OtpVerification?> GetByIdAsync(int id)
        {
            return await _context.OtpVerifications
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> UpdateAsync(OtpVerification otp)
        {
            //maybe later need to add include 
            var existing = await _context.OtpVerifications
                .FirstOrDefaultAsync(x => x.Id == otp.Id);
            if (existing == null)
            {
                return false;
            }
            _context.Entry(existing).CurrentValues.SetValues(otp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var otp = await _context.OtpVerifications.FindAsync(id);
            if (otp == null)
            {
                return false;
            }
            otp.IsActive = false;
            _context.OtpVerifications.Remove(otp);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region OTP Specific Operations

        public async Task<OtpVerification?> GetLatestValidOtpAsync(string email)
        {
            return await _context.OtpVerifications
                .Where(o => o.Email == email
                         && !o.IsUsed
                         && o.ExpiryTime > DateTime.UtcNow
                         && o.IsActive)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> MarkAsUsedAsync(int otpId)
        {
            var otp = await _context.OtpVerifications.FindAsync(otpId);
            if (otp == null)
                return false;

            otp.IsUsed = true;
            otp.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsUsedByEmailAndCodeAsync(string email, string otpCode)
        {
            var otp = await _context.OtpVerifications
                .FirstOrDefaultAsync(o => o.Email == email
                                       && o.OtpCode == otpCode
                                       && !o.IsUsed
                                       && o.ExpiryTime > DateTime.UtcNow);

            if (otp == null)
                return false;

            otp.IsUsed = true;
            otp.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AnyActiveOtpAsync(string email)
        {
            return await _context.OtpVerifications
                .AnyAsync(o => o.Email == email
                            && !o.IsUsed
                            && o.ExpiryTime > DateTime.UtcNow
                            && o.IsActive);
        }

        public async Task DeleteExpiredOtpsAsync()
        {
            var expiredOtps = await _context.OtpVerifications
                .Where(o => o.ExpiryTime < DateTime.UtcNow)
                .ToListAsync();

            if (expiredOtps.Any())
            {
                _context.OtpVerifications.RemoveRange(expiredOtps);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllForEmailAsync(string email)
        {
            var otps = await _context.OtpVerifications
                .Where(o => o.Email == email)
                .ToListAsync();

            if (otps.Any())
            {
                _context.OtpVerifications.RemoveRange(otps);
                await _context.SaveChangesAsync();
            }
        }

        #endregion
    }
}