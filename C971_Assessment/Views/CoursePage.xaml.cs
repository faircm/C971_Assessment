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
    public partial class CoursePage : ContentPage
    {
        private int _termID;
        private Term _selectedTerm;

        public CoursePage(Term selectedTerm)
        {
            InitializeComponent();
            _termID = selectedTerm.Id;
            _selectedTerm = selectedTerm;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Course>();
                    List<Course> courseList = conn.Table<Course>().ToList();

                    for (int i = 0; i < courseList.Count; i++)
                    {
                        if (courseList[i].termId != _termID)
                        {
                            courseList.RemoveAt(i);
                        }
                    }
                    courseListView.ItemsSource = courseList;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Database Connection Error", ex.Message, "Ok");
            }
        }

        private void AddCourseBtn_Clicked(object sender, EventArgs e)
        {
            // Only allow user to add a course if the maximum of 6 per term has not been reached.
            if (_selectedTerm.NumCourses == 6)
            {
                DisplayAlert("Alert", "You have already added the maximum number of courses per term (6).", "Ok");
                return;
            }
            else
                Navigation.PushAsync(new NewCoursePage(_selectedTerm));
        }

        private void courseListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Course selectedCourse = (Course)courseListView.SelectedItem;

            if (selectedCourse != null)
            {
                Navigation.PushAsync(new CourseDetailPage(selectedCourse, _selectedTerm));
            }
        }
    }
}