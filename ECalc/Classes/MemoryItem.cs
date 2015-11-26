﻿using ECalc.Maths;
using System;
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
        Vector
    }

    /// <summary>
    /// Used in memory management
    /// </summary>
    [Serializable]
    public class MemoryItem: IXmlSerializable
    {
        /// <summary>
        /// Register counter
        /// </summary>
        private static int Counter;

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
        public string Name { get; private set; }

        /// <summary>
        /// Variable value
        /// </summary>
        public object Value { get; set; }

        public MemoryItem()
        {
            Name = String.Format("$Reg_{0}", Counter);
            Value = null;
            Counter++;
        }

        public MemoryItem(object val)
        {
            Name = String.Format("$Reg_{0}", Counter);
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
            return Name.GetHashCode() ^ Value.GetHashCode();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Name = reader.GetAttribute("Name");
            var typestring = reader.GetAttribute("Type");
            var type = (VarType)Enum.Parse(typeof(VarType), typestring);
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
                    case VarType.Vector:
                        throw new NotImplementedException();
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
            else if (valtype == typeof(DoubleMatrix)) type = VarType.Matrix;
            else if (valtype == typeof(Vector)) type = VarType.Vector;

            writer.WriteAttributeString("Type", type.ToString());

            string xml = null;
            switch (type)
            {
                case VarType.Double:
                    xml = ((Double)Value).ToString("G17", culture);
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
                    StringBuilder sb = new StringBuilder();
                    DoubleMatrix m = ((DoubleMatrix)Value);
                    for (int row=0; row<m.Rows; row++)
                    {
                        sb.Append("[");
                        for (int column=0; column<m.Columns; column++)
                        {
                            sb.AppendFormat("{0};", m[row, column].ToString("G17", culture));
                        }
                        sb.Append("]\r\n");
                    }
                    writer.WriteElementString("Content", sb.ToString());
                    break;
                case VarType.Vector:
                    throw new NotImplementedException();
            }
        }
    }
}