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
    public partial class ucuslarim : Form
    {
        public ucuslarim()
        {
            InitializeComponent();
        }

        public void getir()
        {
            string kullanici_adi = Login.girNick;
            string baglantiString = $@"Data Source=SQL-GİRİS-İSİM;Initial Catalog={kullanici_adi}; Integrated Security=true";
            using (SqlConnection baglanti = new SqlConnection(baglantiString))
            {
                try
                {
                    baglanti.Open();


                    SqlCommand tabloKontrolCmd = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ucuslarim'", baglanti);
                    int tabloSayisi = (int)tabloKontrolCmd.ExecuteScalar();

                    if (tabloSayisi > 0)
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM ucuslarim", baglanti);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ucuslar.Items.Add($"{reader["veri"]}");
                            ucuslar.HorizontalScrollbar = true;
                            ucuslar.ScrollAlwaysVisible = true;
                        }
                        reader.Close();
                    }
                    else
                    {
                        Console.WriteLine("Tablo bulunamadı.");

                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Hata Oluştu: " + ex.Message);
                }
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
            ucusAra m = new ucusAra();
            m.Show();
            Hide();
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            DestekSistemi m = new DestekSistemi();
            m.Show();
            Hide();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Profile m = new Profile();
            m.Show();
            Hide();
        }

        private void ucuslarim_Load(object sender, EventArgs e)
        {
            getir();

        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            
            string kullanici_adi = Login.girNick;
            if (ucuslar.SelectedItem == null)
            {
                MessageBox.Show("Silinecek Uçuş Seç.","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            string item = ucuslar.SelectedItem.ToString();
           


            SqlConnection baglanti = new SqlConnection($@"Data Source=SQL-GİRİS-İSİM;Initial Catalog={kullanici_adi}; Integrated Security=true");
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("delete from ucuslarim where veri=@item", baglanti);
            cmd.Parameters.AddWithValue("@item", item);
            cmd.ExecuteNonQuery();
            MessageBox.Show($"{item} \n Uçuşu Başarıyla Silindi.","Başarılı",MessageBoxButtons.OK,MessageBoxIcon.Information);
            ucuslar.Items.Clear();
            getir();
        }
    }
}
