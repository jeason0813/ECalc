﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.Classes
{
    /// <summary>
    /// Based on: https://github.com/robertgreiner/NumberText/blob/master/NumberText/NumberText.cs
    /// </summary>
    internal class NumberText
    {

        private Dictionary<long, string> textStrings = new Dictionary<long, string>();
        private Dictionary<long, string> scales = new Dictionary<long, string>();
        private StringBuilder builder;

        public NumberText()
        {
            Initialize();
        }

        /// <summary>
        /// Convert a number to text
        /// </summary>
        /// <param name="num">number to convert</param>
        /// <returns>returns a string representaion of the number</returns>
        public string ToText(long num)
        {
            try
            {
                builder = new StringBuilder();

                if (num == 0)
                {
                    builder.Append(textStrings[num]);
                    return builder.ToString();
                }

                num = scales.Aggregate(num, (current, scale) => Append(current, scale.Key));
                AppendLessThanOneThousand(num);

                return builder.ToString().Trim();
            }
            catch (Exception)
            {
                return "Number is too large to convert it to text";
            }
        }

        /// <summary>
        /// Convert a number to text
        /// </summary>
        /// <param name="num">number to convert</param>
        /// <returns>returns a string representaion of the number</returns>
        public string ToText(double num)
        {
            builder = new StringBuilder();

            if (num == 0)
            {
                builder.Append(textStrings[(long)num]);
                return builder.ToString();
            }
            else if (double.IsNaN(num)) return "Not a Number";
            else if (double.IsInfinity(num)) return "Infinity";
            else if (num - Math.Truncate(num) == 0) return ToText((long)num);
            else
            {
                string text = num.ToString();
                var parts = text.Split(Engine.DecimalSeperator[0]);
                long integer = Convert.ToInt64(parts[0]);
                long floating = Convert.ToInt64(parts[1]);

                return ToText(integer) + " point " + ToText(floating);
            }
        }

        private long Append(long num, long scale)
        {
            if (num > scale - 1)
            {
                var baseScale = ((long)(num / scale));
                AppendLessThanOneThousand(baseScale);
                builder.AppendFormat("{0} ", scales[scale]);
                num = num - (baseScale * scale);
            }
            return num;
        }

        private long AppendLessThanOneThousand(long num)
        {
            num = AppendHundreds(num);
            num = AppendTens(num);
            AppendUnits(num);
            return num;
        }

        private void AppendUnits(long num)
        {
            if (num > 0)
            {
                builder.AppendFormat("{0} ", textStrings[num]);
            }
        }

        private long AppendTens(long num)
        {
            if (num > 20)
            {
                var tens = ((int)(num / 10)) * 10;
                builder.AppendFormat("{0} ", textStrings[tens]);
                num = num - tens;
            }
            return num;
        }

        private long AppendHundreds(long num)
        {
            if (num > 99)
            {
                var hundreds = ((int)(num / 100));
                builder.AppendFormat("{0} hundred ", textStrings[hundreds]);
                num = num - (hundreds * 100);
            }
            return num;
        }

        protected virtual void Initialize()
        {
            textStrings.Add(0, "zero");
            textStrings.Add(1, "one");
            textStrings.Add(2, "two");
            textStrings.Add(3, "three");
            textStrings.Add(4, "four");
            textStrings.Add(5, "five");
            textStrings.Add(6, "six");
            textStrings.Add(7, "seven");
            textStrings.Add(8, "eight");
            textStrings.Add(9, "nine");
            textStrings.Add(10, "ten");
            textStrings.Add(11, "eleven");
            textStrings.Add(12, "twelve");
            textStrings.Add(13, "thirteen");
            textStrings.Add(14, "fourteen");
            textStrings.Add(15, "fifteen");
            textStrings.Add(16, "sixteen");
            textStrings.Add(17, "seventeen");
            textStrings.Add(18, "eighteen");
            textStrings.Add(19, "nineteen");
            textStrings.Add(20, "twenty");
            textStrings.Add(30, "thirty");
            textStrings.Add(40, "forty");
            textStrings.Add(50, "fifty");
            textStrings.Add(60, "sixty");
            textStrings.Add(70, "seventy");
            textStrings.Add(80, "eighty");
            textStrings.Add(90, "ninety");
            textStrings.Add(100, "hundred");
            scales.Add(1000000000, "billion");
            scales.Add(1000000000000, "trillion");
            scales.Add(1000000000000000, "quadrillion");
            scales.Add(1000000000000000000, "quintillion");
            scales.Add(1000000, "million");
            scales.Add(1000, "thousand");
        }
    }
}
