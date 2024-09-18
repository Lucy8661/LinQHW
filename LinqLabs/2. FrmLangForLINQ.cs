using LinqLabs.作業;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmLangForLINQ : Form
    {
        public FrmLangForLINQ()
        {
            InitializeComponent();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //C# 具名方法
            this.buttonX.Click += buttonX_Click;
            this.buttonX.Click += aaa;

            //C# 2.0 匿名方法
            this.buttonX.Click += delegate (object sender1, EventArgs e1)
            {
                MessageBox.Show("匿名方法");
            };

            //C# 3.0 lambda =>goes to
            this.buttonX.Click += (object sender1, EventArgs e1) =>
            {
                MessageBox.Show("lambda運算式");
            };

            this.button9.Click += Button9_Click;

        }
        delegate bool MyDelegate(int n);

        //Step 1: create delegate class
        //Step 2: create delegate object
        //Step 3: Invoke method / call method
        private void Button9_Click(object sender, EventArgs e)
        {
            //C# 具名方法
            //==============================================
            MyDelegate myDelegate = new MyDelegate(Test);
            bool result = myDelegate(7);

            myDelegate = IsEven;
            //result = myDelegate(8);
            result = myDelegate.Invoke(8);

            //C#  2.0 匿名方法
            //=============================================
            myDelegate = delegate (int n)
                        {
                            return n > 5;
                        };
            result = myDelegate.Invoke(9);

            //C# 3.0 lambda =>goes to
            //==============================================
            myDelegate = n => n > 5;
            result = myDelegate(10);
            MessageBox.Show(result.ToString());
        }

        private bool IsEven(int n)
        {
            return n % 2 == 1;
        }

        private bool Test(int n)
        {
            return n > 5;
        }

        private void buttonX_Click(object sender, EventArgs e)
        {
            MessageBox.Show("buttonX_Click");
        }

        private void aaa(object sender, EventArgs e)
        {
            MessageBox.Show("aaa");
        }


        private void button4_Click(object sender, EventArgs e)
        {
            int n1 = 100;
            int n2 = 200;
            Swap<int>(ref n1, ref n2);
            MessageBox.Show(n1 + "," + n2);

        }

        private void Swap<T>(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }
        private void Swap(ref int x, int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int n1 = 100;
            int n2 = 200;
            Swap(ref n1, ref n2);
            MessageBox.Show(n1 + "," + n2);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            MyWhere(nums, test);
            List<int> EvenList = MyWhere(nums, n => n % 2 == 0); //方法的參數二，代入lambda匿名函數
            listBox1.Items.Add(EvenList);

        }

        /// <summary>
        /// 將參數一代入參數二的函數，將結果作判別，參數二可以用匿名函式
        /// </summary>
        /// <param name="n"></param>
        /// <param name="myDelegate"></param>
        /// <returns></returns>
        List<int> MyWhere(int[] n, MyDelegate myDelegate)
        {
            List<int> list = new List<int>();
            foreach (int i in n)
            {
                if (myDelegate(i))
                {
                    list.Add(i);
                }
            }
            return list;
        }

        int add(int n1, int n2)
        {
            return n1 + n2;
        }
        bool test(int n1)
        {
            return n1 > 5;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            MyWhere(nums, test);
            IEnumerable<int> q = nums.Where(n => n % 2 == 0).ToList();
            listBox1.Items.Clear();
            foreach (int i in q)
            {
                listBox1.Items.Add(i);
            }

            //===================================================

            List<string> strings = new List<string> { "aaa", "bbbbb", "cc" };
            IEnumerable<string> q2 = strings.Where(n => n.Length > 3);
            foreach (string i in q2)
            {
                listBox1.Items.Add(i);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            IEnumerable<int> q = MyIterator(nums, n => n > 5);//只是定義，在將列舉進list時才會執行
            foreach (int i in q)
            {
                listBox1.Items.Add(i);
            }


            //有?，表示實質型別可以為空值
            bool? result;
            result = null;
        }
        IEnumerable<int> MyIterator(int[] n, MyDelegate myDelegate)
        {
            foreach (int i in n)
            {
                if (myDelegate(i))
                {
                    yield return i;
                }
            }

        }

        private void button41_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            List<MyPoint> list = new List<MyPoint>
            {
                new MyPoint { p1 = 3,p2 = 3}, //{}裡面是屬性
                new MyPoint { p1 = 31,p2 = 3},
                new MyPoint { p1 = 33,p2 = 3},
                new MyPoint { p1 = 3444,p2 = 3}
            };
            dataGridView2.DataSource = list;
        }

        private void button43_Click(object sender, EventArgs e)
        {
            var pt1 = new { p1 = 100, p2 = 200, p3 = 999 }; //建立唯獨屬性
            var pt2 = new { p1 = 100, p2 = 200, p3 = 999 };
            var pt3 = new { x = 7, y = 8 };
            listBox1.Items.Add(pt1.GetType());
            listBox1.Items.Add(pt2.GetType());
            listBox1.Items.Add(pt3.GetType());

            //=============================
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var q = from x in nums
                    where x > 5
                    select new
                    {
                        Cube = x * x * x,
                        Square = x * x,
                        N = x
                    };

            var q3 = nums.Where(n => n > 5).Select(n => new
            {
                Cube = n * n * n,
                Square = n * n,
                N = n
            });
            dataGridView1.DataSource = q3.ToList();

            //===========================================
            this.productsTableAdapter1.Fill(nwDataSet1.Products);
            var q2 = from p in nwDataSet1.Products
                     where p.UnitPrice > 30
                     select new
                     {
                         ID = p.ProductID,
                         p.ProductName,
                         p.UnitPrice,
                         p.UnitsInStock,
                         TotalPrice = $"{p.UnitPrice * p.UnitsInStock:C2}"
                     };
            dataGridView2.DataSource = q2.ToList();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            string s = "abcd";
            int count = s.WordCount();
            MessageBox.Show("count=" + count);

            //===================================
            //擴充方法
            //對既有的型別擴充方法
            string s1 = "123456789";
            char ch = s1.Chars(2);
            MessageBox.Show("ch=" + ch);

            ch = MyStringExtend.Chars(s1, 2);
            MessageBox.Show("ch=" + ch);
        }
    }

    //擴充方法
    public static class MyStringExtend //擴充類別
    {
        public static int WordCount(this string s) //擴充方法，參數一定要this
        {
            return s.Length;
        }

        public static char Chars(this string s, int index) //第一個方法一定要this
        {
            //return s.ElementAt(index);
            return s[index];
        }
    }

    public class MyPoint //類別
    {
        public int p1 { get; set; }
        public int p2 { get; set; }
        //public void MyPoint(int p1, int p2)
        //{
        //    p1 = p1;
        //    this.p2 = p2;
        //}
    }
}
