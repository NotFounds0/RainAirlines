using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace RAirlines
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }

        int sira = 0;
        string[] resimler = { "s1.jpg", "s2.jpg", "s3.jpg", "s4.jpg", "s5.jpg", "s6.png" };
        string[] makaleler = {
    "Cengiz Topel, Türk pilot yüzbaşı. 1964 yılında Kıbrıs'ın Erenköy bölgesine yapılan hava operasyonunda hayatını kaybeden Topel, Türk Hava Kuvvetleri'nin Cumhuriyet tarihindeki ilk savaş kaybıdır.",
    "27 Nisan Dünya pilotlar günü kutlu olsun...",
    "Türk Yıldızları F5 ile havanın tozunuuuu alıyorrrr.",
    "Türk Hava Yollarına ait B737-800 uçağına su takı(su selamlaması) karşılaması yapılıyor.",
    "B747-400 Uçağını Görüyorsunuz kendisi 4 motorlu jumbo yolcu jetidir.",
    "F16'ların Sahipleri yerlerin ve göklerin tozunu alanlar bir arada resim çekinmişler." };
        private void MainPage_Load(object sender, EventArgs e)
        {
            guna2PictureBox1.ImageLocation = @"resimler/" + resimler[sira];
            textBox1.Text = makaleler[sira];


            string connectionString = @"Data Source=SQL-GİRİS-İSİM;Initial Catalog=UcusOlusturma; Integrated Security=true";
            string query = "SELECT * FROM ucusTable";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    listBox3.Items.Clear();

                    while (reader.Read())
                    {
                        string hat = $"{reader["hat"]}";
                        string listItem = $"Kalkış Noktası: {reader["kalkisNoktasi"]}, Varış Noktası: {reader["varisNoktasi"]}, Tarih: {reader["tarih"]}, Uçuş Kodu: {reader["ucusKodu"]}";
                        if (hat == "İç Hatlar")
                        {
                            listBox2.Items.Add(listItem);
                        }
                        else if (hat == "Dış Hatlar")
                        {
                            listBox3.Items.Add(listItem);
                        }

                        listBox2.HorizontalScrollbar = true;
                        listBox2.ScrollAlwaysVisible = true;
                        listBox3.HorizontalScrollbar = true;
                        listBox3.ScrollAlwaysVisible = true;


                    }
                }

            }

            //perm control
            string nick = Login.girNick;
            string userDatabaseName = $"{nick}";
            string userConnectionString = $@"Data Source=SQL-GİRİS-İSİM;Initial Catalog={userDatabaseName};Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(userConnectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand permIdCommand = new SqlCommand("SELECT perm FROM Users WHERE nick = @nick", connection);
                    permIdCommand.Parameters.AddWithValue("@nick", nick);
                    object permIDResult = permIdCommand.ExecuteScalar();

                    if (permIDResult != null && permIDResult != DBNull.Value)
                    {
                        int permID = Convert.ToInt32(permIDResult);

                        if (permID == 1)
                        {
                            adminButton.Visible = true;
                        }
                        else
                        {
                            adminButton.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata", ex.Message);
                }

            }

        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            sira++;
            if (sira == resimler.Length)
            {
                sira = 0;
            }
            guna2PictureBox1.ImageLocation = @"resimler/" + resimler[sira];
            textBox1.Text = makaleler[sira];
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            sira--;
            if (sira < 0)
            {
                sira = resimler.Length - 1;
            }
            guna2PictureBox1.ImageLocation = @"resimler/" + resimler[sira];
            textBox1.Text = makaleler[sira];
        }

        private async void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            string flightNumber = ucusNo.Text.Trim();
            if (string.IsNullOrEmpty(flightNumber))
            {
                MessageBox.Show("Lütfen Bir Uçuş Numarası Girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "APİ-KEY");
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "HOST-URL");

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"https://flightera-flight-data.p.rapidapi.com/flight/info?flnr={flightNumber}")
                    };

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        JToken jsonResponse = JToken.Parse(responseBody);
                        string formattedResponse = $"\n Kalkış Noktası: {jsonResponse[0]["departure_name"]}, \n Varış Noktası: {jsonResponse[0]["arrival_name"]}, \n ICAO Kodu: {jsonResponse[0]["airline_icao"]}, \n Uçuş Numarası: {jsonResponse[0]["flnr"]}, \n Date: {jsonResponse[0]["date"]}";

                        MessageBox.Show($"İstenilen Uçağın Tüm Verileri Çekildi. \n \n {formattedResponse}", $"Uçak Verisi: {jsonResponse[0]["flnr"]}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listBox1.Items.Add(formattedResponse);
                        listBox1.HorizontalScrollbar = true;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bilinmeyen Bir Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkış Yapmak İstediğinize Emin misiniz?", "Çıkış", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Login l = new Login();
                l.Show();
                Hide();
            }
            else if (result == DialogResult.No) { }
        }

        private void adminButton_Click(object sender, EventArgs e)
        {
            adminpage a = new adminpage();
            a.Show();
            Hide();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            DestekSistemi d = new DestekSistemi();
            d.Show();
            Hide();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            Profile p = new Profile();
            p.Show();
            Hide();
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            ucuslarim u = new ucuslarim();
            u.Show();
            Hide();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            ucusAra u = new ucusAra();
            u.Show();
            Hide();
        }
    }
}
