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
        public CoursePage()
        {
            InitializeComponent();
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
                    courseListView.ItemsSource = courseList;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Database Connection Error", ex.Message, "ok");
            }
        }

        private void AddCourseBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewCoursePage());
        }

        private void courseListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Course selectedCourse = (Course)courseListView.SelectedItem;

            if (selectedCourse != null)
            {
                Navigation.PushAsync(new CourseDetailPage(selectedCourse));
            }
        }
    }
}