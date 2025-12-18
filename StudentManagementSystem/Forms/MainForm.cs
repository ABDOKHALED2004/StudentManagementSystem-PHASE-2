using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Forms
{
    public sealed class MainForm : Form
    {
        private readonly IStudentRepository _repo;

        private Label lblTitle = null!;
        private Label lblStudentId = null!;
        private Label lblName = null!;
        private Label lblAge = null!;
        private Label lblDepartment = null!;
        private Label lblPhone = null!;

        private TextBox txtStudentId = null!;
        private TextBox txtName = null!;
        private TextBox txtAge = null!;
        private TextBox txtDepartment = null!;
        private TextBox txtPhone = null!;

        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnShow = null!;
        private Button btnSearch = null!;

        private PictureBox picBus = null!;
        private PictureBox picBooks = null!;
        private PictureBox picBag = null!;
        private PictureBox picBush = null!;

        public MainForm(IStudentRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            InitializeComponent();
            LoadImages();
        }

        private void InitializeComponent()
        {
            // Form
            // Match the sample screenshot title.
            Text = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 670);
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // Title
            lblTitle = new Label
            {
                AutoSize = false,
                // Match the sample screenshot text (including spelling).
                Text = "Welcom Back to School",
                Font = new Font("Segoe Script", 34F, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 10),
                Size = new Size(980, 70)
            };

            // Labels
            var lblFont = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic);

            lblStudentId = MakeLabel("Student ID", lblFont, new Point(90, 110));
            lblName      = MakeLabel("Name",       lblFont, new Point(120, 160));
            lblAge       = MakeLabel("Age",        lblFont, new Point(130, 210));
            lblDepartment= MakeLabel("Department", lblFont, new Point(85, 260));
            lblPhone     = MakeLabel("Phone",      lblFont, new Point(115, 310));

            // Textboxes
            txtStudentId = MakeTextBox(new Point(200, 110));
            txtName      = MakeTextBox(new Point(200, 160));
            txtAge       = MakeTextBox(new Point(200, 210));
            txtDepartment= MakeTextBox(new Point(200, 260));
            txtPhone     = MakeTextBox(new Point(200, 310));

            txtAge.KeyPress += TxtAge_KeyPress;

            // Buttons
            var btnFont = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic);

            btnAdd    = MakeButton("Add",    btnFont, new Point(60, 380));
            btnUpdate = MakeButton("Update", btnFont, new Point(250, 380));
            btnDelete = MakeButton("Delete", btnFont, new Point(440, 380));
            btnShow   = MakeButton("Show",   btnFont, new Point(110, 480));
            btnSearch = MakeButton("search", btnFont, new Point(320, 480));

            btnAdd.Click    += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnShow.Click   += BtnShow_Click;
            btnSearch.Click += BtnSearch_Click;

            // Pictures
            picBus = MakePictureBox(new Point(560, 120), new Size(340, 260));
            picBooks = MakePictureBox(new Point(720, 360), new Size(280, 280));
            picBag = MakePictureBox(new Point(510, 450), new Size(250, 220));
            picBush = MakePictureBox(new Point(0, 520), new Size(390, 150));

            // Add to form
            Controls.Add(lblTitle);

            Controls.Add(lblStudentId);
            Controls.Add(lblName);
            Controls.Add(lblAge);
            Controls.Add(lblDepartment);
            Controls.Add(lblPhone);

            Controls.Add(txtStudentId);
            Controls.Add(txtName);
            Controls.Add(txtAge);
            Controls.Add(txtDepartment);
            Controls.Add(txtPhone);

            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(btnShow);
            Controls.Add(btnSearch);

            Controls.Add(picBus);
            Controls.Add(picBooks);
            Controls.Add(picBag);
            Controls.Add(picBush);
        }

        private static Label MakeLabel(string text, Font font, Point location)
        {
            return new Label
            {
                AutoSize = true,
                Text = text,
                Font = font,
                Location = location
            };
        }

        private static TextBox MakeTextBox(Point location)
        {
            return new TextBox
            {
                Location = location,
                Size = new Size(240, 30),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular)
            };
        }

        private static Button MakeButton(string text, Font font, Point location)
        {
            return new Button
            {
                Text = text,
                Location = location,
                Size = new Size(150, 60),
                Font = font,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Standard
            };
        }

        private static PictureBox MakePictureBox(Point location, Size size)
        {
            return new PictureBox
            {
                Location = location,
                Size = size,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
        }

        private void LoadImages()
        {
            // Images are copied beside the EXE under Assets\
            var baseDir = AppContext.BaseDirectory;
            var assetsDir = Path.Combine(baseDir, "Assets");

            TryLoad(picBus,  Path.Combine(assetsDir, "bus_crop.png"));
            TryLoad(picBooks,Path.Combine(assetsDir, "books_crop.png"));
            TryLoad(picBag,  Path.Combine(assetsDir, "bag_crop.png"));
            TryLoad(picBush, Path.Combine(assetsDir, "bush_crop.png"));
        }

        private static void TryLoad(PictureBox pic, string path)
        {
            try
            {
                if (File.Exists(path))
                    pic.Image = Image.FromFile(path);
            }
            catch
            {
                // ignore - image is optional
            }
        }

        private void TxtAge_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // digits + backspace only
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var student = ReadStudentFromInputs(requireAll: true);
            if (student is null) return;

            try
            {
                _repo.Add(student);
                MessageBox.Show("Student added successfully ✅", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs(keepId: false);
                txtStudentId.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            var student = ReadStudentFromInputs(requireAll: true);
            if (student is null) return;

            try
            {
                var ok = _repo.Update(student);
                if (!ok)
                {
                    MessageBox.Show("Student ID not found.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MessageBox.Show("Student updated successfully ✅", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs(keepId: false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            var id = txtStudentId.Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Enter Student ID first.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show($"Delete student with ID = {id}?", "Confirm delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                var ok = _repo.Delete(id);
                if (!ok)
                {
                    MessageBox.Show("Student ID not found.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MessageBox.Show("Student deleted ✅", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs(keepId: false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            var id = txtStudentId.Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Write Student ID to search.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var s = _repo.GetById(id);
            if (s is null)
            {
                MessageBox.Show("Not found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FillInputs(s);
        }

        private void BtnShow_Click(object? sender, EventArgs e)
        {
            using var listForm = new StudentsListForm(_repo);
            var result = listForm.ShowDialog(this);

            if (result == DialogResult.OK && listForm.SelectedStudent is not null)
                FillInputs(listForm.SelectedStudent);
        }

        private Student? ReadStudentFromInputs(bool requireAll)
        {
            var id = txtStudentId.Text.Trim();
            var name = txtName.Text.Trim();
            var ageText = txtAge.Text.Trim();
            var dept = txtDepartment.Text.Trim();
            var phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Student ID is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtStudentId.Focus();
                return null;
            }

            if (requireAll)
            {
                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(ageText) ||
                    string.IsNullOrWhiteSpace(dept) ||
                    string.IsNullOrWhiteSpace(phone))
                {
                    MessageBox.Show("Fill all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }
            }

            if (!int.TryParse(ageText, out var age))
            {
                MessageBox.Show("Age must be a number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAge.Focus();
                return null;
            }

            if (age < 1 || age > 120)
            {
                MessageBox.Show("Age should be between 1 and 120.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAge.Focus();
                return null;
            }

            return new Student
            {
                StudentId = id,
                Name = name,
                Age = age,
                Department = dept,
                Phone = phone
            };
        }

        private void FillInputs(Student s)
        {
            txtStudentId.Text = s.StudentId;
            txtName.Text = s.Name;
            txtAge.Text = s.Age.ToString();
            txtDepartment.Text = s.Department;
            txtPhone.Text = s.Phone;
        }

        private void ClearInputs(bool keepId)
        {
            if (!keepId) txtStudentId.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtDepartment.Clear();
            txtPhone.Clear();
        }
    }
}
