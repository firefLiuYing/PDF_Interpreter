﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PdfInterpreter
{
    /// <summary>
    /// DebugWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            InitializeComponent();
        }
        public void Log(string message)
        {
            Dispatcher.Invoke(() =>
            {
                DebugOutput.AppendText($"[{DateTime.Now:HH:mm:ss.fff}] {message}\n");
                DebugOutput.ScrollToEnd();
            });
        }
    }
}
