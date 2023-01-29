using System.Windows;

namespace HiBiKiRadioTool.Launcher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataModelView.AllPrograms.Refresh();
        this.lvProgramList.ItemsSource = DataModelView.AllPrograms;
    }
}
