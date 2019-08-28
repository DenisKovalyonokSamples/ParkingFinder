using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Content.PM;
using ParkCred.Droid.Activities.Base;
using ParkCred.Droid.Fragments;
using ParkCred.Localization;
using ParkCred.Shared.Entities.SQL;
using ParkCred.Shared.Enums;

namespace ParkCred.Droid.Activities
{
    [Activity(Label = "ParkCred", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class AutoParkingActivity : BaseActivity
    {
        MenuFragment menuFragment;

        Button buttonOrderInstallation;
        Button buttonGetFree;

        TextView textAutoParkingTitle;
        TextView textAutoParkingCost;
        TextView textAutoParkingDescription;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AutoParking);
            SetTitleBack();

            buttonOrderInstallation = FindViewById<Button>(Resource.Id.buttonOrderInstallation);
            buttonGetFree = FindViewById<Button>(Resource.Id.buttonGetFree);

            textAutoParkingTitle = FindViewById<TextView>(Resource.Id.textAutoParkingTitle);
            textAutoParkingCost = FindViewById<TextView>(Resource.Id.textAutoParkingCost);
            textAutoParkingDescription = FindViewById<TextView>(Resource.Id.textAutoParkingDescription);

            menuFragment = new MenuFragment();

            var partialMenuSetup = SupportFragmentManager.BeginTransaction();
            partialMenuSetup.Add(Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
            partialMenuSetup.Commit();

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            buttonOrderInstallation.Text = AppResources.OrderInstallation.ToUpper();
            buttonGetFree.Text = AppResources.GetFree.ToUpper();

            textAutoParkingTitle.Text = AppResources.DeviceCost;
            textAutoParkingCost.Text = "3500 " + AppResources.Rub.ToLower();
            textAutoParkingDescription.Text = AppResources.AutoParkingDescription;

            SetupGestures();
        }

        void SetupGestures()
        {
            buttonOrderInstallation.Click += delegate
            {
                User entity = sqliteManager.GetUser();
                if (entity != null)
                {
                    entity.AutoModeStatus = (int)AutoParkingStatus.NeedInitialisation;
                    sqliteManager.SaveUser(entity);
                }

                Finish();
            };

            buttonGetFree.Click += delegate
            {
                var activity = new Intent(this, typeof(RequestQuotationActivity));
                StartActivity(activity);
            };
        }

        #region abstract

        protected override int GetStatusBarColor()
        {
            return Resource.Color.statusbar_blue;
        }

        protected override int GetActiveBarColor()
        {
            return Resource.Color.actionbar_blue;
        }

        protected override string GetTitle()
        {
            return AppResources.AppName;
        }

        #endregion
    }
}