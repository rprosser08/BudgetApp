using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BudgetApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenWizard_Click(object sender, RoutedEventArgs e)
    {
        var wizard = new AccountWindow();
        wizard.ShowDialog(this);
    }
}
