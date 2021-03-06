﻿using ECalc.IronPythonEngine.Types;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ECalc.Classes
{
    [Serializable]
    public enum VarType
    {
        Double,
        Complex,
        Matrix,
        Fraction,
        Vector,
        Set,
        String,
        Time
    }

    /// <summary>
    /// Used in memory management
    /// </summary>
    [Serializable]
    public class MemoryItem: IXmlSerializable, INotifyPropertyChanged
    {
        /// <summary>
        /// Register counter
        /// </summary>
        private static int Counter;

        private object _object;

        private string _name;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static void ResetCounter()
        {
            Counter = 0;
        }

        static MemoryItem()
        {
            Counter = 0;
        }

        /// <summary>
        /// Variable name
        /// </summary>
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Variable value
        /// </summary>
        public object Value
        {
            get { return _object; }
            set
            {
                _object = value;
                OnPropertyChanged("Value");
            }
        }

        public string Type
        {
            get { return _object.GetType().Name; }
        }

        public MemoryItem()
        {
            Name = String.Format("Reg_{0}", Counter);
            Value = null;
            Counter++;
        }

        public MemoryItem(object val)
        {
            Name = String.Format("Reg_{0}", Counter);
            Value = val;
            Counter++;
        }

        public MemoryItem(string name, object val)
        {
            Value = val;
            Name = name;
        }

        public override int GetHashCode()
        {
            if (Value == null)
                return Name.GetHashCode();
            return Name.GetHashCode() ^ Value.GetHashCode();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            int rows = 0;
            int columns = 0;
            int items = 0;
            reader.MoveToContent();
            Name = reader.GetAttribute("Name");
            var typestring = reader.GetAttribute("Type");
            var type = (VarType)Enum.Parse(typeof(VarType), typestring);
            if (type == VarType.Matrix)
            {
                rows = Convert.ToInt32(reader.GetAttribute("Rows"));
                columns = Convert.ToInt32(reader.GetAttribute("Columns"));
            }
            if (type == VarType.Vector)
            {
                columns = Convert.ToInt32(reader.GetAttribute("Dimensions"));
            }
            if (type == VarType.Set)
            {
                items = Convert.ToInt32(reader.GetAttribute("Items"));
            }
            bool isempty = reader.IsEmptyElement;
            reader.ReadStartElement();

            if (!isempty)
            {
                var xml = reader.ReadElementContentAsString();
                string[] parts = null;
                var culture = new CultureInfo("en-US");

                switch (type)
                {
                    case VarType.Double:
                        Value = double.Parse(xml, culture);
                        break;
                    case VarType.Complex:
                        parts = xml.Split(';');
                        var r = double.Parse(parts[0], culture);
                        var i = double.Parse(parts[1], culture);
                        Value = new Complex(r, i);
                        break;
                    case VarType.Fraction:
                        parts = xml.Split(';');
                        var n = long.Parse(parts[0], culture);
                        var d = long.Parse(parts[1], culture);
                        Value = new Fraction(n, d);
                        break;
                    case VarType.Matrix:
                        var lines = xml.Split('\n'); // get lines
                        var matrix = new Matrix(rows, columns);
                        for (int row=0; row<rows; row++)
                        {
                            parts = lines[row].Replace("[", "").Replace("]", "").Split(';');
                            for (int column=0; column<columns; column++)
                            {
                                matrix[row, column] = double.Parse(parts[column], culture);
                            }
                        }
                        Value = matrix;
                        break;
                    case VarType.Set:
                        var contents = xml.Split(';');
                        var set = new Set(items);
                        for (int itm=0; itm<items; itm++)
                        {
                            set.Add(double.Parse(contents[itm]));
                        }
                        Value = set;
                        break;
                    case VarType.String:
                        Value = xml;
                        break;
                    case VarType.Vector:
                        parts = xml.Split(';');
                        var x = double.Parse(parts[0], culture);
                        var y = double.Parse(parts[1], culture);
                        if (columns == 3)
                        {
                            var z = double.Parse(parts[2], culture);
                            Value = new Vector(x, y, z);
                        }
                        else Value = new Vector(x, y);
                        break;
                    case VarType.Time:
                        Value = new Time(double.Parse(xml, culture));
                        break;
                }

                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", Name);

            var culture = new CultureInfo("en-US");

            VarType type = VarType.Double;

            var valtype = Value.GetType();

            if (valtype == typeof(double)) type = VarType.Double;
            else if (valtype == typeof(Complex)) type = VarType.Complex;
            else if (valtype == typeof(Fraction)) type = VarType.Fraction;
            else if (valtype == typeof(Matrix)) type = VarType.Matrix;
            else if (valtype == typeof(Vector)) type = VarType.Vector;
            else if (valtype == typeof(Set)) type = VarType.Set;
            else if (valtype == typeof(string)) type = VarType.String;
            else if (valtype == typeof(Time)) type = VarType.Time;

            writer.WriteAttributeString("Type", type.ToString());

            string xml = null;
            var sb = new StringBuilder();
            switch (type)
            {
                case VarType.Double:
                    xml = (Convert.ToDouble(Value)).ToString("G17", culture);
                    writer.WriteElementString("Content", xml);
                    break;
                case VarType.Complex:
                    var r = ((Complex)Value).Real.ToString("G17", culture);
                    var i = ((Complex)Value).Imaginary.ToString("G17", culture);
                    xml = string.Format("{0};{1}", r, i);
                    writer.WriteElementString("Content", xml);
                    break;
                case VarType.Fraction:
                    var n = ((Fraction)Value).Numerator.ToString(culture);
                    var d = ((Fraction)Value).Denominator.ToString(culture);
                    xml = string.Format("{0};{1}", n, d);
                    writer.WriteElementString("Content", xml);
                    break;
                case VarType.Matrix:
                    Matrix m = ((Matrix)Value);
                    writer.WriteAttributeString("Rows", m.Rows.ToString());
                    writer.WriteAttributeString("Columns", m.Columns.ToString());
                    for (int row=0; row<m.Rows; row++)
                    {
                        sb.Append("[");
                        for (int column=0; column<m.Columns; column++)
                        {
                            sb.AppendFormat("{0};", m[row, column].ToString("G17", culture));
                        }
                        sb.Append("]\n");
                    }
                    writer.WriteElementString("Content", sb.ToString());
                    break;
                case VarType.Vector:
                    var v = (Vector)Value;
                    writer.WriteAttributeString("Dimensions", v.Dimensions.ToString());
                    if (v.Dimensions == 2) xml = string.Format("{0};{1}", v.X.ToString("G17", culture),
                                                                          v.Y.ToString("G17", culture));
                    else xml = string.Format("{0};{1};{2}", v.X.ToString("G17", culture),
                                                            v.Y.ToString("G17", culture),
                                                            ((double)v.Z).ToString("G17", culture));
                    writer.WriteElementString("Content", xml);
                    break;
                case VarType.Set:
                    var s = (Set)Value;
                    writer.WriteAttributeString("Items", s.Count.ToString());
                    sb.Clear();
                    foreach (var item in s)
                    {
                        sb.AppendFormat("{0};", item.ToString("G17", culture));
                    }
                    writer.WriteElementString("Content", sb.ToString());
                    break;
                case VarType.String:
                    writer.WriteElementString("Content", Value.ToString());
                    break;
                case VarType.Time:
                    Time t = (Time)Value;
                    xml = (Convert.ToDouble(t.TotalSeconds).ToString("G17", culture));
                    writer.WriteElementString("Content", xml);
                    break;
            }
        }
    }
}
