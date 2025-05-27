using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MehmetBirolGolge_231502010.Data;
using MehmetBirolGolge_231502010.Models;

namespace MehmetBirolGolge_231502010.Forms
{
	public partial class RegisterForm : Form
	{
		private TextBox txtUsername;
		private TextBox txtPassword;
		private TextBox txtPasswordConfirm;
		private TextBox txtEmail;
		private Button btnRegister;
		private Button btnCancel;
		private Label lblUsername;
		private Label lblPassword;
		private Label lblPasswordConfirm;
		private Label lblEmail;
		private Label lblTitle;

		public RegisterForm()
		{
			InitializeComponent();
			InitializeCustomComponents();
		}

		private void InitializeCustomComponents()
		{
			// Form özellikleri
			this.Text = "Yeni Kullanıcı Kaydı";
			this.Size = new Size(400, 400);
			this.StartPosition = FormStartPosition.CenterScreen;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;

			// Başlık
			lblTitle = new Label
			{
				Text = "Kayıt Formu",
				Location = new Point(50, 20),
				Size = new Size(300, 30),
				Font = new Font("Arial", 16, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleCenter
			};

			// Kullanıcı adı
			lblUsername = new Label
			{
				Text = "Kullanıcı Adı:",
				Location = new Point(50, 70),
				Size = new Size(100, 20)
			};

			txtUsername = new TextBox
			{
				Location = new Point(50, 90),
				Size = new Size(300, 25),
				Font = new Font("Arial", 10)
			};

			// Email
			lblEmail = new Label
			{
				Text = "E-posta:",
				Location = new Point(50, 120),
				Size = new Size(100, 20)
			};

			txtEmail = new TextBox
			{
				Location = new Point(50, 140),
				Size = new Size(300, 25),
				Font = new Font("Arial", 10)
			};

			// Şifre
			lblPassword = new Label
			{
				Text = "Şifre:",
				Location = new Point(50, 170),
				Size = new Size(100, 20)
			};

			txtPassword = new TextBox
			{
				Location = new Point(50, 190),
				Size = new Size(300, 25),
				PasswordChar = '*',
				Font = new Font("Arial", 10)
			};

			// Şifre tekrar
			lblPasswordConfirm = new Label
			{
				Text = "Şifre Tekrar:",
				Location = new Point(50, 220),
				Size = new Size(100, 20)
			};

			txtPasswordConfirm = new TextBox
			{
				Location = new Point(50, 240),
				Size = new Size(300, 25),
				PasswordChar = '*',
				Font = new Font("Arial", 10)
			};

			// Kaydet butonu
			btnRegister = new Button
			{
				Text = "Kayıt Ol",
				Location = new Point(50, 290),
				Size = new Size(140, 40),
				BackColor = Color.LimeGreen,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Arial", 10, FontStyle.Bold)
			};
			btnRegister.Click += BtnRegister_Click;

			// İptal butonu
			btnCancel = new Button
			{
				Text = "İptal",
				Location = new Point(210, 290),
				Size = new Size(140, 40),
				BackColor = Color.Gray,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Arial", 10, FontStyle.Bold)
			};
			btnCancel.Click += (s, e) => this.Close();

			// Kontrolleri forma ekle
			this.Controls.AddRange(new Control[] {
				lblTitle, lblUsername, txtUsername,
				lblEmail, txtEmail,
				lblPassword, txtPassword,
				lblPasswordConfirm, txtPasswordConfirm,
				btnRegister, btnCancel
			});
		}

		private void BtnRegister_Click(object sender, EventArgs e)
		{
			// Validasyon
			if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
				string.IsNullOrWhiteSpace(txtPassword.Text) ||
				string.IsNullOrWhiteSpace(txtEmail.Text))
			{
				MessageBox.Show("Tüm alanları doldurunuz!", "Uyarı",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (txtPassword.Text != txtPasswordConfirm.Text)
			{
				MessageBox.Show("Şifreler eşleşmiyor!", "Uyarı",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (txtPassword.Text.Length < 6)
			{
				MessageBox.Show("Şifre en az 6 karakter olmalıdır!", "Uyarı",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
			{
				MessageBox.Show("Geçerli bir e-posta adresi giriniz!", "Uyarı",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				using (var context = new TodoContext())
				{
					// Kullanıcı adı kontrolü
					if (context.Users.Any(u => u.Username == txtUsername.Text))
					{
						MessageBox.Show("Bu kullanıcı adı zaten kullanımda!", "Hata",
							MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Yeni kullanıcı oluştur
					var newUser = new User
					{
						Username = txtUsername.Text,
						Password = txtPassword.Text,
						Email = txtEmail.Text,
						Role = "User",
						CreatedDate = DateTime.Now
					};

					context.Users.Add(newUser);
					context.SaveChanges();

					// Varsayılan kategoriler oluştur
					CreateDefaultCategories(context);

					MessageBox.Show("Kayıt başarılı! Giriş yapabilirsiniz.", "Başarılı",
						MessageBoxButtons.OK, MessageBoxIcon.Information);

					this.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Kayıt hatası: {ex.Message}", "Hata",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void CreateDefaultCategories(TodoContext context)
		{
			if (!context.Categories.Any())
			{
				var categories = new[]
				{
					new Category { CategoryName = "İş", Color = "#FF0000" },
					new Category { CategoryName = "Kişisel", Color = "#00FF00" },
					new Category { CategoryName = "Alışveriş", Color = "#0000FF" },
					new Category { CategoryName = "Sağlık", Color = "#FFA500" },
					new Category { CategoryName = "Eğitim", Color = "#800080" }
				};

				context.Categories.AddRange(categories);
				context.SaveChanges();
			}
		}

	}
}