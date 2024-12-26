using CID_Tester.DbContexts;
using CID_Tester.DbContexts.DTO;
using CID_Tester.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CID_Tester.Service.DbProvider
{
    public class DbProvider : IDbProvider
    {
        private readonly TesterDbContextFactory _dbContextFactory;

        public DbProvider(TesterDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<DUT>> GetAllDuts()
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                IEnumerable<DutDTO> dutDTOs = await context.DUT.ToListAsync();
                return dutDTOs.Select(r => new DUT()
                {
                    DUT_CODE = r.DUT_CODE,
                    DESCRIPTION = r.DESCRIPTION
                });

            }
        }

        public Task<IEnumerable<TEST_PARAMETER>> GetAllTestParameters()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEST_PROCEDURE>> GetAllTestPlans()
        {
            throw new NotImplementedException();
        }

        public async Task<TEST_USER?> GetUser(string username, string password)
        {
            using(TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                UserDTO? userDTO = await context.TEST_USER.Where(r => r.USER_NAME == username && r.PASSWORD == password).FirstOrDefaultAsync();
                var userCollection = await context.TEST_USER.ToListAsync();

                if (userDTO == null) return null;

                TEST_USER user = new TEST_USER()
                {
                    USER_CODE = userDTO.USER_CODE,
                    FIRST_NAME = userDTO.FIRST_NAME,
                    LAST_NAME = userDTO.LAST_NAME,
                    EMAIL = userDTO.EMAIL,
                    PROFILE_IMAGE = userDTO.PROFILE_IMAGE,
                    USER_NAME = userDTO.USER_NAME,
                    PASSWORD = userDTO.PASSWORD
                };

                return user;
            }
        }
    }
}
