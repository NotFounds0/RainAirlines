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


namespace RAirlines
{
    public partial class Profile : Form
    {
        public Profile()
        {
            InitializeComponent();
        }
        public void getir()
        {

            string kullaniciAdi = Login.girNick;
            string userConnectionString = $@"Data Source=SQL-GİRİS-İSİM;Initial Catalog={kullaniciAdi};Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(userConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Users", connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listBox1.Items.Add($"Ad Soyad: {reader["adSoy"]}, Kullanıcı Adı: {reader["nick"]}, Email: {reader["email"]}, Telefon No: {reader["phone"]}, Şifre: {reader["sifre"]}");
                        int perm = Convert.ToInt32(reader["perm"]);
                        if (perm == 1)
                        {
                            label3.Text = "Yönetici";
                            label3.ForeColor = Color.IndianRed;
                        }
                        else if (perm == 0)
                        {
                            label3.Text = "Kullanıcı";
                            label3.ForeColor = Color.LimeGreen;
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
                listBox1.HorizontalScrollbar = true;
            }
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            MainPage m = new MainPage();
            m.Show();
            Hide();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            DestekSistemi d = new DestekSistemi();
            d.Show();
            Hide();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            ucusAra u = new ucusAra();
            u.Show();
            Hide();
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            ucuslarim u = new ucuslarim();
            u.Show();
            Hide();
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = Login.girNick;
            string adSoy, nick, email, phone, sifre;
            nick = nickTxt.Text;
            adSoy = adSoyTxt.Text;
            email = emailTxt.Text;
            phone = phoneTxt.Text;
            sifre = sifreTxt.Text;

            if (string.IsNullOrEmpty(nickTxt.Text) || string.IsNullOrEmpty(adSoyTxt.Text) || string.IsNullOrEmpty(emailTxt.Text) || string.IsNullOrEmpty(phoneTxt.Text) ||string.IsNullOrEmpty(sifreTxt.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlConnection baglanti = new SqlConnection($@"Data Source=SQL-GİRİS-İSİM;Initial Catalog={kullaniciAdi};Integrated Security=True");
            try
            {
                baglanti.Open();
               // SqlCommand singlecmd = new SqlCommand($"ALTER DATABASE {kullaniciAdi} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", baglanti);
               // singlecmd.ExecuteNonQuery();
                SqlCommand renameCommand = new SqlCommand($"ALTER DATABASE {kullaniciAdi} MODIFY NAME = {nick}", baglanti);
                renameCommand.ExecuteNonQuery();
                SqlCommand cmd = new SqlCommand("update Users set adSoy=@adSoy,nick=@nick,email=@email,phone=@phone,sifre=@sifre", baglanti);
                cmd.Parameters.AddWithValue("@adSoy", adSoy);
                cmd.Parameters.AddWithValue("@nick", nick);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@sifre", sifre);
                cmd.ExecuteNonQuery();

                baglanti.Close();
                MessageBox.Show($"Başarılı Bir Şekilde Veriler Güncellendi. \n Ad Soyad: {adSoy}, \n Kullanıcı Adı: {nick}, \n Email: {email}, \n Telefon No: {phone}, \n Şifre: {sifre}", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBox1.Items.Clear();
                getir();
                MessageBox.Show("Veritabanı adı başarıyla güncellendi. Lütfen tekrar giriş yapın.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Login loginForm = new Login();
                loginForm.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            getir();
        }
    }
}
