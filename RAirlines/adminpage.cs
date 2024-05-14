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
    public partial class adminpage : Form
    {
        public adminpage()
        {
            InitializeComponent();
        }

        public void getir()
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=SQL-GİRİS-İSİM;Initial Catalog=SupportSystem; Integrated Security=true");
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM destek", baglanti);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add($"Kullanıcı Adı: {reader["nick"]}, Ad Soyad: {reader["ad"]}, Email: {reader["email"]}, Telefon No: {reader["telefon"]}, Sorun: {reader["sorun"]}, Mesaj: {reader["mesaj"]}");
                listBox1.HorizontalScrollbar = true;
                listBox1.ScrollAlwaysVisible = true;
            }

            reader.Close();
        }
        public void Usergetir()
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=SQL-GİRİS-İSİM;Initial Catalog=Register; Integrated Security=true");
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM regTable", baglanti);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listBox2.Items.Add($"Kullanıcı Adı: {reader["nick"]}, Ad Soyad: {reader["adSoy"]}, Email: {reader["email"]}, Telefon No: {reader["phone"]}, Şifre: {reader["sifre"]}");
                listBox2.HorizontalScrollbar = true;
                listBox2.ScrollAlwaysVisible = true;
            }

            reader.Close();
        }

        public void ucusGetir()
        {
           
            string baglantiString = $@"Data Source=SQL-GİRİS-İSİM;Initial Catalog=UcusOlusturma; Integrated Security=true";
            using (SqlConnection baglanti = new SqlConnection(baglantiString))
            {
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM ucusTable", baglanti);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ucuslar.Items.Add($"Kalkış Noktası: {reader["kalkisNoktasi"]}, Varış Noktası: {reader["varisNoktasi"]}, Öğrenci Fiyat: {reader["ogrenciFiyat"]}, Tam Fiyat: {reader["tamFiyat"]}, Tarih: {reader["tarih"]}, Hat: {reader["hat"]}, Uçuş Kodu: {reader["ucusKodu"]}");
                    ucuslar.HorizontalScrollbar = true;
                    ucuslar.ScrollAlwaysVisible = true;
                }
                reader.Close();
            }
        }

        private void ucusOlustur_Click(object sender, EventArgs e)
        {
            string kalkisNoktasi, varisNoktasi, ogrenciFiyat, tamFiyat, ucusKodu, tarih, hat;
            kalkisNoktasi = kalkisNoktası.Text;
            varisNoktasi = varisNoktası.Text;
            ogrenciFiyat = ogrenciF.Value.ToString();
            tamFiyat = tamF.Value.ToString();
            ucusKodu = ucusK.Text;
            tarih = guna2DateTimePicker1.Text;
            hat = guna2ComboBox1.Text;

            if(string.IsNullOrEmpty(kalkisNoktası.Text) || string.IsNullOrEmpty(varisNoktası.Text) || string.IsNullOrEmpty(ogrenciF.Value.ToString()) || string.IsNullOrEmpty(tamF.Value.ToString()) || string.IsNullOrEmpty(ucusK.Text) || string.IsNullOrEmpty(guna2ComboBox1.Text))
            {
                MessageBox.Show("Tüm Alanları Doldurun.","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            SqlConnection baglanti = new SqlConnection(@"Data Source=SQL-GİRİS-İSİM;Initial Catalog=UcusOlusturma; Integrated Security=true");
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into ucusTable(kalkisNoktasi,varisNoktasi,ogrenciFiyat,tamFiyat,ucusKodu,tarih,hat) values (@kalkisNoktasi,@varisNoktasi,@ogrenciFiyat,@tamFiyat,@ucusKodu,@tarih,@hat)", baglanti);
            komut.Parameters.AddWithValue("@kalkisNoktasi", kalkisNoktasi);
            komut.Parameters.AddWithValue("@varisNoktasi", varisNoktasi);
            komut.Parameters.AddWithValue("@ogrenciFiyat", ogrenciFiyat);
            komut.Parameters.AddWithValue("@tamFiyat", tamFiyat);
            komut.Parameters.AddWithValue("@ucusKodu", ucusKodu);
            komut.Parameters.AddWithValue("@tarih", tarih);
            komut.Parameters.AddWithValue("@hat", hat);
            komut.ExecuteReader();
            baglanti.Close();
            kalkisNoktası.Text = "";
            varisNoktası.Text = "";
            ucusK.Text = "";
            MessageBox.Show($"Başarılı Uçuş Oluşturuldu: \n Kalkış Noktası: {kalkisNoktasi}, \n Varış Noktası: {varisNoktasi}, \n Oğrenci Fiyat: {ogrenciFiyat}, \n Tam Fiyat: {tamFiyat}, \n Ucuş Kodu: {ucusKodu}, \n Tarih: {tarih}, \n Hat: {hat}", "Başarılı Uçuş Oluşturuldu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MainPage m = new MainPage();
            m.Show();
            Hide();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            string nick;
            nick = guna2TextBox1.Text;
            if (string.IsNullOrEmpty(guna2TextBox1.Text))
            {
                MessageBox.Show("Kullanıcı Adı Girin.","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            SqlConnection baglanti = new SqlConnection(@"Data Source=SQL-GİRİS-İSİM;Initial Catalog=SupportSystem; Integrated Security=true");
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("delete from destek where nick=@nick", baglanti);
            cmd.Parameters.AddWithValue("@nick", nick);
            cmd.ExecuteNonQuery();
            MessageBox.Show($"{nick} İsimli Kullanıcının Talebi Silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listBox1.Items.Clear();
            getir();


        }

        private void adminpage_Load(object sender, EventArgs e)
        {
            getir();
            Usergetir();
            ucusGetir();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            //Geçmiş Günler Gözükmesin
            guna2DateTimePicker1.MinDate = DateTime.Today;
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(guna2TextBox2.Text))
            {
                MessageBox.Show("Silinecek Uçuş Kodunu Yaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ucusKodu = guna2TextBox2.Text;

            SqlConnection baglanti = new SqlConnection($@"Data Source=SQL-GİRİS-İSİM;Initial Catalog=UcusOlusturma; Integrated Security=true");
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ucusTable WHERE ucusKodu=@ucusKodu", baglanti);
            cmd.Parameters.AddWithValue("@ucusKodu", ucusKodu);

            int kayitSayisi = (int)cmd.ExecuteScalar();

            if (kayitSayisi == 0)
            {
                MessageBox.Show($"{ucusKodu} Koduna Sahip Bir Uçuş Bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                cmd = new SqlCommand("DELETE FROM ucusTable WHERE ucusKodu=@ucusKodu", baglanti);
                cmd.Parameters.AddWithValue("@ucusKodu", ucusKodu);
                cmd.ExecuteNonQuery();
                MessageBox.Show($"{ucusKodu} Uçuşu Başarıyla Silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucuslar.Items.Clear();
                ucusGetir();
            }

            baglanti.Close();

        }
    }
}
