using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Analizers
{
    public class One_dim_analysis
    {
        #region Values
        bool hasIntervals = false;

        int lines = 2;
        public double[,] interval;

        double[] xI, fI, xIfI, comulatFreg, xIminusAvg, moduleXiminusAvg, squaredXiminusAvg, cubicXiminusAvg, quardicXiminusAvg;
        double xIfI_Sum = 0, fi_Sum = 0, moduleXiminusAvg_Sum = 0, squaredXiminusAvg_Sum, cubicXiminusAvg_Sum, quardicXiminusAvg_Sum;

        #region Properties
        public double[] Xi => xI;

        public double average    { set; get; }
        public double mode       { set; get; }
        public double median     { set; get; }

        public double scope        { set; get; }
        public double avgDeviation { set; get; }
        public double stDeviation  { set; get; }

        public double variation_scope        { set; get; }
        public double variation_avgDeviation { set; get; }
        public double variation_stDeviation  { set; get; }

        public double uM3           { set; get; }
        public double uM4           { set; get; }
        public double asymmetry_m   { set; get; }
        public double asymmetry_p   { set; get; }
        public double asymmetry_u   { set; get; }
        public double excess        { set; get; }

        public string[,] TableData;
        #endregion
        #endregion

        void CalculateValues()
        {
            #region Values Assigning
            xIfI                = new double[lines - 2];
            comulatFreg         = new double[lines - 2];
            xIminusAvg          = new double[lines - 2];
            moduleXiminusAvg    = new double[lines - 2];
            squaredXiminusAvg   = new double[lines - 2];
            cubicXiminusAvg     = new double[lines - 2];
            quardicXiminusAvg   = new double[lines - 2];
            #endregion

            #region Main Values
            for (int i = 0; i < lines - 2; i++)                      // To get the average values and everything else first xiFi is needed
            {
                xIfI[i] = xI[i] * fI[i];
                comulatFreg[i] = i == 0 ? fI[i] : comulatFreg[i - 1] + fI[i];
                xIfI_Sum += xIfI[i];
                fi_Sum += fI[i];
            }
            average = Round(xIfI_Sum / fi_Sum, 3);

            for (int i = 0; i < lines - 2; i++)
            {
                xIminusAvg[i] = Round((xI[i] - average), 3);

                moduleXiminusAvg[i] = Abs(xIminusAvg[i] * fI[i]);
                moduleXiminusAvg_Sum += moduleXiminusAvg[i];

                squaredXiminusAvg[i] = Round(Pow(xIminusAvg[i], 2) * fI[i], 3);
                squaredXiminusAvg_Sum += squaredXiminusAvg[i];

                cubicXiminusAvg[i] = Round(Pow(xIminusAvg[i], 3) * fI[i], 3);
                cubicXiminusAvg_Sum += cubicXiminusAvg[i];

                quardicXiminusAvg[i] = Round(Pow(xIminusAvg[i], 4) * fI[i], 3);
                quardicXiminusAvg_Sum += quardicXiminusAvg[i];
            }
            #endregion

            #region Mode
            int temp = 0;
            double v = 0;
            for (int i = 1; i < xIfI.Length; i++)
                if (v < fI[i])
                {
                    v = fI[i];
                    temp = i;
                }
            switch (hasIntervals)
            {
                case true:
                    try
                    {
                        mode = Round(interval[temp, 0] + (((fI[temp] - fI[temp - 1]) * (interval[temp, 1] - interval[temp, 0])) /
                                        ((fI[temp] - fI[temp - 1]) + (fI[temp] - fI[temp + 1]))), 2);
                        break;
                    }
                    catch (IndexOutOfRangeException)
                    {

                        mode = 0;
                        break;
                    }

                case false:
                    mode = xI[temp]; break;
            }
            #endregion

            #region Median
            double Nme = (fi_Sum + 1) / 2;

            switch (hasIntervals)
            {
                case false:
                    for (int i = 0; i < comulatFreg.Length; i++)
                        if (Nme < comulatFreg[i])
                        {
                            temp = i;
                            break;
                        }
                    median = xI[temp];
                    break;
                case true:
                    for (int i = 0; i < comulatFreg.Length; i++)
                        if (Nme < comulatFreg[i])
                        {
                            temp = i;
                            break;
                        }
                    median = Round(interval[temp, 0] + (Nme - comulatFreg[temp - 1]) * (interval[temp, 1] - interval[temp, 0]) / fI[temp], 2);
                    break;
            }

            #endregion

            #region scope
            double maxValue = 0, minValue = Double.MaxValue;
            for (int i = 0; i < xIfI.Length; i++)
            {
                maxValue = xIfI[i] > maxValue ? xI[i] : maxValue;
                minValue = xIfI[i] < minValue ? xI[i] : minValue;
            }
            scope = maxValue - minValue;
            #endregion

            avgDeviation = Round(moduleXiminusAvg_Sum / fi_Sum, 2);
            stDeviation = Round(Sqrt(squaredXiminusAvg_Sum / fi_Sum), 2);

            variation_scope = Round(scope / average * 100, 2);
            variation_avgDeviation = Round(avgDeviation / average * 100, 2);
            variation_stDeviation = Round(stDeviation / average * 100, 2);

            uM3 = Round(cubicXiminusAvg_Sum / fi_Sum, 3);
            uM4 = Round(quardicXiminusAvg_Sum / fi_Sum, 3);
            asymmetry_m = Round(uM3 / Pow(stDeviation, 3), 3);
            asymmetry_p = Round(3 * (average - median) / stDeviation, 3);
            asymmetry_u = Round((average - mode) / stDeviation, 3);
            excess = Round(uM4 / Pow(stDeviation, 4), 2);

            SendToData();
        }

        void SendToData()
        {
            TableData = new string[lines, hasIntervals ? 11 : 10];
            switch (hasIntervals)
            {
                #region Has Intervals
                case true:

                    #region Head
                    TableData[0, 0] = $"{(char)8470}";       // №
                    TableData[0, 1] = "Interval";
                    TableData[0, 2] = "Xi";
                    TableData[0, 3] = "Fi";
                    TableData[0, 4] = "XiFi";
                    TableData[0, 5] = "C";
                    TableData[0, 6] = "Xi -x";
                    TableData[0, 7] = "|Xi -x|Fi";
                    TableData[0, 8] = $"(Xi -x)^2Fi";
                    TableData[0, 9] = $"(Xi -x)^3Fi";
                    TableData[0, 10] = $"(Xi -x)^4Fi";
                    #endregion

                    #region body
                    for (int i = 1; i < TableData.GetLength(0) - 1; i++)
                        for (int j = 0; j < TableData.GetLength(1); j++)
                        {
                            switch (j)
                            {
                                case 0:
                                    TableData[i, j] = $"{i}"; break;
                                case 1:
                                    TableData[i, j] = $"{interval[i - 1, 0]} - {interval[i - 1, 1]}"; break;
                                case 2:
                                    TableData[i, j] = xI[i - 1].ToString(); break;
                                case 3:
                                    TableData[i, j] = fI[i - 1].ToString(); break;
                                case 4:
                                    TableData[i, j] = xIfI[i - 1].ToString(); break;
                                case 5:
                                    TableData[i, j] = comulatFreg[i - 1].ToString(); break;
                                case 6:
                                    TableData[i, j] = xIminusAvg[i - 1].ToString(); break;
                                case 7:
                                    TableData[i, j] = moduleXiminusAvg[i - 1].ToString(); break;
                                case 8:
                                    TableData[i, j] = squaredXiminusAvg[i - 1].ToString(); break;
                                case 9:
                                    TableData[i, j] = cubicXiminusAvg[i - 1].ToString(); break;
                                case 10:
                                    TableData[i, j] = quardicXiminusAvg[i - 1].ToString(); break;
                            }
                        }
                    #endregion

                    #region legs
                    TableData[lines - 1, 0] = "-";
                    TableData[lines - 1, 1] = "Total";
                    TableData[lines - 1, 2] = $"-";
                    TableData[lines - 1, 3] = $"{fi_Sum}";
                    TableData[lines - 1, 4] = $"{xIfI_Sum}";
                    TableData[lines - 1, 5] = "-";
                    TableData[lines - 1, 6] = "-";
                    TableData[lines - 1, 7] = $"{moduleXiminusAvg_Sum}";
                    TableData[lines - 1, 8] = $"{squaredXiminusAvg_Sum}";
                    TableData[lines - 1, 9] = $"{cubicXiminusAvg_Sum}";
                    TableData[lines - 1, 10] = $"{quardicXiminusAvg_Sum}";
                    #endregion
                    break;
                #endregion

                #region Doesn't have intervals
                case false:
                    #region Head
                    TableData[0, 0] = $"{(char)8470}";       // №
                    TableData[0, 1] = "Xi";
                    TableData[0, 2] = "Fi";
                    TableData[0, 3] = "XiFi";
                    TableData[0, 4] = "C";
                    TableData[0, 5] = "Xi - x";
                    TableData[0, 6] = "|Xi - x|Fi";
                    TableData[0, 7] = $"(Xi -x)^2Fi";
                    TableData[0, 8] = $"(Xi -x)^3Fi";
                    TableData[0, 9] = $"(Xi -x)^4Fi";
                    #endregion

                    #region body
                    for (int i = 1; i < TableData.GetLength(0) - 1; i++)
                        for (int j = 0; j < TableData.GetLength(1); j++)
                            switch (j)
                            {
                                case 0:
                                    TableData[i, j] = $"{i}";                                     break;
                                case 1:
                                    TableData[i, j] = xI[i - 1].ToString();                       break;
                                case 2:
                                    TableData[i, j] = fI[i - 1].ToString();                       break;
                                case 3:
                                    TableData[i, j] = xIfI[i - 1].ToString();                     break;
                                case 4:
                                    TableData[i, j] = comulatFreg[i - 1].ToString();              break;
                                case 5:
                                    TableData[i, j] = xIminusAvg[i - 1].ToString();               break;
                                case 6:
                                    TableData[i, j] = moduleXiminusAvg[i - 1].ToString();         break;
                                case 7:
                                    TableData[i, j] = squaredXiminusAvg[i - 1].ToString();        break;
                                case 8:
                                    TableData[i, j] = Round(cubicXiminusAvg[i - 1],3).ToString(); break;
                                case 9:
                                    TableData[i, j] = quardicXiminusAvg[i - 1].ToString();        break;
                            }
                        
                    #endregion

                    #region legs
                    TableData[lines - 1, 0] = "-";
                    TableData[lines - 1, 1] = "Total";
                    TableData[lines - 1, 2] = $"{fi_Sum}";
                    TableData[lines - 1, 3] = $"{xIfI_Sum}";
                    TableData[lines - 1, 4] = "-";
                    TableData[lines - 1, 5] = "-";
                    TableData[lines - 1, 6] = $"{moduleXiminusAvg_Sum}";
                    TableData[lines - 1, 7] = $"{squaredXiminusAvg_Sum}";
                    TableData[lines - 1, 8] = $"{cubicXiminusAvg_Sum}";
                    TableData[lines - 1, 9] = $"{quardicXiminusAvg_Sum}";
                    #endregion
                    break;
                    #endregion
            }
        }

        string GenerateConclusions()
        {
            string Asymmetry = $"Pierson's Asymmetry coefficient shows that the distribution has {WriteAsymmetryState(asymmetry_p)} view and {(Abs(asymmetry_p) < 1 ? "moderate" : "considerable")} state of asymmetry\n\n" +
                $"Jul's Asymmetry coefficient shows that the distribution has {WriteAsymmetryState(asymmetry_u)} view and {(Abs(asymmetry_u) < 1 ? "moderate" : "considerable")} state of asymmetry\n\n" +
                $"The Central Asymmetry coefficient shows that the distribution has {WriteAsymmetryState(asymmetry_m)} view and {(Abs(asymmetry_m) < 0.5 ? "moderate" : "considerable")} state of asymmetry\n";

            string Excess = $"The excess in this distribution is {(excess-3 < 0 ? "flattened" : "convexed")}";

            return string.Concat(Asymmetry, "\n", Excess);
        }

        string WriteAsymmetryState(double coef)
        {
            if (coef < 0)
                return "left-handed asymmetrical";
            else if (coef > 0)
                return "right-handed asymmetrical";
            else
                return "symmetrical";
        }

        #region Public methods and constructors

        public string DisplayFullData() => string.Concat(
            $"Average: {average}\n" ,
            $"Mode: {mode}\n" +
            $"Median: {median}\n\n" ,

            $"Scope: {scope}\n" ,
            $"Average deviation: {avgDeviation}\n" ,
            $"Standart deviation: {stDeviation}\n\n" ,

            $"Scope variation: {variation_scope}%\n" ,
            $"Average Deviation variation {variation_avgDeviation}%\n" ,
            $"Standart Deviation variation: {variation_stDeviation}%\n\n" ,

            $"uM3: {uM3}\n" ,
            $"uM4: {uM4}\n\n" ,

            $"Asymmetry_m: {asymmetry_m}\n" ,
            $"Asymmetry_p: {asymmetry_p}\n" ,
            $"Asymmetry_u: {asymmetry_u}\n" ,
            $"Excess: {excess-3}\n\n",
            $"Conclusions:\n{GenerateConclusions()}");

        public One_dim_analysis(int lines, double[] xI, double[] fI)
        {
            this.lines += lines;
            this.xI = xI;
            this.fI = fI;
            CalculateValues();
        }

        public One_dim_analysis(int lines, double[] interval1, double[] interval2, double[] fI)
        {
            hasIntervals = true;
            interval = new double[lines, 2];
            xI = new double[lines];
            this.lines += lines;

            for (int i = 0; i < lines; i++)
                interval[i, 0] = interval1[i];
            for (int j = 0; j < lines; j++)
                interval[j, 1] = interval2[j];

            for (int i = 0; i < lines; i++)
                xI[i] = Round((interval[i, 0] + interval[i, 1]) / 2, 2);

            this.fI = fI;
            CalculateValues();
        }

        #endregion
    }
}
