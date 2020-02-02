using System.Collections.Generic;

namespace OnlineExamer.Models.ViewModels.Admin
{
    public class UserAdminViewModel
    {
        public IList<UserViewModel> Users { get; set; }

        public IList<UserViewModel> Admins { get; set; }
    }
}
