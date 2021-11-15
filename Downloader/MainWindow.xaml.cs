﻿using HiBikiRadioTool.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Downloader
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                ProgramListTask task = new ProgramListTask();
                var programs = task.Run();

                this.Dispatcher.BeginInvoke(new Action(() =>
                    this.lstProgram.ItemsSource = programs
                ));
            }));
        }
    }
}
