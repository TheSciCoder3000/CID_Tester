using System.Windows;
using System.Diagnostics;
using CID_Tester.ViewModel;
using Microsoft.EntityFrameworkCore;
using CID_Tester.Service.DbProvider;
using CID_Tester.Service.DbCreator;
using CID_Tester.Model.DbContexts;
using CID_Tester.ViewModel.DebugSDK;

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
        private readonly bool _isProduction;

        public App()
        {
            
            DotNetEnv.Env.TraversePath().Load();
            _testerDbContextFactory = new TesterDbContextFactory(DotNetEnv.Env.GetString("CONNECTION_STRING"));
            _dbProvider = new DbProvider(_testerDbContextFactory);
            _dbCreator = new DbCreator(_testerDbContextFactory);
            _isProduction = DotNetEnv.Env.GetBool("PRODUCTION");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!_isProduction) 
                using (TesterDbContext dbContext = _testerDbContextFactory.CreateDbContext())
                {
                    dbContext.Database.Migrate();
                }

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

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            short handle = 0;
            Imports.Stop(handle);
            Debug.WriteLine(handle);
        }
    }

}
