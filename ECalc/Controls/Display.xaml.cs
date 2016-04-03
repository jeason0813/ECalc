﻿using ECalc.Classes;
using ECalc.IronPythonEngine;
using ECalc.IronPythonEngine.Types;
using ECalc.Maths;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    internal partial class Display : UserControl
    {
        private ObservableCollection<string> _history;

        public Display()
        {
            InitializeComponent();
            _history = new ObservableCollection<string>();
            HistoryContext.ItemsSource = _history;
        }

        /// <summary>
        /// Occures whern enter is pressed in the editor
        /// </summary>
        public event RoutedEventHandler ExecuteRequested;

        /// <summary>
        /// Calculator mode changed event
        /// </summary>
        public event StringEventHandler ModeChanged;

        public static readonly DependencyProperty EquationTextProperty = DependencyProperty.Register("EquationText", typeof(string), typeof(Display), new PropertyMetadata(""));

        public static readonly DependencyProperty ResultTextProperty = DependencyProperty.Register("ResultText", typeof(string), typeof(Display), new PropertyMetadata("0"));

        /// <summary>
        /// Equation Text
        /// </summary>
        public string EquationText
        {
            get { return (string)GetValue(EquationTextProperty); }
            set { SetValue(EquationTextProperty, value); }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && HistoryText.ContextMenu.IsOpen)
            {
                var value = ((TextBlock)e.Source).DataContext;
                EquationText = value.ToString();
            }
        }

        private void HistoryText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HistoryText.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Result Text
        /// </summary>
        public string ResultText
        {
            get { return (string)GetValue(ResultTextProperty); }
            set
            {
                if (value.Contains("\n")) MainDisplay.FontSize = 20;
                else MainDisplay.FontSize = 40;
                SetValue(ResultTextProperty, value);
            }
        }

        /// <summary>
        /// Add current Equation text to the history
        /// </summary>
        public void AddToHistory()
        {
            int index = _history.IndexOf(EquationText);
            if (index > -1) _history.RemoveAt(index);
            _history.Add(EquationText);
            if (_history.Count > 15) _history.RemoveAt(0);
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (EquationText.Length - 1 > 0)
            {
                string s = EquationText.Substring(0, EquationText.Length - 1);
                EquationText = s;
            }
            else EquationText = "";
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            this.EquationText = "";
            this.ResultText = "0";
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModeChanged != null)
            {
                var text = ((RadioButton)sender).Content.ToString();
                ModeChanged(sender, new StringEventArgs(text));
            }
        }

        private void BtnNumSys_Click(object sender, RoutedEventArgs e)
        {
            var nd = new NumberSystemDisplayDialog();
            nd.SetDisplay(Engine.Ans);
            MainWindow.ShowDialog(nd);
        }

        private void BtnNumToText_Click(object sender, RoutedEventArgs e)
        {
            var ntd = new NumberToTextDialog();
            ntd.SetNumber(Engine.Ans);
            MainWindow.ShowDialog(ntd);
        }

        private void BtnFractions_Click(object sender, RoutedEventArgs e)
        {
            string message = "Complex, Vector, Fraction and Matrix values not supported";
            try
            {
                if (!Helpers.IsSpecialType(Engine.Ans))
                {
                    var f = new Fraction((double)Engine.Ans);
                    message = f.ToString();
                }
            }
            catch (Exception)
            {
                message = "Conversion is not possible";
            }
            MainWindow.ShowDialog("Result as Fraction", message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
        }

        private void BtnNumTest_Click(object sender, RoutedEventArgs e)
        {
            string message = "Complex, Vector, Fraction and Matrix values not supported";
            if (!Helpers.IsSpecialType(Engine.Ans))
            {
                var tester = new NumberTester();
                tester.Test(Engine.Ans);
                message = tester.ToString();
            }
            MainWindow.ShowDialog("Number informations", message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
        }

        private void CbPrefixDisplay_Checked(object sender, RoutedEventArgs e)
        {
            Engine.PreferPrefixes = (bool)CbPrefixDisplay.IsChecked;
        }

        private void CbThousandGrouping_Checked(object sender, RoutedEventArgs e)
        {
            Engine.GroupByThousands = (bool)CbThousandGrouping.IsChecked;
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            TbEditor.Focus();
        }

        private void TbEditor_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (ExecuteRequested != null) ExecuteRequested(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }

        private void BtnClipboardCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ResultText);
        }

        private void BtnClipboardPaste_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText()) EquationText += Clipboard.GetText();
            else MainWindow.ErrorDialog("Clipboard doesn't contain valid text to be inserted");
        }

        private void BtnAnss_Click(object sender, RoutedEventArgs e)
        {
            EquationText += "Var('ans')";
        }

        private void BtnPlot_Click(object sender, RoutedEventArgs e)
        {
            var plot = new Pages.Graphing();
            plot.FunctionY = TbEditor.Text;
            MainWindow.SwithToControl(plot);
        }

        private void BtnFileSize_Click(object sender, RoutedEventArgs e)
        {
            string message = "Complex, Vector, Fraction and Matrix values not supported";
            if (!Helpers.IsSpecialType(Engine.Ans))
            {
                double x = Helpers.GetDouble(Engine.Ans);
                message = Helpers.DivideToFileSize(x);
            }
            MainWindow.ShowDialog("Resut as File size", message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
        }
    }
}
