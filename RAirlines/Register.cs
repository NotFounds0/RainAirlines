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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Login l = new Login();
            l.Show();
            Hide();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            string[] liste = {
            "Hayat kısa, RainAirlines uçuyor.",
          "Her Anı Yağmurla Buluşturan Uçuşlar",
          "Yolculuğunuza Yağmuru Katın",
          "Bulutların Üzerinde Yağmurla Yolculuk",
          "Gökyüzünde Yağmurun Ritmiyle Dans Edin",
          "Yağmurun İncisiyle Yolculuklara Hazır Olun"
        };
            Random random = new Random();
            int rndm = random.Next(liste.Length);
            label2.Text = liste[rndm];
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string adSoy, nick, email, phone, sifre;
            if (string.IsNullOrEmpty(adSoyTxt.Text) || string.IsNullOrEmpty(nickTxt.Text) || string.IsNullOrEmpty(phoneTxt.Text) || string.IsNullOrEmpty(sifreTxt.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            adSoy = adSoyTxt.Text;
            nick = nickTxt.Text;
            email = emailTxt.Text;
            phone = phoneTxt.Text;
            sifre = sifreTxt.Text;
            string userDatabaseName = $"{nick}";

            string connectionString = @"Data Source=SQL-GİRİS-İSİM;Initial Catalog=Register;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kullanıcı adıyla bir veritabanı olup olmadığını kontrol et
                string checkDBExistQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{userDatabaseName}'";
                using (SqlCommand checkDBExistCommand = new SqlCommand(checkDBExistQuery, connection))
                {
                    int existingDBCount = (int)checkDBExistCommand.ExecuteScalar();
                    if (existingDBCount > 0)
                    {
                        MessageBox.Show("Bu kullanıcı adıyla zaten bir hesap bulunmaktadır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Eğer veritabanı yoksa, yeni bir veritabanı oluştur
                string createUserDBQuery = $"CREATE DATABASE {userDatabaseName}";
                using (SqlCommand createDBCommand = new SqlCommand(createUserDBQuery, connection))
                {
                    createDBCommand.ExecuteNonQuery();
                }
            }

            string userConnectionString = $@"Data Source=SQL-GİRİS-İSİM;Initial Catalog={userDatabaseName};Integrated Security=True";
            using (SqlConnection userConnection = new SqlConnection(userConnectionString))
            {
                userConnection.Open();

                string createTableQuery = "CREATE TABLE Users (adSoy VARCHAR(50), nick VARCHAR(50), email VARCHAR(50), phone VARCHAR(50), sifre VARCHAR(50), perm INT DEFAULT 0)";
                using (SqlCommand command = new SqlCommand(createTableQuery, userConnection))
                {
                    command.ExecuteNonQuery();
                }

                string insertQuery = "INSERT INTO Users (adSoy, nick, email, phone, sifre) VALUES (@adSoy, @nick, @email, @phone, @sifre)";
                using (SqlCommand command = new SqlCommand(insertQuery, userConnection))
                {
                    command.Parameters.AddWithValue("@adSoy", adSoy);
                    command.Parameters.AddWithValue("@nick", nick);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@sifre", sifre);
                    command.ExecuteNonQuery();
                }
            }

            // Ana Register veritabanına kullanıcı bilgilerini kaydet
            using (SqlConnection registerConnection = new SqlConnection(connectionString))
            {
                registerConnection.Open();
                string registerInsertQuery = "INSERT INTO regTable (adSoy, nick, email, phone, sifre) VALUES (@adSoy, @nick, @email, @phone, @sifre)";
                using (SqlCommand registerCommand = new SqlCommand(registerInsertQuery, registerConnection))
                {
                    registerCommand.Parameters.AddWithValue("@adSoy", adSoy);
                    registerCommand.Parameters.AddWithValue("@nick", nick);
                    registerCommand.Parameters.AddWithValue("@email", email);
                    registerCommand.Parameters.AddWithValue("@phone", phone);
                    registerCommand.Parameters.AddWithValue("@sifre", sifre);
                    registerCommand.ExecuteNonQuery();
                }
            }


            MessageBox.Show($"Başarılı Kayıt Olundu: \n İsim: {adSoy}, \n Nick: {nick},\n Email: {email}, \n Telefon Numarası: {phone}, \n Şifre: {sifre}", "Başarılı Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Login l = new Login();
            l.Show();
            Hide();
        }

        private void guna2ImageCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ImageCheckBox1.Checked)
            {
                sifreTxt.PasswordChar = '\0';

            }
            else
            {
                sifreTxt.PasswordChar = '*';

            }
        }

        private void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
