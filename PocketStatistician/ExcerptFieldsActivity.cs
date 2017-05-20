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
    public class ExcerptFieldsActivity : Activity
    {
        public static int NumberOfFields;
        public static double[][] intervals;
        public static double[] Xi, Yi;
        private bool continuable = true;
        private EditText[] Field;
        private Button ProcDataBT;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ExcerptFieldsLayout);

            var fieldsLayout = FindViewById<LinearLayout>(Resource.Id.fieldsLayout);

            ShowDialog("To ensure accurate calculations please enter the data as shown.");

            #region Declarations and predefinitions
            EditText[] field = new EditText[NumberOfFields];
            for (int i = 0; i < field.Length; i++)
            {
                field[i] = new EditText(this);
                fieldsLayout.AddView(field[i]);         //https://forums.xamarin.com/discussion/6029/how-to-create-ui-elements-dynamically 

                if (MainActivity.SpinnerPos == (int)MainActivity.AnalysisType.OneDA)
                    field[i].Hint = MainActivity.hasIntervals ? $"{i + 1}. X'~X\" Fi" :
                        $"{i + 1}. Xi Fi";
                else
                    field[i].Hint = $"{i + 1}. Xi Yi";
            }
            Field = field;
            var procDataBT = FindViewById<Button>(Resource.Id.procDataBT);
            ProcDataBT = procDataBT;
            #endregion

            #region Events
            procDataBT.Click += procDataBT_Click;
             
            #endregion
        }


        private void procDataBT_Click(object sender, EventArgs e)
        {
            #region Data assigning and format check
            string[] split = null;
            if (MainActivity.hasIntervals && MainActivity.SpinnerPos == (int)MainActivity.AnalysisType.OneDA)                       // This is used only for the One-Dimention Analysis. Other analysis work with two parameters.
            {
                intervals = new double[2][];
                intervals[0] = new double[Field.Length];
                intervals[1] = new double[Field.Length];

                split = new string[3];
                Yi = new double[Field.Length];
            }
            else
            {
                split = new string[2];
                Xi = new double[Field.Length];
                Yi = new double[Field.Length];
            }

            for (int i = 0; i < Field.Length; i++)
            {
                if (MainActivity.hasIntervals && MainActivity.SpinnerPos == (int)MainActivity.AnalysisType.OneDA)
                {
                    continuable = Field[i].Text.Contains("~") && Field[i].Text.Split().Length == 2;         // checking if the format is correct
                    if (!continuable)
                        break;

                    string[] twoStrings = Field[i].Text.Split();
                    split[0] = twoStrings[0].Split('~')[0];
                    split[1] = twoStrings[0].Split('~')[1];
                    split[2] = twoStrings[1];

                    continuable = double.TryParse(split[0], out intervals[0][i]) &&
                    double.TryParse(split[1], out intervals[1][i]) &&
                    double.TryParse(split[2], out Yi[i]);
                    if (!continuable)
                        break;
                }
                else
                {
                    continuable = Field[i].Text.Split().Length == 2;         // checking if the format is correct
                    if (!continuable)
                        break;

                    split[0] = Field[i].Text.Split()[0];
                    split[1] = Field[i].Text.Split()[1];

                    continuable = double.TryParse(split[0], out Xi[i]) && double.TryParse(split[1], out Yi[i]);
                    if (!continuable)
                        break;
                }

            }

            #endregion

            if (!continuable)
            {
                ShowDialog("Your data is wrongly formated.\nPlease check and try again.");
                return;
            }
            else
            {
                switch (MainActivity.SpinnerPos)
                {
                    case (int)MainActivity.AnalysisType.OneDA:
                        ResultActivity.ODA = MainActivity.hasIntervals ?
                        new Analizers.One_dim_analysis(Field.Length, intervals[0], intervals[1], Yi) :
                        new Analizers.One_dim_analysis(Field.Length, Xi, Yi);
                        break;

                    case (int)MainActivity.AnalysisType.RegrCorA:
                        ResultActivity.RA = new Analizers.Regression_analysis(Field.Length, Xi, Yi);
                        break;
                }

                var intent = new Intent(this, typeof(ResultActivity));
                StartActivity(intent);
                ((Activity)MainActivity.MainActivityContext).Finish();
                Finish();
                MainActivity.DisconnectEvents();
                ProcDataBT.Click -= procDataBT_Click;
            }
        }

        private void ShowDialog(string text)
        {
            var msgPop = new AlertDialog.Builder(this);
            msgPop.SetMessage(text);
            msgPop.SetNeutralButton("Ok", delegate { });

            msgPop.Show();
        }
    }
}