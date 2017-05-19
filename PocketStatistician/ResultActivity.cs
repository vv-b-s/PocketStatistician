using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using Syncfusion.Linq;                              // https://components.xamarin.com/gettingstarted/syncfusionessentialstudio
using Syncfusion.SfDataGrid;
using Syncfusion.GridCommon;
using Syncfusion.SfGridConverter.Android;
using Syncfusion.SfDataGrid.Exporting;
using Syncfusion.Compression;
using Syncfusion.XlsIO;
using Com.Syncfusion.Charts;

using Analizers;
using Java.IO;

namespace PocketStatistician
{
    [Activity(Label = "Pocket Statistician")]
    public class ResultActivity : Activity
    {
        private static SfDataGrid TableData;
        private static SfChart Graph;
        public static One_dim_analysis ODA;
        public static Regression_analysis RA;

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
            TextView output = new TextView(layout.Context);

            if (MainActivity.SpinnerPos == (int)MainActivity.AnalysisType.OneDA)
                output.Text = ODA.DisplayFullData();
            else
                output.Text = RA.DisplayFullData();

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

            ODA_GridRepository oda_viewModel;
            RA_GridRepository ra_viewModel;

            if(MainActivity.SpinnerPos==(int)MainActivity.AnalysisType.OneDA)
            {
                oda_viewModel = new ODA_GridRepository(MainActivity.hasIntervals, ODA.TableData);
                if (MainActivity.hasIntervals)
                {
                    TableData.ItemsSource = oda_viewModel.oda_model_with_intervals;

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
                    columnName[6].HeaderText = $"Xi-{(char)88}{(char)772}";

                    columnName[7].MappingName = "Module_Xi_minus_Avg";
                    columnName[7].HeaderText = $"|Xi-{(char)88}{(char)772}|";

                    columnName[8].MappingName = "Squared_Xi_minus_Avg";
                    columnName[8].HeaderText = $"(Xi-{(char)88}{(char)772}){(char)178}Fi";

                    columnName[9].MappingName = "Cubix_Xi_minus_Avg";
                    columnName[9].HeaderText = $"(Xi-{(char)88}{(char)772}){(char)179}Fi";

                    columnName[10].MappingName = "Quadric_Xi_minus_Avg";
                    columnName[10].HeaderText = $"(Xi-{(char)88}{(char)772}){(char)8308}Fi";
                    #endregion
                }
                else
                {
                    TableData.ItemsSource = oda_viewModel.oda_model_no_intervals;

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
                    columnName[5].HeaderText = $"Xi-{(char)88}{(char)772}";

                    columnName[6].MappingName = "Module_Xi_minus_Avg";
                    columnName[6].HeaderText = $"|Xi-{(char)88}{(char)772}|";

                    columnName[7].MappingName = "Squared_Xi_minus_Avg";
                    columnName[7].HeaderText = $"(Xi-{(char)88}{(char)772}){(char)178}Fi";

                    columnName[8].MappingName = "Cubix_Xi_minus_Avg";
                    columnName[8].HeaderText = $"(Xi-{(char)88}{(char)772}){(char)179}Fi";

                    columnName[9].MappingName = "Quadric_Xi_minus_Avg";
                    columnName[9].HeaderText = $"(Xi-{(char)88}{(char)772}){(char)8308}Fi";
                    #endregion
                }
            }
            else if (MainActivity.SpinnerPos == (int)MainActivity.AnalysisType.RegrCorA)
            {
                ra_viewModel = new RA_GridRepository(RA.TableData);

                TableData.ItemsSource = ra_viewModel.ra_model;

                #region Column Names
                GridTextColumn[] columnName = new GridTextColumn[11];
                for (int i = 0; i < columnName.Length; i++)
                {
                    columnName[i] = new GridTextColumn();
                    TableData.Columns.Add(columnName[i]);
                }

                columnName[0].MappingName = "Nm";
                columnName[0].HeaderText = $"{(char)8470}";

                columnName[1].MappingName = "Xi";
                columnName[1].HeaderText = "Xi";

                columnName[2].MappingName = "Yi";
                columnName[2].HeaderText = "Yi";

                columnName[3].MappingName = "Xi_squared";
                columnName[3].HeaderText = $"Xi{(char)178}";

                columnName[4].MappingName = "XiYi";
                columnName[4].HeaderText = "XiYi";

                columnName[5].MappingName = "Xi_m_Xavg_x_Yi_m_YAvg";
                columnName[5].HeaderText = $"(Xi-{(char)88}{(char)772})(Yi-{(char)562})";

                columnName[6].MappingName = "Xi_m_X_Avg_Squared";
                columnName[6].HeaderText = $"(Xi-{(char)88}{(char)772}){(char)178}";

                columnName[7].MappingName = "Yi_m_Y_Avg_Squared";
                columnName[7].HeaderText = $"(Yi-{(char)562}){(char)178}";

                columnName[8].MappingName = "Y_lineal";
                columnName[8].HeaderText = $"{(char)374}";

                columnName[9].MappingName = "Yi_m_Lineal_Yi_Squared";
                columnName[9].HeaderText = $"(Yi-{(char)374}){(char)178}";

                columnName[10].MappingName = "Lineal_Yi_m_Y_Avg_Squared";
                columnName[10].HeaderText = $"({(char)374}-{(char)562}){(char)178}";
                #endregion
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
            GraphModel model = MainActivity.hasIntervals&& MainActivity.SpinnerPos == (int)MainActivity.AnalysisType.OneDA ?
                new GraphModel(ODA.Xi, ExcerptFieldsActivity.Yi)
                : new GraphModel(ExcerptFieldsActivity.Xi,ExcerptFieldsActivity.Yi);
            
            if(MainActivity.SpinnerPos==(int)MainActivity.AnalysisType.OneDA)
            {
                SplineSeries graphLine = new SplineSeries()                         //https://help.syncfusion.com/xamarin-android/sfchart/getting-started
                {
                    DataSource = model.Data,
                    Color = Color.AntiqueWhite
                };
                graphLine.DataMarker.ShowMarker = true;
                graphLine.DataMarker.MarkerColor = Color.Turquoise;
                Graph.Series.Add(graphLine);
            }
            else if(MainActivity.SpinnerPos == (int)MainActivity.AnalysisType.RegrCorA)
            {
                ScatterSeries graphLine = new ScatterSeries()
                {
                    DataSource = model.Data,
                    Color = Color.DarkSeaGreen,
                    ScatterHeight = 25,
                    ScatterWidth = 25
                };
                Graph.Series.Add(graphLine);
            }

            #endregion
            
            
            layout.AddView(Graph);

        }

        #endregion

        #region Modify Action Bar Menu Buttons
        public override bool OnCreateOptionsMenu(IMenu menu)                    //https://www.youtube.com/watch?v=5MSKuVO2hV4
        {
            MenuInflater.Inflate(Resource.Menu.ActionBarIcons, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.exportToExcel:
                    ExportToExcel();
                    return true;
                case Resource.Id.resetBT:
                    StartActivity(new Intent(this, typeof(MainActivity)));
                    Finish();
                    return true;
                default: return false;
            }
        }
        #endregion

        #region Excel Exportation
        private void ExportToExcel()    // https://help.syncfusion.com/xamarin-android/sfdatagrid/exporting
        {
            DataGridExcelExportingController excelExport = new DataGridExcelExportingController();
            var excelEngine = excelExport.ExportToExcel(TableData, new DataGridExcelExportingOption()
            {
                ExportRowHeight    = false,
                ExportColumnWidth  = false,
                DefaultColumnWidth = 100,
                DefaultRowHeight   = 60
            });
            var workbook = excelEngine.Excel.Workbooks[0];
            MemoryStream stream = new MemoryStream();
            workbook.SaveAs(stream);
            workbook.Close();
            excelEngine.Dispose();
            Save($"{(MainActivity.SpinnerPos == (int)MainActivity.AnalysisType.OneDA ? "OneDA" : "RegrCorA")}_Table",
                "application/msexcel", stream, TableData.Context);
        }

        public void Save(string fileName, String contentType, MemoryStream stream,Context context)
        {
            string exception = string.Empty;
            string root = null;
            if (Android.OS.Environment.IsExternalStorageEmulated)
                root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            else
                root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            Java.IO.File myDir = new Java.IO.File(root + "/PocketStatistician");
            myDir.Mkdir();

            Java.IO.File file = new Java.IO.File(myDir, fileName+".xlsx");

            int fileIndent = 1; 
            while(file.Exists())
            {
                string newFileName = string.Concat(fileName, fileIndent, ".xlsx");
                file = new Java.IO.File(myDir, newFileName);
                fileIndent++;
            }

            try
            {
                FileOutputStream outs = new FileOutputStream(file, false);
                outs.Write(stream.ToArray());

                outs.Flush();
                outs.Close();
            }
            catch(Exception e)
            {
                exception = e.ToString();
            }
            if(file.Exists()&&contentType!="application/html")
            {
                Android.Net.Uri path = Android.Net.Uri.FromFile(file);
                string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
                string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(exception);
                Toast.MakeText(this, "Table successfully exported!", ToastLength.Short).Show();
            }
        }


        #endregion
    }
}