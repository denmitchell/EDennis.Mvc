using EDennis.MvcUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EDennis.MvcHits.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeConstants.FakeAuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly AppUserService<AppUserRolesContext> _service;

        public UsersController(AppUserService<AppUserRolesContext> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            //workaround -- Authorization with Roles or Policy, not working 
            if (!HttpContext.User.IsInRole("IT") && !HttpContext.User.IsInRole("admin"))
                return new StatusCodeResult(403);

            var recs = _service.GetQueryable()
                .Include(a => a.AppRole);


            return View(await recs.ToListAsync());
        }

        // GET: AppUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //workaround -- Authorization with Roles or Policy, not working 
            if (!HttpContext.User.IsInRole("IT") && !HttpContext.User.IsInRole("admin"))
                return new StatusCodeResult(403);

            if (id == null)
                return NotFound();

            var appUser = await _service.GetQueryable()
                .Include(a => a.AppRole)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appUser == null)
                return NotFound();

            return View(appUser);
        }

        // GET: AppUsers/Create
        public IActionResult Create()
        {
            //workaround -- Authorization with Roles or Policy, not working 
            if (!HttpContext.User.IsInRole("IT") && !HttpContext.User.IsInRole("admin"))
                return new StatusCodeResult(403);

            ViewData["Role"] = new SelectList(_service.DbContext.AppRoles, "Id", "RoleName");
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,RoleId,Id")] AppUser appUser)
        {
            //workaround -- Authorization with Roles or Policy, not working 
            if (!HttpContext.User.IsInRole("IT") && !HttpContext.User.IsInRole("admin"))
                return new StatusCodeResult(403);

            if (ModelState.IsValid)
            {
                await(_service.CreateAsync(appUser));
                return RedirectToAction(nameof(Index));
            }
            ViewData["Role"] = new SelectList(_service.DbContext.AppRoles, "Id", "RoleName", appUser.RoleId);
            return View(appUser);
        }

        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //workaround -- Authorization with Roles or Policy, not working 
            if (!HttpContext.User.IsInRole("IT") && !HttpContext.User.IsInRole("admin"))
                return new StatusCodeResult(403);

            if (id == null)
                return NotFound();

            var appUser = await _service.FindAsync(id);
            if (appUser == null)
                return NotFound();

            ViewData["Role"] = new SelectList(_service.DbContext.AppRoles, "Id", "RoleName", appUser.RoleId);
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserName,RoleId,Id,SysGuid,SysUser")] AppUser appUser)
        {
            //workaround -- Authorization with Roles or Policy, not working 
            if (!HttpContext.User.IsInRole("IT") && !HttpContext.User.IsInRole("admin"))
                return new StatusCodeResult(403);

            if (id != appUser.Id)
                return NotFound();

            if (ModelState.IsValid)                
            {
                await _service.UpdateAsync(appUser, id);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Role"] = new SelectList(_service.DbContext.AppRoles, "Id", "RoleName", appUser.RoleId);
            return View(appUser);
        }

        // GET: AppUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //workaround -- Authorization with Roles or Policy, not working 
            if (!HttpContext.User.IsInRole("IT") && !HttpContext.User.IsInRole("admin"))
                return new StatusCodeResult(403);

            if (_service.DbContext.AppUsers == null)
            {
                return Problem("Entity set 'AppUserRolesContext.AppUsers'  is null.");
            }
            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(int id)
        {
            return (_service.DbContext.AppUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
