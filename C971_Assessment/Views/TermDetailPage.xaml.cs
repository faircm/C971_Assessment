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
    public partial class TermDetailPage : ContentPage
    {
        private Term _selectedTerm;

        public TermDetailPage(Term selectedTerm)
        {
            InitializeComponent();
            _selectedTerm = selectedTerm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            termId.Text = _selectedTerm.Id.ToString();
            termTitle.Text = _selectedTerm.Title;
            termStart.Text = _selectedTerm.StartDate.ToString("MM/dd/yyyy");
            termEnd.Text = _selectedTerm.EndDate.ToString("MM/dd/yyyy");
        }

        private void editDetailsBtn_Clicked(object sender, EventArgs e)
        {
            Term termToEdit = new Term();
            termToEdit.Id = Int32.Parse(termId.Text);
            termToEdit.Title = termTitle.Text;
            termToEdit.StartDate = DateTime.Parse(termStart.Text);
            termToEdit.EndDate = DateTime.Parse(termEnd.Text);
            Navigation.PushAsync(new EditTermPage(termToEdit));
        }

        private void deleteBtn_Clicked(object sender, EventArgs e)
        {
            _selectedTerm.Title = termTitle.Text;
            _selectedTerm.StartDate = DateTime.Parse(termStart.Text);
            _selectedTerm.EndDate = DateTime.Parse(termEnd.Text);
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Term>();
                if (conn.Delete(_selectedTerm) > 0)
                {
                    DisplayAlert("Success", "Term deleted successfully.", "Ok");
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Failure", "Term could not be deleted", "Ok");
                }
            }
        }

        private void viewCoursesBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CoursePage(_selectedTerm));
        }
    }
}