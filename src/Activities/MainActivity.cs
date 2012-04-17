using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Prattle
{
	[Activity (Label = "Prattle")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.createGroup);
			
			button.Click += delegate {
				var groupName = new EditText(this);
				new AlertDialog.Builder(this)
					.SetTitle ("Name SMS Group")
					.SetMessage ("Enter the name of your new SMS group")
					.SetView (groupName)
					.SetPositiveButton ("Ok", (o, e) => {
							var intent = new Intent();
							intent.SetClass (this, typeof(SMSGroupActivity));
							intent.PutExtra("name", groupName.Text);
							StartActivity(intent);
						})
					.SetNegativeButton ("Cancel", (o, e) => { })
					.Show ();
			};
			
		}
	}
}