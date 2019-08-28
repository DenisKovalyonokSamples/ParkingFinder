using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Content.PM;
using ParkCred.Droid.Activities.Base;
using ParkCred.Droid.Fragments;
using ParkCred.Localization;

namespace ParkCred.Droid.Activities
{
    [Activity(Label = "ParkCred", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class AppInfoActivity : BaseActivity
    {
        MenuFragment menuFragment;

        TextView textBuyPolice;
        TextView textBasicConditions;
        TextView textSeeYourRating;
        TextView textLessCost;
        TextView textGetBonuses;
        TextView textDeclareInsuranceEvent;
        TextView textFindCar;
        TextView textDownloadAppTitle;

        Button buttonDownload;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AppInfo);
            SetTitleBack();

            textBuyPolice = FindViewById<TextView>(Resource.Id.textBuyPolice);
            textBasicConditions = FindViewById<TextView>(Resource.Id.textBasicConditions);
            textSeeYourRating = FindViewById<TextView>(Resource.Id.textSeeYourRating);
            textLessCost = FindViewById<TextView>(Resource.Id.textLessCost);
            textGetBonuses = FindViewById<TextView>(Resource.Id.textGetBonuses);
            textDeclareInsuranceEvent = FindViewById<TextView>(Resource.Id.textDeclareInsuranceEvent);
            textFindCar = FindViewById<TextView>(Resource.Id.textFindCar);
            textDownloadAppTitle = FindViewById<TextView>(Resource.Id.textDownloadAppTitle);

            buttonDownload = FindViewById<Button>(Resource.Id.buttonDownload);

            menuFragment = new MenuFragment();

            var partialMenuSetup = SupportFragmentManager.BeginTransaction();
            partialMenuSetup.Add(Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
            partialMenuSetup.Commit();

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            textBuyPolice.Text = AppResources.ApplicationFeatures + ":";
            textBasicConditions.Text = "- " + AppResources.SeeMainTerms;
            textSeeYourRating.Text = "- " + AppResources.SeeYourRating;
            textLessCost.Text = "- " + AppResources.LessCost;
            textGetBonuses.Text = "- " + AppResources.EarnBonusPoints;
            textDeclareInsuranceEvent.Text = "- " + AppResources.DeclareInsuranceEvent;
            textFindCar.Text = "- " + AppResources.FindCar;
            textDownloadAppTitle.Text = AppResources.DownloadAppTitle;

            buttonDownload.Text = AppResources.Download.ToUpper();

            SetupGestures();
        }

        void SetupGestures()
        {
            buttonDownload.Click += delegate
            {
                try
                {
                    StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("some private url...")));
                }
                catch (ActivityNotFoundException anfe)
                {
                    StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("some private url...")));
                }
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