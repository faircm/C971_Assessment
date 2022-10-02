using C971_Assessment.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Assessment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTermPage : ContentPage
    {
        public NewTermPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            Term newTerm = new Term(titleEntry.Text, startDatePicker.Date, endDatePicker.Date);

            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Term>();
                if (conn.Insert(newTerm) > 0)
                {
                    DisplayAlert("Success", "Term added successfully.", "Ok");
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Failure", "Term could not be added", "Ok");
                }
            }
        }
    }
}