using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MehmetBirolGolge_231502010.Services
{
	public class DeepSeekService
	{
		private readonly string _apiUrl;
		private readonly HttpClient _httpClient;

		public DeepSeekService()
		{
			// LM Studio için URL (DeepSeek modelini LM Studio'da çalıştırıyorsan)
			_apiUrl = "http://localhost:1234/v1/completions";
			_httpClient = new HttpClient();
			_httpClient.Timeout = TimeSpan.FromSeconds(30);
		}

		public async Task<string> GenerateTaskDescription(string taskTitle)
		{
			try
			{
				var prompt = $"Lütfen '{taskTitle}' başlıklı görev için detaylı bir açıklama oluştur. " +
						   "Açıklama, görevi nasıl tamamlayacağımı adım adım içersin. Maksimum 3-4 cümle olsun.";

				// LM Studio için request format
				var requestBody = new
				{
					model = "deepseek-coder-6.7b-instruct.Q4_K_M.gguf",
					messages = new[]
					{
						new { role = "user", content = prompt }
					},
					temperature = 0.7,
					max_tokens = 200
				};

				var json = JsonConvert.SerializeObject(requestBody);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await _httpClient.PostAsync(_apiUrl, content);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					dynamic result = JsonConvert.DeserializeObject(responseContent);
					return result.choices[0].message.content;
				}
				else
				{
					return "AI açıklama oluşturulamadı.";
				}
			}
			catch (Exception ex)
			{
				return $"AI bağlantı hatası: {ex.Message}";
			}
		}

		public async Task<string> SuggestTaskPriority(string taskTitle, string description)
		{
			try
			{
				var prompt = $"Görev: {taskTitle}\nAçıklama: {description}\n" +
						   "Bu görevin öncelik seviyesini belirle: Düşük, Orta veya Yüksek. " +
						   "Sadece tek kelime (Düşük/Orta/Yüksek) olarak cevap ver.";

				var requestBody = new
				{
					model = "deepseek-coder-6.7b-instruct.Q4_K_M.gguf",
					messages = new[]
					{
						new { role = "user", content = prompt }
					},
					temperature = 0.3,
					max_tokens = 10
				};

				var json = JsonConvert.SerializeObject(requestBody);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await _httpClient.PostAsync(_apiUrl, content);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					dynamic result = JsonConvert.DeserializeObject(responseContent);
					string priority = result.choices[0].message.content.ToString().Trim();

					// Geçerli öncelik değerlerinden biri mi kontrol et
					if (priority.Contains("Yüksek")) return "Yüksek";
					if (priority.Contains("Düşük")) return "Düşük";
					return "Orta"; // Default
				}

				return "Orta";
			}
			catch
			{
				return "Orta"; // Hata durumunda default
			}
		}
	}
}