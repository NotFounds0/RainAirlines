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
using System.Net.Mail;

namespace RAirlines
{
    public partial class sifremiUnuttum : Form
    {
        public sifremiUnuttum()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {

            string kullaniciAdi = guna2TextBox1.Text;
            if (string.IsNullOrEmpty(kullaniciAdi) && string.IsNullOrEmpty(emailTxt.Text))
            {
                MessageBox.Show("Kullanıcı Adı veya Email Boş Geçilemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SqlConnection baglanti = new SqlConnection($@"Data Source=SQL-GİRİS-İSİM;Initial Catalog={kullaniciAdi};Integrated Security=True");

            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT * FROM Users WHERE nick=@nick", baglanti);
                komut.Parameters.AddWithValue("@nick", kullaniciAdi);
                SqlDataReader oku = komut.ExecuteReader();

                if (!oku.HasRows) 
                {
                    MessageBox.Show("Girilen kullanıcı adı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                while (oku.Read()) 
                {
                    SmtpClient smtpserver = new SmtpClient();
                    MailMessage mail = new MailMessage();
                    String tarih = DateTime.Now.ToLongDateString();
                    String mailAdress = "";
                    String password = "";
                    String smptsrvr = "smtp.gmail.com";
                    String gonderilecekKisi = emailTxt.Text;
                    String mesajBaslik = "𝐑𝐚𝐢𝐧𝐀𝐢𝐫𝐥𝐢𝐧𝐞𝐬 Şifre Hatırlatma Talebi.";
                    String mesajIcerigi = "𝐑𝐚𝐢𝐧𝐀𝐢𝐫𝐥𝐢𝐧𝐞𝐬 " + "\n" + "𝗦𝗮𝘆ı𝗻: " + oku["adSoy"].ToString() + "\n" + tarih + " Tarihinde bizden şifre hatırlatma talebi bulundunuz." + "\n" + "𝗦̧𝗶𝗳𝗿𝗲𝗻𝗶𝘇: " + oku["sifre"].ToString() + "\n" + "iyi Günler Dileriz." + "\n" + "𝐑𝐚𝐢𝐧𝐀𝐢𝐫𝐥𝐢𝐧𝐞𝐬 Yönetim Ekibi.";

                    // Mail gönderme işlemleri
                    smtpserver.Credentials = new NetworkCredential(mailAdress, password);
                    smtpserver.Port = 587;
                    smtpserver.Host = smptsrvr;
                    smtpserver.EnableSsl = true;
                    mail.From = new MailAddress(mailAdress);
                    mail.To.Add(gonderilecekKisi);
                    mail.Subject = mesajBaslik;
                    mail.Body = mesajIcerigi;
                    smtpserver.Send(mail);

                    DialogResult bilgi = MessageBox.Show("Girmiş Olduğunuz Bilgiler Uyuştu. Şifreniz Mail Adresinize Gönderildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hide();
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show($"Mail Gönderme Hatası: {hata.Message}","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
