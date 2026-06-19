using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    internal class Member : GymUser
    {
        public string? Photo { get; set; }

        //joined date == CreatedAt of BaseEntity
    }
}
