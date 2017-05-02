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

namespace PocketStatistician
{
    [Activity(Label = "Pocket Statistician")]
    public class ResultActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ResultLayout);

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            SlidingTabsFragment fragment = new SlidingTabsFragment();
            transaction.Replace(Resource.Id.sample_content_fragment, fragment);
            transaction.Commit();
        }

        #region Modify tabs from SlidingTabsFragment.cs
        public static void ModifyDataTab(LinearLayout layout)
        {
            TextView simpleText = new TextView(layout.Context);

            simpleText.Text = "Tabs work perfectly as expected.";
            layout.AddView(simpleText);
        }

        public static void ModifyTableTab(LinearLayout layout)
        {
            Button simpleBT = new Button(layout.Context);
            layout.AddView(simpleBT);
        }

        public static void ModifyGraphTab(LinearLayout layout)
        {

        }

        #endregion
    }
}