using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizers
{
    public class RA_Model
    {
        public string Nm                        { set; get; }
        public string Xi                        { set; get; }
        public string Yi                        { set; get; }
        public string Xi_squared                { set; get; }
        public string XiYi                      { set; get; }
        public string Xi_m_Xavg_x_Yi_m_YAvg     { set; get; }
        public string Xi_m_X_Avg_Squared        { set; get; }
        public string Yi_m_Y_Avg_Squared        { set; get; }
        public string Y_lineal                  { set; get; } 
        public string Yi_m_Lineal_Yi_Squared    { set; get; }
        public string Lineal_Yi_m_Y_Avg_Squared { set; get; }

        public RA_Model(string[] dataFromTable)
        {
            Nm                        = dataFromTable[0];
            Xi                        = dataFromTable[1];
            Yi                        = dataFromTable[2];
            Xi_squared                = dataFromTable[3];
            XiYi                      = dataFromTable[4];
            Xi_m_Xavg_x_Yi_m_YAvg     = dataFromTable[5];
            Xi_m_X_Avg_Squared        = dataFromTable[6];
            Yi_m_Y_Avg_Squared        = dataFromTable[7];
            Y_lineal                  = dataFromTable[8];
            Yi_m_Lineal_Yi_Squared    = dataFromTable[9];
            Lineal_Yi_m_Y_Avg_Squared = dataFromTable[10];
        }
    }

    public class RA_GridRepository
    {
        public ObservableCollection<RA_Model> ra_model { set; get; }

        public RA_GridRepository(string[,] data)
        {
            ra_model = new ObservableCollection<RA_Model>();
            for(int i=0;i<data.GetLength(0);i++)
            {
                string[] dataArray = new string[data.GetLength(1)];
                for (int j = 0; j < data.GetLength(1); j++)
                    dataArray[j] = data[i, j];
                ra_model.Add(new RA_Model(dataArray));
            }

        }
    }
}
