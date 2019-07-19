// TS3AudioBot - An advanced Musicbot for Teamspeak 3
// Copyright (C) 2017  TS3AudioBot contributors
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the Open Software License v. 3.0
//
// You should have received a copy of the Open Software License along with this
// program. If not, see <https://opensource.org/licenses/OSL-3.0>.

namespace TS3AudioBot.Sessions
{
	using CommandSystem;
	using Helper;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using Response = System.Func<string, string>;

	public class UserSession
	{
		private Dictionary<string, object> assocMap;
		protected bool lockToken;

		public Response ResponseProcessor { get; private set; }

		public UserSession()
		{
			ResponseProcessor = null;
		}

		public void SetResponseInstance(Response responseProcessor)
		{
			VerifyLock();

			ResponseProcessor = responseProcessor;
		}

		public void ClearResponse()
		{
			VerifyLock();

			ResponseProcessor = null;
		}

		public R<TData> Get<TData>(string key)
		{
			VerifyLock();

			if (assocMap is null)
				return R.Err;

			if (!assocMap.TryGetValue(key, out object value))
				return R.Err;

			if (!(value is TData))
				return R.Err;

			return (TData)value;
		}

		public void Set<TData>(string key, TData data)
		{
			VerifyLock();

			if (assocMap is null)
				Util.Init(out assocMap);

			assocMap[key] = data;
		}

		public virtual IDisposable GetLock()
		{
			var sessionToken = new SessionLock(this);
			sessionToken.Take();
			return sessionToken;
		}

		protected void VerifyLock()
		{
			if (!lockToken)
				throw new InvalidOperationException("No access lock is currently active");
		}

		private class SessionLock : IDisposable
		{
			private readonly UserSession session;
			public SessionLock(UserSession session) { this.session = session; }

			public void Take() { Monitor.Enter(session); session.lockToken = true; }
			public void Free() { Monitor.Exit(session); session.lockToken = false; }
			public void Dispose() => Free();
		}
	}

	public static class UserSessionExtensions
	{
		public static void SetResponse(this UserSession session, Response responseProcessor)
		{
			if (session is null)
				throw new CommandException("No session context", CommandExceptionReason.CommandError);
			session.SetResponseInstance(responseProcessor);
		}
	}
}
