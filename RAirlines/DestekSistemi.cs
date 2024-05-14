using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RAirlines
{
    public partial class DestekSistemi : Form
    {
        public DestekSistemi()
        {
            InitializeComponent();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string ad, email, sorun, mesaj, telefon, nick;
            nick = guna2TextBox1.Text;
            ad = adSoyTxt.Text;
            email = emailTxt.Text;
            telefon = phoneTxt.Text;
            sorun = guna2ComboBox1.Text;
            mesaj = guna2TextBox2.Text;

            SqlConnection baglanti = new SqlConnection(@"Data Source=SQL-GİRİS-İSİM;Initial Catalog=SupportSystem; Integrated Security=true");
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO destek(ad, email, telefon, sorun, mesaj,nick) VALUES (@ad, @email, @telefon, @sorun, @mesaj,@nick)", baglanti);
            komut.Parameters.AddWithValue("@ad", ad);
            komut.Parameters.AddWithValue("@email", email);
            komut.Parameters.AddWithValue("@telefon", telefon);
            komut.Parameters.AddWithValue("@sorun", sorun);
            komut.Parameters.AddWithValue("@mesaj", mesaj);
            komut.Parameters.AddWithValue("@nick", nick);
            komut.ExecuteNonQuery();
            baglanti.Close();


            MessageBox.Show($"Sorununuz Başarıyla Yetkili Ekibimize Ulaştmıştır. En Kısa Sürede İletişime Geçilecektir: \n İsim: {ad}, \n Email: {email}, \n Telefon Numarası: {telefon}, \n Sorun: {sorun}, \n Mesaj: {mesaj}", "Başarılı.", MessageBoxButtons.OK, MessageBoxIcon.Information);


            adSoyTxt.Text = "";
            emailTxt.Text = "";
            phoneTxt.Text = "";
            guna2TextBox2.Text = "";
        }

        private void DestekSistemi_Load(object sender, EventArgs e)
        {
            string nick = Login.girNick;
            guna2TextBox1.Text = nick;
        }


        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            MainPage m = new MainPage();
            m.Show();
            Hide();
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            ucusAra m = new ucusAra();
            m.Show();
            Hide();
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            ucuslarim m = new ucuslarim();
            m.Show();
            Hide();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Profile m = new Profile();
            m.Show();
            Hide();
        }
    }
}
