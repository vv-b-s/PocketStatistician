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

using Analizers;
using Android.Graphics;

namespace PocketStatistician
{
    [Activity(Label = "Pocket Statistician")]
    public class ResultActivity : Activity
    {
        private static SfDataGrid TableData;
        private static SfChart Graph;
        public static One_dim_analysis ODA;

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
            TextView output = new TextView(layout.Context) { Text = ODA.DisplayFullData() };
            scrollView.AddView(output);

            layout.AddView(scrollView);
            #endregion
        }

        public static void ModifyTableTab(LinearLayout layout)          //https://help.syncfusion.com/xamarin-android/sfdatagrid/getting-started
        {
            TableData = new SfDataGrid(layout.Context);
            TableData.AllowSorting = true;
            TableData.GridStyle.AlternatingRowColor = Color.Aqua;
            TableData.AutoGenerateColumns = false;

            ODA_GridRepository viewModel;

            if(MainActivity.SpinnerPos==(int)MainActivity.AnalysisType.OneDA)
            {
                viewModel = new ODA_GridRepository(MainActivity.hasIntervals, ODA.TableData);
                if (MainActivity.hasIntervals)
                {
                    TableData.ItemsSource = viewModel.oda_model_with_intervals;

                    #region Column Names
                    GridTextColumn[] columnName = new GridTextColumn[11];
                    for (int i = 0; i < columnName.Length; i++)
                    {
                        columnName[i] = new GridTextColumn();
                        TableData.Columns.Add(columnName[i]);
                    }

                    columnName[0].MappingName = "Nm";
                    columnName[0].HeaderText = $"{(char)8470}";

                    columnName[1].MappingName = "Intervals";
                    columnName[1].HeaderText = "X' ~ X\"";

                    columnName[2].MappingName = "Xi";
                    columnName[2].HeaderText = "Xi";

                    columnName[3].MappingName = "Fi";
                    columnName[3].HeaderText = "Fi";

                    columnName[4].MappingName = "XiFi";
                    columnName[4].HeaderText = "XiFi";

                    columnName[5].MappingName = "ComulatFreg";
                    columnName[5].HeaderText = "C";

                    columnName[6].MappingName = "Xi_minus_Avg";
                    columnName[6].HeaderText = "Xi-X";

                    columnName[7].MappingName = "Module_Xi_minus_Avg";
                    columnName[7].HeaderText = "|Xi-X|";

                    columnName[8].MappingName = "Squared_Xi_minus_Avg";
                    columnName[8].HeaderText = "(Xi-X)^2";

                    columnName[9].MappingName = "Cubix_Xi_minus_Avg";
                    columnName[9].HeaderText = "(Xi-X)^3";

                    columnName[10].MappingName = "Quadric_Xi_minus_Avg";
                    columnName[10].HeaderText = "(Xi-X)^4";
                    #endregion
                }
                else
                {
                    TableData.ItemsSource = viewModel.oda_model_no_intervals;

                    #region Column Names
                    GridTextColumn[] columnName = new GridTextColumn[10];
                    for (int i = 0; i < columnName.Length; i++)
                    {
                        columnName[i] = new GridTextColumn();
                        TableData.Columns.Add(columnName[i]);
                    }

                    columnName[0].MappingName = "Nm";
                    columnName[0].HeaderText = $"{(char)8470}";

                    columnName[1].MappingName = "Xi";
                    columnName[1].HeaderText = "Xi";

                    columnName[2].MappingName = "Fi";
                    columnName[2].HeaderText = "Fi";

                    columnName[3].MappingName = "XiFi";
                    columnName[3].HeaderText = "XiFi";

                    columnName[4].MappingName = "ComulatFreg";
                    columnName[4].HeaderText = "C";

                    columnName[5].MappingName = "Xi_minus_Avg";
                    columnName[5].HeaderText = "Xi-X";

                    columnName[6].MappingName = "Module_Xi_minus_Avg";
                    columnName[6].HeaderText = "|Xi-X|";

                    columnName[7].MappingName = "Squared_Xi_minus_Avg";
                    columnName[7].HeaderText = "(Xi-X)^2";

                    columnName[8].MappingName = "Cubix_Xi_minus_Avg";
                    columnName[8].HeaderText = "(Xi-X)^3";

                    columnName[9].MappingName = "Quadric_Xi_minus_Avg";
                    columnName[9].HeaderText = "(Xi-X)^4";
                    #endregion
                }
            }

            layout.AddView(TableData);            
        }

        public static void ModifyGraphTab(LinearLayout layout)
        {
            #region Initializing Graph
            Graph = new SfChart(layout.Context);

            NumericalAxis X = new NumericalAxis();
            X.Title.Text = "Xi";

            NumericalAxis Y = new NumericalAxis();
            Y.Title.Text = MainActivity.SpinnerPos == (int)MainActivity.AnalysisType.RegrCorA ?
                "Yi" : "Fi";

            Graph.PrimaryAxis   = X;
            Graph.SecondaryAxis = Y;
            #endregion

            #region Binding data with Graph
            GraphModel model = new GraphModel(ExcerptFieldsActivity.Xi,ExcerptFieldsActivity.Yi);
            SplineSeries graphLine = new SplineSeries()                         //https://help.syncfusion.com/xamarin-android/sfchart/getting-started
            {
                DataSource = model.Data,
                Color = Color.AntiqueWhite
            };
            graphLine.DataMarker.ShowMarker = true;
            graphLine.DataMarker.MarkerColor = Color.Turquoise;

            Graph.Series.Add(graphLine);
            #endregion
            
            
            layout.AddView(Graph);

        }

        #endregion
    }
}