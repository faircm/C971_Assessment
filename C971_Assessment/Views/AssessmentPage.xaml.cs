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
    public partial class AssessmentPage : ContentPage
    {
        private int _courseId;

        public AssessmentPage(int courseId)
        {
            InitializeComponent();
            _courseId = courseId;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Assessment>();
                    List<Assessment> assessmentList = conn.Table<Assessment>().ToList();

                    for (int i = 0; i < assessmentList.Count; i++)
                    {
                        if (assessmentList[i].CourseID != _courseId)
                        {
                            assessmentList.RemoveAt(i);
                        }
                    }
                    assessmentListView.ItemsSource = assessmentList;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Database Connection Error", ex.Message, "ok");
            }
        }

        private void assessmentListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Assessment selectedAssessment = (Assessment)assessmentListView.SelectedItem;

            if (selectedAssessment != null)
            {
                Navigation.PushAsync(new AssessmentDetailPage(selectedAssessment));
            }
        }

        private void AddAssessmentBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewAssessmentPage(_courseId));
        }
    }
}