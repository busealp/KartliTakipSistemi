using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace Rfid
{
    public partial class Form2 : Form
    {
        SqlConnection sCon = new SqlConnection();

        public Form2()
        {
            InitializeComponent();
        
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLExpress; initial Catalog=Rfid; Integrated Security=true");
        SqlCommand komut;
    
        private void button1_Click(object sender, EventArgs e)
        {
           try
           {
                baglanti.Open();
            

               if ((textBox2.Text == "" |textBox4.Text == "" | textBox5.Text == "" | textBox6.Text == "" | textBox8.Text == "" | textBox9.Text == "" | comboBox1.Text == "" | comboBox2.Text == "" | textBox3.Text == ""))
                {
                    MessageBox.Show("Boş Alan Geçmeyiniz!");
                    
                }
                else
                {
                    komut = new SqlCommand("insert into Kayit(Ad,Soyad,Bolum,Sinif,Telefon,Mail,Resim,Rfid,OgrenciNo,Giris) values('" + textBox4.Text + "','" + textBox2.Text + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + textBox9.Text + "','" + textBox8.Text + "','" + textBox3.Text + "','" + 1 + "')", baglanti);
                    komut.ExecuteNonQuery();
                 

                    MessageBox.Show("Ekleme İşlemi Başarılı");
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox2.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox8.Clear();
                    textBox9.Clear();
                    comboBox1.Text = "";
                    comboBox2.Text = "";

                    
                }
            baglanti.Close();
        }
            catch
            {

                ;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
               serialPort1.Close();
                MessageBox.Show("Lütfen RFID OKU butonuna birkez basın ve kartı yaklaştırın");
            }
            else
            {
                serialPort1.Open();

            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
            textBox9.Text = openFileDialog1.FileName;
        }
        int sayma;


        private void Form2_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void serialPort1_DataReceived_1(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string Rfidkart = serialPort1.ReadLine();


            sCon.ConnectionString = "Data Source=DESKTOP-8H18HGI\\SQLEXPRESS;Initial Catalog=Rfid;Integrated Security=True;";

            SqlCommand comm = new SqlCommand("SELECT * FROM Kayit", sCon);
            sCon.Open();
            SqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                string oku;
                textBox8.Text = Rfidkart;
                oku = dr[7].ToString();

                if (textBox8.Text == oku)
                {
                    MessageBox.Show("KİŞİ KAYITLI");
                    sayma = 1;
                    textBox4.Text = dr[0].ToString();
                    textBox2.Text = dr[1].ToString();
                    textBox5.Text = dr[4].ToString();
                    textBox6.Text = dr[5].ToString();
                    textBox8.Text = dr[7].ToString();
                    textBox9.Text = dr[6].ToString();
                    comboBox1.Text =dr[2].ToString();
                    comboBox2.Text =dr[3].ToString();
                    textBox3.Text = dr[8].ToString();
                    pictureBox1.ImageLocation = dr["Resim"].ToString();
                  
                }
                
            }
       

                sCon.Close();
            dr.Close();
          
            if (sayma != 1)
            {
                textBox8.Text = Rfidkart;
                serialPort1.Close();
                MessageBox.Show("Kişi Kayıtlı Değil Kayıt işlemi Yapabilirsiniz");  
            }

            //sqlDataReader ve SqlConnection kapatılıyor.
            dr.Close();
            sCon.Close();
        }

       
        private void button3_Click(object sender, EventArgs e)
        {
            sCon.Open();
            
            SqlCommand comm = new SqlCommand("Delete from Kayit where Rfid=@p1 ", sCon);
            comm.Parameters.AddWithValue("@p1", textBox8.Text);
            comm.ExecuteNonQuery();
            sCon.Close();
            MessageBox.Show("Kayit Silindi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            textBox4.Clear();
            textBox2.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox3.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
        } 

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand com = new SqlCommand("UPDATE Kayit set Ad=@d1,Soyad=@d2,Telefon=@d3,Mail=@d4,Resim=@d5,Bolum=@d6,Sinif=@d7,Rfid=@d8,OgrenciNo=@d9 where Rfid=@d8", baglanti);
            com.Parameters.AddWithValue("@d1", textBox4.Text);
            com.Parameters.AddWithValue("@d2", textBox2.Text);
            com.Parameters.AddWithValue("@d3", textBox5.Text);
            com.Parameters.AddWithValue("@d4", textBox6.Text);
            com.Parameters.AddWithValue("@d5", textBox9.Text);
            com.Parameters.AddWithValue("@d6", comboBox1.Text);
            com.Parameters.AddWithValue("@d7", comboBox2.Text);
            com.Parameters.AddWithValue("@d8", textBox8.Text);
            com.Parameters.AddWithValue("@d9", textBox3.Text);
            com.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Güncelleme İşlemi Başarılı");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            Form4 form4 = new Form4();
            Form2.ActiveForm.Hide();
            form4.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
         
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        

        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
       

        }

        private void buttonEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
