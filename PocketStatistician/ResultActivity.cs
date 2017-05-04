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

using Syncfusion.Linq;                              // https://components.xamarin.com/gettingstarted/syncfusionessentialstudio
using Syncfusion.SfDataGrid;
using Syncfusion.GridCommon;
using Com.Syncfusion.Charts;

namespace PocketStatistician
{
    [Activity(Label = "Pocket Statistician")]
    public class ResultActivity : Activity
    {
        private static SfDataGrid TableData;
        private static SfChart Graph;

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
            #region Modify the scrollview
            var scrollView = new ScrollView(layout.Context);
            layout.AddView(scrollView);
            #endregion
        }

        public static void ModifyTableTab(LinearLayout layout)
        {
            TableData = new SfDataGrid(layout.Context);
            layout.AddView(TableData);            
        }

        public static void ModifyGraphTab(LinearLayout layout)
        {
            #region Initializing Graph
            Graph = new SfChart(layout.Context);
            NumericalAxis X = new NumericalAxis();
            NumericalAxis Y = new NumericalAxis();
            Graph.PrimaryAxis = X;
            Graph.SecondaryAxis = Y;
            #endregion

            #region Adding data to Graph
            #endregion

            layout.AddView(Graph);

        }

        #endregion
    }
}