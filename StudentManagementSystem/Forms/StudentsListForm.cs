using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Forms
{
    public sealed class StudentsListForm : Form
    {
        private readonly IStudentRepository _repo;

        private DataGridView grid = null!;
        private Button btnSelect = null!;
        private Button btnRefresh = null!;
        private Button btnClose = null!;

        public Student? SelectedStudent { get; private set; }

        public StudentsListForm(IStudentRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            Text = "Students";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(800, 450);
            BackColor = Color.Gray;

            grid = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(780, 360),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            grid.CellDoubleClick += (s, e) => SelectCurrent();

            btnSelect = new Button
            {
                Text = "Select",
                Location = new Point(10, 380),
                Size = new Size(120, 50),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };
            btnSelect.Click += (s, e) => SelectCurrent();

            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(140, 380),
                Size = new Size(120, 50),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };
            btnRefresh.Click += (s, e) => LoadData();

            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(670, 380),
                Size = new Size(120, 50),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };
            btnClose.Click += (s, e) => Close();

            Controls.Add(grid);
            Controls.Add(btnSelect);
            Controls.Add(btnRefresh);
            Controls.Add(btnClose);
        }

        private void LoadData()
        {
            var data = _repo.GetAll().ToList();
            grid.DataSource = data;

            if (grid.Columns.Count > 0)
            {
                grid.Columns[nameof(Student.StudentId)].HeaderText = "Student ID";
                grid.Columns[nameof(Student.Name)].HeaderText = "Name";
                grid.Columns[nameof(Student.Age)].HeaderText = "Age";
                grid.Columns[nameof(Student.Department)].HeaderText = "Department";
                grid.Columns[nameof(Student.Phone)].HeaderText = "Phone";
            }
        }

        private void SelectCurrent()
        {
            if (grid.CurrentRow?.DataBoundItem is Student s)
            {
                SelectedStudent = s;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Select a row first.", "Select", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
