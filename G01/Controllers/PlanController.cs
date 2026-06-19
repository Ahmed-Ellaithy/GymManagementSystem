using G01.Context;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace G01.Controllers
{
    public class PlanController : Controller
    {

        private readonly IPlanRepository _planRepository ;
        public PlanController(IPlanRepository planRepository)
        {
            _planRepository = planRepository;

        }

        // GET :: BaseUrl/Plan/Index

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct : ct);
            return View(plans);
        }



        // GET :: BaseUrl/Plan/Details/{Id}
        public async Task<IActionResult> Details([FromRoute] int id , CancellationToken ct )
        {
            var plan = await _planRepository.GetByIdAsync(id,ct);
            if (plan == null)
                return RedirectToAction(nameof(Index));

            return View(plan);
        }
    }
}
