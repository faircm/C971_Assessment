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

            statusPicker.ItemsSource = PickerOptions.statusOptions;
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            if (titleEntry.Text.Length == 0)
            {
                DisplayAlert("Failure", "Course must have a title.", "Ok");
                return;
            }

            if (DateUtils.startBeforeEnd(startDatePicker.Date, endDatePicker.Date) && DateUtils.isInDateRange(dueDatePicker.Date, startDatePicker.Date, endDatePicker.Date))
            {
                Course newCourse = new Course();
                newCourse.Title = titleEntry.Text;
                newCourse.StartDate = startDatePicker.Date;
                newCourse.EndDate = endDatePicker.Date;
                newCourse.DueDate = dueDatePicker.Date;
                newCourse.Status = PickerOptions.statusOptions[statusPicker.SelectedIndex];
                newCourse.InstructorName = instName.Text;
                newCourse.InstructorEmail = instEmail.Text;
                newCourse.InstructorPhone = instPhone.Text;
                newCourse.Notes = courseNotes.Text;
                newCourse.NotificationsOn = notificationSwitch.IsToggled;

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Assessment>();
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
            else if (!DateUtils.startBeforeEnd(startDatePicker.Date, endDatePicker.Date))
            {
                DisplayAlert("Failure", "The course start date must occur before end date.", "Ok");
                return;
            }
            else if (!DateUtils.isInDateRange(dueDatePicker.Date, startDatePicker.Date, endDatePicker.Date))
            {
                DisplayAlert("Failure", "The course due date must occur within the start and end dates.", "Ok");
                return;
            }
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}