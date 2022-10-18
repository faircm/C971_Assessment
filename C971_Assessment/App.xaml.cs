using C971_Assessment.Services;
using C971_Assessment.Views;
using C971_Assessment.Resources;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using C971_Assessment.Models;

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

            // FOR TESTING PURPOSES, CLEARS DATABASE OF ALL DATA
            /*SampleData.clearDB();
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Term>();
                conn.CreateTable<Course>();
                conn.CreateTable<Assessment>();
            }*/

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