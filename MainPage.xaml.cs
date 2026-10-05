using CalculateurAge.ViewModels;

namespace CalculateurAge;

public partial class MainPage : ContentPage
{
    // Le ViewModel est injecté (partagé avec ResultatPage).
    public MainPage(CalculateurViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
