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
    public partial class TermPage : ContentPage
    {
        public TermPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
                {
                    conn.CreateTable<Term>();
                    List<Term> termList = conn.Table<Term>().ToList();
                    termListView.ItemsSource = termList;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("exception caught", ex.Message, "ok");
            }
        }

        private void AddTermBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewTermPage());
        }

        private void termListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Term selectedTerm = (Term)termListView.SelectedItem;

            if (selectedTerm != null)
            {
                Navigation.PushAsync(new TermDetailPage(selectedTerm));
            }
        }
    }
}