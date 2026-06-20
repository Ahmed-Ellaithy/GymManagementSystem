using G01.Models;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        // Database connection 
        private readonly IGenericRepository<Member> _memberRepo;
        private readonly IGenericRepository<Membership> _membershipRepo;
        private readonly IGenericRepository<Plan> _planRepo;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepo;

        public MemberService(IGenericRepository<Member> memberRepo,
                               IGenericRepository<Membership> membershipRepo,
                               IGenericRepository<Plan> planRepo,
                               IGenericRepository<HealthRecord> healthRecordRepo)
        {
            _memberRepo = memberRepo;
            _membershipRepo = membershipRepo;
            _planRepo = planRepo;
            _healthRecordRepo = healthRecordRepo;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            // email exist or not 
            var emailExist = await _memberRepo.AnyAsync(X => X.Email == model.Email);
            // phone exist or not
            var phoneExist = await _memberRepo.AnyAsync(X => X.Phone == model.Phone);

            if (emailExist || phoneExist) return false;
            // Add member to database
            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    Note = model.HealthRecordViewModel.Note
                }
            };
            var result = await _memberRepo.AddAsync(member);
            return result > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default)
        {
            var members = await _memberRepo.GetAllAsync(ct: ct);
            // members come from Database
            if (!members.Any()) return [];
            //member => view model
            List<MemberViewModel> memberVM = new List<MemberViewModel>();
            foreach (var member in members)
            {
                //Data come from database and i need to send it to view model
                // manual mapping
                var memberViewModel = new MemberViewModel()
                {
                    Id = member.Id,
                    Photo = member.Photo,
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    Gender = member.Gender.ToString()
                };
                memberVM.Add(memberViewModel);
            }
            return memberVM;
        }

        
        
        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int memberId, CancellationToken ct = default)
        {
            // get member by id 
            var member = await _memberRepo.GetByIdAsync(memberId, ct);
            if (member == null) return null;
            // table = member
            // return member details to view model
            var model = new MemberViewModel()
            {
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
                // planName - membershipStart and End                
            };
            // Check if member has active membership = Plan  or not
            var ActiveMembership = await _membershipRepo.FirstOrDefaultAsync(X => X.MemberId == memberId && X.EndDate > DateTime.Now);
            if (ActiveMembership is not null)
            {
                // plan name
                var ActivePlan = await _planRepo.GetByIdAsync(ActiveMembership.PlanId , ct);
                model.PlanName = ActivePlan?.Name;
                model.MembershipStartDate = ActiveMembership.CreatedAt.ToString();
                model.MembershipEndDate = ActiveMembership.EndDate.ToString();
            }
            return model;
        }

        public async Task<HealthRecordViewModel> GetMemberHealthRecord(int memberId, CancellationToken ct = default)
        {
            var record = await _healthRecordRepo.FirstOrDefaultAsync(X => X.MemberId == memberId, ct:ct);
            if (record is null) return null;
            else
                return new HealthRecordViewModel()
                {
                    Weight = record.Weight,
                    Height = record.Height,
                    BloodType = record.BloodType,
                    Note = record.Note
                };
           

        }
    }
}