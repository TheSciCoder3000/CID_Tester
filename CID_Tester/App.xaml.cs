using CID_Tester.Model;
using System.Configuration;
using System.Data;
using System.Windows;
using System.IO;
using System.Diagnostics;
using CID_Tester.ViewModel;
using CID_Tester.DbContexts;
using Microsoft.EntityFrameworkCore;
using CID_Tester.Service.DbProvider;
using CID_Tester.Service.DbCreator;

namespace CID_Tester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly TesterDbContextFactory _testerDbContextFactory;
        private readonly IDbProvider _dbProvider;
        private readonly IDbCreator _dbCreator;

        public App()
        {
            DotNetEnv.Env.TraversePath().Load();
            _testerDbContextFactory = new TesterDbContextFactory(DotNetEnv.Env.GetString("CONNECTION_STRING"));
            _dbProvider = new DbProvider(_testerDbContextFactory);
            _dbCreator = new DbCreator(_testerDbContextFactory);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //using (TesterDbContext dbContext = _testerDbContextFactory.CreateDbContext())
            //{
            //    dbContext.Database.Migrate();
            //}

            base.OnStartup(e);
        }

        private void ApplicationStart(object sender, StartupEventArgs e)
        {

            LoginViewModel vm = new LoginViewModel(_dbProvider, _dbCreator);
            Login login = new Login();
            vm.ClosingRequest += (sender, e) => login.Close();
            login.DataContext = vm;
            login.ShowDialog();


        }
    }

}
