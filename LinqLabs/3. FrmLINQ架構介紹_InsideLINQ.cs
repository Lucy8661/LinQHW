using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmLINQ架構介紹_InsideLINQ : Form
    {
        public FrmLINQ架構介紹_InsideLINQ()
        {
            InitializeComponent();
        }

        //很多傳統型別沒有實作擴充方法，
        private void button30_Click(object sender, EventArgs e)
        {
            System.Collections.ArrayList arrlist = new System.Collections.ArrayList();
            arrlist.Add(12);
            arrlist.Add(3);

            var q = from n in arrlist.Cast<int>() //Cast轉型，<>不可省
                    where n > 3
                    select new { n }; //投射出來可以是各種型別，但datagridview是繫結屬性，所以要匿名型別將n設為屬性

            dataGridView1.DataSource = q.ToList(); //int不是屬性，datagridview是繫結屬性

        }

        private void button7_Click(object sender, EventArgs e)
        {
            productsTableAdapter1.Fill(nwDataSet1.Products);

            var q = (from x in nwDataSet1.Products
                     where x.UnitPrice > 20
                     orderby x.UnitsInStock
                     select x).Take(5);
            dataGridView1.DataSource = q.ToList();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //When execute query
            //1. foreach
            //2.ToXXX()
            //3. Aggregation Sum()...

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            listBox1.Items.Add("Sum = " + nums.Sum());
            listBox1.Items.Add("Avg = " + nums.Average());
            listBox1.Items.Add("Max = " + nums.Max());
            listBox1.Items.Add("count = " + nums.Count());
            listBox1.Items.Add("Min = " + nums.Min());

            //===========================================
            listBox1.Items.Add("Avg.UnitInstock="+nwDataSet1.Products.Average(n=>n.UnitsInStock));
            listBox1.Items.Add("Max.UnitPrice="+nwDataSet1.Products.Max(n=>n.UnitPrice));
        }
    }
}