using System;
using System.Runtime.InteropServices;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using TS3AudioBot;
using TS3AudioBot.Audio;
using TS3AudioBot.Plugins;
using TSLib.Full.Book;

namespace MusicBotPlugin
{
	class Porter
	{
		private readonly Ts3Client ts3Client;

		public Porter(Ts3Client ts3Client)
		{
			this.ts3Client = ts3Client;
		}

		public void Info(string message)
		{
			Chat(message);
		}

		public void Chat(string message)
		{
			ts3Client.SendChannelMessage(message);
		}
	}

	class PlayerManager
	{
		private readonly Porter porter;
		private readonly string cookies;

		private int playMode;

		public PlayerManager(Porter porter, string cookies)
		{
			this.porter = porter;
			this.cookies = cookies;

			playMode = 0;
		}

		public void SetPlayMode(int playMode)
		{
			if (this.playMode == playMode) return;

			if (playMode >= 1 && playMode <= 4)
			{
				this.playMode = playMode;
			}
			else
			{
				porter.Info("错误的播放模式");
			}
		}

		public int GetPlayMode()
		{
			return playMode;
		}

		public string GetCookies()
		{
			return cookies;
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
		PlayManager playManager;
		Ts3Client ts3Client;

		private string apiAddress;

		public MusicBotPlugin(PlayManager playManager, Ts3Client ts3Client)
		{
			this.playManager = playManager;
			this.ts3Client = ts3Client;
		}

		public void Initialize()
		{
			IniConfig config = new IniConfig();
			apiAddress = config.Read("NetcloudApiAddress", "basic");
		}
		public void Dispose() => throw new NotImplementedException();
	}
}
