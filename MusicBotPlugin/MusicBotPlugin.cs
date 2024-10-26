using System;
using System.Text;
using System.IO;
using TS3AudioBot;
using TS3AudioBot.Plugins;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System.Threading.Tasks;
using TSLib.Full;
using System.Threading;
using TS3AudioBot.CommandSystem;
using System.ComponentModel;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using TS3AudioBot.Audio;
using System.Net.Http.Headers;
using TS3AudioBot.CommandSystem.Commands;
using Newtonsoft.Json;
using TS3AudioBot.Playlists;

namespace MusicBotPlugin
{

	public class LoginStatus
	{
		public int code { get; set; }
		public string message { get; set; }
		public string cookie { get; set; }
	}

/* -- split -- */

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

/* -- split -- */

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

/* -- split -- */

	public class TrackResult
	{
		public int code { get; set; }
		public object relatedVideos { get; set; }
		public Playlist playlist { get; set; }
		public object urls { get; set; }
		public Privilege[] privileges { get; set; }
		public object sharedPrivilege { get; set; }
		public object resEntrance { get; set; }
		public object fromUsers { get; set; }
		public int fromUserCount { get; set; }
		public object songFromUsers { get; set; }
	}

	public class Playlist
	{
		public long id { get; set; }
		public string name { get; set; }
		public long coverImgId { get; set; }
		public string coverImgUrl { get; set; }
		public string coverImgId_str { get; set; }
		public int adType { get; set; }
		public long userId { get; set; }
		public long createTime { get; set; }
		public int status { get; set; }
		public bool opRecommend { get; set; }
		public bool highQuality { get; set; }
		public bool newImported { get; set; }
		public long updateTime { get; set; }
		public int trackCount { get; set; }
		public int specialType { get; set; }
		public int privacy { get; set; }
		public long trackUpdateTime { get; set; }
		public string commentThreadId { get; set; }
		public int playCount { get; set; }
		public long trackNumberUpdateTime { get; set; }
		public int subscribedCount { get; set; }
		public int cloudTrackCount { get; set; }
		public bool ordered { get; set; }
		public string description { get; set; }
		public string[] tags { get; set; }
		public object updateFrequency { get; set; }
		public long backgroundCoverId { get; set; }
		public object backgroundCoverUrl { get; set; }
		public int titleImage { get; set; }
		public object titleImageUrl { get; set; }
		public object detailPageTitle { get; set; }
		public object englishTitle { get; set; }
		public object officialPlaylistType { get; set; }
		public bool copied { get; set; }
		public object relateResType { get; set; }
		public int coverStatus { get; set; }
		public Subscriber[] subscribers { get; set; }
		public bool subscribed { get; set; }
		public Creator creator { get; set; }
		public Track[] tracks { get; set; }
		public object videoIds { get; set; }
		public object videos { get; set; }
		public Trackid[] trackIds { get; set; }
		public object bannedTrackIds { get; set; }
		public object mvResourceInfos { get; set; }
		public int shareCount { get; set; }
		public int commentCount { get; set; }
		public object remixVideo { get; set; }
		public object newDetailPageRemixVideo { get; set; }
		public object sharedUsers { get; set; }
		public object historySharedUsers { get; set; }
		public string gradeStatus { get; set; }
		public object score { get; set; }
		public string[] algTags { get; set; }
		public object[] distributeTags { get; set; }
		public int trialMode { get; set; }
		public object displayTags { get; set; }
		public bool displayUserInfoAsTagOnly { get; set; }
		public string playlistType { get; set; }
		public Bizextinfo bizExtInfo { get; set; }
	}

	public class Creator
	{
		public bool defaultAvatar { get; set; }
		public int province { get; set; }
		public int authStatus { get; set; }
		public bool followed { get; set; }
		public string avatarUrl { get; set; }
		public int accountStatus { get; set; }
		public int gender { get; set; }
		public int city { get; set; }
		public int birthday { get; set; }
		public long userId { get; set; }
		public int userType { get; set; }
		public string nickname { get; set; }
		public string signature { get; set; }
		public string description { get; set; }
		public string detailDescription { get; set; }
		public long avatarImgId { get; set; }
		public long backgroundImgId { get; set; }
		public string backgroundUrl { get; set; }
		public int authority { get; set; }
		public bool mutual { get; set; }
		public string[] expertTags { get; set; }
		public object experts { get; set; }
		public int djStatus { get; set; }
		public int vipType { get; set; }
		public object remarkName { get; set; }
		public int authenticationTypes { get; set; }
		public Avatardetail avatarDetail { get; set; }
		public string avatarImgIdStr { get; set; }
		public string backgroundImgIdStr { get; set; }
		public bool anchor { get; set; }
		public string avatarImgId_str { get; set; }
	}

	public class Avatardetail
	{
		public int userType { get; set; }
		public int identityLevel { get; set; }
		public string identityIconUrl { get; set; }
	}

	public class Bizextinfo
	{
	}

	public class Subscriber
	{
		public bool defaultAvatar { get; set; }
		public int province { get; set; }
		public int authStatus { get; set; }
		public bool followed { get; set; }
		public string avatarUrl { get; set; }
		public int accountStatus { get; set; }
		public int gender { get; set; }
		public int city { get; set; }
		public int birthday { get; set; }
		public long userId { get; set; }
		public int userType { get; set; }
		public string nickname { get; set; }
		public string signature { get; set; }
		public string description { get; set; }
		public string detailDescription { get; set; }
		public long avatarImgId { get; set; }
		public long backgroundImgId { get; set; }
		public string backgroundUrl { get; set; }
		public int authority { get; set; }
		public bool mutual { get; set; }
		public object expertTags { get; set; }
		public object experts { get; set; }
		public int djStatus { get; set; }
		public int vipType { get; set; }
		public object remarkName { get; set; }
		public int authenticationTypes { get; set; }
		public object avatarDetail { get; set; }
		public string avatarImgIdStr { get; set; }
		public string backgroundImgIdStr { get; set; }
		public bool anchor { get; set; }
		public string avatarImgId_str { get; set; }
	}

	public class Track
	{
		public string name { get; set; }
		public long id { get; set; }
		public int pst { get; set; }
		public int t { get; set; }
		public Ar[] ar { get; set; }
		public string[] alia { get; set; }
		public int pop { get; set; }
		public int st { get; set; }
		public string rt { get; set; }
		public int fee { get; set; }
		public int v { get; set; }
		public object crbt { get; set; }
		public string cf { get; set; }
		public Al al { get; set; }
		public int dt { get; set; }
		public H h { get; set; }
		public M m { get; set; }
		public L l { get; set; }
		public Sq sq { get; set; }
		public object hr { get; set; }
		public object a { get; set; }
		public string cd { get; set; }
		public int no { get; set; }
		public object rtUrl { get; set; }
		public int ftype { get; set; }
		public object[] rtUrls { get; set; }
		public long djId { get; set; }
		public int copyright { get; set; }
		public long s_id { get; set; }
		public long mark { get; set; }
		public int originCoverType { get; set; }
		public object originSongSimpleData { get; set; }
		public object tagPicList { get; set; }
		public bool resourceState { get; set; }
		public int version { get; set; }
		public object songJumpInfo { get; set; }
		public object entertainmentTags { get; set; }
		public object awardTags { get; set; }
		public int single { get; set; }
		public object noCopyrightRcmd { get; set; }
		public object alg { get; set; }
		public object displayReason { get; set; }
		public int rtype { get; set; }
		public object rurl { get; set; }
		public int mst { get; set; }
		public int cp { get; set; }
		public int mv { get; set; }
		public long publishTime { get; set; }
		public string[] tns { get; set; }
	}

	public class Al
	{
		public long id { get; set; }
		public string name { get; set; }
		public string picUrl { get; set; }
		public string[] tns { get; set; }
		public string pic_str { get; set; }
		public long pic { get; set; }
	}

	public class H
	{
		public int br { get; set; }
		public long fid { get; set; }
		public int size { get; set; }
		public int vd { get; set; }
	}

	public class M
	{
		public int br { get; set; }
		public long fid { get; set; }
		public int size { get; set; }
		public int vd { get; set; }
	}

	public class L
	{
		public int br { get; set; }
		public long fid { get; set; }
		public int size { get; set; }
		public int vd { get; set; }
	}

	public class Sq
	{
		public int br { get; set; }
		public long fid { get; set; }
		public int size { get; set; }
		public int vd { get; set; }
	}

	public class Ar
	{
		public long id { get; set; }
		public string name { get; set; }
		public object[] tns { get; set; }
		public object[] alias { get; set; }
	}

	public class Trackid
	{
		public long id { get; set; }
		public int v { get; set; }
		public int t { get; set; }
		public long at { get; set; }
		public object alg { get; set; }
		public long uid { get; set; }
		public string rcmdReason { get; set; }
		public string rcmdReasonTitle { get; set; }
		public object sc { get; set; }
		public object f { get; set; }
		public object sr { get; set; }
		public object dpr { get; set; }
	}

	public class Privilege
	{
		public long id { get; set; }
		public int fee { get; set; }
		public int payed { get; set; }
		public int realPayed { get; set; }
		public int st { get; set; }
		public int pl { get; set; }
		public int dl { get; set; }
		public int sp { get; set; }
		public int cp { get; set; }
		public int subp { get; set; }
		public bool cs { get; set; }
		public int maxbr { get; set; }
		public int fl { get; set; }
		public object pc { get; set; }
		public bool toast { get; set; }
		public int flag { get; set; }
		public bool paidBigBang { get; set; }
		public bool preSell { get; set; }
		public int playMaxbr { get; set; }
		public int downloadMaxbr { get; set; }
		public string maxBrLevel { get; set; }
		public string playMaxBrLevel { get; set; }
		public string downloadMaxBrLevel { get; set; }
		public string plLevel { get; set; }
		public string dlLevel { get; set; }
		public string flLevel { get; set; }
		public object rscl { get; set; }
		public Freetrialprivilege freeTrialPrivilege { get; set; }
		public int rightSource { get; set; }
		public Chargeinfolist[] chargeInfoList { get; set; }
		public int code { get; set; }
		public object message { get; set; }
	}

	public class Freetrialprivilege
	{
		public bool resConsumable { get; set; }
		public bool userConsumable { get; set; }
		public int listenType { get; set; }
		public int cannotListenReason { get; set; }
		public object playReason { get; set; }
		public object freeLimitTagType { get; set; }
	}

	public class Chargeinfolist
	{
		public int rate { get; set; }
		public object chargeUrl { get; set; }
		public object chargeMessage { get; set; }
		public int chargeType { get; set; }
	}

/* -- split --*/

	public class SongResult
	{
		public Datum[] data { get; set; }
		public int code { get; set; }
	}

	public class Datum
	{
		public long id { get; set; }
		public string url { get; set; }
		public int br { get; set; }
		public int size { get; set; }
		public string md5 { get; set; }
		public int code { get; set; }
		public int expi { get; set; }
		public string type { get; set; }
		public double gain { get; set; }
		public double peak { get; set; }
		public int closedGain { get; set; }
		public int closedPeak { get; set; }
		public int fee { get; set; }
		public object uf { get; set; }
		public int payed { get; set; }
		public int flag { get; set; }
		public bool canExtend { get; set; }
		public Freetrialinfo freeTrialInfo { get; set; }
		public string level { get; set; }
		public string encodeType { get; set; }
		public object channelLayout { get; set; }
		public FreetrialprivilegeCopy freeTrialPrivilegeCopy { get; set; }
		public Freetimetrialprivilege freeTimeTrialPrivilege { get; set; }
		public int urlSource { get; set; }
		public int rightSource { get; set; }
		public object podcastCtrp { get; set; }
		public object effectTypes { get; set; }
		public int time { get; set; }
		public object message { get; set; }
		public object levelConfuse { get; set; }
		public string musicId { get; set; }
	}

	public class Freetrialinfo
	{
		public int fragmentType { get; set; }
		public int start { get; set; }
		public int end { get; set; }
		public object algData { get; set; }
	}

	public class FreetrialprivilegeCopy
	{
		public bool resConsumable { get; set; }
		public bool userConsumable { get; set; }
		public object listenType { get; set; }
		public int cannotListenReason { get; set; }
		public object playReason { get; set; }
		public object freeLimitTagType { get; set; }
	}

	public class Freetimetrialprivilege
	{
		public bool resConsumable { get; set; }
		public bool userConsumable { get; set; }
		public int type { get; set; }
		public int remainTime { get; set; }
	}

/*----------------------------------------------------------------------------*/

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
			Read();
			ClearCookies();
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

		public void ClearCookies()
		{
			data.cookies = "";
			Write();
		}

		private void Read()
		{
			var reader = new StreamReader(ConfigurationPath);
			string json = reader.ReadToEnd();
			data = JsonConvert.DeserializeObject<JsonConfigData>(json);
			//data = JsonSerializer.Deserialize<JsonConfigData>(json);
			reader.Close();
		}

		private void Write()
		{
			string json = JsonConvert.SerializeObject(data);
			//string json = JsonSerializer.Serialize(data);
			var writer = new StreamWriter(ConfigurationPath);
			writer.Write(json);
			writer.Close();
		}
	}

	class Song
	{
		public string id { get; set; }
		public string name { get; set; }
		public string url { get; set; }

		public Song(string id, string name, string url)
		{
			this.id = id;
			this.name = name;
			this.url = url;
		}

	}

	class PlayerManager
	{
		enum PlayMode
		{
			ORDER,
			REPEAT,
			RANDOM
		}

		enum PlayLevel
		{
			STANDARD,
			HIGHER,
			EXHIGH,
			LOSSLESS,
			HIRES,
			JYEFFECT,
			SKY,
			DOLBY,
			JYMASTER
		}

		private PlayMode playMode;
		private PlayLevel playLevel;
		private readonly List<Song> playlist;
		private readonly string gListId; // Plugin list
		private bool isPlayingF;

		public PlayerManager()
		{
			playlist = new List<Song>();
			playMode = PlayMode.ORDER;
			playLevel = PlayLevel.STANDARD;
			isPlayingF = false;
		}

		public void SetOrderMode()
		{
			playMode = PlayMode.ORDER;
		}

		public void SetRepeatMode()
		{
			playMode = PlayMode.REPEAT;
		}

		public void SetRandomMode()
		{
			playMode = PlayMode.RANDOM;
		}

		public string GetPlayMode()
		{
			return playMode switch
			{
				PlayMode.ORDER => "Order Mode",
				PlayMode.REPEAT => "Repeat Mode",
				PlayMode.RANDOM => "Random Mode",
				_ => throw new NotImplementedException(),
			};
		}

		public void SetStandardLevel()
		{
			playLevel = PlayLevel.STANDARD;
		}

		public void SetHigherLevel()
		{
			playLevel = PlayLevel.HIGHER;
		}

		public void SetExhighLevel()
		{
			playLevel = PlayLevel.EXHIGH;
		}

		public void SetLosslessLevel()
		{
			playLevel = PlayLevel.LOSSLESS;
		}

		public void SetHiresLevel()
		{
			playLevel = PlayLevel.HIRES;
		}

		public void SetJyeffectLevel()
		{
			playLevel = PlayLevel.JYEFFECT;
		}

		public void SetSkyLevel()
		{
			playLevel = PlayLevel.SKY;
		}

		public void SetDolbyLevel()
		{
			playLevel = PlayLevel.DOLBY;
		}

		public void SetJymasterLevel()
		{
			playLevel = PlayLevel.JYMASTER;
		}

		public string GetPlayLevel()
		{
			return playLevel switch
			{
				PlayLevel.STANDARD => "standard",
				PlayLevel.HIGHER => "higher",
				PlayLevel.EXHIGH => "exhigh",
				PlayLevel.LOSSLESS => "lossless",
				PlayLevel.HIRES => "hires",
				PlayLevel.JYEFFECT => "jyeffect",
				PlayLevel.SKY => "sky",
				PlayLevel.DOLBY => "dolby",
				PlayLevel.JYMASTER => "jymaster",
				_ => throw new NotImplementedException(),
			};
		}

		public void AddSong(string id, string name, string url)
		{
			playlist.Add(new Song(id, name, url));
		}

		public void ClearPlaylist()
		{
			playlist.Clear();
		}
		
		public Song GetNextSong()
		{
			Song song;
			switch(playMode)
			{
				case PlayMode.ORDER:
					song = playlist[0];
					playlist.RemoveAt(0);
					return song;
				case PlayMode.REPEAT:
					song = playlist[0];
					return song;
				case PlayMode.RANDOM:
					Random random = new Random();
					int index = random.Next(0, playlist.Count);
					song = playlist[index];
					playlist.RemoveAt(index);
					return song;
				default:
					throw new NotImplementedException();
			}
		}

		public bool IsPlaylistEmpty()
		{
			return playlist.Count == 0;
		}

		public void SetPlaying()
		{
			isPlayingF = true;
		}

		public void ClearPlaying()
		{
			isPlayingF = false;
		}

		public bool IsPlaying()
		{
			return isPlayingF;
		}
	}

	class Porter
	{

		private readonly PlayerManager player; 
		private readonly JsonConfig jsonConfig;
		private readonly string apiAddress;
		private static readonly string gListId = "default";

		public Porter()
		{
			player = new PlayerManager();
			jsonConfig = new JsonConfig();
			apiAddress = jsonConfig.GetApiAddress();
		}

		public Porter(string apiAddress)
		{
			player = new PlayerManager();
			this.apiAddress = apiAddress;
		}

		public void CreateGlobalList(PlaylistManager playlistManager)
		{
			MainCommands.CommandListCreate(playlistManager, gListId);
		}

		public string GetApiAddress()
		{
			return apiAddress;
		}

		public void SetCookies(string cookies)
		{
			jsonConfig.SetCookies(cookies);
		}

		public void ClearCookies()
		{
			jsonConfig.ClearCookies();
		}

		private string Url(string apiPath)
		{
			string ip = apiAddress + apiPath;
			return ip;
		}

		private static string GetTimeStamp()
		{
			TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return Convert.ToInt64(ts.TotalSeconds).ToString();
		}

		private string Param(string path, string key, string value)
		{
			return path + "&" + key + "=" + value;
		}
		private string ParamFirst(string path, string key, string value)
		{
			return path + "?" + key + "=" + value;
		}

		private string ParamCookies(string path)
		{
			string cookies = jsonConfig.GetCookies();
			if (cookies.Equals(""))
			{
				return path;
			}
			else
			{
				return path + "&cookie=" + cookies;
			}
		}

		private string ParamCookiesFirst(string path)
		{
			string cookies = jsonConfig.GetCookies();
			if (cookies.Equals(""))
			{
				return path;
			}
			else
			{
				return path + "?cookie=" + cookies;
			}
		}

		private string ParamTimeStamp(string path)
		{
			return path + "&timestamp=" + GetTimeStamp();
		}

		private string ParamTimeStampFirst(string path)
		{
			return path + "?timestamp=" + GetTimeStamp();
		}

		private string Response(string url)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);

			request.Method = "Get";
			request.Accept = "text/html, application/xhtml+xml, */*";
			request.ContentType = "application/json";

			var response = (HttpWebResponse)request.GetResponse();
			using var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
			string result = reader.ReadToEnd();
			reader.Close();
			return result;
		}

		private T Deserialize<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
			//return JsonSerializer.Deserialize<T>(json);
		}

		public bool IsPlaylistEmpty()
		{
			return player.IsPlaylistEmpty();
		}

		public Song GetNextSong()
		{
			return player.GetNextSong();
		}

		public string GetLoginKeyUrl()
		{
			return ParamTimeStampFirst(Url("/login/qr/key"));
		}

		public string GetLoginKey()
		{
			return Deserialize<LoginKey>(Response(GetLoginKeyUrl())).data.unikey;
		}

		public string GetLoginQrUrl(string key)
		{
			return ParamTimeStamp(Param(ParamFirst(Url("/login/qr/create"), "key", key), "qrimg", "true"));
		}

		public string GetLoginQr(string key)
		{
			return Deserialize<LoginQr>(Response(GetLoginQrUrl(key))).data.qrimg;
		}

		public string GetLoginStatusUrl(string key)
		{
			return ParamTimeStamp(ParamFirst(Url("/login/qr/check"), "key", key));
		}

		public (int, string) GetLoginStatus(string key)
		{
			var result = Deserialize<LoginStatus>(Response(GetLoginStatusUrl(key)));
			return (result.code, result.cookie);
		}

		public string GetSongUrlUrl(string id)
		{
			return ParamCookies(Param(ParamFirst(Url("/song/url/v1"), "id", id), "level", player.GetPlayLevel()));
		}

		public string GetSongUrl(string id)
		{
			var result = Deserialize<SongResult>(Response(GetSongUrlUrl(id)));
			return result.data[0].url;
		}
		public string GetPlaylistUrl(string id)
		{
			return ParamFirst(Url("/playlist/detail"), "id", id);
		}

		public (string, int, string) SetPlaylist(string id)
		{
			var playlist = Deserialize<TrackResult>(Response(GetPlaylistUrl(id))).playlist;
			var tracks = playlist.tracks;

			player.ClearPlaylist();

			foreach (Track track in tracks)
			{
				long song_id = track.id;
				string name = track.name;
				string url = GetSongUrl("" + song_id);

				player.AddSong("" + song_id, name, url);
			}

			return (playlist.name, playlist.trackCount, playlist.description);
		}

		public bool IsPlaying()
		{
			return player.IsPlaying();
		}

		public void SetPlaying()
		{
			player.SetPlaying();
		}

		public void ClearPlaying()
		{
			player.ClearPlaying();
		}
	}

	public class MusicBotPlugin : IBotPlugin
	{
		private static Porter porter;

		private SemaphoreSlim slimLock;
		private static Player player;
		private static PlayManager playManager;
		private static PlaylistManager playlistManager;
		private static Ts3Client client;

		private static TsFullClient fullClient;
		private static InvokerData invoker;

		public MusicBotPlugin(Player player, PlayManager playManager, PlaylistManager playlistManager, 
							  Ts3Client client, TsFullClient fullClient)
		{
			MusicBotPlugin.player = player;
			MusicBotPlugin.playManager = playManager;
			MusicBotPlugin.playlistManager = playlistManager;			
			MusicBotPlugin.client = client;
			MusicBotPlugin.fullClient = fullClient;
		}

		public void Initialize()
		{
			player.OnSongEnd += AudioService_Playstoped;
			porter = new Porter();
			porter.CreateGlobalList(playlistManager);
			slimLock = new SemaphoreSlim(1);
		}

		public void Dispose()
		{
		}

		public static void UpdateInvoker(InvokerData newInvoker)
		{
			invoker = newInvoker;
		}

		public async Task AudioService_Playstoped(object? sender, EventArgs e) 
		{
			if (porter.IsPlaylistEmpty()) return;	

			Song song = porter.GetNextSong();
			await MainCommands.CommandPlay(playManager, invoker, song.url);
			await client.SendChannelMessage("正在播放: " + song.name);
		}

		private static async void SendMessage(ClientCall clientCall, string message)
		{
			await client.SendMessage(message, clientCall.ClientId.Value);
		}

		[Command("music login")]
		public static async void CommandQrLogin(ClientCall clientCall)
		{
			//TODO: add sync operatation

			Console.WriteLine("Command: !music login");

			var unikey = porter.GetLoginKey();
			var qrImg = porter.GetLoginQr(unikey);
			SendMessage(clientCall, "生成登录二维码");
			SendMessage(clientCall, qrImg);
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
					Console.WriteLine("Login Qrcode expired");
					SendMessage(clientCall, "登录二维码已过期");
					return;
				case 803:
					//TODO: replace with default avator
					await fullClient.DeleteAvatar();
					porter.SetCookies(result.Item2);

					await client.SendChannelMessage(clientCall.NickName + "已登录");
					Console.WriteLine("Login success!");
					SendMessage(clientCall, "登录成功");
					return;
				default:
					Thread.Sleep(500);
					if ((DateTime.Now.Second - start) > 300)
					{
						//TODO: make wait time configuable
						await fullClient.DeleteAvatar();
						Console.WriteLine("Login timeout!");
						SendMessage(clientCall, "等待超过五分钟，登录失败");
						return;
					}
					else break;
				}
			}
		}

		[Command("music setplid")]
		public static async void CommandSetPlayList(string id, ClientCall clientCall)
		{
			var result = porter.SetPlaylist(id);

			await client.SendChannelMessage("切换播放列表至新歌单");
			await client.SendChannelMessage("歌单名: " + result.Item1);
			await client.SendChannelMessage("歌曲数量: " + result.Item2);
			await client.SendChannelMessage("歌单描述: " + result.Item3);
			porter.ClearPlaying();

			SendMessage(clientCall, "歌单已设置，详情请见频道消息");
		}

		[Command("music play")]
		public static async void CommandPlay(InvokerData newInvoker, Player playerConnection, PlayManager playManager)
		{
			UpdateInvoker(newInvoker);			

			if (porter.IsPlaylistEmpty()) 
			{
				porter.ClearPlaying();
				await client.SendChannelMessage("播放列表为空");
				return;
			}

			if (porter.IsPlaying())
			{
				await playManager.Play(invoker);
			}
			else
			{
				Song song = porter.GetNextSong();
				await playManager.Play(invoker, song.url);
				await client.SendChannelMessage("正在播放: " + song.name);			
			}

		}

		[Command("music pause")]
		public static void CommandPause(Player playerConnection) => playerConnection.Paused = !playerConnection.Paused;


		[Command("music next")]
		public static async void CommandNext(InvokerData newInvoker, PlayManager playManager)
		{
			Song song = porter.GetNextSong();
			await playManager.Play(invoker, song.url);
			await client.SendChannelMessage("正在播放: " + song.name);
			if (!porter.IsPlaying())
			{
				porter.SetPlaying();
			}
		}

		[Command("music sayhi")]
		public static void CommandSayHi(ClientCall clientCall)
		{
			SendMessage(clientCall, "let's say hi!");
		}

	}
}
