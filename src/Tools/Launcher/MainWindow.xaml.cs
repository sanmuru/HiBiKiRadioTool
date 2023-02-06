// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
