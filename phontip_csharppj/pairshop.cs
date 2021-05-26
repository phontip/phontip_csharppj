﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing.Imaging;

namespace phontip_csharppj
{
    public partial class pairshop : Form
    {
        public pairshop()
        {
            InitializeComponent();
        }
        MySqlConnection connection = new MySqlConnection(" datasource=127.0.0.1;port=3306;username=root;password=;database=pair;charset=utf8;");
        public void FillDGV(string valueToSearch)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM stock WHERE CONCAT(ID,Product_ID, Product_Name, Categories, Quantity, Price, image) LIKE '%" + valueToSearch + "%'", connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            dataGrid.AllowUserToAddRows = false;

            dataGrid.DataSource = table;

            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            imgCol = (DataGridViewImageColumn)dataGrid.Columns[6];
            imgCol.ImageLayout = DataGridViewImageCellLayout.Stretch;

            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //ปรับให้เท่ากับขนาด datagrid

        }

        private void pairshop_Load(object sender, EventArgs e)
        {
            
            username_label.Text = login.UserName;//ขึ้นชื่อที่ shop


            FillDGV("");

        }

        int key = 0, stock = 0;
        private void dataGrid_Click(object sender, EventArgs e)
        {
            Byte[] img = (Byte[])dataGrid.CurrentRow.Cells[6].Value;

            MemoryStream ms = new MemoryStream(img);

            pictureBox6.Image = Image.FromStream(ms); //ให้รูปขึ้นที่ pictureBox6

            Product_textBox.Text = dataGrid.CurrentRow.Cells[2].Value.ToString();

            Price_textBox.Text = dataGrid.CurrentRow.Cells[5].Value.ToString();

            if (Product_textBox.Text == "")
            {
                key = 0;
                stock = 0;
            }
            else
            {
                key = Convert.ToInt32(dataGrid.CurrentRow.Cells[0].Value.ToString());
                stock = Convert.ToInt32(dataGrid.CurrentRow.Cells[4].Value.ToString());
            }


        }

       
        private void Accessories_Click(object sender, EventArgs e)
        {
            MySqlDataAdapter da;
            DataTable dt;
            connection.Open();
            da = new MySqlDataAdapter("SELECT * FROM stock WHERE Categories LIKE'" + this.Accessories.Text + "%'", connection);
            dt = new DataTable();
            da.Fill(dt);
            dataGrid.DataSource = dt;
            connection.Close();

        }

        private void Bag_Click(object sender, EventArgs e)
        {
            MySqlDataAdapter da;
            DataTable dt;
            connection.Open();
            da = new MySqlDataAdapter("SELECT * FROM stock WHERE Categories LIKE'" + this.Bag.Text + "%'", connection);
            dt = new DataTable();
            da.Fill(dt);
            dataGrid.DataSource = dt;
            connection.Close();
        }
        private void Hat_Click(object sender, EventArgs e)
        {
            MySqlDataAdapter da;
            DataTable dt;
            connection.Open();
            da = new MySqlDataAdapter("SELECT * FROM stock WHERE Categories LIKE'" + this.Hat.Text + "%'", connection);
            dt = new DataTable();
            da.Fill(dt);
            dataGrid.DataSource = dt;
            connection.Close();
        }


        private void Reset_Button_Click(object sender, EventArgs e)
        {
            Product_textBox.Clear();
            Quantity_textBox.Clear();
            Price_textBox.Clear();

        }

        private void updatedatagrid()
        {
            int newQty = stock - Convert.ToInt32(Quantity_textBox.Text);

;
            connection.Open();
            string sql = $"UPDATE stock SET Quantity=\"{newQty}\"WHERE id =" + key + ";";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader dr;

            dr = cmd.ExecuteReader();
            connection.Close();
            MessageBox.Show(" Thank you for shopping with us.");

        }



       

        private void Print_Button_Click(object sender, EventArgs e)
        {
            updatedatagrid();
            string sql = $"INSERT INTO bill (User_Name,Amount) VALUES (\"{username_label.Text}\",\"{Grdtotal}\")";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            connection.Open();
            int row = cmd.ExecuteNonQuery();
            connection.Close();

           

            printPreviewDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 600);
            printPreviewDialog1.ShowDialog();

            pairshop Obj = new pairshop();
            Obj.Show();
            this.Hide();

        } 
        private void cancle_Button_Click(object sender, EventArgs e)
        {


            pairshop Obj = new pairshop();
            Obj.Show();
            this.Hide();
  

        }

       

        private void Quantity_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        int ID, proqty, Price, tottal, pos = 60;
        int n = 0, Grdtotal = 0;
        private void add_Button_Click(object sender, EventArgs e)
        {
            if (Quantity_textBox.Text == "" || Convert.ToInt32(Quantity_textBox.Text) > stock)
            {
                MessageBox.Show("No Enough Stock Please enter the quantity of product again");
            }
            else
            {
                int total = Convert.ToInt32(Quantity_textBox.Text) * Convert.ToInt32(Price_textBox.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(Bill_DGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = Product_textBox.Text;
                newRow.Cells[2].Value = Price_textBox.Text;
                newRow.Cells[3].Value = Quantity_textBox.Text;
                newRow.Cells[4].Value = total;
                Bill_DGV.Rows.Add(newRow);
                n++;
                Grdtotal = Grdtotal + total;
                totalb.Text = "รวม  " + Grdtotal + "  bath";
            }

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        
        {
                e.Graphics.DrawString("THAI  SILK  HOME  MART", new Font("Century Gothis", 10, FontStyle.Bold), Brushes.Red, new Point(50));
                e.Graphics.DrawString("ID  PRODUCT      PRICE   QUANTITY              TOTal", new Font("Century Gothis", 7, FontStyle.Bold), Brushes.Red, new Point(26, 40));
                foreach (DataGridViewRow row in Bill_DGV.Rows)
                {
                    ID = Convert.ToInt32(row.Cells["Column12"].Value);
                    proname = "" + row.Cells["Column8"].Value;
                    Price = Convert.ToInt32(row.Cells["Column9"].Value);
                    proqty = Convert.ToInt32(row.Cells["Column10"].Value);
                    tottal = Convert.ToInt32(row.Cells["Column11"].Value);
                    e.Graphics.DrawString("" + ID, new Font("Century Gothis", 8, FontStyle.Bold), Brushes.Blue, new Point(26, pos));
                    e.Graphics.DrawString("" + proname, new Font("Century Gothis", 8, FontStyle.Bold), Brushes.Blue, new Point(45, pos));
                    e.Graphics.DrawString("" + Price, new Font("Century Gothis", 8, FontStyle.Bold), Brushes.Blue, new Point(120, pos));
                    e.Graphics.DrawString("" + proqty, new Font("Century Gothis", 8, FontStyle.Bold), Brushes.Blue, new Point(170, pos));
                    e.Graphics.DrawString("" + tottal, new Font("Century Gothis", 8, FontStyle.Bold), Brushes.Blue, new Point(235, pos));
                    pos = pos + 20;

                }
                e.Graphics.DrawString("    Grand Total : " + Grdtotal, new Font("Century Gothis", 10, FontStyle.Bold), Brushes.Crimson, new Point(60, pos + 50));

                e.Graphics.DrawString("______THAI  SILK  HOME  MART______", new Font("Century Gothis", 8, FontStyle.Bold), Brushes.Crimson, new Point(40, pos + 85));
                Bill_DGV.Rows.Clear();
                Bill_DGV.Refresh();
                pos = 100;

                Grdtotal = 0;

            }

            string proname; 
        private void label3_Click(object sender, EventArgs e)
        {
            login Obj = new login();
            Obj.Show();
            this.Hide();
        } 
       



        

        

       
        


    }
}
