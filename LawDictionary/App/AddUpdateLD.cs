using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace LawDictionary
{
    public partial class AddUpdateLD : Form
    {
        private readonly string dir = AppDomain.CurrentDomain.BaseDirectory;
        private string OldFile;

        public AddUpdateLD()
        {
            InitializeComponent();
            FormClosing += AddUpdateLD_FormClosing;
            Load += AddUpdateLD_Load;
            textEditNo.GotFocus += TextEditNo_GotFocus;
            textEditName.GotFocus += TextEditNo_GotFocus;
            comboBoxEditNGO.GotFocus += TextEditNo_GotFocus;
            comboBoxEditYear.GotFocus += TextEditNo_GotFocus;
            comboBoxEditType.GotFocus += TextEditNo_GotFocus;

            textEditDocLocation.EditValueChanged += TextEditDocLocation_EditValueChanged;
        }

        public bool IsUpdate { get; set; }
        public bool IsCloseHandled { get; set; }
        private IList<string> Types { get; set; }
        private IList<string> Years { get; set; }
        private IList<string> NGOs { get; set; }
        public LawDictionaryModel LawDictionaryModel { get; set; }
        public IList<LawDictionaryModel> LawDictionaryModels { get; set; }

        private KZFlyoutDialog kZFlyoutDialog
        {
            get { return new KZFlyoutDialog(); }
        }

        public bool IsSave { get; set; }

        private void TextEditDocLocation_EditValueChanged(object sender, EventArgs e)
        {
            var obj = sender as BaseEdit;
            var superToolTip1 = new SuperToolTip();
            var toolTipItem1 = new ToolTipItem();
            toolTipItem1.Text = obj.Text;
            superToolTip1.Items.Add(toolTipItem1);
            obj.SuperTip = superToolTip1;
        }

        private void TextEditNo_GotFocus(object sender, EventArgs e)
        {
            var control = sender as BaseEdit;
            control.BackColor = Color.White;
        }

        private void AddUpdateLD_Load(object sender, EventArgs e)
        {
            layoutControlGroupLD.Text = IsUpdate ? "កែប្រែឯកសារ​" : "បញ្ចូលឯកសារ​";
            if (!IsUpdate)
            {
                LawDictionaryModel = new LawDictionaryModel();
            }

            Types = new BindingList<string>(LawDictionaryModels.Select(s => s.Type).Distinct().ToList());
            Years = new BindingList<string>(LawDictionaryModels.Select(s => s.Year).Distinct().ToList());
            NGOs = new BindingList<string>(LawDictionaryModels.Select(s => s.NGO).Distinct().ToList());

            BindType();
            BindYear();
            BindNGO();

            if (LawDictionaryModel != null)
            {
                textEditName.EditValue = LawDictionaryModel.Document;
                textEditNo.EditValue = LawDictionaryModel.Code;
                comboBoxEditType.Text = LawDictionaryModel.Type;
                comboBoxEditYear.Text = LawDictionaryModel.Year;
                comboBoxEditNGO.Text = LawDictionaryModel.NGO;

                if (IsUpdate && File.Exists(dir + "Files\\" + LawDictionaryModel.Code))
                {
                    OldFile = textEditDocLocation.Text = dir + "Files\\" + LawDictionaryModel.Code;
                    textEditDocLocation.Text = OldFile;
                }
            }
        }

        private void AddUpdateLD_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !IsCloseHandled && !kZFlyoutDialog.AlertClose(this, "តើអ្នកចង់ត្រលប់ក្រោយដែរឺទេ?");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BindType()
        {
            comboBoxEditType.Properties.Items.Clear();
            comboBoxEditType.Properties.Items.AddRange(Types.ToList());
            comboBoxEditType.SelectedIndex = 0;
        }

        private void BindYear()
        {
            comboBoxEditYear.Properties.Items.Clear();
            comboBoxEditYear.Properties.Items.AddRange(Years.ToList());
            comboBoxEditYear.SelectedIndex = 0;
        }

        private void BindNGO()
        {
            comboBoxEditNGO.Properties.Items.Clear();
            comboBoxEditNGO.Properties.Items.AddRange(NGOs.ToList());
            comboBoxEditNGO.SelectedIndex = 0;
        }

        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            var message = "";
            var newLine = "";

            if (string.IsNullOrEmpty(textEditNo.Text))
            {
                message += newLine + "* សូមបញ្ចូលលេខឯកសារ។";
                newLine = "\n";
                textEditNo.BackColor = Color.LightCoral;
            }
            if (string.IsNullOrEmpty(textEditName.Text))
            {
                message += newLine + "* សូមបញ្ចូលឈ្មោះឯកសារ។";
                newLine = "\n";

                textEditName.BackColor = Color.LightCoral;
            }
            if (string.IsNullOrEmpty(comboBoxEditType.Text))
            {
                message += newLine + "* សូមបញ្ចូលប្រភេទឯកសារ។";
                newLine = "\n";

                comboBoxEditType.BackColor = Color.LightCoral;
            }
            if (string.IsNullOrEmpty(comboBoxEditYear.Text))
            {
                message += newLine + "* សូមបញ្ចូលឆ្នាំឯកសារ។";
                newLine = "\n";

                comboBoxEditYear.BackColor = Color.LightCoral;
            }
            if (string.IsNullOrEmpty(comboBoxEditNGO.Text))
            {
                message += newLine + "* សូមបញ្ចូលក្រសួង / ស្ថាប័ន។";
                newLine = "\n";

                comboBoxEditNGO.BackColor = Color.LightCoral;
            }

            if (!string.IsNullOrEmpty(message))
            {
                kZFlyoutDialog.AlertMessage(this, message);
                return;
            }
            LawDictionaryModel.Document = textEditName.Text;
            LawDictionaryModel.Code = textEditNo.Text;
            LawDictionaryModel.Type = comboBoxEditType.Text;
            LawDictionaryModel.Year = comboBoxEditYear.Text;
            LawDictionaryModel.NGO = comboBoxEditNGO.Text;


            var updateModel =
                LawDictionaryModels.Where(
                    s => s != LawDictionaryModel && s.Code.ToLower() == LawDictionaryModel.Code.ToLower());
            if (updateModel.Any())
            {
                kZFlyoutDialog.AlertMessage(this, " លេខឯកសារនេះបានបង្កើតរួចហើយ។");
                LawDictionaryModel.Code = textEditNo.Text;
                textEditNo.BackColor = Color.LightCoral;
                return;
            }

            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            process.StartInfo = startInfo;


            var newFileName = dir + "Files\\" + LawDictionaryModel.Code;
            if (IsUpdate)
            {
                if (File.Exists(textEditDocLocation.Text))
                {
                    if (File.Exists(OldFile) && OldFile != textEditDocLocation.Text)
                    {
                        File.Delete(OldFile);
                    }
                    if (textEditDocLocation.Text != newFileName)
                    {
                        File.Copy(textEditDocLocation.Text, newFileName, true);
                    }
                }
            }
            else
            {
                if (File.Exists(textEditDocLocation.Text))
                {
                    File.Copy(textEditDocLocation.Text, newFileName, true);
                }
            }

            IsSave = true;
            IsCloseHandled = true;
            Close();
        }

        private void simpleButtonSelectedFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Pdf Files|*.pdf";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textEditDocLocation.Text = openFileDialog1.FileName;
            }
        }
    }
}