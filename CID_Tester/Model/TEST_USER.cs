using CID_Tester.Model.DTO;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CID_Tester.Model
{
    public class TEST_USER
    {

        private readonly IDbCreator _dbCreator;
        private readonly IDbProvider _dbProvider;

        public int USER_CODE { get; }
        public string FIRST_NAME { get; }
        public string LAST_NAME { get; }
        public string EMAIL { get; }
        public string PROFILE_IMAGE { get; }
        public string USER_NAME { get; }
        public string PASSWORD { get; }
        public ICollection<TEST_PROCEDURE> TEST_PLANS { get; }


        public TEST_USER(IDbCreator dbCreator, IDbProvider dbProvider, int userCode, string firstName, string lastName, string email, string profileImage, string username, string password, ICollection<TEST_PROCEDURE> testProcedures)
        {
            _dbCreator = dbCreator;
            _dbProvider = dbProvider;
            USER_CODE = userCode;
            FIRST_NAME = firstName;
            LAST_NAME = lastName;
            EMAIL = email;
            PROFILE_IMAGE = profileImage;
            USER_NAME = username;
            PASSWORD = password;
            TEST_PLANS = testProcedures;
        }

        public override string ToString()
        {
            return $"{FIRST_NAME} {LAST_NAME}";
        }

        public async Task AddTestPlan(TEST_PROCEDURE testPlan)
        {
            await _dbCreator.CreateTestPlan(testPlan, this);
            TEST_PLANS.Add(testPlan);
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public UserDTO ToDTO()
        {
            return new UserDTO()
            {
                USER_CODE = USER_CODE,
                FIRST_NAME = FIRST_NAME,
                LAST_NAME = LAST_NAME,
                EMAIL = EMAIL,
                PROFILE_IMAGE = PROFILE_IMAGE,
                USER_NAME = USER_NAME,
                PASSWORD = PASSWORD
            };
        }

    }
}
