using System;
using System.Drawing;
using DevExpress.XtraSplashScreen;

namespace LawDictionary
{
    public partial class WaitFormView : SplashScreen
    {
        public enum SplashScreenCommand
        {
        }

        public WaitFormView()
        {
            InitializeComponent();
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            pictureBox1.Image = new Bitmap(dir + "Images\\about.jpg");
            LostFocus += WaitForm_LostFocus;
        }

        private void WaitForm_LostFocus(object sender, EventArgs e)
        {
            SplashScreenManager.CloseForm(false);
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion
    }
}