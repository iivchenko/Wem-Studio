﻿using System.Windows;

namespace WemManagementStudio.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window, IMainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        public void TestMethod()
        {
            throw new System.NotImplementedException();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var add = new AddMachineView();

            add.ShowDialog();
        }
    }
}