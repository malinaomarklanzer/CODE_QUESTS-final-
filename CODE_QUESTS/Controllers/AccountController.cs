using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CODE_QUESTS.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CODE_QUESTS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
        }

        // --- LOGIN ---
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        // --- SIGN UP ---
        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // --- UPDATED FILE UPLOAD LOGIC ---
                    if (model.AvatarImage != null)
                    {
                        // 1. Define the folder path (safer)
                        string directoryPath = Path.Combine(_hostEnvironment.WebRootPath, "images", "avatars");

                        // 2. Create the folder if it doesn't exist (safer)
                        Directory.CreateDirectory(directoryPath);

                        // 3. Create a unique file name
                        string uniqueFileName = user.Id + Path.GetExtension(model.AvatarImage.FileName);
                        string imagePath = Path.Combine(directoryPath, uniqueFileName);

                        // 4. Save the file
                        using (var fileStream = new FileStream(imagePath, FileMode.Create))
                        {
                            await model.AvatarImage.CopyToAsync(fileStream);
                        }
                    }
                    // --- END: FILE UPLOAD LOGIC ---

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Dashboard", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // --- LOGOUT ---
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // --- FORGOT/RESET PASSWORD ---
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, code = code }, protocol: Request.Scheme);

                Debug.WriteLine("--- PASSWORD RESET LINK ---");
                Debug.WriteLine(callbackUrl);

                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation() => View();

        [HttpGet]
        public IActionResult ResetPassword(string email, string code)
        {
            if (code == null || email == null) return BadRequest("Invalid parameters.");
            var model = new ResetPasswordViewModel { Code = code, Email = email, Password = "", ConfirmPassword = "" };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return RedirectToAction("ResetPasswordConfirmation");
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
            if (result.Succeeded) return RedirectToAction("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation() => View();

        // --- MANAGE PROFILE PAGES ---
        [Authorize]
        public IActionResult Profile() => View();

        // UPDATED: This method now correctly loads the Manage view
        [Authorize]
        [HttpGet]
        public IActionResult Manage()
        {
            // Create a wrapper model to send to the view
            var model = new ManageViewModel
            {
                ChangeName = new ChangeNameViewModel(),
                ChangePassword = new ChangePasswordViewModel()
            };
            return View(model);
        }

        // UPDATED: This method handles the "Change Name" form
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeName(ChangeNameViewModel changeNameModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.UserName = changeNameModel.NewUserName;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        TempData["StatusMessage"] = "Your username has been changed.";
                        return RedirectToAction("Manage");
                    }
                }
            }

            // If we fail, we must reload the *full* Manage page with both models
            var model = new ManageViewModel
            {
                ChangeName = changeNameModel, // Keep the invalid data
                ChangePassword = new ChangePasswordViewModel() // Add an empty password model
            };
            return View("Manage", model);
        }

        // UPDATED: This method handles the "Change Password" form
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword, changePasswordModel.NewPassword);
                    if (result.Succeeded)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        TempData["StatusMessage"] = "Your password has been changed.";
                        return RedirectToAction("Manage");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If we fail, reload the *full* Manage page
            var model = new ManageViewModel
            {
                ChangeName = new ChangeNameViewModel(), // Add an empty name model
                ChangePassword = changePasswordModel // Keep the invalid data
            };
            return View("Manage", model);
        }
    }
}