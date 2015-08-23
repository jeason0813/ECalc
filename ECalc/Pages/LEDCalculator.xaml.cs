﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for LEDCalculator.xaml
    /// </summary>
    public partial class LEDCalculator : UserControl
    {
        private bool _loaded;

        public LEDCalculator()
        {
            InitializeComponent();
        }

        public void Calculate()
        {
            if (!_loaded) return;
            try
            {
                double resistor = 0;
                double power = 0;
                switch (TbModeSelector.SelectedIndex)
                {
                    case 0:
                        if (EsLEDNumber.Value * EsForwardVoltage.Value > PiInputVolt.Value)
                        {
                            TbResistorPower.Text = "0";
                            TbResitorValue.Text = "Input voltage is not enough to drive the LEDs";
                        }
                        else
                        {
                            resistor = (PiInputVolt.Value - (EsLEDNumber.Value * EsForwardVoltage.Value)) / PiForwardCurrent.Value;
                            power = PiForwardCurrent.Value * PiForwardCurrent.Value * resistor;
                            TbResistorPower.Text = power.ToString();
                            TbResitorValue.Text = resistor.ToString();
                        }
                        break;
                    case 1:
                        if (EsForwardVoltage.Value > PiInputVolt.Value)
                        {
                            TbResistorPower.Text = "0";
                            TbResitorValue.Text = "Input voltage is not enough to drive the LEDs";
                        }
                        else
                        {
                            resistor = PiInputVolt.Value / (EsLEDNumber.Value * PiForwardCurrent.Value);
                            power = PiForwardCurrent.Value * PiForwardCurrent.Value * resistor;
                            TbResistorPower.Text = power.ToString();
                            TbResitorValue.Text = resistor.ToString();
                        }
                        break;
                }
            }
            catch (Exception)
            {
                TbResistorPower.Text = "0";
                TbResitorValue.Text = "Error";
            }
        }

        private void PrefixInput_ValueChanged(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void EditableSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
        }

        private void TbModeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }
    }
}
