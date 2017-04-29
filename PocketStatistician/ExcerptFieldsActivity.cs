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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ExcerptFieldsLayout);

            var fieldsLayout = FindViewById<LinearLayout>(Resource.Id.fieldsLayout);

            ShowDialog("To ensure accurate calculations please enter the data as shown.");

            #region Declarations and predefinitions
            EditText[] field = new EditText[NumberOfFields];
            for(int i=0;i<field.Length;i++)
            {
                field[i] = new EditText(this);
                fieldsLayout.AddView(field[i]);         //https://forums.xamarin.com/discussion/6029/how-to-create-ui-elements-dynamically 

                if (MainActivity.SpinnerPos==(int)MainActivity.AnalysisType.OneDA)
                {
                    field[i].Hint = MainActivity.hasPeriods ? $"{i + 1}. X'~X\" Fi" :
                        $"{i + 1}. Xi Fi";
                }
            }

            var procDataBT = FindViewById<Button>(Resource.Id.procDataBT);
            #endregion

            #region Events
            procDataBT.Click += delegate
             {
                 var intent = new Intent(this, typeof(ResultActivity));
                 StartActivity(intent);
             };
            #endregion
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