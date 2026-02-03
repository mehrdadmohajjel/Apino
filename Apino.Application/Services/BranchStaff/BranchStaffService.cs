using Apino.Application.Common.Helper;
using Apino.Application.Dtos.BranchStaff;
using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.BranchStaff
{
    public class BranchStaffService : IBranchStaffService
    {
        private readonly IReadDbContext _db;

        public BranchStaffService(IReadDbContext db)
        {
            _db = db;
        }

        public async Task<List<BranchStaffListDto>> GetStaffByBranchAsync(long branchId)
        {
            var query = _db.BranchUsers
                .Include(bu => bu.User)
                .ThenInclude(u => u.UserProfile)
                .Where(bu => bu.BranchId == branchId)
                .OrderByDescending(bu => bu.StartWorkDate);

            var list = await query.Select(bu => new BranchStaffListDto
            {
                Id = bu.Id,
                FullName = bu.User.UserProfile.FullName ?? "بدون نام",
                Mobile = bu.User.Mobile,
                Avatar = bu.User.UserProfile.Avatar,
                Role = bu.Role,
                IsActive = bu.IsActive,
                // تبدیل تاریخ میلادی به شمسی (فرض بر وجود کلاس کمکی)
                StartDate = PersianDateHelper.ToShamsi(bu.StartWorkDate)
            }).ToListAsync();

            return list;
        }

        public async Task<List<UserSearchResultDto>> SearchUsersAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 3)
                return new List<UserSearchResultDto>();

            // جستجو بر اساس موبایل یا نام کامل
            return await _db.Users
                .Include(u => u.UserProfile)
                .Where(u => u.Mobile.Contains(term) || (u.UserProfile != null && u.UserProfile.FullName.Contains(term)))
                .Take(10) // فقط ۱۰ نتیجه اول
                .Select(u => new UserSearchResultDto
                {
                    UserId = u.Id,
                    DisplayName = $"{u.UserProfile.FullName ?? "کاربر"} ({u.Mobile})",
                    Avatar = u.UserProfile.Avatar
                })
                .ToListAsync();
        }

        public async Task AddStaffAsync(long branchId, AddStaffVm vm)
        {
            // بررسی تکراری نبودن
            var exists = await _db.BranchUsers.AnyAsync(x => x.BranchId == branchId && x.UserId == vm.UserId);
            if (exists) throw new Exception("این کاربر قبلاً به این شعبه اضافه شده است.");

            var staff = new BranchUser
            {
                BranchId = branchId,
                UserId = vm.UserId,
                Role = vm.Role,
                StartWorkDate = vm.StartWorkDate, // بهتر است از فرم بیاید یا DateTime.Now
                IsActive = true
            };

            _db.BranchUsers.Add(staff);
            await _db.SaveChangesAsync();
        }

        public async Task ToggleActiveAsync(long branchUserId)
        {
            var staff = await _db.BranchUsers.FindAsync(branchUserId);
            if (staff != null)
            {
                staff.IsActive = !staff.IsActive;
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveStaffAsync(long branchUserId)
        {
            var staff = await _db.BranchUsers.FindAsync(branchUserId);
            if (staff != null)
            {
                _db.BranchUsers.Remove(staff);
                await _db.SaveChangesAsync();
            }
        }
    }
}
