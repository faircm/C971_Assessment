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
        private Course _selectedCourse;
        private bool objFlag = false;

        public EditAssessmentPage(Assessment currentAssessment, Course selectedCourse)
        {
            InitializeComponent();
            _currentAssessment = currentAssessment;
            _selectedCourse = selectedCourse;
            assessmentType.ItemsSource = PickerOptions.typeOptions;
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

            if (_currentAssessment.Type == "Objective")
            {
                objFlag = true;
            }
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Check if this assessment will exceed the 1-per-type limit for each course.
                if (PickerOptions.typeOptions[assessmentType.SelectedIndex] == "Performance")
                {
                    if (_selectedCourse.NumPerformance == 1 && objFlag)
                    {
                        DisplayAlert("Error", "Each course may only have one Performance assessment", "Ok");
                        return;
                    }
                }
                else if (PickerOptions.typeOptions[assessmentType.SelectedIndex] == "Objective")
                {
                    if (_selectedCourse.NumObjective == 1 && !objFlag)
                    {
                        DisplayAlert("Error", "Each course may only have one Objective assessment", "Ok");
                        return;
                    }
                }

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
                    _currentAssessment.Type = PickerOptions.typeOptions[assessmentType.SelectedIndex];
                    _currentAssessment.StartDate = startDate.Date;
                    _currentAssessment.EndDate = endDate.Date;
                    _currentAssessment.DueDate = dueDate.Date;
                    _currentAssessment.NotificationsOn = notificationSwitch.IsToggled;

                    conn.CreateTable<Term>();
                    if (conn.Update(_currentAssessment) > 0)
                    {
                        DisplayAlert("Success", "Assessment edited successfully.", "Ok");
                        if (_currentAssessment.Type == "Objective")
                        {
                            if (!objFlag)
                            {
                                _selectedCourse.NumObjective++;
                                _selectedCourse.NumPerformance--;
                            }
                                
                        }
                        else
                        {
                            if (objFlag)
                            {
                                _selectedCourse.NumPerformance++;
                                _selectedCourse.NumObjective--;
                            }
                        }
                            
                    }
                    else
                    {
                        DisplayAlert("Failure", "Assessment could not be edited", "Ok");
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