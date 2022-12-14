using C971_Assessment.Resources;
using C971_Assessment.Views;
using Xamarin.Forms;

namespace C971_Assessment
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(TermPage), typeof(TermPage));
            Notifier.getAssessmentNotifications();
            Notifier.getCourseNotifications();
        }
    }
}