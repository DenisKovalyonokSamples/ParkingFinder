using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Content.PM;
using ParkCred.Droid.Activities.Base;
using ParkCred.Droid.Fragments;
using ParkCred.Localization;
using Android.Support.Design.Widget;
using ParkCred.Shared.Entities.SQL;
using ParkCred.Shared.Enums;

namespace ParkCred.Droid.Activities
{
    [Activity(Label = "ParkCred", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class RequestQuotationActivity : BaseActivity
    {
        MenuFragment menuFragment;

        TextView textBuyPolice;
        TextView textBenefits;
        TextView textQuarterlyPolicy;
        TextView textDiscounts;
        TextView textBonuses;
        TextView textSettlement;
        TextView textHelp;

        TextInputLayout editSurname;
        TextInputLayout editName;
        TextInputLayout editMark;
        TextInputLayout editModel;
        TextInputLayout editYear;
        TextInputLayout editRegistrationNumber;

        Button buttonSendRequest;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RequestQuotation);
            SetTitleBack();

            textBuyPolice = FindViewById<TextView>(Resource.Id.textBuyPolice);
            textBenefits = FindViewById<TextView>(Resource.Id.textBenefits);
            textQuarterlyPolicy = FindViewById<TextView>(Resource.Id.textQuarterlyPolicy);
            textDiscounts = FindViewById<TextView>(Resource.Id.textDiscounts);
            textBonuses = FindViewById<TextView>(Resource.Id.textBonuses);
            textSettlement = FindViewById<TextView>(Resource.Id.textSettlement);
            textHelp = FindViewById<TextView>(Resource.Id.textHelp);

            editSurname = FindViewById<TextInputLayout>(Resource.Id.editSurname);
            editName = FindViewById<TextInputLayout>(Resource.Id.editName);
            editMark = FindViewById<TextInputLayout>(Resource.Id.editMark);
            editModel = FindViewById<TextInputLayout>(Resource.Id.editModel);
            editYear = FindViewById<TextInputLayout>(Resource.Id.editYear);
            editRegistrationNumber = FindViewById<TextInputLayout>(Resource.Id.editRegistrationNumber);

            buttonSendRequest = FindViewById<Button>(Resource.Id.buttonSendRequest);

            menuFragment = new MenuFragment();

            var partialMenuSetup = SupportFragmentManager.BeginTransaction();
            partialMenuSetup.Add(Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
            partialMenuSetup.Commit();

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            textBuyPolice.Text = AppResources.BuyPoliceTitle;
            textBenefits.Text = AppResources.BenefitsTitle;
            textQuarterlyPolicy.Text = "- " + AppResources.QuarterlyPolicy;
            textDiscounts.Text = "- " + AppResources.Discounts;
            textBonuses.Text = "- " + AppResources.BonusSystem;
            textSettlement.Text = "- " + AppResources.Settlement;
            textHelp.Text = "- " + AppResources.HelpOnRoad;

            editSurname.EditText.Hint = AppResources.Surname;
            editName.EditText.Hint = AppResources.Name;
            editMark.EditText.Hint = AppResources.CarMark;
            editModel.EditText.Hint = AppResources.CarModel;
            editYear.EditText.Hint = AppResources.CarYear;
            editRegistrationNumber.EditText.Hint = AppResources.CarRegistrationNumber;

            buttonSendRequest.Text = AppResources.SendRequest.ToUpper();

            SetupGestures();
        }

        void SetupGestures()
        {
            buttonSendRequest.Click += delegate
            {
                User entity = sqliteManager.GetUser();
                if (entity != null)
                {
                    entity.AutoModeStatus = (int)AutoParkingStatus.NeedInitialisationWithInsurance;
                    sqliteManager.SaveUser(entity);
                }

                var activity = new Intent(this, typeof(AppInfoActivity));
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