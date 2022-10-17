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
    public partial class AssessmentPage : ContentPage
    {
        private int _courseId;
        private Course _selectedCourse;

        public AssessmentPage(Course selectedCourse)
        {
            InitializeComponent();
            _courseId = selectedCourse.Id;
            _selectedCourse = selectedCourse;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Assessment>();
                    List<Assessment> assessmentList = conn.Table<Assessment>().ToList();

                    // Sort out assessments whose courseId doesn't match the currently selected course
                    // For matching assessments, determine how many objective and performance assessments are already added.
                    for (int i = 0; i < assessmentList.Count; i++)
                    {
                        if (assessmentList[i].CourseID != _courseId)
                        {
                            assessmentList.RemoveAt(i);
                        }
                    }
                    assessmentListView.ItemsSource = assessmentList;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Database Connection Error", ex.Message, "Ok");
            }
        }

        private void assessmentListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Assessment selectedAssessment = (Assessment)assessmentListView.SelectedItem;

            if (selectedAssessment != null)
            {
                Navigation.PushAsync(new AssessmentDetailPage(selectedAssessment, _selectedCourse));
            }
        }

        private void AddAssessmentBtn_Clicked(object sender, EventArgs e)
        {
            // Continue only if the user has not reached the maximum number of assessments for the course.
            if (_selectedCourse.NumObjective == 1 && _selectedCourse.NumPerformance == 1)
            {
                DisplayAlert("Alert", "You have already added the maximum number of assessments for this course.", "Ok");
                return;
            }
            Navigation.PushAsync(new NewAssessmentPage(_selectedCourse));
        }
    }
}