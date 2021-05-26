using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace phontip_csharppj
{
    public partial class createacc : Form
    {
        public createacc()
        {
            InitializeComponent();
        }
        private MySqlConnection databaseConnection()
        {
            string connectionString = " datasource=127.0.0.1;port=3306;username=root;password=;database=pair;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;

        }


        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = databaseConnection();
           
            if (username_textBox.Text != "" && passw_textBox.Text != "" && phonetextBox.Text != "" && addresstextBox.Text != "" )
            {
                string sql = $"INSERT INTO user (User_Name,Password,Phone,Address) VALUES (\"{username_textBox.Text}\",\"{passw_textBox.Text}\",\"{phonetextBox.Text}\",\"{addresstextBox.Text}\")";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                int row = cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Create Account Successfully");
                login Obj = new login();
                Obj.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Please fill your information completely");
            }
            conn.Close();


        }


        private void phonetextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            login Obj = new login();
            Obj.Show();
            this.Hide();
        }

        
    }
}
