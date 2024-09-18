using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_1 : Form
    {
        private int ProductsCount = -10;
        private int gap = 0;
        public Frm作業_1()
        {
            InitializeComponent();
        }

        private void Frm作業_1_Load(object sender, EventArgs e)
        {
            lblMaster.Text = "";
            lblDetails.Text = "";

            productsTableAdapter1.Fill(nwDataSet1.Products);
            ordersTableAdapter1.Fill(nwDataSet1.Orders);
            order_DetailsTableAdapter1.Fill(nwDataSet1.Order_Details);

            var years = (from x in nwDataSet1.Orders
                         orderby x.OrderDate.Year ascending
                         select x.OrderDate.Year).Distinct();
            foreach (var x in years)
            {
                comboBox1.Items.Add(x.ToString());
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "FileInfo[].Log  擋";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            //files[0].CreationTime
            //this.dataGridView1.DataSource = files;

            IEnumerable<FileInfo> logFiles = from x in files
                                             where x.Name.EndsWith(".log")
                                             select x;

            List<FileInfo> log = logFiles.ToList();
            this.dataGridView1.DataSource = log;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "FileInfo[]- 2023 Created-oerder";

            dataGridView1.DataSource = null;
            DirectoryInfo dir = new DirectoryInfo(@"C:\");
            //DirectoryInfo[] files = dir.GetDirectories();//取得子目錄檔名
            FileInfo[] files = dir.GetFiles();

            var logFiles = from x in files
                           where x.LastWriteTime.Year == 2024
                           select new
                           {
                               x.Name,
                               x.CreationTime,
                               檔案大小 = (x.Length / 1024) + " KB"
                           };
            dataGridView1.DataSource = logFiles.ToList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "All訂單";
            dataGridView1.DataSource = null;

            this.dataGridView1.DataSource = nwDataSet1.Orders;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Int32.TryParse(comboBox1.Text, out _))
                return;
            lblMaster.Text = $"{comboBox1.Text}年訂單";
            lblDetails.Text = $"{comboBox1.Text}年訂單明細";

            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;

            var findorder = from x in nwDataSet1.Orders
                            where x.OrderDate.Year == Convert.ToInt32(comboBox1.Text)
                            select x;

            var findOrderDetail = from x in findorder
                                  join y in nwDataSet1.Order_Details
                                  on x.OrderID equals y.OrderID
                                  where !x.IsShipRegionNull()
                                  select x;

            this.dataGridView1.DataSource = findorder.ToList();
            this.dataGridView2.DataSource = findOrderDetail.ToList();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (!Int32.TryParse(textBox1.Text, out gap))
            {
                return;
            }

            lblMaster.Text = "產品";
            dataGridView1.DataSource = null;
            if ((ProductsCount -= gap) < 0)
            {
                ProductsCount = 0;
            }
            var q = this.nwDataSet1.Products.Skip(ProductsCount).Take(gap);
            dataGridView1.DataSource = q.ToList();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (!Int32.TryParse(textBox1.Text, out gap))
            {
                return;
            }

            lblMaster.Text = "產品";
            dataGridView1.DataSource = null;

            if (nwDataSet1.Products.Count() - ProductsCount > gap)
                ProductsCount += gap;
            var q = this.nwDataSet1.Products.Skip(ProductsCount).Take(gap);
            dataGridView1.DataSource = q.ToList();

            //Distinct()
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "FileInfo[] - 大檔案";
            dataGridView1.DataSource = null;
            DirectoryInfo dir = new DirectoryInfo(@"C:\");
            FileInfo[] files = dir.GetFiles();
            var q = files.Where(n =>n.Length>(1024*1024) );
            dataGridView1.DataSource = q.ToList();

        }
    }
}
