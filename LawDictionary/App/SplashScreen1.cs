using System;
using System.Drawing;
using DevExpress.XtraSplashScreen;

namespace LawDictionary
{
    public partial class SplashScreen1 : SplashScreen
    {
        public enum SplashScreenCommand
        {
        }

        public SplashScreen1()
        {
            InitializeComponent();
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            pictureBox1.Image = new Bitmap(dir + "Images\\ss.jpg");
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion
    }
}