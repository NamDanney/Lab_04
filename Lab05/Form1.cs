using Lab05.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Lab05
{
    public partial class Form1 : Form
    {

        StudentContextDB db;
        public Form1()
        {
            InitializeComponent();
            db = new StudentContextDB();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox4.Text = "0";
            textBox5.Text = "0";
            radioButton2.Checked = true;
            try
            {
                List<Faculty> listFalcuty = db.Faculties.ToList();
                List<Student> listStudent = db.Students.ToList();
                FillFaculty(listFalcuty);
                BindGrid(listStudent);
                CountStudent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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


        private void button1_Click(object sender, EventArgs e)
        {
            float diemTB;
            try
            {
                List<Student> studentList = db.Students.ToList();
                if (studentList.Any(s => s.StudentID == textBox1.Text ) ) {
                    MessageBox.Show("Mã sinh viên đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (textBox1.Text.Length != 10 || !long.TryParse(textBox1.Text, out _))
                {
                    MessageBox.Show("Mã số sinh viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (textBox2.Text.Length < 3 || textBox2.Text.Length > 100 || textBox2.Text.Any(c => !char.IsLetter(c) && c != ' '))
                {
                    MessageBox.Show("Họ tên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (!float.TryParse(textBox3.Text, out diemTB) || diemTB < 0 || diemTB > 10)
                {
                    MessageBox.Show("Điểm trung bình không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var newStudent = new Student
                {
                    StudentID = textBox1.Text,
                    FullName = textBox2.Text,
                    Gender = radioButton1.Checked ? "Male" : "Female",
                    FacultyID = int.Parse(comboBox1.SelectedValue.ToString()),
                    AverageScore = double.Parse(textBox3.Text)
                };

                db.Students.Add(newStudent);
                db.SaveChanges();

                BindGrid(db.Students.ToList());
                MessageBox.Show("Thêm sinh viên thành công");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CountStudent()
        {
            int maleCount = 0;
            int femaleCount = 0;
            foreach(Student student in db.Students)
            {
                if(student.Gender == "Male")
                {
                    maleCount++;
                }else if(student.Gender == "Female")
                {
                    femaleCount++;
                }
            }
            textBox4.Text = maleCount.ToString();
            textBox5.Text = femaleCount.ToString();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = selectedRow.Cells[0].Value.ToString();
                textBox2.Text = selectedRow.Cells[1].Value.ToString();
                string gender = selectedRow.Cells[2].Value.ToString();
                if(gender == "Male")
                {
                    radioButton1.Checked = true;
                }else
                {
                    radioButton2.Checked = true;
                }
                textBox3.Text = selectedRow.Cells[3].Value.ToString();
                comboBox1.Text = selectedRow.Cells[4].Value.ToString();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<Student> studentList = db.Students.ToList();
                var student = studentList.FirstOrDefault(s => s.StudentID == textBox1.Text);
                if(student != null)
                {
                    MessageBox.Show("Bạn có muốn xoá không?","Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    db.Students.Remove(student);
                    db.SaveChanges();
                    BindGrid(db.Students.ToList());
                    CountStudent();
                    MessageBox.Show("Xoá sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }else
                {
                    MessageBox.Show("Sinh viên không tìm thấy", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<Student> studentList = db.Students.ToList();

                var student = studentList.FirstOrDefault(s => s.StudentID == textBox1.Text);
                if (student != null)
                {
                    if (studentList.Any(s => s.StudentID == textBox1.Text && s.StudentID != student.StudentID))
                    {
                        MessageBox.Show("Mã sinh viên đã tồn tại. Vui lòng nhập mã số khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    student.FullName = textBox2.Text;
                    student.Gender = radioButton1.Checked? "Male": "Female";
                    student.FacultyID = int.Parse(comboBox1.SelectedValue.ToString());
                    student.AverageScore = double.Parse(textBox3.Text);

                    // Save changes to the database
                    db.SaveChanges();

                    // Reload the data
                    BindGrid(db.Students.ToList());
                    CountStudent();

                    MessageBox.Show("Chỉnh sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh viên không tìm thấy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}
