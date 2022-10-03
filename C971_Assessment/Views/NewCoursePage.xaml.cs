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
    public partial class NewCoursePage : ContentPage
    {
        public NewCoursePage()
        {
            InitializeComponent();

            statusPicker.ItemsSource = Status.statusOptions;
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            Course newCourse = new Course();
            newCourse.Title = titleEntry.Text;
            newCourse.StartDate = startDatePicker.Date;
            newCourse.EndDate = endDatePicker.Date;
            newCourse.DueDate = dueDatePicker.Date;
            newCourse.Status = Status.statusOptions[statusPicker.SelectedIndex];
            newCourse.InstructorName = instName.Text;
            newCourse.InstructorEmail = instEmail.Text;
            newCourse.InstructorPhone = instPhone.Text;
            newCourse.Notes = courseNotes.Text;
            newCourse.NotificationsOn = notificationSwitch.IsToggled;
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Course>();
                if (conn.Insert(newCourse) > 0)
                {
                    DisplayAlert("Success", "Course added successfully.", "Ok");
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Failure", "Course could not be added", "Ok");
                }
            }
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}