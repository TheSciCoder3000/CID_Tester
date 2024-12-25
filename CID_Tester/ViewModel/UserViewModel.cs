using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        private readonly TEST_USER _user;

        public string Fullname => _user.ToString();

        public UserViewModel(TEST_USER user)
        {
            _user = user;
        }
    }
}
