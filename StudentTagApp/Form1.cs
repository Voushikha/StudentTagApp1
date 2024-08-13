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
        //6.1 list<t>
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
                        SortnDisplay();
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
            SortnDisplay();
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
                SortnDisplay();
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
                SortnDisplay();
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
        //6.9 BinarySearch button
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchQuery = txtBoxName.Text.Trim();
            var foundName = studentNames.Find(s => s.Equals(searchQuery, StringComparison.OrdinalIgnoreCase));

            if (foundName != null)
            {
                lstBox.SelectedItem = foundName;
                txtBoxName.Text = foundName;
                MessageBox.Show("Student found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Student not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtBoxName.Clear();
            txtBoxName.Focus();
            SortnDisplay();
        }
        //6.7
        private void lstBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedName = lstBox.SelectedItem as string;
            if (selectedName != null)
            {
                txtBoxName.Text = selectedName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*",
                Title = "Save Student Names"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    foreach (var name in studentNames)
                    {
                        bw.Write(name);
                    }
                }
                MessageBox.Show("Data saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        //6.8 Sorting and displaying student names
        private void SortnDisplay()
        {
            // Sort the student names list
            studentNames.Sort();

            // Refresh the ListBox to display the sorted names
            RefreshStudentList();

            // Optional: Notify the user that the list has been sorted
            MessageBox.Show("Student names have been sorted and displayed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
