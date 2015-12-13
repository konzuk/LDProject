using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using LawDictionary.Properties;

namespace LawDictionary
{
    public class KZFlyoutDialog : FlyoutDialog
    {
        public KZFlyoutDialog()
        {
            action = new FlyoutAction();
            CommandYes = new FlyoutCommand {Text = "យល់ព្រម", Result = DialogResult.Yes};
            CommandNo = new FlyoutCommand {Text = "មិនយល់ព្រម", Result = DialogResult.No};
            action.Commands.Add(CommandYes);
            action.Commands.Add(CommandNo);


            properties = new FlyoutProperties();
            //properties.Appearance.BackColor = Color.Blue;
            //properties.Appearance.Options.UseBackColor = true;

            properties.Appearance.Font = KZFonts.ContentFont;
            properties.Appearance.Options.UseFont = true;
            properties.AppearanceCaption.Font = KZFonts.HeaderFont;
            properties.AppearanceCaption.Options.UseFont = true;
            properties.AppearanceButtons.Font = KZFonts.ContentFont;
            properties.AppearanceButtons.Options.UseFont = true;
            properties.AppearanceDescription.Font = KZFonts.ContentFont;
            properties.AppearanceDescription.Options.UseFont = true;


            properties.ButtonSize = new Size(150, 40);
            properties.Style = FlyoutStyle.MessageBox;
        }

        private FlyoutAction action { get; }
        public FlyoutCommand CommandYes { get; set; }
        public FlyoutCommand CommandNo { get; set; }
        private FlyoutProperties properties { get; }

        private static bool canCloseFunc(DialogResult parameter)
        {
            return parameter != DialogResult.Cancel;
        }

        public bool AlertClose(Form Owner, string message)
        {
            return Alert(Owner, message, Resources.Info_32x32);
        }

        public bool AlertDelete(Form Owner, string message)
        {
            return Alert(Owner, message, Resources.Info_32x32);
        }

        public bool AlertMessage(Form Owner, string message)
        {
            action.Commands.Clear();
            var CommandClose = new FlyoutCommand {Text = "បិទ", Result = DialogResult.OK};
            action.Commands.Add(CommandClose);

            return Alert(Owner, message, Resources.Info_32x32);
        }

        public bool Alert(Form Owner, string Message, Image Image = null)
        {
            action.Caption = "សំគាល់";
            action.Description = Message;
            action.Image = Image;
            return Show(Owner, action, properties, canCloseFunc) == DialogResult.Yes;
        }
    }
}