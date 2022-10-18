using C971_Assessment.Models;
using C971_Assessment.Resources;
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
    public partial class NewCoursePage : ContentPage
    {
        private Regex emailPattern = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        private Regex phonePattern = new Regex("^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$");

        // (UIBakery, n.d.)

        private Term _selectedTerm;
        private int _termId;

        public NewCoursePage(Term selectedTerm)
        {
            InitializeComponent();
            _termId = selectedTerm.Id;
            _selectedTerm = selectedTerm;
            statusPicker.ItemsSource = PickerOptions.statusOptions;
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
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

                //if (DateUtils.startBeforeEnd(startDatePicker.Date, endDatePicker.Date) && DateUtils.isInDateRange(dueDatePicker.Date, startDatePicker.Date, endDatePicker.Date))
                //{
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
                newCourse.NotificationsOn = notificationSwitch.IsChecked;
                newCourse.termId = _termId;

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Assessment>();
                    if (conn.Insert(newCourse) > 0)
                    {
                        DisplayAlert("Success", "Course added successfully.", "Ok");
                        _selectedTerm.NumCourses++;
                    }
                    else
                    {
                        DisplayAlert("Failure", "Course could not be added", "Ok");
                    }
                }
                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Term>();
                    conn.Update(_selectedTerm);
                    Navigation.PopAsync();
                }
            }
            catch (ArgumentNullException)
            {
                DisplayAlert("Error", "Ensure all fields are complete before proceeding", "Ok");
                return;
            }
            catch (IllegalDateTimeException) when (!DateUtils.startBeforeEnd(startDatePicker.Date, endDatePicker.Date))
            {
                DisplayAlert("Error", "Failure", "The course start date must occur before end date.", "Ok");
                return;
            }
            catch (IllegalDateTimeException) when (!DateUtils.isInDateRange(dueDatePicker.Date, startDatePicker.Date, endDatePicker.Date))
            {
                DisplayAlert("Error", "Failure", "The course due date must occur within the start and end dates.", "Ok");
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