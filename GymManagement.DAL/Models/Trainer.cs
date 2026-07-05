using GymManagement.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Trainer : GymUser
    {
        // HireDate => CreatedAt in BaseEntity
        public Specialty Specialty { get; set; }


        #region Relationships
        public ICollection<Session> Sessions { get; set; }
        #endregion

    }
}
