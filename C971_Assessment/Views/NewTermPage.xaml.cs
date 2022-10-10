using C971_Assessment.Models;
using C971_Assessment.Resources;
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

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            if (!DateUtils.startBeforeEnd(startDatePicker.Date, endDatePicker.Date))
            {
                DisplayAlert("Failure", "The assessment start date must occur before end date.", "Ok");
                return;
            }
            Term newTerm = new Term();
            newTerm.Title = titleEntry.Text;
            newTerm.StartDate = startDatePicker.Date;
            newTerm.EndDate = endDatePicker.Date;

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

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}