using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GYQuery
{
    public interface IMainForm
    {
        string SelectKeyFile { get; }
        string SelectSaveFolderPath { get; }
        string FindNum { get; }

        event EventHandler Save;
    }
    public partial class MainForm : Form, IMainForm
    {
        public MainForm()
        {
            InitializeComponent();
            btnSave.Click += btnSave_Click;
            btnSavePath.Click += btnSavePath_Click;
            btnSelectKeyFile.Click += btnSelectKeyFile_Click;
        }
        #region IMainForm

        public string FindNum
        {
            get { return tbFindNum.Text; }
        }
        public string SelectKeyFile
        {
            get { return tbSelectKeyFile.Text; }
        }
        public string SelectSaveFolderPath
        {
            get { return tbSavePath.Text; }
        }

        public event EventHandler Save;
        #endregion

        #region
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save?.Invoke(this, EventArgs.Empty);
        }
        private void btnSavePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbSavePath.Text = dlg.SelectedPath;
            }
        }
        private void btnSelectKeyFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "File|*.txt|All file|*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbSelectKeyFile.Text = dlg.FileName;
            }
        }
        #endregion
    }
}
