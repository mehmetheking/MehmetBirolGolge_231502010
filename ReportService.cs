using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using MehmetBirolGolge_231502010.Data;
using MehmetBirolGolge_231502010.Models;
using System.Drawing;

namespace MehmetBirolGolge_231502010.Services
{
	public class ReportService
	{
		public void GenerateMonthlyTaskReport(User user)
		{
			try
			{
				using (var context = new TodoContext())
				{
					// Son 30 günün görevlerini al
					var startDate = DateTime.Now.AddDays(-30);
					var tasks = context.Tasks
						.Where(t => t.UserID == user.UserID && t.CreatedDate >= startDate)
						.OrderByDescending(t => t.CreatedDate)
						.ToList();

					// Excel paketi oluştur
					ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
					using (var package = new ExcelPackage())
					{
						// Worksheet oluştur
						var worksheet = package.Workbook.Worksheets.Add("Aylık Görev Raporu");

						// Başlıklar
						worksheet.Cells[1, 1].Value = "AYLIK GÖREV RAPORU";
						worksheet.Cells[1, 1, 1, 7].Merge = true;
						worksheet.Cells[1, 1].Style.Font.Size = 16;
						worksheet.Cells[1, 1].Style.Font.Bold = true;
						worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

						worksheet.Cells[2, 1].Value = $"Kullanıcı: {user.Username}";
						worksheet.Cells[3, 1].Value = $"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy}";
						worksheet.Cells[4, 1].Value = $"Dönem: {startDate:dd.MM.yyyy} - {DateTime.Now:dd.MM.yyyy}";

						// Tablo başlıkları
						int row = 6;
						worksheet.Cells[row, 1].Value = "Başlık";
						worksheet.Cells[row, 2].Value = "Açıklama";
						worksheet.Cells[row, 3].Value = "Öncelik";
						worksheet.Cells[row, 4].Value = "Durum";
						worksheet.Cells[row, 5].Value = "Oluşturma Tarihi";
						worksheet.Cells[row, 6].Value = "Bitiş Tarihi";
						worksheet.Cells[row, 7].Value = "AI Açıklama";

						// Başlık satırını kalın yap ve arka plan rengi ekle
						worksheet.Cells[row, 1, row, 7].Style.Font.Bold = true;
						worksheet.Cells[row, 1, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
						worksheet.Cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

						// Görevleri ekle
						row++;
						foreach (var task in tasks)
						{
							worksheet.Cells[row, 1].Value = task.Title;
							worksheet.Cells[row, 2].Value = task.Description ?? "Açıklama yok";
							worksheet.Cells[row, 3].Value = task.Priority;
							worksheet.Cells[row, 4].Value = task.IsCompleted ? "Tamamlandı" : "Devam Ediyor";
							worksheet.Cells[row, 5].Value = task.CreatedDate.ToString("dd.MM.yyyy HH:mm");
							worksheet.Cells[row, 6].Value = task.DueDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş";
							worksheet.Cells[row, 7].Value = task.AIDescription ?? "AI açıklama yok";

							// Önceliğe göre renklendirme
							if (task.Priority == "Yüksek")
								worksheet.Cells[row, 3].Style.Font.Color.SetColor(Color.Red);
							else if (task.Priority == "Düşük")
								worksheet.Cells[row, 3].Style.Font.Color.SetColor(Color.Green);

							row++;
						}

						// İstatistikler
						row += 2;
						worksheet.Cells[row, 1].Value = "İSTATİSTİKLER";
						worksheet.Cells[row, 1].Style.Font.Bold = true;
						worksheet.Cells[row, 1].Style.Font.Size = 14;

						row++;
						worksheet.Cells[row, 1].Value = "Toplam Görev:";
						worksheet.Cells[row, 2].Value = tasks.Count;

						row++;
						worksheet.Cells[row, 1].Value = "Tamamlanan:";
						worksheet.Cells[row, 2].Value = tasks.Count(t => t.IsCompleted);
						worksheet.Cells[row, 2].Style.Font.Color.SetColor(Color.Green);

						row++;
						worksheet.Cells[row, 1].Value = "Devam Eden:";
						worksheet.Cells[row, 2].Value = tasks.Count(t => !t.IsCompleted);
						worksheet.Cells[row, 2].Style.Font.Color.SetColor(Color.Orange);

						row++;
						worksheet.Cells[row, 1].Value = "Tamamlanma Oranı:";
						var completionRate = tasks.Count > 0
							? (tasks.Count(t => t.IsCompleted) * 100.0 / tasks.Count)
							: 0;
						worksheet.Cells[row, 2].Value = $"%{completionRate:F1}";
						worksheet.Cells[row, 2].Style.Font.Bold = true;

						// Öncelik dağılımı
						row += 2;
						worksheet.Cells[row, 1].Value = "ÖNCELİK DAĞILIMI";
						worksheet.Cells[row, 1].Style.Font.Bold = true;

						row++;
						worksheet.Cells[row, 1].Value = "Yüksek Öncelik:";
						worksheet.Cells[row, 2].Value = tasks.Count(t => t.Priority == "Yüksek");

						row++;
						worksheet.Cells[row, 1].Value = "Orta Öncelik:";
						worksheet.Cells[row, 2].Value = tasks.Count(t => t.Priority == "Orta");

						row++;
						worksheet.Cells[row, 1].Value = "Düşük Öncelik:";
						worksheet.Cells[row, 2].Value = tasks.Count(t => t.Priority == "Düşük");

						// Sütun genişliklerini ayarla
						worksheet.Column(1).Width = 30;
						worksheet.Column(2).Width = 40;
						worksheet.Column(3).Width = 15;
						worksheet.Column(4).Width = 15;
						worksheet.Column(5).Width = 20;
						worksheet.Column(6).Width = 20;
						worksheet.Column(7).Width = 50;

						// Tüm hücrelere kenarlık ekle
						var dataRange = worksheet.Cells[6, 1, row, 7];
						dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
						dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
						dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

						// Dosyayı kaydet
						var saveDialog = new SaveFileDialog
						{
							Filter = "Excel Files|*.xlsx",
							FileName = $"GorevRaporu_{user.Username}_{DateTime.Now:yyyyMMdd}.xlsx"
						};

						if (saveDialog.ShowDialog() == DialogResult.OK)
						{
							File.WriteAllBytes(saveDialog.FileName, package.GetAsByteArray());
							MessageBox.Show("Rapor başarıyla oluşturuldu!", "Başarılı",
								MessageBoxButtons.OK, MessageBoxIcon.Information);

							// Dosyayı aç
							System.Diagnostics.Process.Start(saveDialog.FileName);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Rapor oluşturulurken hata: {ex.Message}", "Hata",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}