using System.Collections.ObjectModel;
using CalculateurAge.Views;

namespace CalculateurAge.ViewModels;

// Fonctionnalités ajoutées (Activité 6) :
//  1. Statut Majeur / Mineur
//  2. Commande Effacer (remet tout à zéro)
//  3. Refus d'une date de naissance future
//  4. Nombre de jours avant le prochain anniversaire
//  5. Historique des calculs
//  6. Navigation vers ResultatPage par le ViewModel (sans URL)
public class CalculateurViewModel : BaseViewModel
{
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private string _statut = "";
    private string _prochainAnniversaire = "";
    private string _erreur = "";
    private bool _resultatVisible;

    public string Nom
    {
        get => _nom;
        set { if (SetField(ref _nom, value)) CalculerCommand.Rafraichir(); }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set { if (SetField(ref _dateNaissance, value)) CalculerCommand.Rafraichir(); }
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    // Fonctionnalité 1
    public string Statut
    {
        get => _statut;
        set => SetField(ref _statut, value);
    }

    // Fonctionnalité 4
    public string ProchainAnniversaire
    {
        get => _prochainAnniversaire;
        set => SetField(ref _prochainAnniversaire, value);
    }

    // Fonctionnalité 3 : message d erreur affiché dans un Label
    // (pas de DisplayAlert dans le ViewModel).
    public string Erreur
    {
        get => _erreur;
        set => SetField(ref _erreur, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set { if (SetField(ref _resultatVisible, value)) VoirDetailCommand.Rafraichir(); }
    }

    // Fonctionnalité 5
    public ObservableCollection<string> Historique { get; } = new();

    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }
    public RelayCommand VoirDetailCommand { get; }
    public RelayCommand RetourCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(Calculer,
            () => !string.IsNullOrWhiteSpace(Nom));
        EffacerCommand = new RelayCommand(Effacer);
        VoirDetailCommand = new RelayCommand(
            async () => await Shell.Current.GoToAsync(nameof(ResultatPage)),
            () => ResultatVisible);
        RetourCommand = new RelayCommand(
            async () => await Shell.Current.GoToAsync(".."));
    }

    private void Calculer()
    {
        DateTime aujourdhui = DateTime.Today;

        // Fonctionnalité 3 : refus d'une date future
        if (DateNaissance.Date > aujourdhui)
        {
            Erreur = "La date de naissance ne peut pas être dans le futur.";
            ResultatVisible = false;
            return;
        }
        Erreur = "";

        int age = aujourdhui.Year - DateNaissance.Year;
        if (DateNaissance.Date > aujourdhui.AddYears(-age)) age--;

        Resultat = $"{Nom.Trim()}, vous avez {age} ans";

        // Fonctionnalité 1
        Statut = age >= 18 ? "Majeur" : "Mineur";

        // Fonctionnalité 4 : le prochain anniversaire est celui des (age + 1) ans
        DateTime prochain = DateNaissance.Date.AddYears(age + 1);
        int jours = (prochain - aujourdhui).Days;
        ProchainAnniversaire = DateNaissance.Date.AddYears(age) == aujourdhui
            ? $"Joyeux anniversaire ! Prochain dans {jours} jours."
            : $"Prochain anniversaire dans {jours} jour(s).";

        // Fonctionnalité 5 : le plus récent en tête
        Historique.Insert(0,
            $"{Nom.Trim()} - {age} ans ({DateNaissance:dd/MM/yyyy})");

        ResultatVisible = true;
    }

    // Fonctionnalité 2
    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        Statut = "";
        ProchainAnniversaire = "";
        Erreur = "";
        ResultatVisible = false;
    }
}
