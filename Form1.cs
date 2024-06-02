using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;

namespace QLNS
{
    public partial class Form1 : Form
    {
        QLNhanVienDataContext dc = new QLNhanVienDataContext();
        Table<NHANVIEN> nhanViens;
        Table<PHONGBAN> phongBans;
        public Form1()
        {
            InitializeComponent();
        }
        #region Phương thức
        public void loadPB()
        {
            //Khai báo nguồn dữ liệu
            phongBans = dc.GetTable<PHONGBAN>();
            //Tạo câu truy vấn Linq
            var query = from pb in phongBans
                        select new { mapb = pb.MaPB, tenpb = pb.TenPB };
            //Thực thi truy vấn
            cboPhongBan.DataSource = query;
            cboPhongBan.DisplayMember = "tenpb";
            cboPhongBan.ValueMember = "mapb";
        }
        public void loadData()
        {
            //Khai báo nguồn dữ liệu
            nhanViens = dc.GetTable<NHANVIEN>();
            //Tạo câu truy vấn Linq
            var query = from nv in nhanViens
                        select nv;
            //Thực thi truy vấn
            dataGridView1.DataSource = query;
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            loadPB();
            loadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                NHANVIEN nv = new NHANVIEN();
                nv.MaNV = txtMaNV.Text;
                nv.TenNV = txtHoTen.Text;
                nv.NgaySinh = Convert.ToDateTime(dtpNgaySinh.Text);
                nv.GioiTinh = rdNam.Checked == true ? true : false;
                nv.SoDT = txtSDT.Text;
                nv.MaPB = cboPhongBan.SelectedValue.ToString();

                nhanViens.InsertOnSubmit(nv);
                dc.SubmitChanges();
                loadData();

            }
            catch
            {
                MessageBox.Show("Bị lỗi trùng mã nhân viên");
            }
        }
        //Xóa nhân viên được chọn trên DataGridView
        private void btnXoa_Click(object sender, EventArgs e)
        {
            int i = dataGridView1.CurrentCell.RowIndex;
            string ma = dataGridView1.Rows[i].Cells[0].Value.ToString();
            //Câu lệnh truy vấn linq
            var query = from nv in nhanViens
                        where nv.MaNV == ma
                        select nv;
            //Thực thi truy vấn
            foreach (var nv in query)
            {
                dc.NHANVIENs.DeleteOnSubmit(nv);
            }
            //Cập nhật xuống database
            dc.SubmitChanges();
            //Load lại dữ liệu
            loadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dataGridView1.CurrentCell.RowIndex;
            txtMaNV.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            txtHoTen.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            dtpNgaySinh.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();

            string gt = dataGridView1.Rows[i].Cells[3].Value.ToString();
            if (gt == "True")
            {
                rdNam.Checked = true;
            }
            else
                rdNu.Checked = true;
            txtSDT.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            cboPhongBan.SelectedValue = dataGridView1.Rows[i].Cells[5].Value.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaNV.Text != "")
                {
                    //Câu lệnh truy vấn linq
                    var query = from nv in nhanViens
                                where nv.MaNV == txtMaNV.Text
                                select nv;
                    //Thực thi truy vấn
                    foreach (var nv in query)
                    {
                        nv.MaNV = txtMaNV.Text;
                        nv.TenNV = txtHoTen.Text;
                        nv.NgaySinh = Convert.ToDateTime(dtpNgaySinh.Text);
                        nv.GioiTinh = rdNam.Checked == true ? true : false;
                        nv.SoDT = txtSDT.Text;
                        nv.MaPB = cboPhongBan.SelectedValue.ToString();
                    }
                    dc.SubmitChanges();
                    loadData();
                }
                else
                    MessageBox.Show("Mã nhân viên bị rỗng");
            }
            catch
            {
                MessageBox.Show("Bị lỗi trùng mã nhân viên");
            }
        }
    }  
}
