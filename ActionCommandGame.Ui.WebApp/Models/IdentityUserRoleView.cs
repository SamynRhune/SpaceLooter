using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Identity;

namespace ActionCommandGame.Ui.WebApp.Models
{
    public class IdentityUserRoleView
    {
        public IdentityUser User { get; set; }
        public IdentityRole Role { get; set; }
        public PlayerResult Player { get; set; }
    }
}
