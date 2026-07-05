using AutoMapper;
using G01.Models;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.AttachmentService;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitofwork, IMapper mapper, IAttachmentService attachmentService) 
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }


        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Member>();

            if (await repo.AnyAsync(m => m.Email == model.Email, ct))
                return Result.Fail("A member with this email already exists.");
            if (await repo.AnyAsync(m => m.Phone == model.Phone, ct))
                return Result.Fail("A member with this phone number already exists.");

            var photo = await _attachmentService.UploadAsync(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName, "MembersPictures", ct);
            if (string.IsNullOrEmpty(photo))
                return Result.Validation("Profile photo upload failed (check file type and size).");

            var member = _mapper.Map<Member>(model);
            member.Photo = photo;

            repo.AddAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            if (result == 0)
            {
                if (!string.IsNullOrEmpty(member.Photo))
                    _attachmentService.Delete(member.Photo, "members");

                return Result.Fail("Failed To Create Member");
            }
            else

                return Result.Ok();
        }



        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            return _mapper.Map<List<MemberViewModel>>(members);
        }
        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return null;

            var viewModel = _mapper.Map<MemberViewModel>(member);

            var activeMembership = (await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(MP => MP.MemberId == memberId
                 && MP.EndDate >= DateTime.Now, ct: ct));

            if (activeMembership is not null)
            {
                var activePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMembership.PlanId, ct);

                viewModel.PlanName = activePlan?.Name;
                viewModel.MembershipStartDate = activeMembership.CreatedAt.ToShortDateString();
                viewModel.MembershipEndDate = activeMembership.EndDate.ToShortDateString();
            }

            return viewModel;
        }


        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x => x.MemberId == memberId, ct: ct);
            return record is null ? null : _mapper.Map<HealthRecordViewModel>(record);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            return member is null ? null : _mapper.Map<MemberToUpdateViewModel>(member);
        }



        public async Task<Result> RemoveMemberAsync(int memberId, CancellationToken ct = default)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();
            var member = await memberRepo.GetByIdAsync(memberId, ct);
            if (member is null) return Result.NotFound("Member not found.");


            var hasFutureSessions = await _unitOfWork.BookingRepository.AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now);

            if (hasFutureSessions)
                return Result.Fail("Cannot delete a member with upcoming sessions.");

            memberRepo.DeleteAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result > 0)
            {
                if (!string.IsNullOrEmpty(member.Photo))
                    _attachmentService.Delete(member.Photo, "members");

                return Result.Ok();
            }
            return Result.Fail("Failed To Delete Member");
        }


        public async Task<Result> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Member>();
            var member = await repo.GetByIdAsync(id, ct);
            if (member is null) return Result.NotFound("Member not found.");
            if (await repo.AnyAsync(m => m.Email == model.Email && m.Id != id, ct))
                return Result.Fail("Another member is already using this email.");
            if (await repo.AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct))
                return Result.Fail("Another member is already using this phone number.");

            _mapper.Map(model, member);
            member.UpdatedAt = DateTime.Now;
            repo.UpdateAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To update Member");
        }

        

    }
}