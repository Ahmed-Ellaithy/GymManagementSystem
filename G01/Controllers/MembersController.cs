using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G01.PL.Controllers
{
    public class MembersController : Controller
    {
        // members service
        private readonly IMemberService _memService;
        public MembersController(IMemberService memService)
        {
            _memService = memService;
        }



        #region Get Members
        // Get :: BaseUrl/Members/Index => list all members
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memService.GetAllAsync(ct);
            // service => GetAllMembers
            return View(members);
        }

        // Get :: BaseUrl/Members/Details/{Id} => view details of a specific member


        // Get :: BaseUrl/Members/HealthReacordDetails/{Id} => get data of a specific member with health record

        #endregion

        #region Create Members
        // Get :: BaseUrl/Members/Create => show empty form
        [HttpGet]
        public IActionResult Create()
            => View();

        // Post :: BaseUrl/Members/Create/{member} => submit form
        // CreateMember
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);
            
            var result = await _memService.CreateMemberAsync(model, ct);

            if(result)
                TempData["SuccessMessage"] = "Member created successfully.";
            else
                TempData["ErrorMessage"] = "Failed to create member. Please try again.";

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        // Get :: BaseUrl/Members/Edit/{id} => show edit form

        // Post :: BaseUrl/Members/Edit/{member} => submit edit form
        #endregion

        #region Delete
        // Get BaseUrl/Members/Delete/{id} => show validation page
        #endregion

    }
}
