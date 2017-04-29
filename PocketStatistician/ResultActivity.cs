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

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;    // http://stackoverflow.com/questions/36499104/how-to-create-action-bar-tab-on-xamarin-android

            #region Declarations
            var dataTab = ActionBar.NewTab();
            var tableTab = ActionBar.NewTab();
            var graphTab = ActionBar.NewTab();

            dataTab.SetText(Resources.GetString())
            #endregion 
        }
    }
}