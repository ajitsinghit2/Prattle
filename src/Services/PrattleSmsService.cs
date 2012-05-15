using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using T = Android.Telephony;

using Prattle.Android.Core;
using Android.Util;

namespace Prattle
{
	[Service]
	public class PrattleSmsService : Service
	{
		PrattleSmsReceiver _receiver;
		IBinder _binder;
		PendingIntent _sentIntent;
		
		public PrattleSmsService ()
		{
			_binder = new LocalBinder(this);
		}
		
		public override void OnCreate ()
		{
			base.OnCreate ();
			_sentIntent = PendingIntent.GetBroadcast (ApplicationContext, 0, new Intent("SMS_SENT"), 0);
		}
		
		public override IBinder OnBind (Intent intent)
		{
			return _binder;
		}
		
		public override void OnStart (Intent intent, int startId)
		{
			base.OnStart (intent, startId);
		}
		
		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			return StartCommandResult.NotSticky;
		}
		
		public override void OnDestroy ()
		{
			base.OnDestroy ();
			UnregisterReceiver (_receiver);
		}
		
		public bool SendMessage (string messageText, SmsGroup smsGroup, List<Contact> recipients)
		{
			var dateSent = DateTime.Now;
			recipients.ForEach (recipient => {
				var message = new SmsMessage{
					Text = messageText,
					SmsGroupId = smsGroup.Id,
					ContactAddressBookId = recipient.AddressBookId,
					ContactName = recipient.Name,
					SentDate = dateSent
				};
				
				_receiver = new PrattleSmsReceiver { Message = message };
				RegisterReceiver (_receiver, new IntentFilter("SMS_SENT"));
				T.SmsManager.Default.SendTextMessage (recipient.MobilePhone, null, messageText, _sentIntent, null);
			});
			return true;
		}
		
		public class LocalBinder : Binder
		{
			PrattleSmsService _self;

			public LocalBinder (PrattleSmsService self)
			{
				_self = self;
			}

			public PrattleSmsService Service
			{
				get { return _self; }
			}
		}
	}
}