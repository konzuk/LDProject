using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using LawDictionary;
using Microsoft.Win32;

namespace LDAdminLuncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        private static string RoleKey = "P@ssw0rd";
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
            string[] version_names = installed_versions.GetSubKeyNames();
            //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
            double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
            var dir = AppDomain.CurrentDomain.BaseDirectory;

            //BinaryFileHelper.WriteToBinaryFile(dir + "Application\\RoleKey.dll", RoleKey);

            if (Framework < 4)
            {
                var result = MessageBox.Show("Please install .Net Framework 4.0. \nClick \"Yes\" to begin the installation. \n\n\nNote: After the installation done, please restart your computer and start the program again.", "Required .Net Framework 4.0",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Process.Start(dir + "Application\\dotNetFx40_Full_x86_x64.exe");
                    }
                    catch 
                    {
                    }
                   
                }
            }
            else
            {
                Process.Start(dir + "Application\\LawDictionary.exe");
            }
        }
}
}
