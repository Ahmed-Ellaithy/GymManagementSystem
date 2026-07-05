using G01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Membership : BaseEntity
    {
        public Member Member { get; set; }
        public int MemberId { get; set; }

        public Plan Plan { get; set; }
        public int PlanId { get; set; }

        // start date == CreatedAt of BaseEntity
        public DateTime EndDate { get; set; }

        // Read only properties 
        // EF core by default will not map these properties to the database, but you can configure it to do so if needed.
        // Read only properties doesnot transfer into table in the database
        [NotMapped]
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;

    }
}
