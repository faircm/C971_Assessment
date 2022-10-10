using C971_Assessment.Models;
using C971_Assessment.Resources;
using Plugin.LocalNotifications;
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
    public partial class EditCoursePage : ContentPage
    {
        private Course _currentCourse;

        public EditCoursePage(Course currentCourse)
        {
            InitializeComponent();
            _currentCourse = currentCourse;
            statusPicker.ItemsSource = PickerOptions.statusOptions;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            courseId.Text = _currentCourse.Id.ToString();
            titleEntry.Text = _currentCourse.Title;
            startDatePicker.Date = _currentCourse.StartDate;
            endDatePicker.Date = _currentCourse.EndDate;
            dueDatePicker.Date = _currentCourse.DueDate;
            statusPicker.SelectedItem = _currentCourse.Status;
            instName.Text = _currentCourse.InstructorName;
            instEmail.Text = _currentCourse.InstructorEmail;
            instPhone.Text = _currentCourse.InstructorPhone;
            courseNotes.Text = _currentCourse.Notes;
            notificationSwitch.IsToggled = _currentCourse.NotificationsOn;
        }

        private void saveTermBtn_Clicked(object sender, EventArgs e)
        {
            if (titleEntry.Text.Length == 0)
            {
                DisplayAlert("Failure", "Course must have a title.", "Ok");
                return;
            }
            if (DateUtils.startBeforeEnd(startDatePicker.Date, endDatePicker.Date) && DateUtils.isInDateRange(dueDatePicker.Date, startDatePicker.Date, endDatePicker.Date))
            {
                _currentCourse.Title = titleEntry.Text;
                _currentCourse.StartDate = startDatePicker.Date;
                _currentCourse.EndDate = endDatePicker.Date;
                _currentCourse.DueDate = dueDatePicker.Date;
                _currentCourse.Status = PickerOptions.statusOptions[statusPicker.SelectedIndex];
                _currentCourse.InstructorName = instName.Text;
                _currentCourse.InstructorEmail = instEmail.Text;
                _currentCourse.InstructorPhone = instPhone.Text;
                _currentCourse.Notes = courseNotes.Text;
                _currentCourse.NotificationsOn = notificationSwitch.IsToggled;
                var notifier = CrossLocalNotifications.Current;

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Term>();
                    if (conn.Update(_currentCourse) > 0)
                    {
                        DisplayAlert("Success", "Course edited successfully.", "Ok");
                        Navigation.PopToRootAsync();
                    }
                    else
                    {
                        DisplayAlert("Failure", "Course could not be edited", "Ok");
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