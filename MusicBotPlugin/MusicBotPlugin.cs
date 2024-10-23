using System;
using System.Text;
using System.IO;
using TS3AudioBot;
using TS3AudioBot.Plugins;
using System.Text.RegularExpressions;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TSLib.Full;
using System.Threading;
using TS3AudioBot.CommandSystem;

namespace MusicBotPlugin
{

	public class LoginStatus
	{
		public int code { get; set; }
		public string message { get; set; }
		public string cookie { get; set; }
	}


	public class LoginKey
	{
		public LoginKeyData data { get; set; }
		public int code { get; set; }
	}

	public class LoginKeyData
	{
		public int code { get; set; }
		public string unikey { get; set; }
	}

	public class LoginQr
	{
		public int code { get; set; }
		public LoginQrData data { get; set; }
	}

	public class LoginQrData
	{
		public string qrurl { get; set; }
		public string qrimg { get; set; }
	}

	public class JsonConfigData
	{
		public string api_address { get; set; }
		public string cookies { get; set; }
	}

	public class JsonConfig
	{
		private JsonConfigData data;
		private static readonly string ConfigurationPath = "config.json";

		public JsonConfig()
		{
		}

		public string GetApiAddress()
		{
			Read();
			return data.api_address;
		}

		public string GetCookies()
		{
			Read();
			return data.cookies;
		}

		public void SetCookies(string cookies)
		{
			data.cookies = cookies;
			Write();
		}

		private void Read()
		{
			var reader = new StreamReader(ConfigurationPath);
			string json = reader.ReadToEnd();
			data = JsonSerializer.Deserialize<JsonConfigData>(json);
			reader.Close();
		}

		private void Write()
		{
			string json = JsonSerializer.Serialize(data);
			var writer = new StreamWriter(ConfigurationPath);
			writer.Write(json);
			writer.Close();
		}
	}


	public class Porter
	{
		private JsonConfig jsonConfig;
		private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
		private readonly string apiAddress;

		public Porter(JsonConfig jsonConfig)
		{
			this.jsonConfig = jsonConfig;
			apiAddress = jsonConfig.GetApiAddress();
		}

		public Porter(string apiAddress)
		{
			this.apiAddress = apiAddress;
		}

		public string GetApiAddress()
		{
			return apiAddress;
		}


		public void SetCookies(string cookies)
		{
			jsonConfig.SetCookies(cookies);
		}

		private string Url(string apiPath)
		{
			string ip = apiAddress + apiPath;
			return ip;
		}

		private string ParamUrl(string apiPath)
		{
			return Url(apiPath) + "?";
		}

		private string Param(string path, string key, string value)
		{
			return path + key + "=" + value + "&";
		}
		private string ParamEnd(string path, string key, string value)
		{
			return path + key + "=" + value;
		}

		private string Response(string url)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);

			request.Method = "Get";
			request.Accept = "text/html, application/xhtml+xml, */*";
			request.ContentType = "application/json";

			var response = (HttpWebResponse)request.GetResponse();
			using var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
			return reader.ReadToEnd();
		}

		private T Deserialize<T>(string json)
		{
			return JsonSerializer.Deserialize<T>(json);
		}

		public string GetLoginKeyUrl()
		{
			return Url("/login/qr/key");
		}

		public string GetLoginKey()
		{
			return Deserialize<LoginKey>(Response(GetLoginKeyUrl())).data.unikey;
		}

		public string GetLoginQrUrl(string key)
		{
			return ParamEnd(Param(ParamUrl("/login/qr/create"), "key", key), "qrimg", "true");
		}

		public string GetLoginQr(string key)
		{
			return Deserialize<LoginQr>(Response(GetLoginQrUrl(key))).data.qrimg;
		}

		public string GetLoginStatusUrl(string key)
		{
			return ParamEnd(ParamUrl("/login/qr/check"), "key", key);
		}

		public (int, string) GetLoginStatus(string key)
		{
			var status = Deserialize<LoginStatus>(Response(GetLoginStatusUrl(key)));
			return (status.code, status.cookie);
		}
	}

	class PlayerManager
	{
		private readonly Porter porter;

		private int playMode;

		public PlayerManager(Porter porter, string cookies)
		{
			this.porter = porter;

			playMode = 0;
		}

		public void SetPlayMode(int playMode)
		{
			if (this.playMode == playMode) return;

			if (playMode >= 1 && playMode <= 4)
			{
				this.playMode = playMode;
			}
		}

		public int GetPlayMode()
		{
			return playMode;
		}

	}

	public class MusicBotPlugin : IBotPlugin
	{
		private static Porter porter;

		public MusicBotPlugin()
		{
		}

		public void Initialize()
		{
			JsonConfig config = new JsonConfig();
			porter = new Porter(config);
		}
		public void Dispose() => throw new NotImplementedException();

		[Command("music login")]
		public static async Task<string> QrLogin(Ts3Client client, TsFullClient fullClient)
		{
			Console.WriteLine("Command: !music login");

			var unikey = porter.GetLoginKey();
			var qrImg = porter.GetLoginQr(unikey);
			await client.SendChannelMessage("生成登录二维码");
			await client.SendChannelMessage(qrImg);
			var bytes = Convert.FromBase64String(qrImg.Split(",")[1]);
			var stream = new MemoryStream(bytes);
			await fullClient.UploadAvatar(stream);
			await client.ChangeDescription("使用网易云音乐APP扫码登录");

			Console.WriteLine("Wait for scanning QrCode");
			var start = DateTime.Now.Second;
			while (true)
			{
				var result = porter.GetLoginStatus(unikey);
				switch (result.Item1)
				{
				case 800:
					await fullClient.DeleteAvatar();
					await client.SendChannelMessage("登录二维码已过期");
					Console.WriteLine("Login Qrcode expired");
					return "登录二维码已过期";
				case 803:
					//TODO: replace with default avator
					await fullClient.DeleteAvatar();
					porter.SetCookies(result.Item2);

					await client.SendChannelMessage("登录成功");
					Console.WriteLine("Login success!");
					return "登录成功";
				default:
					Thread.Sleep(500);
					if ((DateTime.Now.Second - start) > 300)
					{
						//TODO: make wait time configuable
						await fullClient.DeleteAvatar();
						await client.SendChannelMessage("等待时间超过5分钟,取消登录");
						Console.WriteLine("Login timeout!");
						return "等待时间超过5分钟,取消登录";
					}
					else break;
				}
			}
		}
	}
}
