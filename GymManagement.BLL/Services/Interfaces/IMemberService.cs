using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        // Get all     
        Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default);

        // Create member
        Task<bool> CreateMemberAsync( CreateMemberViewModel member, CancellationToken ct = default);

        // Get member Details 
        Task<MemberViewModel?> GetMemberDetailsByIdAsync(int memberId, CancellationToken ct = default);

        // Get member with health record details
        Task<HealthRecordViewModel> GetMemberHealthRecord(int memberId, CancellationToken ct = default);
    }
}
