using C971_Assessment.Models;
using C971_Assessment.Resources;
using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Assessment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCoursePage : ContentPage
    {
        //https://uibakery.io/regex-library/email-regex-csharp email regex
        //https://uibakery.io/regex-library/phone-number-csharp phone number regex

        private Regex emailPattern = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        private Regex phonePattern = new Regex("^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$");

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
            termId.Text = _currentCourse.termId.ToString();
            titleEntry.Text = _currentCourse.Title;
            startDatePicker.Date = _currentCourse.StartDate;
            endDatePicker.Date = _currentCourse.EndDate;
            dueDatePicker.Date = _currentCourse.DueDate;
            statusPicker.SelectedItem = _currentCourse.Status;
            instName.Text = _currentCourse.InstructorName;
            instEmail.Text = _currentCourse.InstructorEmail;
            instPhone.Text = _currentCourse.InstructorPhone;
            courseNotes.Text = _currentCourse.Notes;
            notificationSwitch.IsChecked = _currentCourse.NotificationsOn;
        }

        private void saveCourse_btn(object sender, EventArgs e)
        {
            try
            {
                if (titleEntry.Text.Length == 0 || statusPicker.SelectedIndex == -1 || startDatePicker.Date == null || endDatePicker.Date == null || dueDatePicker.Date == null || instName.Text.Length == 0 || instPhone.Text.Length == 0 || instEmail.Text.Length == 0)
                {
                    throw new ArgumentNullException();
                }

                if (!DateUtils.startBeforeEnd(startDatePicker.Date, endDatePicker.Date) || !DateUtils.isInDateRange(dueDatePicker.Date, startDatePicker.Date, endDatePicker.Date))
                {
                    throw new IllegalDateTimeException();
                }

                if (!emailPattern.IsMatch(instEmail.Text))
                {
                    throw new InvalidEmailAddressException();
                }
                if (!phonePattern.IsMatch(instPhone.Text))
                {
                    throw new InvalidPhoneNumberException();
                }

                _currentCourse.Id = Int32.Parse(courseId.Text);
                _currentCourse.termId = Int32.Parse(termId.Text);
                _currentCourse.Title = titleEntry.Text;
                _currentCourse.StartDate = startDatePicker.Date;
                _currentCourse.EndDate = endDatePicker.Date;
                _currentCourse.DueDate = dueDatePicker.Date;
                _currentCourse.Status = PickerOptions.statusOptions[statusPicker.SelectedIndex];
                _currentCourse.InstructorName = instName.Text;
                _currentCourse.InstructorEmail = instEmail.Text;
                _currentCourse.InstructorPhone = instPhone.Text;
                _currentCourse.Notes = courseNotes.Text;
                _currentCourse.NotificationsOn = notificationSwitch.IsChecked;
                var notifier = CrossLocalNotifications.Current;

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Term>();
                    if (conn.Update(_currentCourse) > 0)
                    {
                        DisplayAlert("Success", "Course edited successfully.", "Ok");
                        Navigation.PopAsync();
                    }
                    else
                    {
                        DisplayAlert("Failure", "Course could not be edited", "Ok");
                    }
                }
                //}
            }
            catch (ArgumentNullException)
            {
                DisplayAlert("Error", "Ensure all fields are complete before proceeding", "Ok");
                return;
            }
            catch (IllegalDateTimeException) when (!DateUtils.startBeforeEnd(startDatePicker.Date, endDatePicker.Date))
            {
                DisplayAlert("Error", "The course start date must occur before end date.", "Ok");
                return;
            }
            catch (IllegalDateTimeException) when (!DateUtils.isInDateRange(dueDatePicker.Date, startDatePicker.Date, endDatePicker.Date))
            {
                DisplayAlert("Error", "The course due date must occur within the start and end dates.", "Ok");
                return;
            }
            catch (InvalidEmailAddressException)
            {
                DisplayAlert("Error", "You have entered an invalid email address.", "Ok");
                return;
            }
            catch (InvalidPhoneNumberException)
            {
                DisplayAlert("Error", "You have entered an invalid phone number.", "Ok");
                return;
            }
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}