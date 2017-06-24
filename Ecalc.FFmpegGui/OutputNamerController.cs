﻿using AppLib.WPF.MVVM;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Ecalc.FFmpegGui
{
    public class OutputNamerController : ViewModel
    {
        private string _renamepattern;
        private string _extension;
        private string _search;
        private string _replace;
        private int _counterstart;
        private int _counterincrement;
        private int _couterpadding;
        private bool _regex;
        private string _OutputDir;

        public OutputNamerController()
        {
            RenamePattern = "[N]";
            Extension = "[E]";
            CounterStart = 1;
            CounterIncrement = 1;
            CounterPadding = 1;
            Regex = false;
            Inputs = new ObservableCollection<string>();
            Outputs = new ObservableCollection<string>();
        }

        private void ReplaceIfNeeded(ref string s, string pattern, object value, bool regex = false)
        {
            if (s.Contains(pattern) && !regex)
            {
                s = s.Replace(pattern, value.ToString());
            }
            else if (regex)
            {
                s = System.Text.RegularExpressions.Regex.Replace(s, pattern, value.ToString());
            }
        }

        private void Transform()
        {
            Outputs.Clear();
            var now = DateTime.Now;
            var counter = CounterStart;
            foreach (var input in Inputs)
            {

                var name = Path.GetFileNameWithoutExtension(input);
                var ext = Path.GetExtension(input);
                var output = Path.Combine(OutputDirectory, RenamePattern + "." + Extension);
                ReplaceIfNeeded(ref output, "[N]", name);
                ReplaceIfNeeded(ref output, "[Y]", now.Year);
                ReplaceIfNeeded(ref output, "[M]", now.Month);
                ReplaceIfNeeded(ref output, "[D]", now.Day);
                ReplaceIfNeeded(ref output, "[h]", now.Hour);
                ReplaceIfNeeded(ref output, "[m]", now.Minute);
                ReplaceIfNeeded(ref output, "[s]", now.Second);
                ReplaceIfNeeded(ref output, "[E]", ext);
                ReplaceIfNeeded(ref output, "[C]", counter.ToString().PadLeft(CounterPadding, '0'));
                ReplaceIfNeeded(ref output, Search, Replace, Regex);
                counter += CounterIncrement;
            }
        }

        public string RenamePattern
        {
            get { return _renamepattern; }
            set
            {
                if (SetValue(ref _renamepattern, value))
                    Transform();
            }
        }

        public string Extension
        {
            get { return _extension; }
            set
            {
                if (SetValue(ref _extension, value))
                    Transform();
            }
        }

        public int CounterStart
        {
            get { return _counterstart; }
            set
            {
                if (SetValue(ref _counterstart, value))
                    Transform();
            }
        }

        public int CounterIncrement
        {
            get { return _counterincrement; }
            set
            {
                if (SetValue(ref _counterincrement, value))
                    Transform();
            }
        }

        public int CounterPadding
        {
            get { return _couterpadding; }
            set
            {
                if (SetValue(ref _couterpadding, value))
                    Transform();
            }
        }

        public bool Regex
        {
            get { return _regex; }
            set
            {
                if (SetValue(ref _regex, value))
                    Transform();
            }
        }

        public string Search
        {
            get { return _search; }
            set
            {
                if (SetValue(ref _search, value))
                    Transform();
            }
        }

        public string Replace
        {
            get { return _replace; }
            set
            {
                if (SetValue(ref _replace, value))
                    Transform();
            }
        }

        public string OutputDirectory
        {
            get { return _OutputDir; }
            set
            {
                if (SetValue(ref _OutputDir, value))
                    Transform();
            }
        }


        public ObservableCollection<string> Inputs
        {
            get;
            private set;
        }

        public ObservableCollection<string> Outputs
        {
            get;
            private set;
        }
    }
}
