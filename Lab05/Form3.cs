using Lab05.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab05
{
    public partial class Form3 : Form
    {
        StudentContextDB db;
        public Form3()
        {
            InitializeComponent();
            db = new StudentContextDB();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
            textBox3.Text = "0";
            try
            {
                List<Faculty> listFalcuty = db.Faculties.ToList();
                List<Student> listStudent = db.Students.ToList();
                FillFaculty(listFalcuty);
                BindGrid(listStudent);
                button1.Click += button1_Click;
                textBox3.Text = listStudent.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.Show();
            }
        }

        private void BindGrid(List<Student> studentList)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in studentList)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["Col_MSV"].Value = item.StudentID;
                dataGridView1.Rows[index].Cells["Col_HoTen"].Value = item.FullName;
                dataGridView1.Rows[index].Cells["Col_GT"].Value = item.Gender;
                dataGridView1.Rows[index].Cells["Col_Khoa"].Value = item.Faculty?.FacultyName;
                dataGridView1.Rows[index].Cells["Col_DTB"].Value = item.AverageScore.ToString("0.0");


            }
        }

        private void FillFaculty(List<Faculty> listFalcuty)
        {
            this.comboBox1.DataSource = listFalcuty;
            this.comboBox1.DisplayMember = "FacultyName";
            this.comboBox1.ValueMember = "FacultyID";
        }

        private void CountStudent()
        {
            int total = db.Students.Count();
            textBox3.Text = total.ToString();
        }   

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = textBox1.Text;
                string fullName = textBox2.Text;
                string gender = radioButton1.Checked ? "Male" : radioButton2.Checked ? "Female" : string.Empty;
                int facultyID = comboBox1.SelectedValue != null ? (int)comboBox1.SelectedValue : -1;

                var query = db.Students.AsQueryable();

                if (!string.IsNullOrEmpty(studentID))
                {
                    query = query.Where(s => s.StudentID.Contains(studentID));
                }

                if (!string.IsNullOrEmpty(fullName))
                {
                    query = query.Where(s => s.FullName.Contains(fullName));
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    query = query.Where(s => s.Gender == gender);
                }

                if (facultyID != -1)
                {
                    query = query.Where(s => s.FacultyID == facultyID);
                }

                List<Student> result = query.ToList();

                if (result.Count > 0)
                {
                    BindGrid(result);
                    textBox3.Text = result.Count.ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CountStudent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int facultyID = int.Parse(textBox1.Text);
                var faculty = db.Faculties.FirstOrDefault(f => f.FacultyID == facultyID);

                if (faculty == null)
                {
                    MessageBox.Show("Mã khoa không tồn tại trong hệ thống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khoa này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    db.Faculties.Remove(faculty);
                    db.SaveChanges();

                    BindGrid(db.Students.ToList());

                    MessageBox.Show("Xóa khoa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa khoa: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
