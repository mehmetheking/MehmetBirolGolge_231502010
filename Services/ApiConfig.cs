using System;
using System.IO;
using System.Text;

namespace MehmetBirolGolge_231502010
{
	public static class ApiConfig
	{
		private static string configFilePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"MehmetBirolGolge_231502010",
			"config.txt"
		);

		public static string GetApiKey()
		{
			try
			{
				if (File.Exists(configFilePath))
				{
					return File.ReadAllText(configFilePath).Trim();
				}
			}
			catch { }

			return string.Empty;
		}

		public static void SaveApiKey(string apiKey)
		{
			try
			{
				string directory = Path.GetDirectoryName(configFilePath);
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				File.WriteAllText(configFilePath, apiKey);
			}
			catch (Exception ex)
			{
				throw new Exception($"API anahtarı kaydedilemedi: {ex.Message}");
			}
		}

		public static bool HasApiKey()
		{
			return !string.IsNullOrEmpty(GetApiKey());
		}
	}
}