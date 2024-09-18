using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_2 : Form
    {
        public Frm作業_2()
        {
            InitializeComponent();
        }

        private void Frm作業_2_Load(object sender, EventArgs e)
        {
            productPhotoTableAdapter1.Fill(adventureWorks1.ProductPhoto);
            var q = (adventureWorks1.ProductPhoto.Select(n => n.ModifiedDate.Year)).Distinct();
            comboBox3.DataSource = q.ToList();
        }


        private void button11_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = adventureWorks1.ProductPhoto;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            
            var q = from x in adventureWorks1.ProductPhoto
                    where x.ModifiedDate <= Convert.ToDateTime(dateTimePicker2.Text) && x.ModifiedDate >= Convert.ToDateTime(dateTimePicker1.Text)
                    select x;
            dataGridView1.DataSource = q.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            if (!Int32.TryParse(comboBox3.Text.Trim(), out _))
            {
                return;
            }
            var q = from x in adventureWorks1.ProductPhoto
                    where x.ModifiedDate.Year.ToString() == comboBox3.Text
                    select x;
            dataGridView1.DataSource = q.ToList();
        }

        bool choose(DateTime xDate, string target)
        {
            int month1 = 0;
            int month2 = 0;
            switch (target)
            {
                case "第一季":
                    month1 = 1;
                    month2 = 3;
                    break;
                case "第二季":
                    month1 = 4;
                    month2 = 6;
                    break;
                case "第三季":
                    month1 = 7;
                    month2 = 9;
                    break;
                case "第四季":
                    month1 = 10;
                    month2 = 12;
                    break;
            }
            return xDate.Month >= month1 && xDate.Month <= month2;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            dataGridView1.DataSource = null;
            string[] strings = { "第一季", "第二季", "第三季", "第四季" };
            int count = 0;
            if (!Int32.TryParse(comboBox3.Text, out _) || string.IsNullOrEmpty(comboBox3.Text) || !strings.Contains(comboBox2.Text))
            {
                return;
            }
            
            var q = from x in adventureWorks1.ProductPhoto
                    where x.ModifiedDate.Year.ToString() == comboBox3.Text && choose(Convert.ToDateTime(x.ModifiedDate), comboBox2.Text)
                    select x;
            dataGridView1.DataSource = q.ToList();
            count = q.ToList().Count;
            label1.Text = $"西元{comboBox3.Text}年 {comboBox2.Text} 共有{count}筆";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex<0) return;
            DataTable dt = dataGridView1.DataSource as DataTable;
            int photoID = Convert.ToInt32(dt.Rows[e.RowIndex][0]);
            var q = adventureWorks1.ProductPhoto.Where(n => n.ProductPhotoID == photoID).Select(n => n.LargePhoto).ToList();
            Stream stream = new MemoryStream(q[0]);
            pictureBox1.Image = Bitmap.FromStream(stream);
        }
    }
}


