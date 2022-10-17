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
    public partial class EditAssessmentPage : ContentPage
    {
        private Assessment _currentAssessment;

        public EditAssessmentPage(Assessment currentAssessment)
        {
            InitializeComponent();
            _currentAssessment = currentAssessment;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            assessmentId.Text = _currentAssessment.Id.ToString();
            courseId.Text = _currentAssessment.CourseID.ToString();
            assessmentTitle.Text = _currentAssessment.Title;
            assessmentType.SelectedItem = _currentAssessment.Type;
            startDate.Date = _currentAssessment.StartDate;
            endDate.Date = _currentAssessment.EndDate;
            dueDate.Date = _currentAssessment.DueDate;
            notificationSwitch.IsToggled = _currentAssessment.NotificationsOn;
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (assessmentTitle.Text.Length == 0 || assessmentType.SelectedIndex == -1 || startDate.Date == null || endDate.Date == null || dueDate.Date == null)
                {
                    throw new ArgumentNullException();
                }

                if (!DateUtils.startBeforeEnd(startDate.Date, endDate.Date) || !DateUtils.isInDateRange(dueDate.Date, startDate.Date, endDate.Date))
                {
                    throw new IllegalDateTimeException();
                }

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    _currentAssessment.Id = Int32.Parse(assessmentId.Text);
                    _currentAssessment.CourseID = Int32.Parse(courseId.Text);
                    _currentAssessment.Title = assessmentTitle.Text;
                    _currentAssessment.Type = assessmentType.SelectedItem.ToString();
                    _currentAssessment.StartDate = startDate.Date;
                    _currentAssessment.EndDate = endDate.Date;
                    _currentAssessment.DueDate = dueDate.Date;
                    _currentAssessment.NotificationsOn = notificationSwitch.IsToggled;

                    conn.CreateTable<Term>();
                    if (conn.Update(_currentAssessment) > 0)
                    {
                        DisplayAlert("Success", "Assessment edited successfully.", "Ok");
                        Navigation.PopToRootAsync();
                    }
                    else
                    {
                        DisplayAlert("Failure", "Assessment could not be edited", "Ok");
                    }
                }
            }
            catch (ArgumentNullException)
            {
                DisplayAlert("Error", "Ensure all fields are complete before proceeding", "Ok");
                return;
            }
            catch (IllegalDateTimeException) when (!DateUtils.startBeforeEnd(startDate.Date, endDate.Date))
            {
                DisplayAlert("Error", "The assessment start date must occur before end date.", "Ok");
                return;
            }
            catch (IllegalDateTimeException) when (!DateUtils.isInDateRange(dueDate.Date, startDate.Date, endDate.Date))
            {
                DisplayAlert("Error", "The assessment due date must occur within the start and end dates.", "Ok");
                return;
            }
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}