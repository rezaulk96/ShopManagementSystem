using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopManagementSystem.Data;
using System.Threading.Tasks;

namespace ShopManagementSystem.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //Role List Get in Index Page
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.RoleList = roles;
            return View();
        }

        //Create Role
        public async Task<IActionResult> CreateRole(string userRole)
        {
            string msg = "";

            if (!string.IsNullOrEmpty(userRole))
            {
                if(await _roleManager.RoleExistsAsync(userRole))
                {
                    msg = "Role [" + userRole + "] exist!";
                }
                else
                {
                    IdentityRole r = new IdentityRole(userRole);
                    await _roleManager.CreateAsync(r);
                    msg = "Role [" + userRole + "] has been created successfully!";
                }
            }
            else
            {
                msg = "Please enter a valid role name!";
            }
            ViewBag.msg = msg;

            //For Role List Get
            ViewBag.RoleList = _roleManager.Roles.ToList();

            return View(nameof(Index));
        }

        //Get Assign Role
        public async Task<IActionResult> AssignRole()
        {
            ViewBag.users = _userManager.Users;
            ViewBag.roles = _roleManager.Roles;
            ViewBag.msg = TempData["msg"];

            //User + Roles list
           var userList = new List<dynamic>();

            foreach (var user in _userManager.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    Email = user.Email,
                    Roles = roles
                });
            }
            ViewBag.userRoles = userList;
            return View();
        }
        //Post Edit
        [HttpPost]
        public async Task<IActionResult> Edit(string email, string newRole)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);

            // remove old roles
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // add new role
            await _userManager.AddToRoleAsync(user, newRole);

            TempData["msg"] = "Role updated successfully!";
            return RedirectToAction(nameof(AssignRole));
        }

        //Role Remove
        public async Task<IActionResult> Delete(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
                await _userManager.RemoveFromRolesAsync(user, roles);

            TempData["msg"] = "All roles removed from user!";
            return RedirectToAction(nameof(AssignRole));
        }
        //Role Edit Get
        public async Task<IActionResult> Edit(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();

            ViewBag.roles = _roleManager.Roles.ToList();
            ViewBag.userRoles = await _userManager.GetRolesAsync(user);

            return View(user);
        }
        //Role Post
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userData, string roleData)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(userData) && !string.IsNullOrEmpty(roleData))
            {
                ApplicationUser u = await _userManager.FindByEmailAsync(userData);
                if (u != null)
                {
                    if (await _roleManager.RoleExistsAsync(roleData))
                    {
                        await _userManager.AddToRoleAsync(u, roleData);
                        msg = "Role has been assign to user!!!";
                    }
                    else
                    {
                        msg = "Role does not exist!!";
                    }
                }
                else
                {
                    msg = "User is not found!!";
                }
            }
            else
            {
                msg = "Please select a valid user and role!!!";
            }
            TempData["msg"] = msg;
            return RedirectToAction(nameof(AssignRole));
        }
    }
}
