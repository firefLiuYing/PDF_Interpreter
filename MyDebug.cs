using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PDF_Interpreter
{
    internal static class MyDebug
    {
#if DEBUG
        private static DebugWindow debugWindow;
#endif
        public static void Show(Window owner)
        {
#if DEBUG
            debugWindow = new();
            debugWindow.Owner = owner;
            debugWindow.Show();
#endif
        }
        public static void Log(string message)
        {
#if DEBUG
            debugWindow?.Log(message);
#endif
            Debug.WriteLine(message);
        }
        public static void Close()
        {
#if DEBUG
            debugWindow?.Close();
#endif
        }
    }
}
