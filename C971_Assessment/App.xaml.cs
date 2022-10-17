using C971_Assessment.Services;
using C971_Assessment.Views;
using C971_Assessment.Resources;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Assessment
{
    public partial class App : Application
    {
        public static string _databaseLocation = string.Empty;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        public App(string databaseLocation)
        {
            _databaseLocation = databaseLocation;
            InitializeComponent();
            SampleData.populateDB();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}