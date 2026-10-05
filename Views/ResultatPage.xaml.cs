using CalculateurAge.ViewModels;

namespace CalculateurAge.Views;

// Plus de QueryProperty : la page lit le même ViewModel que MainPage.
public partial class ResultatPage : ContentPage
{
    public ResultatPage(CalculateurViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
