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
        public static int spinnerPos = 0;
        #endregion
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

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
            optionalQuestionLayout.Visibility = spinnerPos==0? ViewStates.Visible:ViewStates.Invisible;
            #endregion

            #region Events
            ActivateSpinner(analysisSwitch, Resource.Array.anayisis_array);

            #endregion
        }

        #region Methods

        #region Spinner
        private void ActivateSpinner(Spinner spinner, int textArrayResld)
        {
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, textArrayResld, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            spinnerPos = e.Position;
        }
        #endregion

        #endregion
    }
}

