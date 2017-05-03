using JeppesenRaveVariableUsageReportGenerator.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JeppesenRaveVariableUsageReportGenerator.WinForms
{
    public partial class UI : Form
    {
        private FolderBrowserDialog folderBrowserDialog;

        public UI()
        {
            InitializeComponent();
            folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select carmusr directory.";
            folderBrowserDialog.ShowNewFolderButton = false;
        }

        private void btn_selectFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();

            if (folderBrowserDialog.SelectedPath.Length < 2)
            {
                return;
            }
            else
            {
                Dictionary<string, VariableInfo> variableUsage = Helper.ExportVariablesFromModules(folderBrowserDialog.SelectedPath);
                Helper.GetVariableUsage(variableUsage, folderBrowserDialog.SelectedPath);
                Helper.WriteHTMLReport(variableUsage);

                MessageBox.Show("Report generated successfully!");
            }
        }
    }
}
