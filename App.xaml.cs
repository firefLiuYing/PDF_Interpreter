﻿using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace PdfInterpreter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); 
        }
        protected override void OnExit(ExitEventArgs e)
        {
            MyDebug.Close();
            base.OnExit(e);
        }
    }

}
