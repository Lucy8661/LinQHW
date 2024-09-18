using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics.Eventing.Reader;
using LinqLabs;
using LinqLabs.NWDataSetTableAdapters;
using System.Diagnostics;

namespace Starter
{
    public partial class FrmLINQ_To_XXX : Form
    {
        public FrmLINQ_To_XXX()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            IEnumerable<IGrouping<int, int>> q = from x in nums
                                                 group x by (x % 2);

            this.dataGridView1.DataSource = q.ToList(); //會得到q的KEY屬性

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //splite => Apply => Combine
            var q = from x in nums
                    group x by x % 2 == 0 ? "偶數" : "奇數" into g //將分群內容存到g
                    select new { MyKey = g.Key, Mycount = g.Count(), MyGroup = g, MyAvg = g.Average() }; //匿名型別寫不出確切型別，所以用var

            this.dataGridView1.DataSource = q.ToList(); //會得到q的KEY屬性

            //===========================================================
            foreach (var group in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.MyKey.ToString() + $"{group.Mycount}個");
                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());
                }
            }
            chart1.DataSource = q.ToList();
            chart1.Series[0].XValueMember = "Mykey";
            chart1.Series[0].YValueMembers = "Mycount";
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            chart1.Series[1].XValueMember = "Mykey";
            chart1.Series[1].YValueMembers = "MyAvg";
            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //splite => Apply => Combine
            var q = from x in nums
                    group x by MyKey(x) into g //將分群內容存到g
                    select new { MyKey = g.Key, Mycount = g.Count(), MyGroup = g, MyAvg = g.Average() };

            this.dataGridView1.DataSource = q.ToList(); //會得到q的KEY屬性

            foreach (var group in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.MyKey.ToString() + $"{group.Mycount}個");
                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());
                }
            }
        }

        private string MyKey(int n)
        {
            if (n < 5)
            { return "Small"; }
            else if (n < 10)
            { return "median"; }
            else
            { return "large"; }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(@"C:\Windows");
            FileInfo[] fileInfo = dir.GetFiles();

            var q = from f in fileInfo
                    group f by f.Extension into g
                    orderby g.Count() descending
                    select new { g.Key, Count = g.Count() };

            dataGridView1.DataSource = q.ToList();

        }

        private void button12_Click(object sender, EventArgs e)
        {
            ordersTableAdapter1.Fill(nwDataSet1.Orders);

            var q = from x in nwDataSet1.Orders
                    group x by x.OrderDate.Year into g
                    orderby g.Key ascending
                    select new { Year = g.Key, Count = g.Count(), Mygroup = g };

            foreach (var group in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.Year + $" ({group.Count}個)");
                foreach (var item in group.Mygroup)
                {
                    node.Nodes.Add(item.OrderDate.ToString() + "\t" + item.CustomerID.ToString());
                }
            }

            dataGridView2.DataSource = q.ToList();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //StreamReader...
            string s = "This is a pen. this is an apple. this is a book.";
            char[] chs = { ' ' };
            string[] words = s.Split(chs);

            var q = from x in words
                    where x != " "
                    group x by x.ToUpper() into g
                    select new { g.Key, Count = g.Count() };

            dataGridView1.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(@"C:\Windows");
            FileInfo[] fileInfo = dir.GetFiles();

            var q = from f in fileInfo
                    let s = f.Extension  //用let暫存資料
                    where s == ".log"
                    select f;

            dataGridView1.DataSource = q.ToList();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            int[] nums1 = { 1, 2,4,5,67,2,4};
            int[] nums2 = {2,4,11,111,12 };

            //集合運算子 Distinct / Union / Intersect / Except
            IEnumerable<int> q;
            q = nums1.Intersect(nums2); //交集
            q = nums1.Union(nums2);  //聯集
            q = nums1.Distinct();  //唯一值

            //切割運算子 Take / Skip
            q = q.Take(2);

            //數量詞作業 Any / All / Contains
            bool result;
            result = nums1.Any(n=>n > 4);
            result = nums1.All(n=>n > 4);
            result = nums1.Contains(999);

            //單一元素運算子
            //First / Last / Single / ElementAt
            //FirstOrDefault / LastOrDefault / SingleOrDefault / ElementAtOrDefault
            int N = nums1.First();
            N = nums1.Last();
            //N = nums1.ElementAt(32); //Exception
            N=nums1.ElementAtOrDefault(32);

            //產生作業 : Generation -Range / Repeat / Empty DefaultIfEmpty
            RangeTest();
        }

        private void RangeTest()
        {
            IEnumerable<int> q1 = Enumerable.Repeat(60,100);
            dataGridView2.DataSource= q1.Select(x => new {x}).ToList(); //要用new {n}，因為datagridview是繫結屬性

            //=====================================
            
            var source = Enumerable.Range(1, 100000000);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //PLINQ AsParallel()
            //什麼是平行查詢?
            //主要差別在於PLINQ會嘗試充分運用系統上的所有處理器
            //它的作法是將資料來源分割成多個區段，然後以平行方式，以個別的背景工作執行緒在多個處理器上對每個區段執行查詢
            //在許多情況下，平行執行可讓查詢速度快許多

            var q2 = from n in source.AsParallel()
                     where n % 2 == 0
                     orderby n
                     select new { N = n };
            dataGridView1.DataSource= q2.ToList();
            stopwatch.Stop();
            double seconds = stopwatch.Elapsed.TotalSeconds;
            MessageBox.Show("seconds=" + seconds);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            productsTableAdapter1.Fill(nwDataSet1.Products);

            var q = from p in nwDataSet1.Products
                    group p by p.CategoryID into g 
                    select new { CategoryID = g.Key,AbgUnitPrice = g.Average(p=>p.UnitPrice) };
            dataGridView1.DataSource = q.ToList();

            //======================
            //join
            categoriesTableAdapter1.Fill(nwDataSet1.Categories);

            var q2 = from c in nwDataSet1.Categories
                     join p in nwDataSet1.Products
                     on c.CategoryID equals p.CategoryID
                     //select new { CategoryID = c.CategoryID,c.CategoryName,p.ProductID,p.ProductName,p.UnitPrice};
                     group p by c.CategoryName into g
                     select new { Category  = g.Key, AvgUnitPrice=g.Average(n=>n.UnitPrice)};

            dataGridView2.DataSource = q2.ToList();
        }
    }
}
