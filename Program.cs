using System;
using System.Windows.Forms;

namespace ImagePixelDemo
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm()); // استدعاء الفورم اللي كتبناه
        }
    }
}