using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmHelloLinq : Form
    {
        public FrmHelloLinq()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //syntax sugar
            //foreach (int i in nums)
            //{
            //    this.listBox1.Items.Add(i);
            //}
            //string.Format... =語法糖 $"{}"

            //C# 內部轉譯
            System.Collections.IEnumerator en = nums.GetEnumerator();
            while (en.MoveNext())
            {
                this.listBox1.Items.Add(en.Current);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<int> nums = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int>.Enumerator en = nums.GetEnumerator();
            while (en.MoveNext())
            {
                this.listBox1.Items.Add(en.Current);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Step 1:define Data Source object
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            //Step 2:define Query，x的型別不用打，它會自動判定

            IEnumerable<int> q = from x in nums
                                     //where x > 5 && x <= 10
                                     //where x < 5 || x >= 10
                                     //where x % 2 == 0
                                 select x;

            //Step 3:execute Query=>foreach (...in q...)
            this.listBox1.Items.Clear();
            foreach (int x in q)
            {
                this.listBox1.Items.Add(x);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            IEnumerable<int> q = from x in nums
                                 where IsEven(x)
                                 select x;


            this.listBox1.Items.Clear();
            foreach (int x in q)
            {
                this.listBox1.Items.Add(x);
            }

        }
        bool IsEven(int x)
        {
            return x % 2 == 0;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Step 1:define Data Source object
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            //Step 2:define Query，x的型別不用打，它會自動判定
            IEnumerable<Point> q = from x in nums
                                   where x > 5 && x <= 10
                                   select new Point(x, x * x);

            //Step 3:execute Query=>foreach (...in q...)
            this.listBox1.Items.Clear();
            //foreach (Point x in q)
            //{
            //    this.listBox1.Items.Add(x);
            //}

            //Step 3:execute Query=>ToXXX()
            List<Point> list = q.ToList(); //foreach (...in q...)

            this.listBox1.DataSource = list;
            this.dataGridView1.DataSource = list;
            this.chart1.DataSource = list;
            this.chart1.Series[0].XValueMember = "X";
            this.chart1.Series[0].YValueMembers = "Y";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            this.chart1.Series[0].BorderWidth = 4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] names = { "aaa", "Apple", "piniApple", "bbb" };
            IEnumerable<string> q = from x in names
                                    where (x.ToLower()).Contains("apple")
                                    orderby x descending
                                    select x;

            List<string> strings = q.ToList();
            this.listBox1.Items.Clear();
            this.listBox1.DataSource = strings;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);

            var q = from x in this.nwDataSet1.Products
                    where x.UnitPrice > 30 && x.ProductName.StartsWith("P")
                    select x;
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);

            var q = from x in this.nwDataSet1.Orders
                    where x.OrderDate.Year.Equals(1997) && !x.IsShipRegionNull() && x.OrderDate.Month > 3 && x.OrderDate.Month < 6
                    orderby x.OrderDate ascending
                    select x;
            //var q = from x in this.nwDataSet1.Orders
            //        where x.OrderDate.Year.Equals(1997) && !x.IsShipRegionNull() && x.OrderDate.Month > 3 && x.OrderDate.Month < 6
            //        orderby x.OrderDate ascending
            //        select new
            //        {
            //            x.OrderID,
            //            x.CustomerID,
            //            x.OrderDate,
            //            x.Freight,
            //            Region = x["ShipRegion"] == DBNull.Value ? "無資料" : x.ShipRegion
            //        };

            this.dataGridView1.DataSource = q.ToList();
        }
    }
}