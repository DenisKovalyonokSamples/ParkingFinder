using Android.Gms.Common;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Views;
using Android.App;
using Android.Widget;
using Android.Content.PM;
using ParkCred.Droid.Activities.Base;
using ParkCred.Localization;
using ParkCred.Droid.Fragments;
using System.Threading.Tasks;
using ParkCred.Shared.DataAccess;
using System;
using System.Linq;
using ParkCred.Shared.Entities.SQL;
using System.Collections.Generic;
using Android.Graphics;
using Android.Support.V4.Content;
using ParkCred.Shared.Enums;

namespace ParkCred.Droid.Activities
{
    [Activity(Label = "ParkCred", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class PlanSessionActivity : BaseActivity, IOnMapReadyCallback
    {
        MenuFragment menuFragment;

        MapView mapView;
        GoogleMap _map;
        LatLng CurrentLocation;
        LatLng UserLocation;
        LinearLayout mapCurrentLocation;
        ImageView mapCurrentLocationImg;

        LinearLayout layoutSelectedParkingData;
        TextView textParkingZoneNumber;
        TextView textCurrentLocationAddress;
        TextView textParkingStartDescription;
        TextView textParkingStartTimeNumber;
        TextView textParkingStartTimeType;
        TextView textParkingWorkTimeDescription;
        TextView textParkingWeekendDescription;

        Button buttonPlanSession;
        TextView textCurrentLocationTitle;
        TextView textCurrentUserAddress;

        RelativeLayout layoutSessionStartTime;
        TextView textStartSessionTitle;
        TextView textMinusTime;
        TextView textPlusTime;
        TextView textTimeHoursTen;
        TextView textTimeHours;
        TextView textTimeMinutesTen;
        TextView textTimeMinutes;
        TextView textButtonStart;
        TextView textButtonCancel;

        ImageView mapLocationMarker;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PlanSession);
            SetTitleBack();

            mapView = FindViewById<MapView>(Resource.Id.map);
            mapView.OnCreate(savedInstanceState);

            mapCurrentLocation = FindViewById<LinearLayout>(Resource.Id.mapCurrentLocation);
            mapCurrentLocationImg = FindViewById<ImageView>(Resource.Id.mapCurrentLocationImg);
            mapLocationMarker = FindViewById<ImageView>(Resource.Id.mapLocationMarker);
            layoutSelectedParkingData = FindViewById<LinearLayout>(Resource.Id.layoutSelectedParkingData);
            textParkingZoneNumber = FindViewById<TextView>(Resource.Id.textParkingZoneNumber);
            textCurrentLocationAddress = FindViewById<TextView>(Resource.Id.textCurrentLocationAddress);
            textParkingStartDescription = FindViewById<TextView>(Resource.Id.textParkingStartDescription);
            textParkingStartTimeNumber = FindViewById<TextView>(Resource.Id.textParkingStartTimeNumber);
            textParkingStartTimeType = FindViewById<TextView>(Resource.Id.textParkingStartTimeType);
            textParkingWorkTimeDescription = FindViewById<TextView>(Resource.Id.textParkingWorkTimeDescription);
            textParkingWeekendDescription = FindViewById<TextView>(Resource.Id.textParkingWeekendDescription);

            buttonPlanSession = FindViewById<Button>(Resource.Id.buttonPlanSession);
            textCurrentLocationTitle = FindViewById<TextView>(Resource.Id.textCurrentLocationTitle);
            textCurrentUserAddress = FindViewById<TextView>(Resource.Id.textCurrentUserAddress);

            layoutSessionStartTime = FindViewById<RelativeLayout>(Resource.Id.layoutSessionStartTime);
            textStartSessionTitle = FindViewById<TextView>(Resource.Id.textStartSessionTitle);
            textMinusTime = FindViewById<TextView>(Resource.Id.textMinusTime);
            textPlusTime = FindViewById<TextView>(Resource.Id.textPlusTime);
            textTimeHoursTen = FindViewById<TextView>(Resource.Id.textTimeHoursTen);
            textTimeHours = FindViewById<TextView>(Resource.Id.textTimeHours);
            textTimeMinutesTen = FindViewById<TextView>(Resource.Id.textTimeMinutesTen);
            textTimeMinutes = FindViewById<TextView>(Resource.Id.textTimeMinutes);
            textButtonStart = FindViewById<TextView>(Resource.Id.textButtonStart);
            textButtonCancel = FindViewById<TextView>(Resource.Id.textButtonCancel);

            menuFragment = new MenuFragment();

            var partialMenuSetup = SupportFragmentManager.BeginTransaction();
            partialMenuSetup.Add(Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
            partialMenuSetup.Commit();

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            buttonPlanSession.Text = AppResources.ScheduleParkingSession.ToUpper();
            textCurrentLocationTitle.Text = AppResources.CurrentLocation.ToUpper();
            textCurrentUserAddress.Text = AppResources.Undefined;

            mapCurrentLocationImg.SetColorFilter(new Color(ContextCompat.GetColor(this, Resource.Color.map_control_gray)), PorterDuff.Mode.SrcAtop);
            mapLocationMarker.Visibility = ViewStates.Gone;

            SetupGestures();

            User entity = sqliteManager.GetUser();
            if (entity.SessionStatus == (int)ParkingSessionStatus.Planned && entity.ActiveParkingId != 0)
            {
                buttonPlanSession.Text = AppResources.ChangeZone.ToUpper();

                List<Parking> entities = sqliteManager.GetParkings();
                if (entities != null && entities.Count > 0)
                {
                    Parking parking = entities.Where(e => e.Id == entity.ActiveParkingId).FirstOrDefault();

                    activeParkingId = parking.Id;

                    textParkingZoneNumber.Text = parking.Name;
                    textCurrentLocationAddress.Text = parking.Address;
                    textParkingStartDescription.Text = AppResources.FirstTimeFreeTitle;
                    textParkingStartTimeNumber.Text = parking.Conditions;
                    textParkingStartTimeType.Text = AppResources.Min.ToLower();
                    textParkingWorkTimeDescription.Text = parking.WorkTime;
                    textParkingWeekendDescription.Text = parking.WeekendConditions;
                }
            }
            else
            {
                ShowSelectedParkingDescription();
            }
        }

        int activeParkingId = 0;
        void ShowSelectedParkingDescription()
        {
            List<Parking> entities = sqliteManager.GetParkings();
            if (entities != null && entities.Count > 0)
            {
                Parking parking;
                if (activeParkingId == 0 || activeParkingId == 3)
                {
                    parking = entities.FirstOrDefault();
                }
                else
                {
                    parking = entities.Where(e => e.Id == activeParkingId + 1).FirstOrDefault();
                }

                activeParkingId = parking.Id;

                textParkingZoneNumber.Text = parking.Name;
                textCurrentLocationAddress.Text = parking.Address;
                textParkingStartDescription.Text = AppResources.FirstTimeFreeTitle;
                textParkingStartTimeNumber.Text = parking.Conditions;
                textParkingStartTimeType.Text = AppResources.Min.ToLower();
                textParkingWorkTimeDescription.Text = parking.WorkTime;
                textParkingWeekendDescription.Text = parking.WeekendConditions;
            }
        }

        void SetupGestures()
        {
            buttonPlanSession.Click += delegate
            {
                if (buttonPlanSession.Text == AppResources.ChangeZone.ToUpper())
                {
                    ShowConfirmDialog();
                }
                else
                {
                    buttonPlanSession.Visibility = ViewStates.Gone;
                    textCurrentLocationTitle.Visibility = ViewStates.Gone;
                    textCurrentUserAddress.Visibility = ViewStates.Gone;

                    layoutSessionStartTime.Visibility = ViewStates.Visible;

                    SetScheduleClockValue(DateTime.Now.AddMinutes(10));
                }
            };

            textButtonCancel.Click += delegate
            {
                Finish();
            };

            textPlusTime.Click += delegate
            {
                PlusScheduleClockValue();
            };

            textMinusTime.Click += delegate
            {
                MinusScheduleClockValue();
            };

            mapCurrentLocation.Click += delegate
            {
                if (UserLocation != null)
                {
                    mapLocationMarker.Visibility = ViewStates.Gone;
                    ShowParkingMarker = false;
                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(UserLocation, 17);
                    _map.MoveCamera(cameraUpdate);
                }
            };

            textButtonStart.Click += delegate
            {
                User entity = sqliteManager.GetUser();
                if (entity != null)
                {
                    entity.SessionStatus = (int)ParkingSessionStatus.Planned;
                    entity.ActiveParkingId = activeParkingId;
                    entity.SessionStartTime = currentTime;

                    sqliteManager.SaveUser(entity);
                }

                Finish();
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            InitMapFragment();
        }

        #region Dialogs 

        protected void ShowConfirmDialog()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(AppResources.ChangeZone);
            alert.SetMessage(AppResources.ChangeZoneConfirmMessage);
            alert.SetPositiveButton(AppResources.Yes, (senderAlert, args) =>
            {
                User entity = sqliteManager.GetUser();
                if (entity != null)
                {
                    entity.ActiveParkingId = activeParkingId;

                    sqliteManager.SaveUser(entity);
                }

                Finish();
            });
            alert.SetNegativeButton(AppResources.No, (senderAlert, args) =>
            {
            });

            Android.App.Dialog dialog = alert.Create();
            dialog.Show();
        }

        #endregion

        #region Schedule Control

        DateTime currentTime;

        void SetScheduleClockValue(DateTime value)
        {
            currentTime = value;

            int hours = value.Hour;
            int minutes = value.Minute;

            if (hours < 10)
            {
                textTimeHoursTen.Text = "0";
                textTimeHours.Text = hours.ToString();
            }
            else
            {
                int tens = 0;
                while (hours >= 10)
                {
                    tens++;
                    hours = hours - 10;
                }

                textTimeHoursTen.Text = tens.ToString();
                textTimeHours.Text = (value.Hour - tens * 10).ToString();
            }

            if (minutes < 10)
            {
                textTimeMinutesTen.Text = "0";
                textTimeMinutes.Text = minutes.ToString();
            }
            else
            {
                int tens = 0;
                while (minutes >= 10)
                {
                    tens++;
                    minutes = minutes - 10;
                }

                textTimeMinutesTen.Text = tens.ToString();
                textTimeMinutes.Text = (value.Minute - tens * 10).ToString();
            }
        }

        void PlusScheduleClockValue()
        {
            SetScheduleClockValue(currentTime.AddMinutes(1));
        }

        void MinusScheduleClockValue()
        {
            if (currentTime.AddMinutes(-1) >= DateTime.Now)
            {
                SetScheduleClockValue(currentTime.AddMinutes(-1));
            }
            else
            {
                Toast.MakeText(this, AppResources.ParkingSessionTimeMinusError, ToastLength.Long).Show();
            }
        }

        #endregion

        #region Map

        bool ShowParkingMarker = false;

        void InitMapFragment()
        {
            if (_map == null)
            {
                mapView.GetMapAsync(this);
            }
            else
            {
                mapView.OnResume();
            }

            InitLocationOnMap();
        }

        async void InitLocationOnMap()
        {
            if (_map != null)
            {
                _map.MyLocationEnabled = true;
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            if (_map != null)
            {
                try
                {
                    MapsInitializer.Initialize(this);
                }
                catch (GooglePlayServicesNotAvailableException e)
                {

                }

                _map.UiSettings.ZoomControlsEnabled = true;
                _map.MyLocationEnabled = true; 
                _map.MyLocationChange += _map_MyLocationChange;
                _map.CameraChange += delegate
                {
                    var coordinate = _map.CameraPosition.Target;

                    if (coordinate.Longitude == 0 || coordinate.Latitude == 0)
                        return;

                    CurrentLocation = coordinate;

                    if (ShowParkingMarker)
                    {
                        mapLocationMarker.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        ShowParkingMarker = true;
                    }

                    ShowSelectedParkingDescription();
                };

                View locationButton = mapView.FindViewWithTag("GoogleMapMyLocationButton");
                locationButton.Visibility = ViewStates.Gone;

                mapView.OnResume();
            }
        }

        private void _map_MyLocationChange(object sender, GoogleMap.MyLocationChangeEventArgs e)
        {
            CurrentLocation = new LatLng(e.Location.Latitude, e.Location.Longitude);

            if (CurrentLocation.Longitude == 0 || CurrentLocation.Latitude == 0)
                return;

            CameraUpdate cameraUpdate1 = CameraUpdateFactory.NewLatLngZoom(CurrentLocation, 17);
            _map.MoveCamera(cameraUpdate1);

            if (_map.MyLocationEnabled)
            {
                UserLocation = CurrentLocation;
                _map.MyLocationEnabled = false;

                MarkerOptions marker = new MarkerOptions();
                marker.SetPosition(UserLocation);
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.ic_user_location));
                _map.AddMarker(marker);

                GetAddressByLocation();
            }
        }

        async Task GetAddressByLocation()
        {
            if (UserLocation != null)
            {
                string address = await APIDataManager.GetSuggestedAddress(UserLocation.Latitude.ToString(), UserLocation.Longitude.ToString());
                if (!string.IsNullOrEmpty(address))
                {
                    textCurrentUserAddress.Text = address;
                }
            }
        }

        #endregion

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