﻿using C971_Assessment.Models;
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
    public partial class CourseDetailPage : ContentPage
    {
        private Course _selectedCourse;
        private Term _selectedTerm;

        public CourseDetailPage(Course selectedCourse, Term selectedTerm)
        {
            InitializeComponent();
            _selectedCourse = selectedCourse;
            _selectedTerm = selectedTerm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            courseId.Text = _selectedCourse.Id.ToString();
            termId.Text = _selectedCourse.termId.ToString();
            numObjective.Text = _selectedCourse.NumObjective.ToString();
            numPerformance.Text = _selectedCourse.NumPerformance.ToString();
            titleEntry.Text = _selectedCourse.Title;
            startDate.Text = _selectedCourse.StartDate.ToString("MM/dd/yyyy");
            endDate.Text = _selectedCourse.EndDate.ToString("MM/dd/yyyy");
            dueDate.Text = _selectedCourse.DueDate.ToString("MM/dd/yyyy");
            courseStatus.Text = _selectedCourse.Status;
            instName.Text = _selectedCourse.InstructorName;
            instEmail.Text = _selectedCourse.InstructorEmail;
            instPhone.Text = _selectedCourse.InstructorPhone;
            courseNotes.Text = _selectedCourse.Notes;
            notificationSwitch.IsToggled = _selectedCourse.NotificationsOn;
        }

        private void editDetailsBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditCoursePage(_selectedCourse));
        }

        private void deleteBtn_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Term>();
                if (conn.Delete(_selectedCourse) > 0)
                {
                    DisplayAlert("Success", "Course deleted successfully.", "Ok");
                    _selectedTerm.NumCourses--;
                }
                else
                {
                    DisplayAlert("Failure", "Course could not be deleted", "Ok");
                }
            }
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Term>();
                conn.Update(_selectedTerm);
                Navigation.PopAsync();
            }
        }

        private void viewAssessmentsBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentPage(_selectedCourse));
        }
    }
}