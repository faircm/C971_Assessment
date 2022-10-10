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
    public partial class EditAssessmentPage : ContentPage
    {
        private Assessment _currentAssessment;

        public EditAssessmentPage(Assessment currentAssessment)
        {
            InitializeComponent();
            _currentAssessment = currentAssessment;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            assessmentId.Text = _currentAssessment.Id.ToString();
            assessmentTitle.Text = _currentAssessment.Title;
            assessmentType.SelectedItem = _currentAssessment.Type;
            startDate.Date = _currentAssessment.StartDate;
            endDate.Date = _currentAssessment.EndDate;
            notificationSwitch.IsChecked = _currentAssessment.NotificationsOn;
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            if (!DateUtils.startBeforeEnd(startDate.Date, endDate.Date))
            {
                DisplayAlert("Failure", "The assessment start date must occur before end date.", "Ok");
                return;
            }
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                _currentAssessment.Id = Int32.Parse(assessmentId.Text);
                _currentAssessment.Title = assessmentTitle.Text;
                _currentAssessment.Type = assessmentType.SelectedItem.ToString();
                _currentAssessment.StartDate = startDate.Date;
                _currentAssessment.EndDate = endDate.Date;
                _currentAssessment.NotificationsOn = notificationSwitch.IsChecked;

                conn.CreateTable<Term>();
                if (conn.Update(_currentAssessment) > 0)
                {
                    DisplayAlert("Success", "Assessment edited successfully.", "Ok");
                    Navigation.PopToRootAsync();
                }
                else
                {
                    DisplayAlert("Failure", "Assessment could not be edited", "Ok");
                }
            }
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}