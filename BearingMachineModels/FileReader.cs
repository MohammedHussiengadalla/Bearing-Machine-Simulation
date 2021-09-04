using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BearingMachineModels
{
    enum KeyWords
    {
        DowntimeCost, RepairPersonCost, BearingCost, NumberOfHours,
        NumberOfBearings, RepairTimeForOneBearing, RepairTimeForAllBearings,
        DelayTimeDistribution, BearingLifeDistribution,
    };
    public static class FileReader
    {
        public static SimulationSystem read(string FileName)
        {
           
            StreamReader SR = new StreamReader(FileName);
            SimulationSystem SimSys = new SimulationSystem();
            string CurLine = "";
            
            do
            {
                CurLine = SR.ReadLine();
                if(CurLine =="")
                    continue;
                else if (CurLine == KeyWords.DowntimeCost.ToString())
                {
                    CurLine = SR.ReadLine();
                    SimSys.DowntimeCost = Convert.ToInt32(CurLine);
                }
                else if (CurLine == KeyWords.RepairPersonCost.ToString())
                {
                    CurLine = SR.ReadLine();
                    SimSys.RepairPersonCost = Convert.ToInt32(CurLine);
                }
                else if (CurLine == KeyWords.BearingCost.ToString())
                {
                    CurLine = SR.ReadLine();
                    SimSys.BearingCost = Convert.ToInt32(CurLine);
                }
                else if (CurLine == KeyWords.NumberOfHours.ToString())
                {
                    CurLine = SR.ReadLine();
                    SimSys.NumberOfHours = Convert.ToInt32(CurLine);
                }
                else if (CurLine == KeyWords.NumberOfBearings.ToString())
                {
                    CurLine = SR.ReadLine();
                    SimSys.NumberOfBearings = Convert.ToInt32(CurLine);
                }
                else if (CurLine == KeyWords.RepairTimeForOneBearing.ToString())
                {
                    CurLine = SR.ReadLine();
                    SimSys.RepairTimeForOneBearing = Convert.ToInt32(CurLine);
                }
                else if (CurLine == KeyWords.RepairTimeForAllBearings.ToString())
                {
                    CurLine = SR.ReadLine();
                    SimSys.RepairTimeForAllBearings = Convert.ToInt32(CurLine);
                }
                else if (CurLine == KeyWords.DelayTimeDistribution.ToString())
                {
                    TimeDistribution TimDist_Row;
                    string[] CurLine_splited;
                    CurLine = SR.ReadLine();
                    while (CurLine!="" && CurLine != null)
                    {
                        CurLine_splited = CurLine.Split(',');
                        TimDist_Row = new TimeDistribution();
                        TimDist_Row.Time = Convert.ToInt32(CurLine_splited[0].Replace(" ",""));
                        TimDist_Row.Probability = Convert.ToDecimal(CurLine_splited[1].Replace(" ", ""));
                        SimSys.DelayTimeDistribution.Add(TimDist_Row);
                        CurLine = SR.ReadLine();
                    } 
                    
                }  
                else if (CurLine == KeyWords.BearingLifeDistribution.ToString())
                {
                    TimeDistribution TimDist_Row;
                    string[] CurLine_splited;
                    CurLine = SR.ReadLine();
                    while (CurLine!="" && CurLine != null)
                    {

                        CurLine_splited = CurLine.Split(',');
                        TimDist_Row = new TimeDistribution();
                        TimDist_Row.Time = Convert.ToInt32(CurLine_splited[0]);
                        TimDist_Row.Probability = Convert.ToDecimal(CurLine_splited[1]);
                        SimSys.BearingLifeDistribution.Add(TimDist_Row);
                        CurLine = SR.ReadLine();
                    } 
                }
                

            } while (CurLine != null);

            return SimSys;

        }

    }
}
