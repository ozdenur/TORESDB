using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KullanıcıGırısPanelı
{
    public partial class LogınForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-V05EINL;Initial Catalog=TORESDB;Integrated Security=True"); // burada SQL bağlantı kurduk.
      

        public LogınForm()
        {
            InitializeComponent();
        }
        DateTime loginTime, errorTime,logoutTime; // burada giriş/çıkı,hata günlerini tanımladık.
        string message;
        private void btnExıt_Click(object sender, EventArgs e)
        {
            LogMsg();
            logoutTime = DateTime.Now;
            Application.Exit();
        }

        private void btnSıgnIn_Click(object sender, EventArgs e)
        {
            connection.Open();
            string username = txtUserName.Text;
            string password = txtUserPass.Text;
            SqlCommand cmd = new SqlCommand("Select * From datUser where UserName=@userName and UserPass=@userPass",connection); // SQL komut vererek verileri birbirlerini bağladık
            cmd.Parameters.AddWithValue("@userName",username);
            cmd.Parameters.AddWithValue("@userPass",password);
            SqlDataReader dr = cmd.ExecuteReader(); // bu verileri veritabanından  oku 
            if (dr.Read()) // degerleri okuduysan
            {
                loginTime=DateTime.Now; // logintime 'a giriş saaatini ve tarihini atadık..
                //Ana panel formundan bir nesne üret
                //anapaneli göster Show ile
                //bunu gizle
                MessageBox.Show("Congrats");

               
            }
            else
            {
                dr.Close();
                errorTime = DateTime.Now;
                LogErMsg();
               
                MessageBox.Show("UserName or UserPass Wrong! Please try again.","Login Error",MessageBoxButtons.OK,MessageBoxIcon.Error);// hatalı girişler için mesaj kutusu oluşturduk.
            }
            connection.Close();
        }

       

        private void LogMsg()// SQL den aldığımız komutlar ile kullanıcı isimlerini parametrelere çevirerek veri tabanına işledik.
        {
            connection.Open();
            string userName = txtUserName.Text;
            message = $"Login Message:{userName} successfully login";
            connection.Open();
            SqlCommand cmd2 = new SqlCommand("Insert into datLog (UserID,LoginDT,LogoutDT,LogNotes) values (@userId,@loginDt,@logoutDt,@logNotes",connection);
            cmd2.Parameters.AddWithValue("@userId",userName);
            cmd2.Parameters.AddWithValue("@loginDt", loginTime.ToString()) ;
            cmd2.Parameters.AddWithValue("@logoutDt", logoutTime.ToString());
            cmd2.Parameters.AddWithValue("@logNotes", message.ToString());
            cmd2.ExecuteNonQuery(); // değişiklikleri veritabanına yansıt .. Kaydet gibi
            connection.Close();
        }
        private void LogErMsg()
        {
            string userName = txtUserName.Text;
            message = "Error Message:login error";
            SqlCommand cmd2 = new SqlCommand("Insert into datLog (UserID,LoginDT,LogoutDT,LogNotes) values (@userId,@loginDt,@logoutDt,@logNotes)", connection);
            cmd2.Parameters.AddWithValue("@userId", 3);
            cmd2.Parameters.AddWithValue("@loginDt", errorTime);
            cmd2.Parameters.AddWithValue("@logoutDt", errorTime);
            cmd2.Parameters.AddWithValue("@logNotes", message);
            cmd2.ExecuteNonQuery(); // değişiklikleri veritabanına yansıt .. Kaydet gibi
        }
    }
}
