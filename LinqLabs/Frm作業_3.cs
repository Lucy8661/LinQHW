using ServiceStack.Text;
using Starter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LinqLabs.作業
{
    public partial class Frm作業_3 : Form
    {
        private List<Student> students_scores;
        NorthwindEntities dbContext = new NorthwindEntities();
        public Frm作業_3()
        {
            InitializeComponent();


            //hint
            students_scores = new List<Student>
            {
                new Student { Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
                new Student { Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
                new Student { Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
                new Student { Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
                new Student { Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
                new Student { Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },
            };
        }

        public void clearDisplay()
        {
            dataGridView1.DataSource = null;
            treeView1.Nodes.Clear();
        }

        public class Student
        {
            /// <summary>
            /// 姓名
            /// </summary>
            public string Name { get; set; }
            public string Class { get; set; }
            public int Chi { get; set; }
            public int Eng { get; set; }
            public int Math { get; set; }
            public string Gender { get; set; }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            clearDisplay();
            int nCount = 0;
            List<Student> hundred_list = new List<Student>();
            string[] charName = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            Random random = new Random();
            for (int i = 0; i < 26; i++)
            {

                for (int j = 0; j < 26; j++)
                {
                    if (nCount == 100) break;
                    nCount++;
                    Student hundred_student = new Student();
                    hundred_student.Name = "a" + charName[i] + charName[j];
                    hundred_student.Class = "CS_103";
                    hundred_student.Chi = random.Next(0, 101);
                    hundred_student.Eng = random.Next(0, 101);
                    hundred_student.Math = random.Next(0, 101);
                    hundred_student.Gender = random.Next(0, 2) == 0 ? "Male" : "Female";
                    hundred_list.Add(hundred_student);
                }
            }
            // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
            label1.Text = $"待加強(60~69),佳(70~89),優良(90~100)";
            var q = hundred_list.GroupBy(x => interval((x.Chi + x.Eng + x.Math) / 3));

            DataTable dt = new DataTable();
            dt.Columns.Add("Score Range");
            dt.Columns.Add("Name");
            dt.Columns.Add("Math");
            dt.Columns.Add("Chi");
            dt.Columns.Add("Eng");
            dt.Columns.Add("Avg");

            string[] spiltCH = { "優良(90~100)", "佳(70~89)", "待加強(60~69)", "請加油" };

            for (int i = 0; i < spiltCH.Length; i++)
            {
                foreach (var group in q)
                {
                    if (group.Key != spiltCH[i]) continue;
                    TreeNode treeNode = treeView1.Nodes.Add(group.Key + $" ({group.Count()})");
                    foreach (var item in group)
                    {
                        treeNode.Nodes.Add(item.Name);
                        dt.Rows.Add(
                           group.Key,
                           item.Name,
                           item.Math,
                           item.Chi,
                           item.Eng,
                           (item.Chi + item.Math + item.Eng) / 3);
                    }
                }
            }
            dataGridView1.DataSource = dt;
            // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 找出 前面三個 的學員所有科目成績	
            clearDisplay();
            label1.Text = "前面三個 的學員所有科目成績";
            var q = (students_scores.Select(x => new { x.Name, x.Chi, x.Eng, x.Math })).Take(3);
            dataGridView1.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 找出 後面兩個 的學員所有科目成績
            clearDisplay();
            label1.Text = "後面兩個 的學員所有科目成績";
            var q2 = (students_scores.Select(x => new { x.Name, x.Chi, x.Eng, x.Math })).Skip(students_scores.Count - 2).Take(2);
            dataGridView1.DataSource = q2.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績	
            clearDisplay();
            label1.Text = "'aaa','bbb','ccc' 的學員國文英文科目成績";
            var q3 = students_scores
                .Where(x => x.Name is "aaa" || x.Name is "bbb" || x.Name is "ccc")
                .Select(x => new { x.Name, x.Chi, x.Eng });
            dataGridView1.DataSource = q3.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績 
            clearDisplay();
            label1.Text = "'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績";
            string[] names = new string[] { "aaa", "bbb", "ccc" };
            var q4 = students_scores.Where(x => names.Contains(x.Name)).Select(x => new { x.Name, x.Chi, x.Math });
            dataGridView1.DataSource = q4.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 共幾個 學員成績 ?
            clearDisplay();
            var q6 = students_scores.Count();
            label1.Text = $"共{q6}個學員成績";
            dataGridView1.DataSource = students_scores.ToList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	
            clearDisplay();
            label1.Text = "除了 'bbb' 學員的學員的所有成績";
            var q5 = students_scores.Where(x => x.Name != "bbb").Select(x => new { x.Name, x.Chi, x.Eng, x.Math });
            dataGridView1.DataSource = q5.ToList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // 找出學員 'bbb' 的成績
            clearDisplay();
            label1.Text = $"學員 b 的成績";
            IEnumerable<Student> q7 = students_scores.Where(n => n.Name.Contains('b'));
            dataGridView1.DataSource = q7.ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // 數學不及格 ... 是誰 
            clearDisplay();
            label1.Text = $"數學不及格";
            var q8 = students_scores.Where(n => n.Math < 60).Select(n => new { n.Name, n.Math });
            dataGridView1.DataSource = q8.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //個人 sum, min, max, avg
            clearDisplay();
            label1.Text = "個人 sum, min, max, avg";
            var q = students_scores.Select(n => personalCaculate(n));
            dataGridView1.DataSource = q.ToList();
        }

        private object personalCaculate(Student n)
        {
            int sum = (n.Math + n.Chi + n.Eng);
            int avg = sum / 3;
            int min = n.Eng < n.Chi ? (n.Eng < n.Math ? n.Eng : n.Math) : (n.Math < n.Chi ? n.Math : n.Chi);
            int max = Math.Max(n.Math, Math.Max(n.Chi, n.Eng));
            return new
            {
                n.Name,
                n.Chi,
                n.Eng,
                n.Math,
                sum,
                avg,
                min,
                max
            };
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // 統計 每個學生個人成績 並排序
            clearDisplay();
            label1.Text = "統計 每個學生個人成績 並排序";

            IEnumerable<object> q = students_scores
                .Select(x =>
                new
                {
                    x.Name,
                    x.Chi,
                    x.Eng,
                    Math1 = x.Math,
                    Sum = (x.Math + x.Chi + x.Eng),
                    avg = (x.Math + x.Chi + x.Eng) / 3,
                    min = x.Eng < x.Chi ? (x.Eng < x.Math ? x.Eng : x.Math) : (x.Math < x.Chi ? x.Math : x.Chi),
                    max = Math.Max(x.Math, Math.Max(x.Chi, x.Eng)),
                }).OrderByDescending(x => x.Sum);

            dataGridView1.DataSource = q.ToList();
        }

        private string interval(int n)
        {
            if (n <= 100 && n >= 90)
            { return "優良(90~100)"; }
            else if (n <= 89 && n >= 70)
            { return "佳(70~89)"; }
            else if (n <= 69 && n >= 60)
            { return "待加強(60~69)"; }
            else
            { return "請加油"; }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            clearDisplay();
            label1.Text = $"全部成員資料";
            dataGridView1.DataSource = students_scores.ToList();
        }

        private void button38_Click(object sender, EventArgs e)
        {
            label1.Text = button38.Text;
            clearDisplay();
            DirectoryInfo dir = new DirectoryInfo(@"C:\Windows");
            FileInfo[] fileInfo = dir.GetFiles();

            var q = from f in fileInfo
                    orderby f.Length descending
                    group f by groupLength(f.Length) into g
                    select new { g.Key, g, Count = g.Count() };

            foreach (var f in q)
            {
                TreeNode tn = treeView1.Nodes.Add(f.Key.ToString() + $"( {f.Count})");
                foreach (var item in f.g)
                {
                    tn.Nodes.Add(item.ToString());
                }
            }
            //30000,600000
        }

        private string groupLength(long length)
        {
            if (length <= 30000 && length >= 0)
            {
                return "小檔案(<3w)";
            }
            else if (length <= 600000 && length > 30000)
            {
                return "中檔案(3w-60W)";
            }
            else
            {
                return "大檔案(>60W)";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label1.Text = button6.Text;
            clearDisplay();
            DirectoryInfo dir = new DirectoryInfo(@"C:\Windows");
            FileInfo[] fileInfo = dir.GetFiles();
            var q = from f in fileInfo
                    group f by f.CreationTime.Year into g
                    orderby g.Key
                    select new
                    {
                        g.Key,
                        Count = g.Count(),
                        FileLength = from x in g
                                     group x by groupLength(x.Length) into fx
                                     select new { fx.Key, LengthCount = fx.Count(), fx }
                    };

            dataGridView1.DataSource = fileInfo.ToList();

            foreach (var f in q)
            {
                TreeNode treeNode = treeView1.Nodes.Add(f.Key + $"({f.Count})");
                foreach (var fxkey in f.FileLength)
                {
                    TreeNode tr = new TreeNode(fxkey.Key);
                    foreach (var fx in fxkey.fx)
                    {
                        tr.Nodes.Add(fx.ToString());
                    }
                    treeNode.Nodes.Add(tr);
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            label1.Text = "NW Products 低中高 價產品 ";
            clearDisplay();
            var q = from x in dbContext.Products.AsParallel()
                    group x by MyClass.ProductLevel(Convert.ToInt32(x.UnitPrice)) into g
                    select new { g.Key, Count = g.Count(), g };
            dataGridView1.DataSource = dbContext.Products.OrderBy(n => n.UnitPrice).ToList();

            foreach (var p in q)
            {
                TreeNode treeNode = treeView1.Nodes.Add(p.Key + $"({p.Count})");
                foreach (var pName in p.g)
                {
                    treeNode.Nodes.Add($"{pName.ProductName}  {pName.UnitPrice}");
                }
            }
        }

        public static class MyClass
        {
            public static string ProductLevel(int price)
            {
                if (price < 0)
                {
                    return "異常";
                }
                else if (price <= 50)
                {
                    return "低價產品";
                }
                else if (price <= 100)
                {
                    return "中價產品";
                }
                else
                {
                    return "高價產品";
                }
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            label1.Text = " Orders -  Group by 年";
            clearDisplay();
            var q = dbContext.Orders.GroupBy(n => n.OrderDate.Value.Year).Select(n => new { 年 = n.Key, Count = n.Count(), n });
            foreach (var i in q)
            {
                TreeNode treeNode = treeView1.Nodes.Add(i.年.ToString() + "年");
                foreach (var j in i.n)
                {
                    treeNode.Nodes.Add($"ID:{j.OrderID} 日期:{j.OrderDate}");
                }
            }
            dataGridView1.DataSource = q.ToList();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            label1.Text = button14.Text;
            clearDisplay();
            var q = from x in dbContext.Orders
                    group x by x.OrderDate.Value.Year into g
                    select new
                    {
                        g.Key,
                        Count = g.Count(),
                        groupMonth = from s in g
                                     group s by s.OrderDate.Value.Month into m
                                     select new { m.Key, Count = m.Count(), m }
                    };

            foreach (var p in q)
            {
                TreeNode treeNode = treeView1.Nodes.Add(p.Key + $"({p.Count})");
                foreach (var gm in p.groupMonth)
                {
                    TreeNode tr = new TreeNode(gm.Key + $" 月 ({gm.Count}筆)");
                    foreach (var gml in gm.m)
                    {
                        tr.Nodes.Add($"ID:{gml.OrderID} 日期:{gml.OrderDate.Value.Date}");
                    }
                    treeNode.Nodes.Add(tr);
                }
            }

            dataGridView1.DataSource = dbContext.Orders.AsEnumerable().GroupBy(n => new { n.OrderDate.Value.Year, n.OrderDate.Value.Month, n })
                .Select(n => new { 類 = $"{n.Key.Year}年{n.Key.Month}月", ID = n.Key.n.OrderID, 訂單日期 = n.Key.n.OrderDate })
                .ToList();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            label1.Text = button16.Text;
            clearDisplay();
            var q = (from x in dbContext.Order_Details
                     orderby x.ProductID
                     select new
                     {
                         x.Product.ProductName,
                         x.Quantity,
                         x.UnitPrice,
                         //x
                     })
                     .GroupBy(n => new { n.ProductName })
                     .Select(n => new { n.Key.ProductName, total = n.Sum(y => y.Quantity * y.UnitPrice) })
                     .ToList();


            //SelectMany對陣列、集合
            List<ICollection<Order_Detail>> a1 = dbContext.Products.Select(x => x.Order_Details).ToList();
            List<Order_Detail> a2 = dbContext.Products.SelectMany(x => x.Order_Details).ToList();

            var products = a2
                .GroupBy(data => data.Product.ProductName)
                .Select(x => new { x.Key, totoal = x.Sum(y => y.Quantity * y.UnitPrice) })
                .OrderBy(x => x.Key)
                .ToList();

            var a3 = dbContext.Order_Details
                .GroupBy(x => x.Product.ProductName)
                .Select(x => new { x.Key, totoal = x.Sum(y => y.Quantity * y.UnitPrice) })
                .OrderBy(x => x.Key)
                .ToList();

            IList<string> tsst = new List<string> { "A1", "B2" };
            var aa11 = tsst.SelectMany(x => x);

            //SelectMany 拆開一層LIST
            List<List<string>> test1 = new List<List<string>> {
                new List<string> { "A1" },
                new List<string> { "B2" } };
            List<string> test3 = test1.SelectMany(x => x).ToList();

            dataGridView1.DataSource = q.ToList();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            label1.Text = button16.Text;
            clearDisplay();

            var q = dbContext.Products.OrderByDescending(n => n.UnitPrice).Select(n => new { n.ProductName, n.Category.CategoryName, n.UnitPrice }).Take(5);
            dataGridView1.DataSource = q.ToList();

        }

        private void button17_Click(object sender, EventArgs e)
        {
            label1.Text = button16.Text;
            clearDisplay();

            var q = dbContext.Products.Where(n => n.UnitPrice > 300).Select(n => new { n.ProductName, n.UnitPrice });
            if (q.Count() > 0)
            {
                dataGridView1.DataSource = q.ToList();
            }
            else { MessageBox.Show("沒有產品單價大於300"); }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            var q = dbContext.Orders
                .SelectMany(n => n.Order_Details)
                .GroupBy(n => new { n.Order.EmployeeID })
                .Select(n => new { n.Key.EmployeeID,total = n.Sum(x => x.UnitPrice * x.Quantity) })
                .OrderByDescending(n=>n.total)
                .Take(5);

            dataGridView1 .DataSource = q.ToList();
        }
    }
}
