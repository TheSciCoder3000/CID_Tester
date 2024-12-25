using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly UserViewModel _user;
        public UserViewModel User => _user;

        public MainViewModel(TEST_USER user)
        {
            _user = new UserViewModel(user);
        }
    }
}
