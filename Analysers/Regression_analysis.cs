using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using MathNet.Numerics.Distributions;

namespace Analizers
{
    public class Regression_analysis
    {
        #region Values
        public double[] 
            Xi, Yi, 
            Xi_squared, 
            XiYi, 
            Xi_m_Xavg_x_Yi_m_YAvg, 
            Xi_m_X_Avg_Squared, 
            Yi_m_Y_Avg_Squared, 
            Y_lineal, 
            Yi_m_Lineal_Yi_Squared, 
            Lineal_Yi_m_Y_Avg_Squared;

        double
            SUM_X=0,
            SUM_Y=0,
            SUM_Xi_squared = 0,
            SUM_XiYi = 0,
            SUM_Xi_m_Xavg_x_Yi_m_YAvg = 0,
            SUM_Xi_m_X_Avg_Squared = 0,
            SUM_Yi_m_Y_Avg_Squared = 0,
            SUM_Y_lineal = 0,
            SUM_Yi_m_Lineal_Yi_Squared = 0,
            SUM_Lineal_Yi_m_Y_Avg_Squared = 0;

        int lines = 1;
        #endregion

        #region Properties
        public double B0            { set; get; }
        public double B1            { set; get; }
        public double Avg_X         { set; get; }
        public double Avg_Y         { set; get; }
        public double Determ_Coef   { set; get; }
        public double Indeterm_Coef { set; get; }
        public double Corelation_P  { set; get; }
        public double Corelation_B  { set; get; }
        public double Standard_dev  { set; get; }
        public double Dispersion_1  { set; get; }
        public double Dispersion_2  { set; get; }
        public double F_em          { set; get; }
        public double[] F_t         { set; get; }

        public string[,] TableData;
        #endregion

        void CalculateValues()
        {
            #region Values Assigning
            Xi_squared                   = new double[lines - 1];
            XiYi                         = new double[lines - 1];
            Xi_m_Xavg_x_Yi_m_YAvg        = new double[lines - 1];
            Xi_m_X_Avg_Squared           = new double[lines - 1];
            Yi_m_Y_Avg_Squared           = new double[lines - 1];
            Y_lineal                     = new double[lines - 1];
            Yi_m_Lineal_Yi_Squared       = new double[lines - 1];
            Lineal_Yi_m_Y_Avg_Squared    = new double[lines - 1];
            #endregion

            for(int i=0;i<lines-1;i++)
            {
                Xi_squared[i] = Pow(Xi[i], 2);
                XiYi[i] = Xi[i] * Yi[i];

                SUM_X += Xi[i];
                SUM_Y += Yi[i];
                SUM_Xi_squared += Xi_squared[i];
                SUM_XiYi += XiYi[i];
            }
            Avg_X = SUM_X / (lines - 1);
            Avg_Y = SUM_Y / (lines - 1);

            B1 = ((lines - 1) * SUM_XiYi - SUM_X * SUM_Y) / ((lines - 1) * SUM_Xi_squared - Pow(SUM_X, 2));
            B0 = SUM_Y / (lines - 1) - B1 * SUM_X / (lines - 1);

            for (int i=0;i<lines-1;i++)
            {
                Xi_m_Xavg_x_Yi_m_YAvg[i] = (Xi[i] - Avg_X) * (Yi[i] - Avg_Y);
                Xi_m_X_Avg_Squared[i] = Pow(Xi[i] - Avg_X, 2);
                Yi_m_Y_Avg_Squared[i] = Pow(Yi[i] - Avg_Y, 2);
                Y_lineal[i] = B0 + B1 * Xi[i];
                Yi_m_Lineal_Yi_Squared[i] = Pow(Yi[i] - Y_lineal[i], 2);
                Lineal_Yi_m_Y_Avg_Squared[i] = Pow(Y_lineal[i] - Avg_Y, 2);

                SUM_Xi_m_Xavg_x_Yi_m_YAvg += Xi_m_Xavg_x_Yi_m_YAvg[i];
                SUM_Xi_m_X_Avg_Squared += Xi_m_X_Avg_Squared[i];
                SUM_Yi_m_Y_Avg_Squared += Yi_m_Y_Avg_Squared[i];
                SUM_Y_lineal += Y_lineal[i];
                SUM_Yi_m_Lineal_Yi_Squared += Yi_m_Lineal_Yi_Squared[i];
                SUM_Lineal_Yi_m_Y_Avg_Squared += Lineal_Yi_m_Y_Avg_Squared[i];
            }

            Determ_Coef = SUM_Lineal_Yi_m_Y_Avg_Squared / SUM_Yi_m_Y_Avg_Squared;
            Indeterm_Coef = 1 - Determ_Coef;
            Corelation_P = Sqrt(Determ_Coef);
            Corelation_B = SUM_Xi_m_Xavg_x_Yi_m_YAvg / Sqrt(SUM_Xi_m_X_Avg_Squared * SUM_Yi_m_Y_Avg_Squared);
            Standard_dev = Sqrt(SUM_Yi_m_Lineal_Yi_Squared / (lines - 3));
            Dispersion_1 = SUM_Lineal_Yi_m_Y_Avg_Squared;
            Dispersion_2 = SUM_Yi_m_Lineal_Yi_Squared / (lines - 3);
            F_em = Dispersion_1 / Dispersion_2;
            F_t = new double[2];
            F_t[0] = FisherSnedecor.InvCDF(1, lines - 3, 0.95);
            F_t[1] = (lines == 4) ? 4052 : FisherSnedecor.InvCDF(1, lines - 3, 0.99);

            SendToData();
        }

        void SendToData()
        {
            TableData = new string[lines, 11];

            #region Body
            for(int i=0;i<TableData.GetLength(0)-1;i++)
                for(int j=0;j<TableData.GetLength(1);j++)
                    switch(j)
                    {
                        case 0:
                            TableData[i, j] = $"{i + 1}"; break;
                        case 1:
                            TableData[i, j] = Xi[i].ToString(); break;
                        case 2:
                            TableData[i, j] = Yi[i].ToString(); break;
                        case 3:
                            TableData[i, j] = Round(Xi_squared[i],3).ToString(); break;
                        case 4:
                            TableData[i, j] = Round(XiYi[i],3).ToString(); break;
                        case 5:
                            TableData[i, j] = Round(Xi_m_Xavg_x_Yi_m_YAvg[i],3).ToString(); break;
                        case 6:
                            TableData[i, j] = Round(Xi_m_X_Avg_Squared[i],3).ToString(); break;
                        case 7:
                            TableData[i, j] = Round(Yi_m_Y_Avg_Squared[i], 3).ToString(); break;
                        case 8:
                            TableData[i, j] = Round(Y_lineal[i], 3).ToString(); break;
                        case 9:
                            TableData[i, j] = Round(Yi_m_Lineal_Yi_Squared[i], 3).ToString(); break;
                        case 10:
                            TableData[i, j] = Round(Lineal_Yi_m_Y_Avg_Squared[i],3).ToString(); break;
                    }
            #endregion

            #region Legs
            TableData[lines - 1, 0] = $"{(char)931}";
            TableData[lines - 1, 1] = Round(SUM_X, 3).ToString();
            TableData[lines - 1, 2] = Round(SUM_Y, 3).ToString();
            TableData[lines - 1, 3] = Round(SUM_Xi_squared, 3).ToString();
            TableData[lines - 1, 4] = Round(SUM_XiYi, 3).ToString();
            TableData[lines - 1, 5] = Round(SUM_Xi_m_Xavg_x_Yi_m_YAvg, 3).ToString();
            TableData[lines - 1, 6] = Round(SUM_Xi_m_X_Avg_Squared, 3).ToString();
            TableData[lines - 1, 7] = Round(SUM_Yi_m_Y_Avg_Squared, 3).ToString();
            TableData[lines - 1, 8] = Round(SUM_Y_lineal, 3).ToString();
            TableData[lines - 1, 9] = Round(SUM_Yi_m_Lineal_Yi_Squared, 3).ToString();
            TableData[lines - 1, 10] = Round(SUM_Lineal_Yi_m_Y_Avg_Squared, 3).ToString();
            #endregion
        }

        private string GenerateConclusions()
        {
            string B1_Conclusion = $"The regression coefficient B1 shows that increasing Xi with 1 unit will {(B1 >= 0 ? "increase" : "decrease")} Yi with {Round(Abs(B1), 2)} units.\n";
            string Determination_Conclusion = $"The determination coefficient shows that {Round(Determ_Coef * 100)}% of the changes in Yi are due to the changes in Xi.\n";
            string Indetermination_Conclusion = $"The indetermination coefficient shows that {Round(Indeterm_Coef * 100)}% of the changes in Yi are due to factors beyond the reach of this exerpt.\n";

            string corelationSterngth;
            if (Corelation_P <= 0.3)
                corelationSterngth = "weak";
            else if (Corelation_P > 0.3 && Corelation_P <= 0.5)
                corelationSterngth = "moderate";
            else if (Corelation_P > 0.5 && Corelation_P <= 0.7)
                corelationSterngth = "considerable";
            else if (Corelation_P > 0.7 && Corelation_P <= 0.9)
                corelationSterngth = "strong";
            else
                corelationSterngth = "very strong";

            string Corelation_Conclusion = $"Pierson and Brave's corelation coefficients show that the corelation is {corelationSterngth} and {(Corelation_B >= 0 ? "straightforward" : "reversed")}.\n";
            string Adequacy = $"With risk of deviation {(char)945} = 0.05, it can be argued that the excerpt is {(F_t[0] >= F_em ? "inadequate" : "adequate")}.\n" +
                $"With risk of deviation {(char)945} = 0.01, it can be argued that the excerpt is {(F_t[1] >= F_em ? "inadequate" : "adequate")}.\n";

            return string.Concat("Conclusions:\n", B1_Conclusion, "\n", Determination_Conclusion, Indetermination_Conclusion, "\n", Corelation_Conclusion, "\n", Adequacy);
        }

        #region Public Methods and constructors
        public string DisplayFullData() => string.Concat(
            $"{(char)88}{(char)772}: {Round(Avg_X, 3)}\n",
            $"{(char)562}: {Round(Avg_Y, 3)}\n\n",

            $"b{(char)8320}: {Round(B0, 3)}\n",
            $"b{(char)8321}: {Round(B1, 3)}\n",
            $"{(char)374} = {Round(B0, 3)} {(B1 >= 0 ? "+" : "-")} {Abs(Round(B1, 3))}Xi\n\n",

            $"Determination Coefficient: {Round(Determ_Coef, 3)}\n",
            $"Indetermination Coefficient: {Round(Indeterm_Coef, 3)}\n\n",

            $"Corelation_P: {Round(Corelation_P, 3)}\n",
            $"Corelation_B: {Round(Corelation_B, 3)}\n\n",

            $"Standard Deviation: {Round(Standard_dev, 3)}\n",
            $"{(char)963}{(char)8321}: {Round(Dispersion_1, 3)}\n",
            $"{(char)963}{(char)8322}: {Round(Dispersion_2, 3)}\n",
            $"Fem = {(char)963}1/{(char)963}2 = {Round(F_em, 3)}\n",
            $"Ft ({(char)945} = 0.05) = {Round(F_t[0], 3)}\n",
            $"Ft ({(char)945} = 0.01) = {Round(F_t[1], 3)}\n\n",

            $"df{(char)8321}: p-1 = 1\n",
            $"df{(char)8322}: n-p = {lines - 3}\n\n",
            GenerateConclusions());

        public Regression_analysis(int lines, double[] Xi,double[] Yi)
        {
            this.Xi = Xi;
            this.Yi = Yi;
            this.lines += lines;
            CalculateValues();
        }
        #endregion
    }
}
