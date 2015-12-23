﻿using ECalc.Api;
using System.Windows.Controls;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ECalc.Modules
{
    public class HashCalc : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "IT Tools"; }
        }

        public override string ModuleName
        {
            get { return "Hash Calculator"; }
        }

        public override UserControl GetControl()
        {
            return new ECalc.Modules.HashCalculators();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/hash.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Olive; }
        }
    }

    public class LEDCalc : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Analog Electronics"; }
        }

        public override string ModuleName
        {
            get { return "LED Calculator"; }
        }

        public override UserControl GetControl()
        {
            return new ECalc.Modules.LEDCalculator();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/led.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Brown; }
        }
    }

    public class LogicMinimizer : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Digital Electrinics"; }
        }

        public override string ModuleName
        {
            get { return "Logic Minimizer"; }
        }

        public override UserControl GetControl()
        {
            return new ECalc.Modules.LogicFunctionMinimizer();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/electronics.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Lime; }
        }
    }

    public class NumSys : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "IT Tools"; }
        }

        public override string ModuleName
        {
            get { return "Number System converter"; }
        }

        public override UserControl GetControl()
        {
            return new ECalc.Modules.NumberSystems();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/numsystems.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8GAmber; }
        }
    }

    public class Opamps : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Analog Electronics"; }
        }

        public override string ModuleName
        {
            get { return "OpAmp Calculator"; }
        }

        public override UserControl GetControl()
        {
            return new ECalc.Modules.OpAmp();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/opamp.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Crimson; }
        }
    }

    public class ResistiorColor : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Analog Electronics"; }
        }

        public override string ModuleName
        {
            get { return "Resistor Color"; }
        }

        public override UserControl GetControl()
        {
            return new ECalc.Modules.ResistorColorDecoder();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/resistor.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Mauve; }
        }
    }

    public class ResistorSolv : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Analog Electronics"; }
        }

        public override string ModuleName
        {
            get { return "Resistor Value Solver"; }
        }

        public override UserControl GetControl()
        {
            return new ECalc.Modules.ResistorSolver();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/resistor.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Violet; }
        }
    }


    public class VoltageDiv : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Analog Electronics"; }
        }

        public override string ModuleName
        {
            get { return "Voltage & Current divider"; }
        }

        public override UserControl GetControl()
        {
            return new ECalc.Modules.VoltageCurrentDivider();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/electro.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Emerald; }
        }
    }

    public class IPsubnet : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "IT Tools"; }
        }

        public override string ModuleName
        {
            get { return "IP Subnet Calculator"; }
        }

        public override UserControl GetControl()
        {
            return new ECalc.Modules.SubnetCalculator();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/ipaddress.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Magenta; }
        }
    }

    public class Seg714 : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Digital Electrinics"; }
        }

        public override string ModuleName
        {
            get { return "7/14 segment display calculator"; }
        }

        public override UserControl GetControl()
        {
            return new Segment714Calculator();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/segments.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Brown; }
        }
    }

    public class Colorspaces : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "IT Tools"; }
        }

        public override string ModuleName
        {
            get { return "Color Space converter"; }
        }

        public override UserControl GetControl()
        {
            return new ColorConverters();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/colorconv.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Cyan; }
        }
    }

    public class ne555tool : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Analog Electronics"; }
        }

        public override string ModuleName
        {
            get { return "555 timer tool"; }
        }

        public override UserControl GetControl()
        {
            return new ne555Calc();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/ne555.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Pink; }
        }
    }

    public class CuttingSpeedCalc : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Mechanics"; }
        }

        public override string ModuleName
        {
            get { return "Cutting speed calculator"; }
        }

        public override UserControl GetControl()
        {
            return new CuttingSpeedCalculator();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/cutspeed.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Teal; }
        }
    }

    public class TrigTool : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "Mechanics"; }
        }

        public override string ModuleName
        {
            get { return "Trigonometry"; }
        }

        public override UserControl GetControl()
        {
            return new Trigonometry();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/trigonometry.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8DarkOrange; }
        }
    }

    public class Randoms : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "IT Tools"; }
        }

        public override string ModuleName
        {
            get { return "Random Generators"; }
        }

        public override UserControl GetControl()
        {
            return new RandomGenerators();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/dice.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get
            {
                Random r = new Random();
                return r.Next(0, 0xc0c0c0);
            }
        }
    }

    public class DateCalcs : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "IT Tools"; }
        }

        public override string ModuleName
        {
            get { return "Date & Time Calculators"; }
        }

        public override UserControl GetControl()
        {
            return new DateTimeCalc();
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("/ECalc;component/Images/100px/date.png", UriKind.Relative)); }
        }

        public override int Color
        {
            get { return (int)TileColor.W8Steel; }
        }
    }

    public class MediaCalc : EcalcModule
    {
        public override string ModuleCategory
        {
            get { return "IT Tools"; }
        }

        public override string ModuleName
        {
            get { return "Media Calculators"; }
        }

        public override UserControl GetControl()
        {
            return new MediaCalculator();
        }
    }
}
