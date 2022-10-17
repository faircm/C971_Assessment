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
    public partial class EditTermPage : ContentPage
    {
        private Term _currentTerm;

        public EditTermPage(Term currentTerm)
        {
            InitializeComponent();
            _currentTerm = currentTerm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            termId.Text = _currentTerm.Id.ToString();
            titleEntry.Text = _currentTerm.Title;
            startDatePicker.Date = _currentTerm.StartDate;
            endDatePicker.Date = _currentTerm.EndDate;
        }

        private void saveTermBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (titleEntry.Text.Length == 0 || startDatePicker.Date == null || endDatePicker.Date == null)
                {
                    throw new ArgumentNullException();
                }

                if (!DateUtils.startBeforeEnd(startDatePicker.Date, endDatePicker.Date))
                {
                    throw new IllegalDateTimeException();
                }
                _currentTerm.Id = Int32.Parse(termId.Text);
                _currentTerm.Title = titleEntry.Text;
                _currentTerm.StartDate = startDatePicker.Date;
                _currentTerm.EndDate = endDatePicker.Date;
                _currentTerm.NumCourses = Int32.Parse(numCourses.Text);

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Term>();
                    if (conn.Update(_currentTerm) > 0)
                    {
                        DisplayAlert("Success", "Term edited successfully.", "Ok");
                        Navigation.PopToRootAsync();
                    }
                    else
                    {
                        DisplayAlert("Failure", "Term could not be edited", "Ok");
                    }
                }
            }
            catch (ArgumentNullException)
            {
                DisplayAlert("Error", "Ensure all fields are complete before proceeding", "Ok");
                return;
            }
            catch (IllegalDateTimeException)
            {
                DisplayAlert("Error", "The term start date must occur before end date.", "Ok");
                return;
            }
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}