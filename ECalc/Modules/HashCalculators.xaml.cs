﻿using ECalc.Engineering;
using System;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for HashCalculators.xaml
    /// </summary>
    public partial class HashCalculators : UserControl
    {
        private bool _loaded;

        public HashCalculators()
        {
            InitializeComponent();
        }

        private async void ComputeHash()
        {
            if (!_loaded) return;
            var selected = (CbHash.Items[CbHash.SelectedIndex] as ComboBoxItem).Content.ToString();
            HashFunctions.Algorithms algo = HashFunctions.Algorithms.MD5;
            bool convresult = Enum.TryParse<HashFunctions.Algorithms>(selected, out algo);
            if (!convresult) algo = HashFunctions.Algorithms.MD5;
           
            string result = await HashFunctions.HashString(algo, TbInput.Text);
            TbOutput.Text = result;
        }

        private void TbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComputeHash();
        }

        private void CbHash_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComputeHash();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _loaded = true;
        }
    }
}
