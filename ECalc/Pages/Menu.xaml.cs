﻿using ECalc.Classes;
using ECalc.Controls;
using ECalc.Docs;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            var w = new AboutDialog();
            MainWindow.ShowDialog(w);
        }

        private void BtnIssue_Click(object sender, RoutedEventArgs e)
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "https://github.com/webmaster442/ECalc/issues";
            p.Start();
        }

        private void BtnScreenKeyboard_Click(object sender, RoutedEventArgs e)
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "osk.exe";
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        private void BtnDocs_Click(object sender, RoutedEventArgs e)
        {
            var dv = new DocumentationViewer();
            MainWindow.SwithToControl(dv);
        }

        private void BtnDonate_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=AVUW7CAHC2LES");
        }

        private void BtnWindManager_Click(object sender, RoutedEventArgs e)
        {
            var wm = new WindowsManager();
            MainWindow.SwithToControl(wm);
        }

        private void BtnChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            App.NextTheme();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Updater.DoUpdate();
        }
    }
}
