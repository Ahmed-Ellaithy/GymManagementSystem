using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Booking : BaseEntity
    {
        public Member Member { get; set; }
        public int MemberId { get; set; }

        public Session Session { get; set; }
        public int SessionId { get; set; }


        // booking date == CreatedAt of BaseEntity
        public bool IsAttended { get; set; }
    }
}
