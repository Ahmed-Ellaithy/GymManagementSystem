using GymManagement.DAL.Models;

namespace G01.Models
{
    public class Plan : BaseEntity
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public bool IsActive { get; set; }

        #region Relationships
        public ICollection<Membership> PlanMembers { get; set; }
        #endregion
    }
}
