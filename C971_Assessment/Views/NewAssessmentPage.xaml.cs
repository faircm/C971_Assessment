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
    public partial class NewAssessmentPage : ContentPage
    {
        public NewAssessmentPage()
        {
            InitializeComponent();
            assessmentType.ItemsSource = PickerOptions.typeOptions;
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            if (DateUtils.startBeforeEnd(startDate.Date, endDate.Date) && DateUtils.isInDateRange(dueDate.Date, startDate.Date, endDate.Date))
            {
                if (assessmentTitle.Text.Length == 0)
                {
                    DisplayAlert("Failure", "Assessment must have a title.", "Ok");
                    return;
                }

                Assessment newAssessment = new Assessment();
                newAssessment.Title = assessmentTitle.Text;
                newAssessment.StartDate = startDate.Date;
                newAssessment.EndDate = endDate.Date;
                newAssessment.DueDate = dueDate.Date;
                newAssessment.Type = PickerOptions.typeOptions[assessmentType.SelectedIndex];
                newAssessment.NotificationsOn = notificationSwitch.IsToggled;

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Assessment>();
                    if (conn.Insert(newAssessment) > 0)
                    {
                        DisplayAlert("Success", "Assessment added successfully.", "Ok");
                        Navigation.PopAsync();
                    }
                    else
                    {
                        DisplayAlert("Failure", "Assessment could not be added", "Ok");
                    }
                }
            }
            else if (!DateUtils.startBeforeEnd(startDate.Date, endDate.Date))
            {
                DisplayAlert("Failure", "The assessment start date must occur before end date.", "Ok");
                return;
            }
            else if (!DateUtils.isInDateRange(dueDate.Date, startDate.Date, endDate.Date))
            {
                DisplayAlert("Failure", "The assessment due date must occur within the start and end dates.", "Ok");
                return;
            }
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}