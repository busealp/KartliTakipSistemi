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
using System.Collections;





namespace Rfid
{
    public partial class Form3 : Form
    {
        DataTable table = new DataTable();
        public static SqlConnection sCon = new SqlConnection("Data Source=DESKTOP-8H18HGI\\SQLEXPRESS;Database=Rfid;Trusted_Connection=True;Integrated Security=True;");
        delegate void dtSbagla(DataTable dts);
       
        public static Form3 form1 = new Form3();
        public static Form4 form2 = new Form4();
        public Form3()
        {
            InitializeComponent();
           
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
        
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            e.Graphics.DrawImage(bm, 0, 0);
            
        }

        
        private void button2_Click(object sender, EventArgs e)
        {

            PrintPreviewDialog onizleme = new PrintPreviewDialog();
            onizleme.Document = printDocument1;
            onizleme.ShowDialog(); 
            printDocument1.Print();
            sCon.Open();
            SqlCommand comm = new SqlCommand("Delete from Hoca ", sCon);
            comm.ExecuteNonQuery();
            sCon.Close();

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            SqlDataAdapter adtr = new SqlDataAdapter("Select * From Hoca", sCon);

            table.Clear();
            adtr.Fill(table);
            datagrideDTbagla(table);
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            Form3.ActiveForm.Hide();
            form4.Show();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

    
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
