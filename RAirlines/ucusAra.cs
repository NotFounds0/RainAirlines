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
    public partial class ucusAra : Form
    {
        public ucusAra()
        {
            InitializeComponent();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            MainPage m = new MainPage();
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

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            ucuslarim m = new ucuslarim();
            m.Show();
            Hide();
        }

        private void ucusAra_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=SQL-GİRİS-İSİM;Initial Catalog=UcusOlusturma; Integrated Security=true";
            string query = "SELECT DISTINCT kalkisNoktasi FROM ucusTable";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                guna2ComboBox1.Items.Add(reader["kalkisNoktasi"].ToString());
                            }
                        }
                        else
                        {
                            guna2ComboBox1.Items.Add("Veri Bulunamadı");
                        }
                    }
                }

            query = "SELECT DISTINCT varisNoktasi FROM ucusTable";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                guna2ComboBox2.Items.Add(reader["varisNoktasi"].ToString());
                            }
                        }
                        else
                        {
                            guna2ComboBox2.Items.Add("Veri Bulunamadı");
                        }
                    }
                }

            }



            //Geçmiş Günler Gözükmesin
            guna2DateTimePicker1.MinDate = DateTime.Today;

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string kalkisNoktası = guna2ComboBox1.Text;
            string varisNoktası = guna2ComboBox2.Text;
            string tarih = guna2DateTimePicker1.Value.Date.ToString("d MMMM yyyy dddd");
            string connectionString = @"Data Source=SQL-GİRİS-İSİM;Initial Catalog=UcusOlusturma; Integrated Security=true";
            string query = "SELECT * FROM ucusTable WHERE kalkisNoktasi = @kalkisNoktası AND varisNoktasi = @varisNoktası AND tarih=@tarih";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kalkisNoktası", kalkisNoktası);
                    command.Parameters.AddWithValue("@varisNoktası", varisNoktası);
                  command.Parameters.AddWithValue("@tarih", tarih);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        listBox1.Items.Clear();

                        while (reader.Read())
                        {
                            string listItem = $"Kalkış Noktası: {reader["kalkisnoktasi"]}, Varış Noktası: {reader["varisNoktasi"]}, Öğrenci Fiyat: {reader["ogrenciFiyat"]}, Tam Fiyat: {reader["tamFiyat"]}, Hat: {reader["hat"]}, Tarih: {reader["tarih"]}, Uçuş Kodu: {reader["ucusKodu"]}";
                            listBox1.Items.Add(listItem);
                        }
                        int itemCount = listBox1.Items.Count;
                        label5.Text = itemCount.ToString();
                        listBox1.HorizontalScrollbar = true;
                    }
                }
            }
        }



        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
           
            string kullanici_adi = Login.girNick;
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    MessageBox.Show("Oluşturulacak Uçuş Seç.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (var selectedItem in listBox1.SelectedItems)
                {

                    string item = selectedItem.ToString();
                    

                    using (SqlConnection baglanti = new SqlConnection($@"Data Source=SQL-GİRİS-İSİM;Initial Catalog={kullanici_adi}; Integrated Security=true"))
                    {
                        baglanti.Open();

                     
                        string tableCheckQuery = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ucuslarim') BEGIN CREATE TABLE ucuslarim (veri NVARCHAR(MAX)) END";
                        using (SqlCommand tableCheckCommand = new SqlCommand(tableCheckQuery, baglanti))
                        {
                            tableCheckCommand.ExecuteNonQuery();
                        }

                     
                        string itemCheckQuery = "SELECT COUNT(*) FROM ucuslarim WHERE veri = @item";
                        using (SqlCommand itemCheckCommand = new SqlCommand(itemCheckQuery, baglanti))
                        {
                            itemCheckCommand.Parameters.AddWithValue("@item", item);
                            int existingCount = (int)itemCheckCommand.ExecuteScalar();
                            if (existingCount > 0)
                            {
                                throw new Exception("Bu uçuş zaten mevcut.");
                            }
                        }

                       
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO ucuslarim (veri) VALUES (@item)", baglanti))
                        {
                            cmd.Parameters.AddWithValue("@item", item);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("Başarılı Bir Şekilde Uçuş Oluşturuldu", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string lokasyon = guna2ComboBox1.Text;
            guna2ComboBox1.Text = guna2ComboBox2.Text;
            guna2ComboBox2.Text = lokasyon.ToString();
        }
    }
}
