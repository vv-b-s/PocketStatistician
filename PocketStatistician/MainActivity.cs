using System;
using System.Linq;
using System.Globalization;

using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Graphics;

namespace PocketStatistician
{
    [Activity(Label = "Pocket Statistician", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        #region Global Controls Access
        public enum AnalysisType { OneDA, RegrCorA };
        public static bool hasIntervals;
        public static int SpinnerPos = 0;
        public static Context MainActivityContext;
        #endregion

        #region Private Controls
        private static LinearLayout OptionalQuestionLayout;
        private static Button NextBT;
        private static EditText FieldsSize;
        private static RadioButton[] RB;
        private static Spinner AnalysisSwitch;
        private static EventHandler<AdapterView.ItemSelectedEventArgs> SpinnerEventHandler;
        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MainActivityContext = this;
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            #region Controls Declaration
            var analysisSwitch         = FindViewById <Spinner>         (Resource.Id.analysisSwitch);
            var optionalQuestionLayout = FindViewById <LinearLayout>    (Resource.Id.optionalQuestionLayout);
            RadioButton[] rb           = {
                                         FindViewById <RadioButton>     (Resource.Id.rb1),
                                         FindViewById <RadioButton>     (Resource.Id.rb2)
                                         };
            var fieldsSize             = FindViewById <EditText>        (Resource.Id.fieldsSize);
            var nextBT                 = FindViewById <Button>          (Resource.Id.nextBT);
            #endregion

            #region Declarations and predefinements
            optionalQuestionLayout.Visibility = SpinnerPos==0? ViewStates.Visible:ViewStates.Invisible;
            OptionalQuestionLayout = optionalQuestionLayout;
            nextBT.Enabled = false;
            NextBT = nextBT;
            FieldsSize = fieldsSize;
            RB = rb;
            AnalysisSwitch = analysisSwitch;
            #endregion

            #region Events
            ActivateSpinner(analysisSwitch, Resource.Array.anayisis_array);

            analysisSwitch.ItemSelected += analysisSwitch_ItemSelected;
            fieldsSize.TextChanged += fieldsSize_TextChange;
            nextBT.Click += nextBT_click;

            #endregion
        }

        #region Methods

        #region Spinner
        private void ActivateSpinner(Spinner spinner, int textArrayResld)
        {
            SpinnerEventHandler = new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            spinner.ItemSelected += SpinnerEventHandler;
            var adapter = ArrayAdapter.CreateFromResource(this, textArrayResld, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }
        #endregion

        #region Events
        private static void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            SpinnerPos = e.Position;
        }

        private static void analysisSwitch_ItemSelected(object sender, EventArgs e) => 
            OptionalQuestionLayout.Visibility = SpinnerPos == (int)AnalysisType.OneDA ? ViewStates.Visible : ViewStates.Invisible;

        private static void fieldsSize_TextChange(object sender, EventArgs e)
        {
            NextBT.Enabled = FieldsSize.Text != "" &&
            int.TryParse(FieldsSize.Text, out ExcerptFieldsActivity.NumberOfFields) &&
            !FieldsSize.Text.Contains("-") &&
            ExcerptFieldsActivity.NumberOfFields > 2 ? true : false;
        }

        private static void nextBT_click(object sender, EventArgs e)
        {
            hasIntervals = RB[0].Checked;
            var intent = new Intent(NextBT.Context, typeof(ExcerptFieldsActivity));
            NextBT.Context.StartActivity(intent);
        }

        public static void DisconnectEvents()
        {
            AnalysisSwitch.ItemSelected -= analysisSwitch_ItemSelected;
            AnalysisSwitch.ItemSelected -= SpinnerEventHandler;
            FieldsSize.TextChanged -= fieldsSize_TextChange;
            NextBT.Click -= nextBT_click;

        }
        #endregion

        #endregion
    }
}

