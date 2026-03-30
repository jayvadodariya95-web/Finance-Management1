using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Helpers;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Services
{
    public class UserService : IUserServices
    {
        private readonly IUserRepository userRepo;
        private readonly ILogger<UserService> logger;
        public UserService(IUserRepository userRepo,
        ILogger<UserService> logger)
        {
            this.userRepo = userRepo;
            this.logger = logger;
        }
        public async Task<User> CreateAsync(User user)
        {
            var existingUser = await userRepo.GetByEmailAsync(user.Email);

            if (existingUser != null)
            {
                logger.LogWarning("Duplicate email attempt: {Email}", user.Email);
                throw new Exception("User with this email already exists.");
            }

            return await userRepo.CreateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await userRepo.DeleteAsync(id);
        }

        public async Task<PagedResult<User>> GetAllAsync(PaginationParams paginationParams)
        {
            return await userRepo.GetAllAsync(paginationParams);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await userRepo.GetByEmailAsync(email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await userRepo.GetByIdAsync(id);
        }

        public async Task<UpdateUserDTO> UpdateAsync(UpdateUserDTO updateUser, int id)
        {
            var user = await userRepo.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            user.Username = updateUser.Username;
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.Email = updateUser.Email;

            if (!string.IsNullOrWhiteSpace(updateUser.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUser.Password);
            }

            user.Role = updateUser.Role;
            user.Gender = updateUser.Gender;
            user.MobileNumber = updateUser.MobileNumber;
            user.EmergencyMobileNumber = updateUser.EmergencyMobileNumber;

            await userRepo.UpdateAsync(user);

            return updateUser;
        }

        public async Task<User> PatchAsync(PatchUserDTO patchUser, int id)
        {
            var user = await userRepo.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            if (patchUser.Username != null)
                user.Username = patchUser.Username;

            if (patchUser.FirstName != null)
                user.FirstName = patchUser.FirstName;

            if (patchUser.LastName != null)
                user.LastName = patchUser.LastName;

            if (patchUser.Email != null)
                user.Email = patchUser.Email;

            if (!string.IsNullOrWhiteSpace(patchUser.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(patchUser.Password);
            }
            if (patchUser.Role.HasValue)
                user.Role = (Domain.Enums.UserRole)patchUser.Role.Value;

            if (patchUser.Gender.HasValue)
                user.Gender = (Domain.Enums.UserGender)patchUser.Gender.Value;

            if (patchUser.MobileNumber != null)
                user.MobileNumber = patchUser.MobileNumber;

            if (patchUser.EmergencyMobileNumber != null)
                user.EmergencyMobileNumber = patchUser.EmergencyMobileNumber;

            await userRepo.UpdateAsync(user);

            return user;
        }

        //    public async Task<Partner> PatchAsync(UpdatedPartnerDTO patchUser, int id)
        //    {
        //        var user = await userRepo.GetByIdAsync(id);

        //        if (user == null)
        //            throw new Exception("User not found");

        //        if (patchUser.SharePercentage != null)
        //            user.SharePercentage = patchUser.SharePercentage;

        //        if (patchUser.PartnershipType != null)
        //            user.FirstName = patchUser.FirstName;

        //        if (patchUser.LastName != null)
        //            user.LastName = patchUser.LastName;

        //        if (patchUser.Email != null)
        //            user.Email = patchUser.Email;

        //        if (patchUser.Role.HasValue)
        //            user.Role = (Domain.Enums.UserRole)patchUser.Role.Value;

        //        if (patchUser.Gender.HasValue)
        //            user.Gender = (Domain.Enums.UserGender)patchUser.Gender.Value;

        //        if (patchUser.MobileNumber != null)
        //            user.MobileNumber = patchUser.MobileNumber;

        //        if (patchUser.EmergencyMobileNumber != null)
        //            user.EmergencyMobileNumber = patchUser.EmergencyMobileNumber;

        //        await userRepo.UpdateAsync(user);

        //        return user;
        //    }
        //}

        //    public class PartnerServices : IPartnerServices
        //    {
        //        private readonly IPartnerRepository userRepo;
        //        private readonly ILogger<UserService> logger;
        //        public PartnerServices(IPartnerRepository userRepo,
        //        ILogger<UserService> logger)
        //        {
        //            this.userRepo = userRepo;
        //            this.logger = logger;
        //        }

        //        public Task<bool> DeleteAsync(int id)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public Task<Partner> GetPartnerByID(int id)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public Task<Partner> PatchAsync(UpdatedPartnerDTO partner, int id)
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //}

        public class PartnersService : IPartnerServices
        {
            private readonly IPartnerRepository _partnerRepo;
            private readonly ILogger<UserService> logger;
            public PartnersService(IPartnerRepository partnerRepo,
            ILogger<UserService> logger)
            {
                this._partnerRepo = partnerRepo;
                this.logger = logger;
            }
            public async Task<Partner> CreateAsync(PartnerDto partner)
            {
                //if(partner == null) throw new ArgumentNullException(nameof(partner));
                var newPartner = new Partner
                {
                    UserId = partner.UserId,
                    IsMainPartner = partner.IsMainPartner,
                    SharePercentage = partner.SharePercentage,
                    PartnershipType = partner.PartnershipType,
                    BranchId = 1

                };
                return await _partnerRepo.CreateAsync(newPartner);
            
            }

            public async Task<IEnumerable<Partner>> GetAllAsync()
            {
                return await _partnerRepo.GetAllAsync();
            }

            public async Task<Partner?> GetByIdAsync(int id)
            {
                return await _partnerRepo.GetByIdAsync(id);
            }

            public async Task<Partner?> GetByUserID(int userId)
            {
                return await _partnerRepo.GetByUserID(userId);
            }

            public async Task<IEnumerable<Partner>> GetMainPartnersAsync()
            {
                return await _partnerRepo.GetMainPartnersAsync();
            }

            public async Task<IEnumerable<Project>> GetPartnerProjectsAsync(int partnerId)
            {
                return await _partnerRepo.GetPartnerProjectsAsync(partnerId);
            }

            public async Task<Partner> PatchAsync(int id, UpdatedPartnerDTO partner)
            {
                var patchUser = await _partnerRepo.GetByIdAsync(id);

                if (patchUser == null)
                {
                    throw new InvalidOperationException("Invalid Partner");
                }
                if (partner.SharePercentage.HasValue)
                {
                    patchUser.SharePercentage = partner.SharePercentage.Value;
                }
                if (partner.PartnershipType != null)
                {
                    patchUser.PartnershipType = partner.PartnershipType;
                }
                if (partner.IsMainPartner.HasValue)
                {
                    patchUser.IsMainPartner = partner.IsMainPartner.Value;
                }
                if (partner.BranchId.HasValue)
                {
                    patchUser.BranchId = partner.BranchId.Value;
                }
                await _partnerRepo.UpdateAsync(patchUser);
                return patchUser;
            }

            public async Task<Partner> UpdateAsync(Partner partner, int id)
            {
                var Updatedpartner = await _partnerRepo.GetByIdAsync(id);
                if (partner == null)
                {
                    throw new InvalidOperationException("Invalid Partner");
                }
                await _partnerRepo.UpdateAsync(partner);
                return Updatedpartner;
            }


        }

        public class EmployeeService : IEmployeeServices
        {
            private readonly IEmployeeRepository _empRepo;
           

            public EmployeeService(IEmployeeRepository empRepo)
            {
                _empRepo = empRepo;
                
            }

            public async Task<Employee> CreateAsync(EmployeeDto employee)
            {
                var newEmp = new Employee
                {
                    UserId = employee.UserId,
                    BranchId = employee.BranchId ?? 0, 
                    EmployeeCode = employee.EmployeeCode,
                    CurrentCTC = employee.CurrentCTC,
                    JoinDate = employee.JoinDate,
                    MonthlySalary = employee.MonthlySalary,
                    PreviousCTC = employee.PreviousCTC,
                    Position = employee.Position,
                    TakenLeave = employee.TakenLeave ?? 0, 
                    Department = employee.Department,
                    RelievingDate = employee.RelievingDate
                };

                return await _empRepo.CreateAsync(newEmp);
            }

            public async Task<IEnumerable<Employee>> GetAllAsync()
            {
                return await _empRepo.GetAllAsync();
            }

            public async Task<Employee?> GetByIdAsync(int id)
            {
                return await _empRepo.GetByIdAsync(id);
            }

            public async Task<Employee?> GetEmployeeByUserIdAsync(int userId)
            {
                return await _empRepo.GetEmployeeByUserIdAsync(userId);
            }

            public async Task<IEnumerable<Project>> GetEmployeeProjectsAsync(int employeeId)
            {
                return await _empRepo.GetEmployeeProjectsAsync(employeeId);
            }

            public async Task<Employee> UpdateAsync(int id, Employee employee)
            {
                var UpdatedEmployee = await _empRepo.GetByIdAsync(id);
                if (employee == null)
                {
                    throw new InvalidOperationException("Invalid Partner");
                }
                await _empRepo.UpdateAsync(employee);
                return UpdatedEmployee;
            }
        }
    }
}
