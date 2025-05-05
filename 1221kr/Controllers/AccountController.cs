using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using _1221kr.Models;
using Microsoft.Owin.Security;
using static ApplicationUserManager;

public class AccountController : Controller
{
    private ApplicationSignInManager _signInManager;
    private ApplicationUserManager _userManager;

    public AccountController()
    {
    }

    public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
    {
        UserManager = userManager;
        SignInManager = signInManager;
    }

    public ApplicationUserManager UserManager
    {
        get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private set => _userManager = value;
    }

    public ApplicationSignInManager SignInManager
    {
        get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        private set => _signInManager = value;
    }

    // GET: Account/Register
    [AllowAnonymous]
    public ActionResult Register()
    {
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Main", "Home");
            }
            AddErrors(result);
        }

        return View(model);
    }

    // GET: Account/Login
    [AllowAnonymous]
    public ActionResult Login(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await SignInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            shouldLockout: false);

        switch (result)
        {
            case SignInStatus.Success:
                return RedirectToLocal(returnUrl);
            case SignInStatus.Failure:
            default:
                ModelState.AddModelError("", "Неверные учетные данные");
                return View(model);
        }
    }

    // POST: Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Logout()
    {
        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        return RedirectToAction("Main", "Home");
    }

    #region Helpers
    private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error);
        }
    }

    private ActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction("Main", "Home");
    }
    #endregion
}