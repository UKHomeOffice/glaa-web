using System;
using System.Collections.Generic;
using System.Text;

namespace GLAA.ViewModels.Admin
{
    public class AdminUserListViewModel
    {
        public AdminUserListViewModel()
        {
            Users = new Dictionary<string, IEnumerable<AdminUserViewModel>>();
        }

        public IDictionary<string, IEnumerable<AdminUserViewModel>> Users { get; }
    }
}
