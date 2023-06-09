using Microsoft.AspNetCore.Mvc.RazorPages;

namespace spl.Pages
{
    public class DashboardModel : PageModel
    {
        public string? Layout { get; private set; }

        public void OnGet()
        {
            string userType = Request.Cookies["UserType"] ?? "";

            if (userType == "admin")
            {
                Layout = "../Shared/_AdminLayout.cshtml";
            }
            else
            {
                Layout = "../Shared/_UrusetiaLayout.cshtml";
            }
        }
    }
}
