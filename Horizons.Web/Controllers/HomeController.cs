namespace Horizons.Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels;

    public class HomeController : BaseController
    {
        [HttpGet]
        [AllowAnonymous] //available to unregistered users
        public IActionResult Index()
        {
            //logged in users not to access home page
            try
            {
                if(this.IsUserAuthenticated())
                { 
                    return this.RedirectToAction(nameof(Index), "Destination");
                }

                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
