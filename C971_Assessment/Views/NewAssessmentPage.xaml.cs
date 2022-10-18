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
    public partial class NewAssessmentPage : ContentPage
    {
        private int _courseId;
        private Course _selectedCourse;

        public NewAssessmentPage(Course selectedCourse)
        {
            InitializeComponent();
            _courseId = selectedCourse.Id;
            _selectedCourse = selectedCourse;

            assessmentType.ItemsSource = PickerOptions.typeOptions;
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Check if this assessment will exceed the 1-per-type limit for each course.
                if (PickerOptions.typeOptions[assessmentType.SelectedIndex] == "Performance")
                {
                    if (_selectedCourse.NumPerformance == 1)
                    {
                        DisplayAlert("Error", "Each course may only have one Performance assessment", "Ok");
                        return;
                    }
                    else
                        _selectedCourse.NumPerformance++;
                }
                else if (PickerOptions.typeOptions[assessmentType.SelectedIndex] == "Objective")
                {
                    if (_selectedCourse.NumObjective == 1)
                    {
                        DisplayAlert("Error", "Each course may only have one Objective assessment", "Ok");
                        return;
                    }
                    else
                        _selectedCourse.NumObjective++;
                }

                if (assessmentTitle.Text.Length == 0 || assessmentType.SelectedIndex == -1 || startDate.Date == null || endDate.Date == null || dueDate.Date == null)
                {
                    throw new ArgumentNullException();
                }

                if (!DateUtils.startBeforeEnd(startDate.Date, endDate.Date) || !DateUtils.isInDateRange(dueDate.Date, startDate.Date, endDate.Date))
                {
                    throw new IllegalDateTimeException();
                }

                Assessment newAssessment = new Assessment();
                newAssessment.Title = assessmentTitle.Text;
                newAssessment.StartDate = startDate.Date;
                newAssessment.EndDate = endDate.Date;
                newAssessment.DueDate = dueDate.Date;
                newAssessment.Type = PickerOptions.typeOptions[assessmentType.SelectedIndex];
                newAssessment.NotificationsOn = notificationSwitch.IsChecked;
                newAssessment.CourseID = _courseId;

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Assessment>();
                    if (conn.Insert(newAssessment) > 0)
                    {
                        DisplayAlert("Success", "Assessment added successfully.", "Ok");
                    }
                    else
                    {
                        DisplayAlert("Failure", "Assessment could not be added", "Ok");
                    }
                }
                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Course>();
                    conn.Update(_selectedCourse);
                    Navigation.PopAsync();
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