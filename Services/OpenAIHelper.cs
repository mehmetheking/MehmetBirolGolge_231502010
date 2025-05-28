using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace MehmetBirolGolge_231502010
{
	public class OpenAIHelper
	{
		private OpenAIService openAiService;

		public OpenAIHelper()
		{
			InitializeService();
		}

		private void InitializeService()
		{
			string apiKey = ApiConfig.GetApiKey();
			if (!string.IsNullOrEmpty(apiKey))
			{
				openAiService = new OpenAIService(new OpenAiOptions()
				{
					ApiKey = apiKey
				});
			}
		}

		public void UpdateApiKey(string newApiKey)
		{
			ApiConfig.SaveApiKey(newApiKey);
			InitializeService();
		}

		public async Task<string> GenerateResponse(string prompt)
		{
			if (openAiService == null)
			{
				return "Hata: API anahtarı ayarlanmamış. Lütfen önce API anahtarınızı girin.";
			}

			try
			{
				var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
				{
					Messages = new List<ChatMessage>
					{
						ChatMessage.FromSystem("Sen yardımsever bir asistansın. Türkçe olarak yanıt ver."),
						ChatMessage.FromUser(prompt)
					},
					Model = Models.Gpt_3_5_Turbo,
					MaxTokens = 1000,
					Temperature = 0.7f
				});

				if (completionResult.Successful)
				{
					return completionResult.Choices.First().Message.Content;
				}
				else
				{
					if (completionResult.Error == null)
					{
						return "Bilinmeyen hata oluştu.";
					}
					return $"Hata: {completionResult.Error.Message}";
				}
			}
			catch (Exception ex)
			{
				return $"Hata oluştu: {ex.Message}";
			}
		}

		public bool IsConfigured()
		{
			return openAiService != null && ApiConfig.HasApiKey();
		}
	}
}