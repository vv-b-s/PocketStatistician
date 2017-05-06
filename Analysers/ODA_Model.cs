using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Analizers
{
    public class ODA_Model_NO_INTERVALS
    {
        public string Nm                        { set; get; }
        public string Xi                        { set; get; }
        public string Fi                        { set; get; }
        public string XiFi                      { set; get; }
        public string ComulatFreg               { set; get; }
        public string Xi_minus_Avg              { set; get; }
        public string Module_Xi_minus_Avg       { set; get; }
        public string Squared_Xi_minus_Avg      { set; get; }
        public string Cubix_Xi_minus_Avg        { set; get; }
        public string Quadric_Xi_minus_Avg      { set; get; }

        public ODA_Model_NO_INTERVALS(string[] dataFromTable)
        {
            Nm                   = dataFromTable[0];
            Xi                   = dataFromTable[1];
            Fi                   = dataFromTable[2];
            XiFi                 = dataFromTable[3];
            ComulatFreg          = dataFromTable[4];
            Xi_minus_Avg         = dataFromTable[5];
            Module_Xi_minus_Avg  = dataFromTable[6];
            Squared_Xi_minus_Avg = dataFromTable[7];
            Cubix_Xi_minus_Avg   = dataFromTable[8];
            Quadric_Xi_minus_Avg = dataFromTable[9];
        }
    }

    public class ODA_Model_WITH_INTERVALS
    {
        public string Nm                    { set; get; }
        public string Intervals             { set; get; }
        public string Xi                    { set; get; }
        public string Fi                    { set; get; }
        public string XiFi                  { set; get; }
        public string ComulatFreg           { set; get; }
        public string Xi_minus_Avg          { set; get; }
        public string Module_Xi_minus_Avg   { set; get; }
        public string Squared_Xi_minus_Avg  { set; get; }
        public string Cubix_Xi_minus_Avg    { set; get; }
        public string Quadric_Xi_minus_Avg  { set; get; }

        public ODA_Model_WITH_INTERVALS(string[] dataFromTable)
        {
            Nm                   = dataFromTable[0];
            Intervals            = dataFromTable[1];
            Xi                   = dataFromTable[2];
            Fi                   = dataFromTable[3];
            XiFi                 = dataFromTable[4];
            ComulatFreg          = dataFromTable[5];
            Xi_minus_Avg         = dataFromTable[6];
            Module_Xi_minus_Avg  = dataFromTable[7];
            Squared_Xi_minus_Avg = dataFromTable[8];
            Cubix_Xi_minus_Avg   = dataFromTable[9];
            Quadric_Xi_minus_Avg = dataFromTable[10];
        }
    }

    public class ODA_GridRepository
    {
        public ObservableCollection <ODA_Model_NO_INTERVALS>   oda_model_no_intervals   { set; get; }
        public ObservableCollection <ODA_Model_WITH_INTERVALS> oda_model_with_intervals { set; get; }

        public ODA_GridRepository(bool hasIntervals, string[,] data)
        {
            if(hasIntervals)
            {
                oda_model_with_intervals = new ObservableCollection<ODA_Model_WITH_INTERVALS>();
                for(int i=1;i<data.GetLength(0);i++)
                {
                    string[] dataArray = new string[data.GetLength(1)];
                    for (int j = 0; j < data.GetLength(1); j++)
                        dataArray[j] = data[i, j];
                    oda_model_with_intervals.Add(new ODA_Model_WITH_INTERVALS(dataArray));
                }
                    
            }
            else
            {
                oda_model_no_intervals = new ObservableCollection<ODA_Model_NO_INTERVALS>();
                for (int i = 1; i < data.GetLength(0); i++)
                {
                    string[] dataArray = new string[data.GetLength(1)];
                    for (int j = 0; j < data.GetLength(1); j++)
                        dataArray[j] = data[i, j];
                    oda_model_no_intervals.Add(new ODA_Model_NO_INTERVALS(dataArray));
                }
            }
            
        }
    }
}