using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.UserSkins;

namespace LawDictionary
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var asm = typeof (SkinOffice).Assembly;
            SkinManager.Default.RegisterAssembly(asm);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }


    public class SkinRegistration : Component
    {
        public SkinRegistration()
        {
            SkinManager.Default.RegisterAssembly(typeof (SkinOffice).Assembly);
        }
    }
}