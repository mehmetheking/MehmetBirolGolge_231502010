using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MehmetBirolGolge_231502010.Data;

namespace MehmetBirolGolge_231502010.Forms
{
	public partial class LoginForm : Form
	{
		private TextBox txtUsername;
		private TextBox txtPassword;
		private Button btnLogin;
		private Button btnRegister;
		private Label lblUsername;
		private Label lblPassword;
		private Label lblTitle;

		public LoginForm()
		{
			InitializeComponent();
			InitializeCustomComponents();
		}

		private void InitializeCustomComponents()
		{
			// Form özellikleri
			this.Text = "AI Todo App - Giriş";
			this.Size = new Size(400, 300);
			this.StartPosition = FormStartPosition.CenterScreen;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;

			// Başlık
			lblTitle = new Label
			{
				Text = "AI Destekli Görev Takip Sistemi",
				Location = new Point(50, 20),
				Size = new Size(300, 30),
				Font = new Font("Arial", 16, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleCenter
			};

			// Kullanıcı adı label
			lblUsername = new Label
			{
				Text = "Kullanıcı Adı:",
				Location = new Point(50, 70),
				Size = new Size(100, 20)
			};

			// Kullanıcı adı textbox
			txtUsername = new TextBox
			{
				Location = new Point(50, 90),
				Size = new Size(300, 25),
				Font = new Font("Arial", 10)
			};

			// Şifre label
			lblPassword = new Label
			{
				Text = "Şifre:",
				Location = new Point(50, 120),
				Size = new Size(100, 20)
			};

			// Şifre textbox
			txtPassword = new TextBox
			{
				Location = new Point(50, 140),
				Size = new Size(300, 25),
				PasswordChar = '*',
				Font = new Font("Arial", 10)
			};

			// Enter tuşu ile giriş yapma
			txtPassword.KeyPress += (sender, e) =>
			{
				if (e.KeyChar == (char)Keys.Enter)
				{
					BtnLogin_Click(sender, e);
				}
			};

			// Giriş butonu
			btnLogin = new Button
			{
				Text = "Giriş Yap",
				Location = new Point(50, 180),
				Size = new Size(140, 40),
				BackColor = Color.DodgerBlue,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Arial", 10, FontStyle.Bold)
			};
			btnLogin.Click += BtnLogin_Click;

			// Kayıt ol butonu
			btnRegister = new Button
			{
				Text = "Kayıt Ol",
				Location = new Point(210, 180),
				Size = new Size(140, 40),
				BackColor = Color.LimeGreen,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Arial", 10, FontStyle.Bold)
			};
			btnRegister.Click += BtnRegister_Click;

			// Kontrolleri forma ekle
			this.Controls.AddRange(new Control[] {
				lblTitle, lblUsername, txtUsername,
				lblPassword, txtPassword, btnLogin, btnRegister
			});
		}

		private void BtnLogin_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
			{
				MessageBox.Show("Kullanıcı adı ve şifre boş olamaz!", "Uyarı",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				using (var context = new TodoContext())
				{
					var user = context.Users.FirstOrDefault(u =>
						u.Username == txtUsername.Text &&
						u.Password == txtPassword.Text);

					if (user != null)
					{
						// Giriş başarılı - Ana formu aç
						this.Hide();
						var mainForm = new MainForm(user);
						mainForm.ShowDialog();
						this.Close();
					}
					else
					{
						MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata",
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Hata",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BtnRegister_Click(object sender, EventArgs e)
		{
			var registerForm = new RegisterForm();
			registerForm.ShowDialog();
		}

	}
}