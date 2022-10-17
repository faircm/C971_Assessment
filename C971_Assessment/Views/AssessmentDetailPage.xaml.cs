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
    public partial class AssessmentDetailPage : ContentPage
    {
        private Assessment _selectedAssessment;
        private Course _selectedCourse;

        public AssessmentDetailPage(Assessment selectedAssessment, Course selectedCourse)
        {
            InitializeComponent();
            _selectedAssessment = selectedAssessment;
            _selectedCourse = selectedCourse;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            assessmentId.Text = _selectedAssessment.Id.ToString();
            courseId.Text = _selectedAssessment.CourseID.ToString();
            assessmentTitle.Text = _selectedAssessment.Title;
            startDate.Text = _selectedAssessment.StartDate.ToString("MM/dd/yyyy");
            endDate.Text = _selectedAssessment.EndDate.ToString("MM/dd/yyyy");
            dueDate.Text = _selectedAssessment.DueDate.ToString("MM/dd/yyy");
            assessmentType.Text = _selectedAssessment.Type;
            notificationSwitch.IsChecked = _selectedAssessment.NotificationsOn;
        }

        private void editDetailsBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditAssessmentPage(_selectedAssessment, _selectedCourse));
        }

        private void deleteBtn_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Assessment>();
                if (conn.Delete(_selectedAssessment) > 0)
                {
                    DisplayAlert("Success", "Assessment deleted successfully.", "Ok");
                    if (_selectedAssessment.Type == "Objective")
                    {
                        _selectedCourse.NumObjective--;
                        if (_selectedCourse.NumObjective < 0)
                            _selectedCourse.NumObjective = 0;
                    }
                    else
                    {
                        _selectedCourse.NumPerformance--;
                        if (_selectedCourse.NumPerformance < 0)
                            _selectedCourse.NumPerformance = 0;
                    }
                }
                else
                {
                    DisplayAlert("Failure", "Assessment could not be deleted", "Ok");
                }
            }
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Course>();
                conn.Update(_selectedCourse);
                Navigation.PopAsync();
            }
        }
    }
}