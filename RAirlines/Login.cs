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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Register r = new Register();
            r.Show();
            Hide();
        }

        public static string girNick;
        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            girNick = girisNick.Text;
            string nick = girisNick.Text;
            string sifre = guna2TextBox1.Text;
            if (string.IsNullOrWhiteSpace(nick) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userDatabaseName = $"{nick}";
            string userConnectionString = $@"Data Source=SQL-GİRİS-İSİM;Initial Catalog={userDatabaseName};Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(userConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE nick = @nick AND sifre = @sifre", connection);
                command.Parameters.AddWithValue("@nick", nick);
                command.Parameters.AddWithValue("@sifre", sifre);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        MainPage mm = new MainPage();
                        SqlCommand permIdCommand = new SqlCommand("SELECT perm FROM Users WHERE nick = @nick", connection);
                        permIdCommand.Parameters.AddWithValue("@nick", nick);
                        int permID = (int)permIdCommand.ExecuteScalar();

                        if (permID == 1)
                        {

                            mm.adminButton.Visible = true;
                        }
                        else
                        {

                            mm.adminButton.Visible = false;
                        }

                        mm.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı Adı veya şifre yanlış", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       
        private void guna2ImageCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ImageCheckBox1.Checked)
            {
                guna2TextBox1.PasswordChar = '\0';
              
            }
            else
            {
                guna2TextBox1.PasswordChar = '*';
               
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sifremiUnuttum s = new sifremiUnuttum();
            s.Show();
        }

        private void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
