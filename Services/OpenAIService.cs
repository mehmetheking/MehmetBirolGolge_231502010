using System;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using static OpenAI.ObjectModels.Models;

namespace MehmetBirolGolge_231502010.Services
{
	public class OpenAIService
	{
		private readonly OpenAIAPI api;

		public OpenAIService()
		{
			// API anahtarını buraya yapıştır
			// OpenAI.com'dan alacağın API key'i buraya yaz
			string apiKey = "sk-..."; // BURAYA API KEY'İNİ YAPIŞTIR

			api = new OpenAIAPI(apiKey);
		}

		public async Task<string> GenerateTaskDescription(string taskTitle)
		{
			try
			{
				var prompt = $"'{taskTitle}' başlıklı görev için detaylı bir açıklama oluştur. " +
						   "Açıklama, görevi nasıl tamamlayacağımı adım adım içersin. " +
						   "Maksimum 3-4 cümle olsun. Türkçe yaz.";

				var result = await api.Completions.CreateCompletionAsync(new CompletionRequest
				{
					Prompt = prompt,
					Model = Model.Davinci3,
					MaxTokens = 150,
					Temperature = 0.7
				});

				return result.Completions[0].Text.Trim();
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
				var prompt = $"Görev: {taskTitle}\nAçıklama: {description}\n\n" +
						   "Bu görevin öncelik seviyesini belirle. " +
						   "Sadece şu kelimelerden birini yaz: Düşük, Orta veya Yüksek";

				var result = await api.Completions.CreateCompletionAsync(new CompletionRequest
				{
					Prompt = prompt,
					Model = Model.Davinci3,
					MaxTokens = 10,
					Temperature = 0.3
				});

				string priority = result.Completions[0].Text.Trim();

				// Geçerli değerlerden biri mi kontrol et
				if (priority.Contains("Yüksek")) return "Yüksek";
				if (priority.Contains("Düşük")) return "Düşük";
				return "Orta";
			}
			catch
			{
				return "Orta"; // Hata durumunda varsayılan
			}
		}

		// Bonus: Görev önerisi oluşturma
		public async Task<string> SuggestRelatedTasks(string taskTitle)
		{
			try
			{
				var prompt = $"'{taskTitle}' görevi ile ilgili yapılması gereken 3 alt görev öner. " +
						   "Her birini yeni satırda listele. Kısa ve net ol.";

				var result = await api.Completions.CreateCompletionAsync(new CompletionRequest
				{
					Prompt = prompt,
					Model = Model.Davinci3,
					MaxTokens = 100,
					Temperature = 0.8
				});

				return result.Completions[0].Text.Trim();
			}
			catch
			{
				return "İlgili görev önerisi oluşturulamadı.";
			}
		}
	}
}