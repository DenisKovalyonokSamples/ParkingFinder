using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using ParkCred.Droid.Activities.Base;
using ParkCred.Droid.Fragments;
using ParkCred.Shared.Enums;
using ParkCred.Shared.Entities.SQL;
using System.Collections.Generic;
using ParkCred.Localization;
using Android.Views;
using System;
using System.Linq;
using System.Threading;
using ParkCred.Droid.Activities;
using Android.Content;
using Android.Gms.Location;
using Android.Gms.Common.Apis;
using Android;
using static Android.Support.V4.App.ActivityCompat;
using Android.Gms.Common;
using ParkCred.Shared.Managers;

namespace ParkCred.Droid
{
    [Activity(Label = "ParkCred", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/StartupTheme",
        LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class MainActivity : BaseActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, IResultCallback, IOnRequestPermissionsResultCallback
    {
        MenuFragment menuFragment;

        ScrollView scrollMainContainer;
        TextView textSum;
        TextView textBalanceTitle;
        Button buttonReplenish;
        TextView textStatusTitle;
        RelativeLayout layoutPlanParking;
        TextView textStatus;
        RelativeLayout layoutParkingStartPlanning;
        Button buttonPlanSession;
        TextView textWaitParkingSessionTitle;
        RelativeLayout layoutParkingNumber;
        TextView textParkingNumberTitle;
        TextView textParkingNumber;
        RelativeLayout layoutSessionStartTime;
        TextView textSessionStartTimeTitle;
        TextView textSessionStartTime;
        RelativeLayout layoutSessionUsedTime;
        TextView textSessionUsedTimeTitle;
        TextView textSessionUsedTime;
        RelativeLayout layoutFirstHourCost;
        TextView textFirstHourCost;
        RelativeLayout layoutHourCost;
        TextView textHourCost;
        RelativeLayout layoutButtonsContainer;
        Button buttonChangeZone;
        Button buttonCancelSession;
        TextView textAutoParkingTitle;
        Switch switchAutoParking;
        TextView textAutoParkingDescription;

        RelativeLayout layoutSessionEndTime;
        TextView textSessionEndTimeTitle;
        TextView textSessionEndTime;

        LinearLayout layoutSelectedParkingData;
        TextView textCurrentLocationAddress;
        TextView textParkingStartDescription;
        TextView textParkingStartTimeNumber;
        TextView textParkingStartTimeType;
        TextView textParkingWorkTimeDescription;
        TextView textParkingWeekendDescription;

        Timer dataSyncTimer;
        Timer parkingUsedTimer;
        Timer autoParkingTimer;

        int five = 300;
        int ten = 600;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            scrollMainContainer = FindViewById<ScrollView>(Resource.Id.scrollMainContainer);
            layoutSelectedParkingData = FindViewById<LinearLayout>(Resource.Id.layoutSelectedParkingData);
            textCurrentLocationAddress = FindViewById<TextView>(Resource.Id.textCurrentLocationAddress);
            textParkingStartDescription = FindViewById<TextView>(Resource.Id.textParkingStartDescription);
            textParkingStartTimeNumber = FindViewById<TextView>(Resource.Id.textParkingStartTimeNumber);
            textParkingStartTimeType = FindViewById<TextView>(Resource.Id.textParkingStartTimeType);
            textParkingWorkTimeDescription = FindViewById<TextView>(Resource.Id.textParkingWorkTimeDescription);
            textParkingWeekendDescription = FindViewById<TextView>(Resource.Id.textParkingWeekendDescription);

            textSum = FindViewById<TextView>(Resource.Id.textSum);
            textBalanceTitle = FindViewById<TextView>(Resource.Id.textBalanceTitle);
            buttonReplenish = FindViewById<Button>(Resource.Id.buttonReplenish);
            textStatusTitle = FindViewById<TextView>(Resource.Id.textStatusTitle);
            layoutPlanParking = FindViewById<RelativeLayout>(Resource.Id.layoutPlanParking);
            textStatus = FindViewById<TextView>(Resource.Id.textStatus);
            layoutParkingStartPlanning = FindViewById<RelativeLayout>(Resource.Id.layoutParkingStartPlanning);
            buttonPlanSession = FindViewById<Button>(Resource.Id.buttonPlanSession);
            textWaitParkingSessionTitle = FindViewById<TextView>(Resource.Id.textWaitParkingSessionTitle);
            layoutParkingNumber = FindViewById<RelativeLayout>(Resource.Id.layoutParkingNumber);
            textParkingNumberTitle = FindViewById<TextView>(Resource.Id.textParkingNumberTitle);
            textParkingNumber = FindViewById<TextView>(Resource.Id.textParkingNumber);
            layoutSessionStartTime = FindViewById<RelativeLayout>(Resource.Id.layoutSessionStartTime);
            textSessionStartTimeTitle = FindViewById<TextView>(Resource.Id.textSessionStartTimeTitle);
            textSessionStartTime = FindViewById<TextView>(Resource.Id.textSessionStartTime);
            layoutSessionUsedTime = FindViewById<RelativeLayout>(Resource.Id.layoutSessionUsedTime);
            textSessionUsedTimeTitle = FindViewById<TextView>(Resource.Id.textSessionUsedTimeTitle);
            textSessionUsedTime = FindViewById<TextView>(Resource.Id.textSessionUsedTime);
            layoutFirstHourCost = FindViewById<RelativeLayout>(Resource.Id.layoutFirstHourCost);
            textFirstHourCost = FindViewById<TextView>(Resource.Id.textFirstHourCost);
            layoutHourCost = FindViewById<RelativeLayout>(Resource.Id.layoutHourCost);
            textHourCost = FindViewById<TextView>(Resource.Id.textHourCost);
            layoutButtonsContainer = FindViewById<RelativeLayout>(Resource.Id.layoutButtonsContainer);
            buttonChangeZone = FindViewById<Button>(Resource.Id.buttonChangeZone);
            buttonCancelSession = FindViewById<Button>(Resource.Id.buttonCancelSession);
            textAutoParkingTitle = FindViewById<TextView>(Resource.Id.textAutoParkingTitle);
            switchAutoParking = FindViewById<Switch>(Resource.Id.switchAutoParking);
            textAutoParkingDescription = FindViewById<TextView>(Resource.Id.textAutoParkingDescription);
            layoutSessionEndTime = FindViewById<RelativeLayout>(Resource.Id.layoutSessionEndTime);
            textSessionEndTimeTitle = FindViewById<TextView>(Resource.Id.textSessionEndTimeTitle);
            textSessionEndTime = FindViewById<TextView>(Resource.Id.textSessionEndTime);

            menuFragment = new MenuFragment();

            var partialMenuSetup = SupportFragmentManager.BeginTransaction();
            partialMenuSetup.Add(Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
            partialMenuSetup.Commit();

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            textSum.Text = "0" + " " + AppResources.Rub.ToLower();
            textBalanceTitle.Text = AppResources.Balance.ToUpper();
            buttonReplenish.Text = AppResources.Replenish.ToUpper();
            textStatusTitle.Text = AppResources.ParkingStatus.ToUpper();
            buttonPlanSession.Text = AppResources.Schedule.ToUpper();
            textWaitParkingSessionTitle.Text = AppResources.WaitingForParkingSession;
            textParkingNumberTitle.Text = AppResources.ParkingAreaNumber;
            textSessionStartTimeTitle.Text = AppResources.DTOfSessionStart;
            textSessionEndTimeTitle.Text = AppResources.DTOfSessionEnd;
            textSessionUsedTimeTitle.Text = AppResources.Used;
            buttonChangeZone.Text = AppResources.ChangeZone.ToUpper();
            textAutoParkingTitle.Text = AppResources.AutomaticParking.ToUpper();
            textAutoParkingDescription.Text = AppResources.AutoParkingFunctionDescription;

            CheckLocalData();

            ConnectToGooglePlayServices();
        }

        protected override void OnResume()
        {
            base.OnResume();

            User entity = sqliteManager.GetUser();
            if (entity != null)
            {
                if (entity.AutoModeStatus != (int)AutoParkingStatus.TurnedOff)
                {
                    if (entity.AutoModeStatus == (int)AutoParkingStatus.NeedInitialisation)
                    {
                        switchAutoParking.Checked = true;
                        textAutoParkingDescription.Text = AppResources.AutoParkingOrderAccepted;
                        autoParkingTimer = new System.Threading.Timer(new TimerCallback(InitialiseAutoParking_Handler), null, (long)TimeSpan.FromSeconds(10).TotalMilliseconds, Timeout.Infinite);
                    }

                    if (entity.AutoModeStatus == (int)AutoParkingStatus.NeedInitialisationWithInsurance)
                    {
                        switchAutoParking.Checked = true;
                        textAutoParkingDescription.Text = AppResources.InsuranceAccepted;
                        autoParkingTimer = new System.Threading.Timer(new TimerCallback(InitialiseAutoParking_Handler), null, (long)TimeSpan.FromSeconds(10).TotalMilliseconds, Timeout.Infinite);
                    }

                    if (entity.AutoModeStatus == (int)AutoParkingStatus.Initialised)
                    {
                        switchAutoParking.Checked = true;
                        textAutoParkingDescription.Text = AppResources.AutoParkingModeMessage;
                        buttonPlanSession.Visibility = ViewStates.Gone;

                        autoParkingTimer = new System.Threading.Timer(new TimerCallback(PlanAutoParking_Handler), null, (long)TimeSpan.FromSeconds(five).TotalMilliseconds, Timeout.Infinite);
                    }

                    if (entity.AutoModeStatus == (int)AutoParkingStatus.Planned)
                    {
                        switchAutoParking.Checked = true;
                        textAutoParkingDescription.Text = AppResources.AutoParkingModeMessage;
                        buttonPlanSession.Visibility = ViewStates.Gone;
                    }

                    if (entity.AutoModeStatus == (int)AutoParkingStatus.Activated)
                    {
                        switchAutoParking.Checked = true;
                        textAutoParkingDescription.Text = AppResources.AutoParkingModeMessage;
                        buttonPlanSession.Visibility = ViewStates.Gone;
                    }
                }

                textSum.Text = entity.Balance.ToString() + " " + AppResources.Rub.ToLower();
                SetViewStateData(entity);
            }

            if (initialisation)
            {
                SetupGestures();
                initialisation = false;
            }
        }

        bool initialisation = true;

        #region Auto Parking Mode

        public void InitialiseAutoParking_Handler(object o)
        {
            this.RunOnUiThread(() =>
            {
                User entity = sqliteManager.GetUser();
                if (entity != null)
                {
                    entity.AutoModeStatus = (int)AutoParkingStatus.Initialised;
                    sqliteManager.SaveUser(entity);
                }

                textAutoParkingDescription.Text = AppResources.AutoParkingModeMessage;
                buttonPlanSession.Visibility = ViewStates.Gone;
            });

            autoParkingTimer = new System.Threading.Timer(new TimerCallback(PlanAutoParking_Handler), null, (long)TimeSpan.FromSeconds(five).TotalMilliseconds, Timeout.Infinite);
        }

        public void PlanAutoParking_Handler(object o)
        {
            this.RunOnUiThread(() =>
            {
                textAutoParkingDescription.Text = AppResources.AutoParkingModeMessage;
                buttonPlanSession.Visibility = ViewStates.Gone;

                User entity = sqliteManager.GetUser();
                if (entity != null)
                {
                    var random = new Random();

                    entity.SessionStatus = (int)ParkingSessionStatus.Planned;
                    entity.AutoModeStatus = (int)AutoParkingStatus.Planned;
                    entity.ActiveParkingId = random.Next(1, 3);
                    entity.SessionStartTime = DateTime.Now.AddSeconds(ten);

                    sqliteManager.SaveUser(entity);

                    SetViewStateData(entity);
                }
            });
        }

        public void FinishAutoParking_Handler(object o)
        {
            this.RunOnUiThread(() =>
            {
                FinishParkingSession();
                InitPlanParkingMode();

                autoParkingTimer = new System.Threading.Timer(new TimerCallback(InitialiseAutoParking_Handler), null, (long)TimeSpan.FromSeconds(1).TotalMilliseconds, Timeout.Infinite);
            });
        }

        #endregion

        void SetViewStateData(User user)
        {
            if (user.SessionStatus == (int)ParkingSessionStatus.NoSession)
            {
                InitPlanParkingMode();
            }
            if (user.SessionStatus == (int)ParkingSessionStatus.Planned)
            {
                if (user.SessionStartTime.HasValue)
                {
                    if (user.SessionStartTime.Value < DateTime.Now)
                    {
                        InitCurrentSessionMode();
                        CalculateCost(user.SessionStartTime.Value);                        

                        if (user.AutoModeStatus == (int)AutoParkingStatus.Planned)
                        {
                            user.AutoModeStatus = (int)AutoParkingStatus.Activated;
                            sqliteManager.SaveUser(user);

                            autoParkingTimer = new System.Threading.Timer(new TimerCallback(FinishAutoParking_Handler), null, (long)TimeSpan.FromSeconds(ten).TotalMilliseconds, Timeout.Infinite);
                        }
                    }
                    else
                    {
                        int diff = Convert.ToInt32((user.SessionStartTime.Value - DateTime.Now).TotalSeconds);
                        InitPlannedSessionMode(diff);
                    }

                    FillSessionSectionData(user);
                }
            }
            if (user.SessionStatus == (int)ParkingSessionStatus.Activated)
            {
                InitCurrentSessionMode();
                FillSessionSectionData(user);
                CalculateCost(user.SessionStartTime.Value);

                SessionTimeCounter = Convert.ToInt32((DateTime.Now - user.SessionStartTime.Value).TotalMinutes);
                parkingUsedTimer = new System.Threading.Timer(new TimerCallback(SetParkingTimeCost_Handler), null, (long)TimeSpan.FromMinutes(1).TotalMilliseconds, Timeout.Infinite);

                if (user.AutoModeStatus == (int)AutoParkingStatus.Activated)
                {
                    autoParkingTimer = new System.Threading.Timer(new TimerCallback(FinishAutoParking_Handler), null, (long)TimeSpan.FromSeconds(ten).TotalMilliseconds, Timeout.Infinite);
                }
            }
        }

        void FillSessionSectionData(User user)
        {
            List<Parking> entities = sqliteManager.GetParkings();
            if (entities != null)
            {
                Parking entity = entities.Where(e => e.Id == user.ActiveParkingId).FirstOrDefault();
                if (entity != null)
                {
                    textParkingNumber.Text = entity.Name.Replace("Parking zone ", "");
                    textSessionStartTime.Text = user.SessionStartTime.Value.ToString("HH:mm dd.MM.yyyy");
                    textFirstHourCost.Text = "First hour - 60";
                    textHourCost.Text = "Each other - 100";

                    FillParkingDescription(entity);
                }
            }
        }

        #region Location Request

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            if (_apiClient != null)
            {
                if (CheckCallingOrSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted &&
                    CheckCallingOrSelfPermission(Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
                {
                    LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder().AddLocationRequest(_locRequest);
                    var result = LocationServices.SettingsApi.CheckLocationSettings(_apiClient, builder.Build());
                    result.SetResultCallback(this);
                }
            }
        }

        LocationRequest _locRequest;
        GoogleApiClient _apiClient;

        public void ConnectToGooglePlayServices()
        {
            if (IsGooglePlayServicesInstalled())
            {
                if (_apiClient == null)
                    _apiClient = new GoogleApiClient.Builder(this)
                        .AddApi(LocationServices.API)
                        .AddConnectionCallbacks(this)
                        .AddOnConnectionFailedListener(this)
                        .Build();

                if (!_apiClient.IsConnected)
                    _apiClient.Connect();
            }
            else
            {
                ShowInstallGooglePlayServicesDialog();
            }
        }

        void BuildLocationRequest()
        {
            _locRequest = new LocationRequest();
            _locRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            _locRequest.SetFastestInterval(1 * 1000);
            _locRequest.SetInterval(2 * 1000);

            int sdk = (int)Android.OS.Build.VERSION.SdkInt;
            if (sdk < 23 ||
                (this.CheckCallingOrSelfPermission(Android.Manifest.Permission.AccessFineLocation) == Permission.Granted &&
                    this.CheckCallingOrSelfPermission(Android.Manifest.Permission.AccessCoarseLocation) == Permission.Granted))
            {
                LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder().AddLocationRequest(_locRequest);
                builder.SetAlwaysShow(true);

                var result = LocationServices.SettingsApi.CheckLocationSettings(_apiClient, builder.Build());
                result.SetResultCallback(this);
            }
            else
            {
                Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new System.String[] { Android.Manifest.Permission.AccessFineLocation, Android.Manifest.Permission.AccessCoarseLocation }, 1);
            }
        }

        void ShowInstallGooglePlayServicesDialog()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Google Play Services");
            alert.SetMessage(AppResources.GooglePlayServicesNotInstalledMessage);
            alert.SetPositiveButton("OK", (senderAlert, args) =>
            {
            });

            Android.App.Dialog dialog = alert.Create();
            dialog.Show();
        }

        bool IsGooglePlayServicesInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
            }
            return false;
        }

        public void OnConnected(Bundle bundle)
        {
            BuildLocationRequest();
        }

        public void OnDisconnected()
        {
        }

        public void OnConnectionFailed(ConnectionResult bundle)
        {
        }

        public void OnConnectionSuspended(int i)
        {
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == requestCheckSettings)
            {
                if (resultCode == Result.Ok)
                {
                    //RequestLocationUpdates();
                }
                else
                {
                    Toast.MakeText(this, AppResources.EnableGPS, ToastLength.Long).Show();
                }
            }
        }

        const int requestCheckSettings = 2002;
        public void OnResult(Java.Lang.Object result)
        {
            var locationSettingsResult = result as LocationSettingsResult;

            Statuses status = locationSettingsResult.Status;
            switch (status.StatusCode)
            {
                case CommonStatusCodes.Success:
                    break;
                case CommonStatusCodes.ResolutionRequired:
                    try
                    {
                        status.StartResolutionForResult(this, requestCheckSettings);
                    }
                    catch (IntentSender.SendIntentException)
                    {
                    }
                    break;
                case LocationSettingsStatusCodes.SettingsChangeUnavailable:
                    break;
            }
        }

        #endregion

        void SetupGestures()
        {
            switchAutoParking.CheckedChange += delegate
            {
                if (switchAutoParking.Checked)
                {
                    var activity = new Intent(this, typeof(AutoParkingActivity));
                    StartActivity(activity);
                }
                else
                {
                    textAutoParkingDescription.Text = AppResources.AutoParkingFunctionDescription;
                    buttonPlanSession.Visibility = ViewStates.Visible;
                    User entity = sqliteManager.GetUser();
                    if (entity != null)
                    {
                        if (entity.AutoModeStatus != (int)AutoParkingStatus.TurnedOff)
                        {
                            if (dataSyncTimer != null)
                                dataSyncTimer.Change(Timeout.Infinite, Timeout.Infinite);
                            if (autoParkingTimer != null)
                                autoParkingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                            if (parkingUsedTimer != null)
                                parkingUsedTimer.Change(Timeout.Infinite, Timeout.Infinite);

                            entity.AutoModeStatus = (int)AutoParkingStatus.TurnedOff;
                            entity.SessionStatus = (int)ParkingSessionStatus.NoSession;
                            entity.ActiveParkingId = 0;
                            entity.SessionStartTime = null;
                            sqliteManager.SaveUser(entity);

                            InitPlanParkingMode();
                            buttonPlanSession.Visibility = ViewStates.Visible;
                        }
                    }
                }
            };

            buttonReplenish.Click += delegate
            {
                User user = sqliteManager.GetUser();
                if (user != null)
                {
                    int balance = user.Balance + 500;
                    textSum.Text = balance.ToString() + " " + AppResources.Rub.ToLower();

                    user.Balance = balance;
                    sqliteManager.SaveUser(user);
                }
            };

            buttonPlanSession.Click += delegate
            {
                var activity = new Intent(this, typeof(PlanSessionActivity));
                StartActivity(activity);
            };

            buttonChangeZone.Click += delegate
            {
                if (buttonChangeZone.Text == AppResources.ChangeZone.ToUpper())
                {
                    var activity = new Intent(this, typeof(PlanSessionActivity));
                    StartActivity(activity);
                }

                if (buttonChangeZone.Text == AppResources.FinishAndSchedule.ToUpper())
                {
                    FinishParkingSession();
                    InitPlanParkingMode();

                    var activity = new Intent(this, typeof(PlanSessionActivity));
                    StartActivity(activity);
                }
            };

            buttonCancelSession.Click += delegate
            {
                if (buttonCancelSession.Text == AppResources.CancelSession.ToUpper())
                {
                    if (dataSyncTimer != null)
                        dataSyncTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    if (autoParkingTimer != null)
                        autoParkingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    InitPlanParkingMode();
                    FinishParkingSession();

                    User entity = sqliteManager.GetUser();
                    if (entity.AutoModeStatus != (int)AutoParkingStatus.TurnedOff)
                    {
                        autoParkingTimer = new System.Threading.Timer(new TimerCallback(InitialiseAutoParking_Handler), null, (long)TimeSpan.FromSeconds(1).TotalMilliseconds, Timeout.Infinite);
                    }
                }

                if (buttonCancelSession.Text == AppResources.EndSession.ToUpper())
                {
                    if (buttonChangeZone.Visibility == ViewStates.Visible)
                    {
                        if (dataSyncTimer != null)
                            dataSyncTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        if (autoParkingTimer != null)
                            autoParkingTimer.Change(Timeout.Infinite, Timeout.Infinite);

                        FinishParkingSession();
                        InitPlanParkingMode();

                        User entity = sqliteManager.GetUser();
                        if (entity.AutoModeStatus != (int)AutoParkingStatus.TurnedOff)
                        {
                            autoParkingTimer = new System.Threading.Timer(new TimerCallback(InitialiseAutoParking_Handler), null, (long)TimeSpan.FromSeconds(1).TotalMilliseconds, Timeout.Infinite);
                        }
                    }
                    else if (finishAutoModeCheck)
                    {
                        if (dataSyncTimer != null)
                            dataSyncTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        if (autoParkingTimer != null)
                            autoParkingTimer.Change(Timeout.Infinite, Timeout.Infinite);

                        FinishParkingSession();
                        InitPlanParkingMode();

                        User entity = sqliteManager.GetUser();
                        if (entity.AutoModeStatus != (int)AutoParkingStatus.TurnedOff)
                        {
                            autoParkingTimer = new System.Threading.Timer(new TimerCallback(InitialiseAutoParking_Handler), null, (long)TimeSpan.FromSeconds(1).TotalMilliseconds, Timeout.Infinite);
                        }

                        finishAutoModeCheck = false;
                    }
                    else
                    {
                        layoutSessionEndTime.Visibility = ViewStates.Visible;
                        layoutSelectedParkingData.Visibility = ViewStates.Gone;
                        layoutFirstHourCost.Visibility = ViewStates.Gone;
                        layoutHourCost.Visibility = ViewStates.Invisible;
                        textSessionEndTime.Text = DateTime.Now.ToString("HH:mm dd.MM.yyyy");

                        User entity = sqliteManager.GetUser();
                        if (entity.AutoModeStatus != (int)AutoParkingStatus.TurnedOff)
                        {
                            if (dataSyncTimer != null)
                                dataSyncTimer.Change(Timeout.Infinite, Timeout.Infinite);
                            if (autoParkingTimer != null)
                                autoParkingTimer.Change(Timeout.Infinite, Timeout.Infinite);

                            finishAutoModeCheck = true;
                        }
                        else
                        {
                            buttonChangeZone.Text = AppResources.FinishAndSchedule.ToUpper();
                            buttonChangeZone.Visibility = ViewStates.Visible;
                            layoutButtonsContainer.LayoutParameters.Height = this.Resources.GetDimensionPixelSize(Resource.Dimension.container_two_buttons_height);
                        }
                    }
                }
            };
        }

        bool finishAutoModeCheck = false;

        void FinishParkingSession()
        {
            User user = sqliteManager.GetUser();
            if (user != null)
            {
                if (parkingUsedTimer != null)
                    parkingUsedTimer.Change(Timeout.Infinite, Timeout.Infinite);

                if (user.SessionStatus == (int)ParkingSessionStatus.Activated)
                {
                    int cost = GetFinalCost(user.SessionStartTime.Value);
                    if (cost < user.Balance)
                    {
                        user.Balance = user.Balance - cost;
                    }
                    else
                    {
                        user.Balance = 0;
                    }

                    textSum.Text = user.Balance.ToString() + " " + AppResources.Rub.ToLower();
                }

                user.SessionStatus = (int)ParkingSessionStatus.NoSession;

                if (user.AutoModeStatus != (int)AutoParkingStatus.TurnedOff)
                {
                    user.AutoModeStatus = (int)AutoParkingStatus.Finished;
                }

                user.ActiveParkingId = 0;
                user.SessionStartTime = null;
                sqliteManager.SaveUser(user);
            }
        }

        void CalculateCost(DateTime start)
        {
            string result = string.Empty;

            int cost = 0;
            int usedTime = Convert.ToInt32((DateTime.Now - start).TotalMinutes);

            if (usedTime <= 60)
            {
                textSessionUsedTime.Text = usedTime.ToString() + " min. / " + usedTime.ToString() + " r.";
            }
            else
            {
                cost = 60;
                int time = usedTime - 60;
                decimal partCost = time * 100 / 60;
                cost += Convert.ToInt32(Math.Round(partCost, 0));
                textSessionUsedTime.Text = usedTime.ToString() + " min. / " + cost.ToString() + " r.";
            }
        }

        int GetFinalCost(DateTime start)
        {
            int result = 0;

            int cost = 0;
            int usedTime = Convert.ToInt32((DateTime.Now - start).TotalMinutes);

            if (usedTime <= 60)
            {
                result = usedTime;
            }
            else
            {
                cost = 60;
                int time = usedTime - 60;
                decimal partCost = time * 100 / 60;
                cost += Convert.ToInt32(Math.Round(partCost, 0));
                result = cost;
            }

            return result;
        }

        void FillParkingDescription(Parking parking)
        {
            textCurrentLocationAddress.Text = parking.Address;
            textParkingStartDescription.Text = AppResources.FirstTimeFreeTitle;
            textParkingStartTimeNumber.Text = parking.Conditions;
            textParkingStartTimeType.Text = AppResources.Min.ToLower();
            textParkingWorkTimeDescription.Text = parking.WorkTime;
            textParkingWeekendDescription.Text = parking.WeekendConditions;
        }

        void InitPlanParkingMode()
        {
            textStatus.Text = AppResources.NoScheduledParkingSession;

            layoutParkingStartPlanning.Visibility = ViewStates.Visible;
            layoutParkingNumber.Visibility = ViewStates.Gone;
            layoutSessionStartTime.Visibility = ViewStates.Gone;
            layoutSessionEndTime.Visibility = ViewStates.Gone;
            layoutSessionUsedTime.Visibility = ViewStates.Gone;
            layoutFirstHourCost.Visibility = ViewStates.Gone;
            layoutHourCost.Visibility = ViewStates.Gone;
            layoutButtonsContainer.Visibility = ViewStates.Gone;
            layoutSelectedParkingData.Visibility = ViewStates.Gone;
        }

        void InitPlannedSessionMode(int period = 0)
        {
            textStatus.Text = AppResources.PlannedSession;

            layoutParkingStartPlanning.Visibility = ViewStates.Gone;
            layoutParkingNumber.Visibility = ViewStates.Visible;
            layoutSessionStartTime.Visibility = ViewStates.Visible;
            layoutSessionEndTime.Visibility = ViewStates.Gone;
            layoutSessionUsedTime.Visibility = ViewStates.Gone;
            layoutFirstHourCost.Visibility = ViewStates.Gone;
            layoutHourCost.Visibility = ViewStates.Gone;
            layoutButtonsContainer.Visibility = ViewStates.Visible;
            layoutSelectedParkingData.Visibility = ViewStates.Visible;

            buttonCancelSession.Text = AppResources.CancelSession.ToUpper();
            buttonChangeZone.Text = AppResources.ChangeZone.ToUpper();

            if (period == 0)
            {
                dataSyncTimer = new System.Threading.Timer(new TimerCallback(DataSyncHandler), null, (long)TimeSpan.FromSeconds(600).TotalMilliseconds, Timeout.Infinite);
            }
            else
            {
                dataSyncTimer = new System.Threading.Timer(new TimerCallback(DataSyncHandler), null, (long)TimeSpan.FromSeconds(period).TotalMilliseconds, Timeout.Infinite);
            }
        }

        void InitCurrentSessionMode()
        {
            textStatus.Text = AppResources.CurrentSession;

            layoutParkingStartPlanning.Visibility = ViewStates.Gone;
            layoutParkingNumber.Visibility = ViewStates.Visible;
            layoutSessionStartTime.Visibility = ViewStates.Visible;
            layoutSessionEndTime.Visibility = ViewStates.Gone;
            layoutSessionUsedTime.Visibility = ViewStates.Visible;
            layoutFirstHourCost.Visibility = ViewStates.Gone;
            layoutHourCost.Visibility = ViewStates.Gone;
            layoutSelectedParkingData.Visibility = ViewStates.Visible;
            layoutButtonsContainer.Visibility = ViewStates.Visible;
            buttonChangeZone.Visibility = ViewStates.Gone;
            layoutButtonsContainer.LayoutParameters.Height = this.Resources.GetDimensionPixelSize(Resource.Dimension.container_one_buttons_height);

            buttonCancelSession.Text = AppResources.EndSession.ToUpper();

            User user = sqliteManager.GetUser();
            if (user != null)
            {
                user.SessionStatus = (int)ParkingSessionStatus.Activated;
                sqliteManager.SaveUser(user);
            }
        }

        public void SetParkingTimeCost_Handler(object o)
        {
            this.RunOnUiThread(() =>
            {
                SessionTimeCounter++;

                CalculateCost(DateTime.Now.AddMinutes(-SessionTimeCounter));

                parkingUsedTimer = new System.Threading.Timer(new TimerCallback(SetParkingTimeCost_Handler), null, (long)TimeSpan.FromMinutes(1).TotalMilliseconds, Timeout.Infinite);
            });
        }

        int SessionTimeCounter = 0;
        public void DataSyncHandler(object o)
        {
            this.RunOnUiThread(() =>
            {
                InitCurrentSessionMode();
                SessionTimeCounter = 0;
                parkingUsedTimer = new System.Threading.Timer(new TimerCallback(SetParkingTimeCost_Handler), null, (long)TimeSpan.FromMinutes(1).TotalMilliseconds, Timeout.Infinite);

                User user = sqliteManager.GetUser();
                List<Parking> entities = sqliteManager.GetParkings();
                if (entities != null && user != null)
                {
                    Parking entity = entities.Where(e => e.Id == user.ActiveParkingId).FirstOrDefault();

                    if (entity != null)
                    {
                        textParkingNumber.Text = entity.Name.Replace("Parking zone ", "");
                        textSessionStartTime.Text = user.SessionStartTime.Value.ToString("HH:mm dd.MM.yyyy");
                        textFirstHourCost.Text = "First hour - 60.";
                        textHourCost.Text = "Each other - 100.";

                        CalculateCost(user.SessionStartTime.Value);

                        if (user.AutoModeStatus == (int)AutoParkingStatus.Planned)
                        {
                            user.AutoModeStatus = (int)AutoParkingStatus.Activated;
                            sqliteManager.SaveUser(user);

                            autoParkingTimer = new System.Threading.Timer(new TimerCallback(FinishAutoParking_Handler), null, (long)TimeSpan.FromSeconds(ten).TotalMilliseconds, Timeout.Infinite);
                        }
                    }
                }
            });


            if (dataSyncTimer != null)
                dataSyncTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        void CheckLocalData()
        {
            User user = sqliteManager.GetUser();
            if (user == null)
            {
                var entity = new User();
                entity.Name = "Test User";
                entity.IsActiveSession = false;
                entity.ActiveParkingId = 0;
                entity.SessionStatus = (int)ParkingSessionStatus.NoSession;
                entity.AutoModeStatus = (int)AutoParkingStatus.TurnedOff;
                entity.Balance = 2000;
                entity.SessionStartTime = null;
                sqliteManager.SaveUser(entity);
            }

            //STAB (only for sample code): Change to real data 
            List<Parking> parkings = sqliteManager.GetParkings();
            if (parkings == null || parkings.Count == 0)
            {
                var parkingOne = new Parking();
                parkingOne.Name = "Parking zone 1001";
                parkingOne.Conditions = "15";
                sqliteManager.SaveParking(parkingOne);

                var parkingTwo = new Parking();
                parkingTwo.Name = "Parking zone 2002";
                parkingTwo.Conditions = "15";
                sqliteManager.SaveParking(parkingTwo);

                var parkingThree = new Parking();
                parkingThree.Name = "Parking zone 3003";
                parkingThree.Conditions = "15";
                sqliteManager.SaveParking(parkingThree);
            }
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

