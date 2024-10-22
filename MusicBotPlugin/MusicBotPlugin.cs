using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using TS3AudioBot;
using TS3AudioBot.Audio;
using TS3AudioBot.Plugins;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using FileInfo = System.IO.FileInfo;
using System.Threading.Tasks;
using TSLib.Full;
using System.Reflection.Emit;
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


	class Porter
	{
		private IniConfig iniConfig;

		private string cookies;
		private readonly string apiAddress;

		public Porter(IniConfig iniConfig)
		{
			this.iniConfig = iniConfig;
			apiAddress = iniConfig.Read("api_address", "basic");
		}

		public void SetCookies(string cookies)
		{
			this.cookies = cookies;
			iniConfig.Write("cookies", cookies, "basic");
		}

		private static bool MatchIP(string ip)
		{
			if (!string.IsNullOrEmpty(ip))
			{
				//判断是否为IP
				return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
			}
			return false;
		}
		private string Url(string apiPath)
		{
			string ip = apiAddress + apiPath;
			Debug.Assert(MatchIP(ip));
			return ip;
		}

		private string ParamUrl(string apiPath)
		{
			return Url(apiPath) + "?";
		}

		private string Param(string path, string key, string value)
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

		public string GetLoginKey()
		{
			return Deserialize<LoginKey>(Response(Url("/login/qr/key"))).data.unikey;
		}

		public string GetLoginQr(string key)
		{
			return Deserialize<LoginQr>(
						Response(
							Param(Param(ParamUrl("/login/qr/create"), "key", key), "qrimg", "true"))).data.qrimg;
		}

		public (int, string) GetLoginStatus(string key)
		{
			var status = Deserialize<LoginStatus>(Response(Param(ParamUrl("/login/qr/check"), "key", key)));
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

	class IniConfig
	{
		private readonly string path;

		[DllImport("kernel32", CharSet = CharSet.Unicode)]
		static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

		[DllImport("kernel32", CharSet = CharSet.Unicode)]
		static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

		public IniConfig()
		{
			path = new FileInfo("netcloud.ini").FullName;
		}

		public string Read(string Key, string Section)
		{
			var RetVal = new StringBuilder(6000);
			GetPrivateProfileString(Section, Key, "", RetVal, 6000, path);
			return RetVal.ToString();
		}

		public void Write(string Key, string Value, string Section)
		{
			WritePrivateProfileString(Section, Key, Value, path);
		}

		public void DeleteKey(string Key, string Section)
		{
			Write(Key, null, Section);
		}

		public void DeleteSection(string Section = null)
		{
			Write(null, null, Section);
		}

		public bool KeyExists(string Key, string Section)
		{
			return Read(Key, Section).Length > 0;
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
			IniConfig config = new IniConfig();
			porter = new Porter(config);
		}
		public void Dispose() => throw new NotImplementedException();

		[Command("music login")]
		public static async Task<string> QrLogin(Ts3Client client, TsFullClient fullClient)
		{
			var unikey = porter.GetLoginKey();
			var qrImg = porter.GetLoginQr(unikey);
			await client.SendChannelMessage("生成登录二维码");
			await client.SendChannelMessage(qrImg);
			var bytes = Convert.FromBase64String(qrImg.Split(",")[1]);
			var stream = new MemoryStream(bytes);
			await fullClient.UploadAvatar(stream);
			await client.ChangeDescription("使用网易云音乐APP扫码登录");

			var start = DateTime.Now.Second;
			while (true)
			{
				var result = porter.GetLoginStatus(unikey);
				switch (result.Item1)
				{
				case 800:
					await fullClient.DeleteAvatar();
					await client.SendChannelMessage("登录二维码已过期");
					return "登录二维码已过期";
				case 803:
					//TODO: replace with default avator
					await fullClient.DeleteAvatar();
					porter.SetCookies(result.Item2);

					await client.SendChannelMessage("登录成功");
					return "登录成功";
				default:
					Thread.Sleep(1000);
					if ((DateTime.Now.Second - start) > 300)
					{
						//TODO: make wait time configuable
						await fullClient.DeleteAvatar();
						await client.SendChannelMessage("等待时间超过5分钟,取消登录");
						return "等待时间超过5分钟,取消登录";
					}
					else break;
				}
			}
		}
	}
}
