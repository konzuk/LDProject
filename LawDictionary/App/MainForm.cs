using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;

namespace LawDictionary
{
    public partial class MainForm : Form
    {
        private static readonly string AllString = "ទាំងអស់";
        private static readonly string RoleKey = "P@ssw0rd";
        private readonly string dir = AppDomain.CurrentDomain.BaseDirectory;
        private bool _isAdmin;
        private static int _limitRecord = 20;

        public MainForm()
        {
            InitializeComponent();

            defaultLookAndFeel1.LookAndFeel.SetSkinStyle("MySkin_Office 2013");

            Resize += Form2_Resize;
            gridView1.ColumnWidthChanged += GridView1_ColumnWidthChanged;
            FormClosing += AppView_FormClosing;
            gridView1.RowCellClick += GridView1_RowCellClick;
            GridView1_ColumnWidthChanged(null, null);


            GetData();


            pictureBox1.Image = new Bitmap(dir + "Images\\top.png");


            Form2_Resize(null, null);
            
        }

        private IList<LawDictionaryModel> LawDictionaryModels { get; set; }
        private IList<LawDictionaryModel> BindSource { get; set; }
        private IList<string> Types { get; set; }
        private IList<string> Years { get; set; }
        private IList<string> NGOs { get; set; }

        private KZFlyoutDialog kZFlyoutDialog
        {
            get { return new KZFlyoutDialog(); }
        }

        private LawDictionaryModel SelectedModel
        {
            get { return gridView1.GetFocusedRow() as LawDictionaryModel; }
        }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                BinaryFileHelper.WriteToBinaryFile(dir + "RoleKey.dll", "");
            }
        }

        private void GridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                var document = dir + "Files\\" + SelectedModel.Code;

                if (File.Exists(document))
                {
                    var view = new KZPDFViewer();
                    view.Document = document;
                    view.ShowDialog();
                }
                else
                {
                    kZFlyoutDialog.AlertMessage(this, "ឯកសារ PDF មិនទានបញ្ចូលក្នុងកម្មវិធី។");
                }
            }
        }

        private void gridView1_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return;
            var rect = e.Bounds;
            ControlPaint.DrawBorder3D(e.Graphics, e.Bounds);
            var brush =
                e.Cache.GetGradientBrush(rect, e.Column.AppearanceHeader.BackColor,
                    e.Column.AppearanceHeader.BackColor2, e.Column.AppearanceHeader.GradientMode);
            rect.Inflate(-1, -1);
            // Fill column headers with the specified colors.
            e.Graphics.FillRectangle(brush, rect);
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            // Draw the filter and sort buttons.
            foreach (DrawElementInfo info in e.Info.InnerElements)
            {
                if (!info.Visible) continue;
                ObjectPainter.DrawObject(e.Cache, info.ElementPainter,
                    info.ElementInfo);
            }
            e.Handled = true;
        }

        private void writeData()
        {
            try
            {

                if (LawDictionaryModels.Count() < _limitRecord)
                {
                    BinaryFileHelper.WriteToBinaryFile(dir + "MyLDData.dll", LawDictionaryModels);
                }
                else
                {
                    kZFlyoutDialog.AlertMessage(this, "ការប្រើប្រាស់លើសចំនួនកំនត់សំរាប់ការសាកល្បង។");
                }
            }
            finally
            {
                GetData();
            }
        }

        private void GetData()
        {
            LawDictionaryModels = new BindingList<LawDictionaryModel>();

            // Read the file contents back into a variable.
            try
            {
                LawDictionaryModels =
                    BinaryFileHelper.ReadFromBinaryFile<IList<LawDictionaryModel>>(dir + "MyLDData.dll");
            }
            catch
            {
            }

            var roleKey = "";

            try
            {
                roleKey = BinaryFileHelper.ReadFromBinaryFile<string>(dir + "RoleKey.dll");
            }
            catch
            {
            }


            if (roleKey.Equals(RoleKey))
            {
                IsAdmin = true;
            }

            Types = new BindingList<string>(LawDictionaryModels.Select(s => s.Type).Distinct().ToList());
            Years = new BindingList<string>(LawDictionaryModels.Select(s => s.Year).Distinct().ToList());
            NGOs = new BindingList<string>(LawDictionaryModels.Select(s => s.NGO).Distinct().ToList());

            BindButtons();
            BindType();
            BindYear();
            BindNGO();

            Search();
        }

        private void BindButtons()
        {
            simpleButtonAdd.Visible = IsAdmin;
            simpleButtonUpdate.Visible = IsAdmin;
            simpleButtonDelete.Visible = IsAdmin;
        }

        private void GridView1_ColumnWidthChanged(object sender, ColumnEventArgs e)
        {
            layoutControlItemNo.Size = new Size(No.Width + 1, layoutControlItemNo.Size.Height);
            layoutControlItemCode.Size = new Size(Document.Width, layoutControlItemCode.Size.Height);
            layoutControlItemType.Size = new Size(Type.Width, layoutControlItemType.Size.Height);
            layoutControlItemYear.Size = new Size(Year.Width, layoutControlItemYear.Size.Height);
        }

        private void BindType()
        {
            comboBoxEditType.Properties.Items.Clear();
            comboBoxEditType.Properties.Items.Add(AllString);
            comboBoxEditType.Properties.Items.AddRange(Types.ToList());
            comboBoxEditType.SelectedIndex = 0;
        }

        private void BindYear()
        {
            comboBoxEditYear.Properties.Items.Clear();
            comboBoxEditYear.Properties.Items.Add(AllString);
            comboBoxEditYear.Properties.Items.AddRange(Years.ToList());
            comboBoxEditYear.SelectedIndex = 0;
        }

        private void BindNGO()
        {
            comboBoxEditNGO.Properties.Items.Clear();
            comboBoxEditNGO.Properties.Items.Add(AllString);
            comboBoxEditNGO.Properties.Items.AddRange(NGOs.ToList());
            comboBoxEditNGO.SelectedIndex = 0;
        }

        private void BindGrid()
        {
            No.FieldName = "Code";
            Document.FieldName = "Document";
            Type.FieldName = "Type";
            Year.FieldName = "Year";
            NOG.FieldName = "NGO";

            gridControl1.DataSource = BindSource;
            labelRight.Text = "សរុបឯកសារ: " + BindSource.Count + "/" + LawDictionaryModels.Count;
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            var height = 0;
            var width = Size.Width;
            //1355,155
            var percentage = 155*100/1355;
            height = Width*percentage/100;
            pictureBox1.Size = new Size(width, height);
        }

        private void ShowAjax()
        {
            SplashScreenManager.ShowDefaultWaitForm(this, true, true, false, 0);
        }

        private void CloseAjax()
        {
            SplashScreenManager.CloseDefaultWaitForm();
        }

        private void Search()
        {
            var code = textEditCode.Text ?? "";
            var doc = textEditDoc.Text ?? "";


            BindSource = new BindingList<LawDictionaryModel>(LawDictionaryModels.Where(s =>
                (string.IsNullOrEmpty(code) || s.Code.ToLower().Contains(code.ToLower())) &&
                (string.IsNullOrEmpty(doc) || s.Document.ToLower().Contains(doc.ToLower())) &&
                (comboBoxEditType.Text == null || comboBoxEditType.Text == AllString || comboBoxEditType.Text == s.Type) &&
                (comboBoxEditYear.Text == null || comboBoxEditYear.Text == AllString || comboBoxEditYear.Text == s.Year) &&
                (comboBoxEditNGO.Text == null || comboBoxEditNGO.Text == AllString || comboBoxEditNGO.Text == s.NGO)
                ).ToList());
            BindGrid();
        }

        private void textEditDoc_EditValueChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void textEditCode_EditValueChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void comboBoxEditType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void comboBoxEditYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void comboBoxEditNGO_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void AppView_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !kZFlyoutDialog.AlertClose(this, "តើអ្នកចង់បិទកម្មវិធីដែរឺទេ?");
        }

        private void simpleButtonAbout_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof (WaitFormView), true, true, false);
        }

        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            if (SelectedModel == null) return;
            var isDelete = kZFlyoutDialog.AlertDelete(this,
                "តើអ្នកពិតជាចង់លុបឯកសារលេខ \"" + SelectedModel.Code + "\" ដែរឺទេ?");

            if (isDelete)
            {
                ShowAjax();
                var file = dir + "Files\\" + SelectedModel.Code;
                LawDictionaryModels.Remove(SelectedModel);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                writeData();

                CloseAjax();
            }
        }

        private void simpleButtonUpdate_Click(object sender, EventArgs e)
        {
            if (SelectedModel == null) return;

            var view = new AddUpdateLD();
            view.IsUpdate = true;
            view.LawDictionaryModel = SelectedModel;
            view.LawDictionaryModels = LawDictionaryModels;
            view.ShowDialog(this);
            if (view.IsSave)
            {
                ShowAjax();
                writeData();
                CloseAjax();
            }
        }

        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            var view = new AddUpdateLD();
            view.LawDictionaryModels = LawDictionaryModels;
            view.ShowDialog(this);
            if (view.IsSave)
            {
                ShowAjax();
                LawDictionaryModels.Add(view.LawDictionaryModel);
                writeData();
                CloseAjax();
            }
        }
    }
}