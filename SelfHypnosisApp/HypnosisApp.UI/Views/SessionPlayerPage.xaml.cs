using HypnosisApp.UI.ViewModels;

namespace HypnosisApp.UI.Views;

public partial class SessionPlayerPage : ContentPage
{
    public SessionPlayerPage(SessionPlayerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
