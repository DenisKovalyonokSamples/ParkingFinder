using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using ParkCred.Droid.Fragments.Base;
using ParkCred.Localization;
using ParkCred.Shared.Enums;
using ParkCred.Shared.Managers;

namespace ParkCred.Droid.Fragments
{
    public class MenuFragment : BaseFragment
    {
        RelativeLayout mainLayout;
        RelativeLayout paymentLayout;
        RelativeLayout parkingLayout;
        RelativeLayout historyLayout;
        RelativeLayout moreLayout;

        ImageView imageViewMain;
        ImageView imageViewPolicies;
        ImageView imageViewBonuses;
        ImageView imageViewUsefull;
        ImageView imageViewProfile;

        TextView mainTitle;
        TextView paymentTitle;
        TextView parkingTitle;
        TextView historyTitle;
        TextView moreTitle;

        LinearLayout selectorMain;
        LinearLayout selectorPayment;
        LinearLayout selectorParking;
        LinearLayout selectorHistory;
        LinearLayout selectorMore;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            mainLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutMain);
            paymentLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutPolicies);
            parkingLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutBonuses);
            historyLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutUsefull);
            moreLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutProfile);

            imageViewMain = partial.FindViewById<ImageView>(Resource.Id.imageViewMain);
            imageViewPolicies = partial.FindViewById<ImageView>(Resource.Id.imageViewPolicies);
            imageViewBonuses = partial.FindViewById<ImageView>(Resource.Id.imageViewBonuses);
            imageViewUsefull = partial.FindViewById<ImageView>(Resource.Id.imageViewUsefull);
            imageViewProfile = partial.FindViewById<ImageView>(Resource.Id.imageViewProfile);

            selectorMain = partial.FindViewById<LinearLayout>(Resource.Id.selectorMain);
            selectorPayment = partial.FindViewById<LinearLayout>(Resource.Id.selectorPolicies);
            selectorParking = partial.FindViewById<LinearLayout>(Resource.Id.selectorBonuses);
            selectorHistory = partial.FindViewById<LinearLayout>(Resource.Id.selectorUsefull);
            selectorMore = partial.FindViewById<LinearLayout>(Resource.Id.selectorProfile);

            mainTitle = partial.FindViewById<TextView>(Resource.Id.textMain);
            paymentTitle = partial.FindViewById<TextView>(Resource.Id.textPolicies);
            parkingTitle = partial.FindViewById<TextView>(Resource.Id.textBonuses);
            historyTitle = partial.FindViewById<TextView>(Resource.Id.textUsefull);
            moreTitle = partial.FindViewById<TextView>(Resource.Id.textProfile);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            if (this.Activity is MainActivity)
            {
                SwitchSelectedItem(PartialType.Main);
            }
            else
            {
                ResetSelections();
            }

            mainTitle.Text = AppResources.Main;
            paymentTitle.Text = AppResources.Payment;
            parkingTitle.Text = AppResources.ParkingPlaces;
            historyTitle.Text = AppResources.History;
            moreTitle.Text = AppResources.More;

            SetupGestures();
        }

        void SetupGestures()
        {
            mainLayout.Click += delegate {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.Main);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.Main;
                    this.Activity.Finish();
                }
            };
            paymentLayout.Click += delegate {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.Payment);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.Payment;
                    this.Activity.Finish();
                }
            };
            parkingLayout.Click += delegate {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.Parking);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.Parking;
                    this.Activity.Finish();
                }
            };
            historyLayout.Click += delegate
            {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.History);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.History;
                    this.Activity.Finish();
                }
            };
            moreLayout.Click += delegate {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.More);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.More;
                    this.Activity.Finish();
                }
            };
        }

        public void SwitchSelectedItem(PartialType type)
        {
            ResetSelections();

            if (type == PartialType.Main)
            {
                selectorMain.Visibility = ViewStates.Visible;
                imageViewMain.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                mainTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
            if (type == PartialType.Payment)
            {
                selectorPayment.Visibility = ViewStates.Visible;
                imageViewPolicies.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                paymentTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
            if (type == PartialType.Parking)
            {
                selectorParking.Visibility = ViewStates.Visible;
                imageViewBonuses.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                parkingTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
            if (type == PartialType.More)
            {
                selectorMore.Visibility = ViewStates.Visible;
                imageViewProfile.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                moreTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
            if (type == PartialType.History)
            {
                selectorHistory.Visibility = ViewStates.Visible;
                imageViewUsefull.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                historyTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
        }

        void ResetSelections()
        {
            selectorMain.Visibility = ViewStates.Gone;
            selectorPayment.Visibility = ViewStates.Gone;
            selectorParking.Visibility = ViewStates.Gone;
            selectorMore.Visibility = ViewStates.Gone;
            selectorHistory.Visibility = ViewStates.Gone;

            imageViewMain.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);
            imageViewPolicies.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);
            imageViewBonuses.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);
            imageViewProfile.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);
            imageViewUsefull.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);

            mainTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
            paymentTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
            parkingTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
            moreTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
            historyTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.Menu;
        }

        #endregion
    }
}
