using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string? Photo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }



        // member details
        public string? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? PlanName { get; set; }
        public string? MembershipStartDate { get; set; }
        public string? MembershipEndDate { get; set; }

    }
}
