using System;
using System.Configuration;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace MehmetBirolGolge_231502010.Services
{
	public class OpenAIService
	{
		private readonly string apiKey;
		private readonly RestClient client;
		string openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

		public OpenAIService()
		{
			// Önce Environment Variable'dan dene
			apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

			// Yoksa App.config'den al
			if (string.IsNullOrEmpty(apiKey))
			{
				apiKey = ConfigurationManager.AppSettings["OpenAI_API_Key"];
			}

			if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_API_KEY_HERE")
			{
				throw new Exception("OpenAI API anahtarı bulunamadı! Lütfen Environment Variable veya App.config'e ekleyin.");
			}

			client = new RestClient("https://api.openai.com/v1/");
		}

		public async Task<string> GenerateTaskDescription(string taskTitle)
		{
			try
			{
				var request = new RestRequest("chat/completions", Method.POST);
				request.AddHeader("Authorization", $"Bearer {apiKey}");
				request.AddHeader("Content-Type", "application/json");

				var body = new
				{
					model = "gpt-3.5-turbo",
					messages = new[]
					{
						new { role = "system", content = "Sen yardımcı bir asistansın. Görev açıklamaları oluşturuyorsun." },
						new { role = "user", content = $"'{taskTitle}' başlıklı görev için detaylı bir açıklama oluştur. Açıklama, görevi nasıl tamamlayacağımı adım adım içersin. Maksimum 3-4 cümle olsun." }
					},
					temperature = 0.7
				};

				request.AddJsonBody(body);

				var response = await client.ExecuteTaskAsync(request);

				if (response.IsSuccessful)
				{
					dynamic result = JsonConvert.DeserializeObject(response.Content);
					return result.choices[0].message.content;
				}
				else
				{
					// Hata detayını görelim
					return $"AI Hatası: {response.StatusCode} - {response.Content}";
				}
			}
			catch (Exception ex)
			{
				return $"AI açıklama oluşturulamadı: {ex.Message}";
			}
		}

		public async Task<string> SuggestTaskPriority(string taskTitle, string description)
		{
			try
			{
				var request = new RestRequest("chat/completions", Method.POST);
				request.AddHeader("Authorization", $"Bearer {apiKey}");
				request.AddHeader("Content-Type", "application/json");

				var body = new
				{
					model = "gpt-3.5-turbo",
					messages = new[]
					{
						new { role = "system", content = "Görev öncelik seviyesi belirleyen bir asistansın. Sadece 'Düşük', 'Orta' veya 'Yüksek' cevap ver." },
						new { role = "user", content = $"Görev: {taskTitle}\nAçıklama: {description}\n\nBu görevin öncelik seviyesi nedir?" }
					},
					temperature = 0.3,
					max_tokens = 10
				};

				request.AddJsonBody(body);

				var response = await client.ExecuteTaskAsync(request);

				if (response.IsSuccessful)
				{
					dynamic result = JsonConvert.DeserializeObject(response.Content);
					string priority = result.choices[0].message.content.ToString().Trim();

					if (priority.Contains("Yüksek")) return "Yüksek";
					if (priority.Contains("Düşük")) return "Düşük";
					return "Orta";
				}

				return "Orta";
			}
			catch
			{
				return "Orta";
			}
		}

		public async Task<string> SuggestRelatedTasks(string taskTitle)
		{
			try
			{
				var request = new RestRequest("chat/completions", Method.POST);
				request.AddHeader("Authorization", $"Bearer {apiKey}");
				request.AddHeader("Content-Type", "application/json");

				var body = new
				{
					model = "gpt-3.5-turbo",
					messages = new[]
					{
						new { role = "system", content = "Görev planlama asistanısın. İlgili alt görevler öneriyorsun." },
						new { role = "user", content = $"'{taskTitle}' görevi ile ilgili yapılması gereken 3 alt görev öner. Her birini yeni satırda listele." }
					},
					temperature = 0.8,
					max_tokens = 150
				};

				request.AddJsonBody(body);

				var response = await client.ExecuteTaskAsync(request);

				if (response.IsSuccessful)
				{
					dynamic result = JsonConvert.DeserializeObject(response.Content);
					return result.choices[0].message.content;
				}

				return "İlgili görev önerisi oluşturulamadı.";
			}
			catch
			{
				return "İlgili görev önerisi oluşturulamadı.";
			}
		}
	}
}