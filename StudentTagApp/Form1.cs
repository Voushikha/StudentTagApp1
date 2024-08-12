using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StudentTagApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //6.1
        private List<string> studentNames = new List<string>();
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //6.2 Load Button
        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        studentNames.Clear();
                        while (fs.Position < fs.Length)
                        {
                            string name = br.ReadString();
                            studentNames.Add(name);
                        }
                        RefreshStudentList();
                        MessageBox.Show("Data loaded successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

        }
       
        //6.3 Add Button
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var name = txtBoxName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           //6.4
            ValidName();
          
            studentNames.Add(name);
            RefreshStudentList();
            ClearInput();
            MessageBox.Show("Student added successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //6.5 Delete button
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedName = lstBox.SelectedItem as string;
            if (selectedName != null)
            {
                studentNames.Remove(selectedName);
                RefreshStudentList();
                ClearInput();
                MessageBox.Show("Student deleted successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //6.6 Edit button
        private void btnEdit_Click(object sender, EventArgs e)
        {
            var selectedName = lstBox.SelectedItem as string;
            var newName = txtBoxName.Text.Trim();

            if (selectedName != null && !string.IsNullOrWhiteSpace(newName))
            {
                studentNames[studentNames.IndexOf(selectedName)] = newName;
                RefreshStudentList();
                ClearInput();
                MessageBox.Show("Student updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #region Methods
        //6.4 Valid name 
        private void ValidName()
        {
            var name = txtBoxName.Text.Trim();
            if (studentNames.Exists(s => s.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Duplicate name detected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }



        //6.10  Clear Textboxes Method
        private void ClearInput()
        {
            txtBoxName.Clear();
            txtBoxName.Focus();
        }


        private void RefreshStudentList()
        {
            lstBox.DataSource = null;
            lstBox.DataSource = studentNames;
        }

        #endregion

       
    }
}

