using C971_Assessment.Models;
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
            termTitle.Text = selectedTerm.Title;
            termStart.Text = selectedTerm.StartDate.ToString();
            termEnd.Text = selectedTerm.EndDate.ToString();
        }

        private void editDetailsBtn_Clicked(object sender, EventArgs e)
        {
            Term currentTerm = new Term(termTitle.Text, DateTime.Parse(termStart.Text), DateTime.Parse(termEnd.Text));
            Navigation.PushAsync(new EditTermPage(currentTerm));
        }
    }
}