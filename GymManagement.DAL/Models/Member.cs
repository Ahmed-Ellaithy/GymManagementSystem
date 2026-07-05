using G01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }

        //joined date == CreatedAt of BaseEntity

        #region Relationships
        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<Membership> MemberPlans { get; set; }

        public ICollection<Booking> MemberSession { get; set; }
        #endregion
    }
}
