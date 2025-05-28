using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MehmetBirolGolge_231502010.Data;
using MehmetBirolGolge_231502010.Models;
using MehmetBirolGolge_231502010.Services;

namespace MehmetBirolGolge_231502010.Forms
{
	public partial class MainForm : Form
	{
		private User currentUser;
		private DataGridView dgvTasks;
		private TextBox txtTaskTitle;
		private TextBox txtDescription;
		private ComboBox cmbPriority;
		private ComboBox cmbCategory;
		private DateTimePicker dtpDueDate;
		private CheckBox chkCompleted;
		private Button btnAdd;
		private Button btnUpdate;
		private Button btnDelete;
		private Button btnGenerateAI;
		private Button btnReport;
		private Button btnRefresh;
		private Label lblWelcome;
		private Label lblTaskTitle;
		private Label lblDescription;
		private Label lblPriority;
		private Label lblCategory;
		private Label lblDueDate;
		private TextBox txtAIDescription;
		private Label lblAIDescription;
		private OpenAIService aiService;
		private ReportService reportService;

		public MainForm(User user)
		{
			currentUser = user;
			aiService = new OpenAIService();
			reportService = new ReportService();
			InitializeComponent();
			InitializeCustomComponents();
			LoadData();
		}

		private void InitializeCustomComponents()
		{
			// Form özellikleri
			this.Text = $"AI Todo App - Hoşgeldin {currentUser.Username}";
			this.Size = new Size(1200, 700);
			this.StartPosition = FormStartPosition.CenterScreen;

			// Hoşgeldin etiketi
			lblWelcome = new Label
			{
				Text = $"Hoşgeldin, {currentUser.Username}! (Rol: {currentUser.Role})",
				Location = new Point(20, 10),
				Size = new Size(400, 30),
				Font = new Font("Arial", 14, FontStyle.Bold)
			};

			// Sol panel - Görev ekleme/düzenleme
			Panel leftPanel = new Panel
			{
				Location = new Point(20, 50),
				Size = new Size(400, 500),
				BorderStyle = BorderStyle.FixedSingle
			};

			// Görev başlığı
			lblTaskTitle = new Label
			{
				Text = "Görev Başlığı:",
				Location = new Point(10, 10),
				Size = new Size(100, 20)
			};

			txtTaskTitle = new TextBox
			{
				Location = new Point(10, 30),
				Size = new Size(380, 25),
				Font = new Font("Arial", 10)
			};

			// Açıklama
			lblDescription = new Label
			{
				Text = "Açıklama:",
				Location = new Point(10, 60),
				Size = new Size(100, 20)
			};

			txtDescription = new TextBox
			{
				Location = new Point(10, 80),
				Size = new Size(380, 60),
				Multiline = true,
				Font = new Font("Arial", 10)
			};

			// Öncelik
			lblPriority = new Label
			{
				Text = "Öncelik:",
				Location = new Point(10, 150),
				Size = new Size(100, 20)
			};

			cmbPriority = new ComboBox
			{
				Location = new Point(10, 170),
				Size = new Size(180, 25),
				DropDownStyle = ComboBoxStyle.DropDownList
			};
			cmbPriority.Items.AddRange(new[] { "Düşük", "Orta", "Yüksek" });
			cmbPriority.SelectedIndex = 1;

			// Kategori
			lblCategory = new Label
			{
				Text = "Kategori:",
				Location = new Point(210, 150),
				Size = new Size(100, 20)
			};

			cmbCategory = new ComboBox
			{
				Location = new Point(210, 170),
				Size = new Size(180, 25),
				DropDownStyle = ComboBoxStyle.DropDownList
			};

			// Bitiş tarihi
			lblDueDate = new Label
			{
				Text = "Bitiş Tarihi:",
				Location = new Point(10, 200),
				Size = new Size(100, 20)
			};

			dtpDueDate = new DateTimePicker
			{
				Location = new Point(10, 220),
				Size = new Size(180, 25),
				Format = DateTimePickerFormat.Short
			};

			// Tamamlandı
			chkCompleted = new CheckBox
			{
				Text = "Tamamlandı",
				Location = new Point(210, 220),
				Size = new Size(180, 25)
			};

			// AI Açıklama
			lblAIDescription = new Label
			{
				Text = "AI Açıklama:",
				Location = new Point(10, 250),
				Size = new Size(100, 20)
			};

			txtAIDescription = new TextBox
			{
				Location = new Point(10, 270),
				Size = new Size(380, 80),
				Multiline = true,
				ReadOnly = true,
				Font = new Font("Arial", 9),
				BackColor = Color.LightYellow
			};

			// Butonlar
			btnAdd = new Button
			{
				Text = "Yeni Görev Ekle",
				Location = new Point(10, 360),
				Size = new Size(120, 40),
				BackColor = Color.LimeGreen,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat
			};
			btnAdd.Click += BtnAdd_Click;

			btnUpdate = new Button
			{
				Text = "Güncelle",
				Location = new Point(140, 360),
				Size = new Size(120, 40),
				BackColor = Color.Orange,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat
			};
			btnUpdate.Click += BtnUpdate_Click;

			btnDelete = new Button
			{
				Text = "Sil",
				Location = new Point(270, 360),
				Size = new Size(120, 40),
				BackColor = Color.Red,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat
			};
			btnDelete.Click += BtnDelete_Click;

			btnGenerateAI = new Button
			{
				Text = "AI ile Açıklama Oluştur",
				Location = new Point(10, 410),
				Size = new Size(180, 40),
				BackColor = Color.DodgerBlue,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat
			};
			btnGenerateAI.Click += BtnGenerateAI_Click;

			btnReport = new Button
			{
				Text = "Excel Rapor Al",
				Location = new Point(210, 410),
				Size = new Size(180, 40),
				BackColor = Color.Purple,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat
			};
			btnReport.Click += BtnReport_Click;

			// Sol panele kontrolleri ekle
			leftPanel.Controls.AddRange(new Control[] {
				lblTaskTitle, txtTaskTitle,
				lblDescription, txtDescription,
				lblPriority, cmbPriority,
				lblCategory, cmbCategory,
				lblDueDate, dtpDueDate,
				chkCompleted,
				lblAIDescription, txtAIDescription,
				btnAdd, btnUpdate, btnDelete,
				btnGenerateAI, btnReport
			});

			// Sağ panel - Görev listesi
			Panel rightPanel = new Panel
			{
				Location = new Point(440, 50),
				Size = new Size(720, 500),
				BorderStyle = BorderStyle.FixedSingle
			};

			// Yenile butonu
			btnRefresh = new Button
			{
				Text = "Listeyi Yenile",
				Location = new Point(620, 10),
				Size = new Size(90, 30),
				BackColor = Color.SteelBlue,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat
			};
			btnRefresh.Click += (s, e) => LoadData();

			// DataGridView
			dgvTasks = new DataGridView
			{
				Location = new Point(10, 50),
				Size = new Size(700, 440),
				AllowUserToAddRows = false,
				ReadOnly = true,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
			};
			dgvTasks.SelectionChanged += DgvTasks_SelectionChanged;

			rightPanel.Controls.Add(btnRefresh);
			rightPanel.Controls.Add(dgvTasks);

			// Ana forma ekle
			this.Controls.AddRange(new Control[] {
				lblWelcome, leftPanel, rightPanel
			});

			LoadCategories();
		}

		private void LoadCategories()
		{
			try
			{
				using (var context = new TodoContext())
				{
					var categories = context.Categories.ToList();
					cmbCategory.DataSource = categories;
					cmbCategory.DisplayMember = "CategoryName";
					cmbCategory.ValueMember = "CategoryID";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Kategori yükleme hatası: {ex.Message}");
			}
		}

		private void LoadData()
		{
			try
			{
				using (var context = new TodoContext())
				{
					var tasks = context.Tasks
						.Where(t => t.UserID == currentUser.UserID)
						.Select(t => new
						{
							t.TaskID,
							t.Title,
							t.Description,
							t.Priority,
							Durum = t.IsCompleted ? "Tamamlandı" : "Devam Ediyor",
							BitisTarihi = t.DueDate,
							OlusturmaTarihi = t.CreatedDate,
							t.AIDescription
						})
						.OrderByDescending(t => t.OlusturmaTarihi)
						.ToList();

					dgvTasks.DataSource = tasks;

					// Kolon başlıklarını düzenle
					if (dgvTasks.Columns.Count > 0)
					{
						dgvTasks.Columns["TaskID"].Visible = false;
						dgvTasks.Columns["Title"].HeaderText = "Başlık";
						dgvTasks.Columns["Description"].HeaderText = "Açıklama";
						dgvTasks.Columns["Priority"].HeaderText = "Öncelik";
						dgvTasks.Columns["AIDescription"].HeaderText = "AI Açıklama";
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Veri yükleme hatası: {ex.Message}");
			}
		}

		private void DgvTasks_SelectionChanged(object sender, EventArgs e)
		{
			if (dgvTasks.CurrentRow != null)
			{
				using (var context = new TodoContext())
				{
					int taskId = Convert.ToInt32(dgvTasks.CurrentRow.Cells["TaskID"].Value);
					var task = context.Tasks.Find(taskId);

					if (task != null)
					{
						txtTaskTitle.Text = task.Title;
						txtDescription.Text = task.Description;
						cmbPriority.Text = task.Priority;
						chkCompleted.Checked = task.IsCompleted;
						dtpDueDate.Value = task.DueDate ?? DateTime.Now;
						txtAIDescription.Text = task.AIDescription;

						// Kategoriyi seç
						var taskCategory = context.TaskCategories
							.FirstOrDefault(tc => tc.TaskID == taskId);
						if (taskCategory != null)
						{
							cmbCategory.SelectedValue = taskCategory.CategoryID;
						}
					}
				}
			}
		}

		private async void BtnGenerateAI_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtTaskTitle.Text))
			{
				MessageBox.Show("Önce görev başlığı giriniz!");
				return;
			}

			try
			{
				btnGenerateAI.Enabled = false;
				btnGenerateAI.Text = "AI Düşünüyor...";

				// AI'dan açıklama al
				string aiDescription = await aiService.GenerateTaskDescription(txtTaskTitle.Text);
				txtAIDescription.Text = aiDescription;

				// AI'dan öncelik önerisi al
				string suggestedPriority = await aiService.SuggestTaskPriority(txtTaskTitle.Text, txtDescription.Text);
				cmbPriority.Text = suggestedPriority;

				MessageBox.Show("AI açıklama ve öncelik önerisi oluşturuldu!", "Başarılı",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"AI hatası: {ex.Message}");
			}
			finally
			{
				btnGenerateAI.Enabled = true;
				btnGenerateAI.Text = "AI ile Açıklama Oluştur";
			}
		}

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtTaskTitle.Text))
			{
				MessageBox.Show("Görev başlığı boş olamaz!");
				return;
			}

			try
			{
				using (var context = new TodoContext())
				{
					var newTask = new Task
					{
						UserID = currentUser.UserID,
						Title = txtTaskTitle.Text,
						Description = txtDescription.Text,
						Priority = cmbPriority.Text,
						DueDate = dtpDueDate.Value,
						IsCompleted = chkCompleted.Checked,
						AIDescription = txtAIDescription.Text,
						CreatedDate = DateTime.Now
					};

					context.Tasks.Add(newTask);
					context.SaveChanges();

					// Kategori ilişkisi
					if (cmbCategory.SelectedValue != null)
					{
						var taskCategory = new TaskCategory
						{
							TaskID = newTask.TaskID,
							CategoryID = (int)cmbCategory.SelectedValue
						};
						context.TaskCategories.Add(taskCategory);
						context.SaveChanges();
					}

					MessageBox.Show("Görev başarıyla eklendi!");
					ClearForm();
					LoadData();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ekleme hatası: {ex.Message}");
			}
		}

		private void BtnUpdate_Click(object sender, EventArgs e)
		{
			if (dgvTasks.CurrentRow == null)
			{
				MessageBox.Show("Güncellenecek görev seçiniz!");
				return;
			}

			try
			{
				using (var context = new TodoContext())
				{
					int taskId = Convert.ToInt32(dgvTasks.CurrentRow.Cells["TaskID"].Value);
					var task = context.Tasks.Find(taskId);

					if (task != null)
					{
						task.Title = txtTaskTitle.Text;
						task.Description = txtDescription.Text;
						task.Priority = cmbPriority.Text;
						task.DueDate = dtpDueDate.Value;
						task.IsCompleted = chkCompleted.Checked;
						task.AIDescription = txtAIDescription.Text;

						context.SaveChanges();

						MessageBox.Show("Görev güncellendi!");
						LoadData();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Güncelleme hatası: {ex.Message}");
			}
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			if (dgvTasks.CurrentRow == null)
			{
				MessageBox.Show("Silinecek görev seçiniz!");
				return;
			}

			if (MessageBox.Show("Bu görevi silmek istediğinize emin misiniz?", "Onay",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{
					using (var context = new TodoContext())
					{
						int taskId = Convert.ToInt32(dgvTasks.CurrentRow.Cells["TaskID"].Value);
						var task = context.Tasks.Find(taskId);

						if (task != null)
						{
							// Önce kategori ilişkilerini sil
							var taskCategories = context.TaskCategories
								.Where(tc => tc.TaskID == taskId);
							context.TaskCategories.RemoveRange(taskCategories);

							// Sonra görevi sil
							context.Tasks.Remove(task);
							context.SaveChanges();

							MessageBox.Show("Görev silindi!");
							ClearForm();
							LoadData();
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Silme hatası: {ex.Message}");
				}
			}
		}

		private void BtnReport_Click(object sender, EventArgs e)
		{
			try
			{
				reportService.GenerateMonthlyTaskReport(currentUser);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Rapor hatası: {ex.Message}");
			}
		}

		private void ClearForm()
		{
			txtTaskTitle.Clear();
			txtDescription.Clear();
			txtAIDescription.Clear();
			cmbPriority.SelectedIndex = 1;
			chkCompleted.Checked = false;
			dtpDueDate.Value = DateTime.Now;
		}

	}
}