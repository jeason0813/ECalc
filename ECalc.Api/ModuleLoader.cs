﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Api
{
    /// <summary>
    /// Module loader class to load modules
    /// </summary>
    public class ModuleLoader
    {
        private List<EcalcModule> _modules;

        /// <summary>
        /// Creates a new Instance of ModuleLoaer
        /// </summary>
        public ModuleLoader()
        {
            _modules = new List<EcalcModule>();
        }

        /// <summary>
        /// Loads Modules from an assembly file
        /// </summary>
        /// <param name="assembly">assembly to load from</param>
        public void LoadFromAssembly(Assembly assembly)
        {
            try
            {
                var modules = from t in Assembly.GetExecutingAssembly().GetTypes()
                              where
                              t.IsClass &&
                              t.BaseType == typeof(EcalcModule)
                              select t;

                foreach (var module in modules)
                {
                    var mod = (EcalcModule)Activator.CreateInstance(module);
                    _modules.Add(mod);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Runs a module by it's name
        /// </summary>
        /// <param name="name">Module name to run</param>
        /// <returns>A module's user control</returns>
        public UserControl RunByName(string name)
        {
            var q = from module in _modules where module.ModuleName == name select module;
            var x = q.FirstOrDefault();
            if (x != null) return x.GetControl();
            else return null;
        }
    }
}
