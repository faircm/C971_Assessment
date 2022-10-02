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
    public partial class EditTermPage : ContentPage
    {
        private Term _currentTerm;

        public EditTermPage(Term currentTerm)
        {
            InitializeComponent();
            _currentTerm = currentTerm;

            titleEntry.Text = _currentTerm.Title;
            startDatePicker.Date = _currentTerm.StartDate;
            endDatePicker.Date = _currentTerm.EndDate;
        }

        private void saveTermBtn_Clicked(object sender, EventArgs e)
        {
            _currentTerm.Title = titleEntry.Text;
            _currentTerm.StartDate = startDatePicker.Date;
            _currentTerm.EndDate = endDatePicker.Date;

            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Term>();
                if (conn.Update(_currentTerm) > 0)
                {
                    DisplayAlert("Success", "Term edited successfully.", "Ok");
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Failure", "Term could not be edited", "Ok");
                }
            }
        }
    }
}