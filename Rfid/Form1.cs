using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.Threading;

namespace Rfid
{

    public partial class Form1 : Form
    {
        public static SqlConnection sCon = new SqlConnection("Data Source=DESKTOP-8H18HGI\\SQLEXPRESS;Database=Rfid;Trusted_Connection=True;Integrated Security=True;");

        SqlDataAdapter dta = new SqlDataAdapter();
        SqlCommand kayitGetir = new SqlCommand();

        delegate void dtSbagla(DataTable dts);
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (!serialPort1.IsOpen)
            {
                serialPort1.Open();
            }

        }

        private void serialPort1_DataReceived_1(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string Rfidkart = serialPort1.ReadLine();
            Rfidkart = Rfidkart.Replace("\r", string.Empty);

            try
            {

                DataSet dtst = new DataSet();
                DataTable table = new DataTable();
                dtst.Tables.Add(table);
                textBox1.Text = Rfidkart;
                SqlCommand comm = new SqlCommand("select Rfid From Kayit WHERE Rfid Like'" + textBox1.Text + "%'", sCon);
                sCon.Open();
                SqlDataReader dr = comm.ExecuteReader();
                
                string oku;
                dr.Read();
                
                    oku = dr["Rfid"].ToString();
                    dr.Close();
                    oku = oku.Replace("\r", string.Empty);

                    if (textBox1.Text == oku)
                    {
                        SqlCommand cmd = new SqlCommand("select Giris,Resim From Kayit WHERE Rfid Like'" + textBox1.Text + "%'", sCon);
                        SqlDataReader d = cmd.ExecuteReader();
                        d.Read();
                        pictureBox1.ImageLocation = d["Resim"].ToString();
                        bool kontrol = Convert.ToBoolean(d["Giris"].ToString());

                        if (kontrol == true)
                        {
                            d.Close();

                            serialPort1.WriteLine("1");

                            SqlCommand con = new SqlCommand("Update Kayit set GTarihSaat = '" + DateTime.Now.ToString() + "'where Rfid Like'" + textBox1.Text + "%'", sCon);
                            con.ExecuteNonQuery();
                            SqlDataAdapter adtr = new SqlDataAdapter("Select Ad,Soyad,OgrenciNo,GTarihSaat,CTarihSaat,Giris From Kayit WHERE Rfid Like'" + textBox1.Text + "%'", sCon);

                            table.Clear();
                            adtr.Fill(table);


                            datagrideDTbagla(table);
                            SqlCommand kmt = new SqlCommand("insert into Hoca(Ad,Soyad,OgrenciNo,GTarihSaat) values('" + dataGridView1.CurrentRow.Cells[0].Value + "','" + dataGridView1.CurrentRow.Cells[1].Value + "','" + dataGridView1.CurrentRow.Cells[2].Value + "','" + dataGridView1.CurrentRow.Cells[3].Value + "')", sCon);
                            kmt.ExecuteNonQuery();
                            kontrol = !kontrol;
                            int deg = Convert.ToInt32(kontrol);

                            SqlCommand com = new SqlCommand("Update Kayit set Giris = '" + deg + "'where Rfid Like'" + textBox1.Text + "%'", sCon);
                            com.ExecuteNonQuery();

                            textBox1.Clear();

                        }

                        else
                        {
                            serialPort1.WriteLine("1");
                            d.Close();
                            SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-8H18HGI\\SQLEXPRESS;Database=Rfid;Trusted_Connection=True;Integrated Security=True;");
                            baglanti.Open();
                            SqlCommand con = new SqlCommand("Update Kayit set CTarihSaat = '" + DateTime.Now.ToString() + "'where Rfid Like'" + textBox1.Text + "%'", sCon);
                            con.ExecuteNonQuery();

                            SqlCommand de = new SqlCommand("Update Kayit set CTarihSaat = '" + DateTime.Now.ToString() + "'where Rfid Like'" + textBox1.Text + "%'", sCon);
                            de.ExecuteNonQuery();
                            SqlDataAdapter adtr = new SqlDataAdapter("Select Ad,Soyad,OgrenciNo,GTarihSaat,CTarihSaat,Giris From Kayit WHERE Rfid Like'" + textBox1.Text + "%'", sCon);

                            table.Clear();
                            adtr.Fill(table);


                            datagrideDTbagla(table);

                            SqlCommand kmt = new SqlCommand("insert into Hoca(Ad,Soyad,OgrenciNo,CTarihSaat) values('" + dataGridView1.CurrentRow.Cells[0].Value + "','" + dataGridView1.CurrentRow.Cells[1].Value + "','" + dataGridView1.CurrentRow.Cells[2].Value + "','" + dataGridView1.CurrentRow.Cells[4].Value + "')", sCon);
                            kmt.ExecuteNonQuery();
                            kontrol = !kontrol;
                            int deg = Convert.ToInt32(kontrol);
                            baglanti.Close();
                            SqlCommand com = new SqlCommand("Update Kayit set Giris = '" + deg + "'where Rfid Like'" + textBox1.Text + "%'", sCon);
                            com.ExecuteNonQuery();
                            textBox1.Clear();

                        }

                    }

                    dr.Close();
                    sCon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                sCon.Close();
            }
           
        }
       
        private void datagrideDTbagla(DataTable dts)
        {
            if (dataGridView1.InvokeRequired)
            {
                dtSbagla dtbag = new dtSbagla(datagrideDTbagla);
                dataGridView1.Invoke(dtbag, new object[] { dts });
            }
            else
            {
                dataGridView1.DataSource = dts;
                dataGridView1.Refresh();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //serialPort1.Close();
            Form4 form4 = new Form4();
            Form1.ActiveForm.Hide();
            form4.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

 
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
