using HypnosisApp.UI.Views;

namespace HypnosisApp.UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute("SessionPlayerPage", typeof(SessionPlayerPage));
    }
}
