using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DataGridViewExample
{
    public partial class MainForm : Form
    {
        private int serialNumber = 1; // Track the serial number

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set up the DataGridView columns
            dataGridView1.Columns.Add("SerialNumber", "Serial Number");
            dataGridView1.Columns.Add("Label", "Label");
            dataGridView1.Columns.Add("FileFolder", "File/Folder");
            dataGridView1.Columns.Add("Password", "Password");
            dataGridView1.Columns.Add("SaveButton", "Save");
            dataGridView1.Columns.Add("ProgressBar", "Progress");
            dataGridView1.Columns.Add("SelectedFileFolder", "Selected File/Folder");

            // Assign event handlers to relevant columns
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
        }

        private void AddRow()
        {
            // Add a new row to the DataGridView
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView1);

            // Set the default values for each cell
            row.Cells[0].Value = serialNumber++;
            row.Cells[1].Value = "Label";
            row.Cells[2].Value = "Browse...";
            row.Cells[4].Value = "Save";
            row.Cells[5].Value = 0;

            dataGridView1.Rows.Add(row);
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle button click event in column 3
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)dataGridView1.Rows[e.RowIndex].Cells[2];
                if (buttonCell.Value.ToString() == "Browse...")
                {
                    // Open a file or folder dialog
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.FileName = "";
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.CheckPathExists = true;
                    openFileDialog.Multiselect = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Set the selected file/folder in column 3
                        dataGridView1.Rows[e.RowIndex].Cells[2].Value = openFileDialog.FileName;
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = openFileDialog.FileName;
                    }
                }
            }
            // Handle button click event in column 4 (Save button)
            else if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {
                DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)dataGridView1.Rows[e.RowIndex].Cells[4];
                if (buttonCell.Value.ToString() == "Save")
                {
                    // Retrieve the password from column 3
                    string password = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

                    // Save the password or perform any other action here

                    // Update the progress bar in column 6
                    DataGridViewProgressCell progressCell = (DataGridViewProgressCell)dataGridView1.Rows[e.RowIndex].Cells[5];
                    progressCell.Value = 100;

                    // Update the selected file/folder label in column 7
                    dataGridView1.Rows[e.RowIndex].Cells[6].Value = dataGridView1.Rows[e.RowIndex].Cells[2].Value;
                }
            }
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Update the label in column 1 when the cell value changes
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Cells[1].Value = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Handle changes to the file/folder path in column 3
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                // Update the selected file/folder label in column 7
                dataGridView1.Rows[e.RowIndex].Cells[6].Value = dataGridView1.Rows[e.RowIndex].Cells[2].Value;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            // Add a new row when the Add button is clicked
            AddRow();
        }
    }

    public class DataGridViewProgressCell : DataGridViewImageCell
    {
        // Default constructor
        public DataGridViewProgressCell()
        {
            // Set the cell type to DataGridViewProgressCell
            this.ValueType = typeof(int);
        }

        // Override the Clone method
        public override object Clone()
        {
            DataGridViewProgressCell cell = (DataGridViewProgressCell)base.Clone();
            return cell;
        }

        // Override the Paint method
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // Call the base Paint method
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
                cellStyle, advancedBorderStyle, paintParts);

            if (value != null)
            {
                // Calculate the progress bar dimensions
                int progressVal = (int)value;
                int progressBarWidth = (cellBounds.Width - 4) * progressVal / 100;
                int progressBarHeight = cellBounds.Height - 4;

                // Draw the progress bar
                Rectangle progressBarRect = new Rectangle(cellBounds.X + 2, cellBounds.Y + 2, progressBarWidth, progressBarHeight);
                graphics.FillRectangle(Brushes.Green, progressBarRect);
            }
        }
    }
}
