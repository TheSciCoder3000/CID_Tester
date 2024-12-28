using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.ViewModel
{
    public class ResultsViewModel : BaseViewModel
    {
        private readonly TEST_USER _user;
        public string Title { get; set; }

        public ResultsViewModel(TEST_USER user, string title)
        {
            _user = user;
            Title = title;
        }
    }
}
