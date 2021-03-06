﻿using ECalc.Classes;
using ECalc.Controls;
using ECalc.ExcelInterop;
using ECalc.IronPythonEngine;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppLib.Common.MessageHandler;
using System.Collections.Generic;
using AppLib.Common;
using ECalc.IronPythonEngine.Types;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class Calculator : UserControl, IDisposable, IMessageClient<List<double>>, IMessageClient<double[,]>
    {
        private Engine _engine;
        private StringBuilder _stdout;
        private ExcelInteropControl _eip;

        public UId MessageReciverID
        {
            get;
            private set;
        }

        public Calculator()
        {
            InitializeComponent();
            MessageReciverID = new UId();
            Messager.Instance.SubScribe(this);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (designTime) return;
            _engine = new Engine();
            _engine.StdOutWriten += _engine_StdOutWriten;
            _engine.MemoryManager = Keypad;
            _engine.LoadUserFunctions();
            FncList.FillFunctionList(Engine.Functions);
            _stdout = new StringBuilder();
            _eip = new ExcelInteropControl();
            Display.Focus();
        }

        private async void KeyPad_ExecuteClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Display.IsCalculating = true;
                _stdout.Clear();
                var result = await _engine.EvaluateAsync(Display.EquationText);
                _engine.DisplayLastError();
                if (_stdout.Length > 0)
                {
                    var mld = new MultiLineResultDialog();
                    mld.Text = _stdout.ToString();
                    MainWindow.ShowDialog(mld);
                }
                Display.AddToHistory();
                if (!string.IsNullOrEmpty(result))
                {
                    Display.ResultText = result;
                }
                else Display.ResultText = "Completed";
                Display.IsCalculating = false;
            }
            catch (Exception ex)
            {
                Display.ResultText = "Error";
                Display.IsCalculating = false;
                MainWindow.ErrorDialog(ex.Message);
            }
            Display.EquationText = "";
        }

        private void _engine_StdOutWriten(object sender, MyEvtArgs<string> e)
        {
            _stdout.Append(e.Value);
        }

        private void KeyPad_ButtonClicked(object sender, StringEventArgs e)
        {
            if (Engine.IsOperator(e.Text))
                Display.InsertText(string.Format(" {0} ", e.Text));
            else
                Display.InsertText(e.Text);
        }

        private async void Keypad_FromExpressionClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                await _engine.EvaluateAsync(Display.EquationText);
                _engine.DisplayLastError();
                Keypad.SetItem(Engine.Ans);
                Display.EquationText = "";
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        public void SaveMemSession()
        {
            Keypad.MemMan.Hybernate();
        }

        private void Display_ModeChanged(object sender, StringEventArgs e)
        {
            TrigMode output;
            if (Enum.TryParse<TrigMode>(e.Text, out output))
            {
                Engine.Mode = output;
            }
        }

        private void Extended_BackClicked(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => { InputSelector.SelectedIndex = 0; });
        }

        public void FocusInput()
        {
            var ctrl = Keyboard.FocusedElement;
            if (ctrl is TextBox) return;
            else Display.FocusInput();
        }

        protected virtual void Dispose(bool direct)
        {
            if (_engine != null)
            {
                _engine.Dispose();
                _engine = null;
            }
            if (_eip != null)
            {
                ExcelInterop.ExcelInterop.Instance.ComCleanUp();
                _eip = null;
            }
        }

        private void FncList_FunctionButtonCliked(object sender, string e)
        {
            Display.InsertText(string.Format(" {0}( ", e));
            Dispatcher.Invoke(() => { InputSelector.SelectedIndex = 0; });
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void ExcelInterop_Click(object sender, RoutedEventArgs e)
        {
            _eip.Visibility = ExcelInteropBtn.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        }

        public void HandleMessage(List<double> message)
        {
            Set set = new Set(message);
            Keypad.MemMan.SetItem(set);
            Keypad.SwitchTo(KeyPad.Views.MemMan);
        }

        public void HandleMessage(double[,] message)
        {
            Matrix m = new Matrix(message);
            Keypad.MemMan.SetItem(m);
            Keypad.SwitchTo(KeyPad.Views.MemMan);
        }
    }
}
