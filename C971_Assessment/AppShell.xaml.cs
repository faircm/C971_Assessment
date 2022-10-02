using C971_Assessment.Views;
using Xamarin.Forms;

namespace C971_Assessment
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(TermDetailPage), typeof(TermDetailPage));
            Routing.RegisterRoute(nameof(NewTermPage), typeof(NewTermPage));
        }
    }
}