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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Lab05
{
    public partial class Form2 : Form
    {
        StudentContextDB db;
        public Form2()
        {
            InitializeComponent();
            db = new StudentContextDB();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.Show();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox4.Text = "0";
            comboBox1.Items.Add("Tăng dần");
            comboBox1.Items.Add("Giảm dần");
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            try
            {
                List<Faculty> listFalcuty = db.Faculties.ToList();
                BindGrid(listFalcuty);
                CountTGS();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindGrid(List<Faculty> facultyList)
        {
            dataGridView2.Rows.Clear();
            foreach (var item in facultyList)
            {
                int index = dataGridView2.Rows.Add();
                dataGridView2.Rows[index].Cells["Col_MaKhoa"].Value = item.FacultyID;
                dataGridView2.Rows[index].Cells["Col_TenKhoa"].Value = item.FacultyName;
                dataGridView2.Rows[index].Cells["Col_TGS"].Value = item.TotalProfessor;


            }
        }

        private void CountTGS()
        {
            textBox4.Text = db.Faculties.Sum(s => s.TotalProfessor).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<Faculty> facultyList = db.Faculties.ToList();


                if (facultyList.Any(s => s.FacultyID == int.Parse(textBox1.Text)))
                {
                    MessageBox.Show("Mã số đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (textBox1.Text.Any(c => !char.IsDigit(c)))
                {
                    MessageBox.Show("Mã khoa không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (textBox2.Text.Length < 3 || textBox2.Text.Length > 100 || textBox2.Text.Any(c => !char.IsLetter(c) && c != ' '))
                {
                    MessageBox.Show("Tên khoa không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (int.Parse(textBox3.Text) < 0 || int.Parse(textBox3.Text) > 15)
                {
                    MessageBox.Show("Tổng giáo sư không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var newFaculty = new Faculty()
                {
                    FacultyID = int.Parse(textBox1.Text),
                    FacultyName = textBox2.Text,
                    TotalProfessor = int.Parse(textBox3.Text)
                };

                db.Faculties.Add(newFaculty);
                db.SaveChanges();
                BindGrid(db.Faculties.ToList());
                CountTGS();
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];
                textBox1.Text = selectedRow.Cells[0].Value.ToString();
                textBox2.Text = selectedRow.Cells[1].Value.ToString();
                textBox3.Text = selectedRow.Cells[2].Value.ToString();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<Faculty> facultyList = db.Faculties.ToList();

                var faculty = facultyList.FirstOrDefault(s => s.FacultyID == int.Parse(textBox1.Text));
                if (faculty != null)
                {
                    if (facultyList.Any(s => s.FacultyID == int.Parse(textBox1.Text) && s.FacultyID != faculty.FacultyID))
                    {
                        MessageBox.Show("Mã sinh viên đã tồn tại. Vui lòng nhập mã số khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (textBox1.Text.Any(c => !char.IsDigit(c)))
                    {
                        MessageBox.Show("Mã khoa không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (textBox2.Text.Length < 3 || textBox2.Text.Length > 100 || textBox2.Text.Any(c => !char.IsLetter(c) && c != ' '))
                    {
                        MessageBox.Show("Tên khoa không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (int.Parse(textBox3.Text) < 0 || int.Parse(textBox3.Text) > 15)
                    {
                        MessageBox.Show("Tổng giáo sư không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    faculty.FacultyID = int.Parse(textBox1.Text);
                    faculty.FacultyName = textBox2.Text;
                    faculty.TotalProfessor = int.Parse(textBox3.Text);

                    // Save changes to the database
                    db.SaveChanges();

                    // Reload the data
                    BindGrid(db.Faculties.ToList());
                    CountTGS();

                    MessageBox.Show("Chỉnh sửa thông tin Khoa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khoa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<Faculty> facultyList = db.Faculties.ToList();
                var faculty = facultyList.FirstOrDefault(s => s.FacultyID == int.Parse(textBox1.Text));
                if (faculty != null) {
                    MessageBox.Show("Bạn có muốn xóa không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    db.Faculties.Remove(faculty);
                    db.SaveChanges();
                    BindGrid(db.Faculties.ToList());
                    CountTGS();
                    MessageBox.Show("Xóa khoa thành công");
                }else
                {
                    MessageBox.Show("Không tìm thấy khoa", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<Faculty> facultyList = db.Faculties.ToList();
                if (comboBox1.SelectedItem.ToString() == "Tăng dần")
                {
                    facultyList = facultyList.OrderBy(f => f.TotalProfessor).ToList();
                }
                else if (comboBox1.SelectedItem.ToString() == "Giảm dần")
                {
                    facultyList = facultyList.OrderByDescending(f => f.TotalProfessor).ToList();
                }
                BindGrid(facultyList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
