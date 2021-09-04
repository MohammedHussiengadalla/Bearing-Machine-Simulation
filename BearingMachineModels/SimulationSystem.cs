using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearingMachineModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            DelayTimeDistribution = new List<TimeDistribution>();
            BearingLifeDistribution = new List<TimeDistribution>();

            CurrentSimulationTable = new List<CurrentSimulationCase>();
            CurrentPerformanceMeasures = new PerformanceMeasures();

            ProposedSimulationTable = new List<ProposedSimulationCase>();
            ProposedPerformanceMeasures = new PerformanceMeasures();
        }

        void FillCummPropOFDelayTimeDistribution()
        {
            /*fill cummulative probability collumn of DelayTimeDistribution  table*/
            this.DelayTimeDistribution[0].CummProbability = this.DelayTimeDistribution[0].Probability;
            for(int i=1; i < this.DelayTimeDistribution.Count();i++)
            {
                this.DelayTimeDistribution[i].CummProbability = this.DelayTimeDistribution[i].Probability + this.DelayTimeDistribution[i-1].CummProbability;
            }
        } 
        void FillCummPropOFBearingLifeDistribution()
        {
            /*fill cummulative probability collumn of BearingLifeDistribution  table*/
            this.BearingLifeDistribution[0].CummProbability = this.BearingLifeDistribution[0].Probability;
            for (int i = 1; i < this.BearingLifeDistribution.Count(); i++)
            {
                this.BearingLifeDistribution[i].CummProbability = this.BearingLifeDistribution[i].Probability + this.BearingLifeDistribution[i - 1].CummProbability;
            }

        }
        private void FillMinMaxRangeOfDelayTimeDistribution()
        {
            /*fill MaxRange and MinRange collumns of DelayTimeDistribution  table*/
            this.DelayTimeDistribution[0].MinRange = 1;
            this.DelayTimeDistribution[0].MaxRange =(int)(this.DelayTimeDistribution[0].CummProbability*100);
            for (int i = 1; i < this.DelayTimeDistribution.Count; i++)
            {
                this.DelayTimeDistribution[i].MinRange = this.DelayTimeDistribution[i - 1].MaxRange + 1;
                this.DelayTimeDistribution[i].MaxRange = (int)(this.DelayTimeDistribution[i].CummProbability * 100);
            }
        }
        private void FillMinMaxRangeOfBearingLifeDistribution()
        {
            /*fill MaxRange and MinRange collumns of BearingLifeDistribution  table*/
            this.BearingLifeDistribution[0].MinRange = 1;
            this.BearingLifeDistribution[0].MaxRange = (int)(this.BearingLifeDistribution[0].CummProbability * 100);
            for (int i = 1; i < this.BearingLifeDistribution.Count; i++)
            {
                this.BearingLifeDistribution[i].MinRange = this.BearingLifeDistribution[i - 1].MaxRange + 1;
                this.BearingLifeDistribution[i].MaxRange = (int)(this.BearingLifeDistribution[i].CummProbability * 100);
            }
        }
        
        Random random = new Random();
        private int Generate_Random()
        {
            /*genetate Rondom number*/
            return random.Next(1, 101);
        }
        public void Initialize()
        {
            /*Intialization for Simulation System Values*/
            this.FillCummPropOFBearingLifeDistribution();
            this.FillCummPropOFDelayTimeDistribution();
            this.FillMinMaxRangeOfDelayTimeDistribution();
            this.FillMinMaxRangeOfBearingLifeDistribution();
        }
        private int GetTime(int rondomNum, List<TimeDistribution> TimeDist)
        {
            /*return the (delay/life) hours based on rondom nymber generated*/
            int Hours=-1;
            for (int i = 0; i < TimeDist.Count; i++)
            {
                if (rondomNum >= TimeDist[i].MinRange && rondomNum <= TimeDist[i].MaxRange)
                {
                    Hours = TimeDist[i].Time;
                    break;
                }
            }
            return Hours;
        }
        private List<CurrentSimulationCase> Generate_BearingCases_CurSim(int BearingIndex)
        {
            /*generate bearing cases for each bearing*/
            List<CurrentSimulationCase> CurSim_BearingCases = new List<CurrentSimulationCase>();
            CurrentSimulationCase CurCase=new CurrentSimulationCase();
            int index=1;
            CurCase.Bearing.Index = BearingIndex;
            int rand= Generate_Random();
            CurCase.Bearing.RandomHours = rand;
            CurCase.Bearing.Hours = GetTime(CurCase.Bearing.RandomHours, this.BearingLifeDistribution);
            CurCase.RandomDelay = Generate_Random();
            CurCase.Delay = GetTime(CurCase.RandomDelay, this.DelayTimeDistribution);
            CurCase.AccumulatedHours = CurCase.Bearing.Hours;
            CurSim_BearingCases.Add(CurCase);


            while (CurCase.AccumulatedHours < this.NumberOfHours)
            {
                CurCase = new CurrentSimulationCase();
                CurCase.Bearing.Index = BearingIndex;
                CurCase.Bearing.RandomHours = Generate_Random();
                CurCase.Bearing.Hours = GetTime(CurCase.Bearing.RandomHours, this.BearingLifeDistribution);
                CurCase.RandomDelay = Generate_Random();
                CurCase.Delay = GetTime(CurCase.RandomDelay, this.DelayTimeDistribution);
                CurCase.AccumulatedHours = CurCase.Bearing.Hours + CurSim_BearingCases[index - 1].AccumulatedHours;
                CurSim_BearingCases.Add(CurCase);
                index++;
            }

            return CurSim_BearingCases;
        }

        private void arrangeSimTable()
        {
            Int32 MinVal, MinValIndx;
            List<CurrentSimulationCase> TembTable = new List<CurrentSimulationCase>();
            int Count = CurrentSimulationTable.Count;
            for (int j = 0; j < Count; j++)
            {
                MinVal = CurrentSimulationTable[0].AccumulatedHours;
                MinValIndx = 0;
                for (int i = 1; i < this.CurrentSimulationTable.Count; i++)
                {
                    if (CurrentSimulationTable[i].AccumulatedHours < MinVal)
                    {
                        MinVal = CurrentSimulationTable[i].AccumulatedHours;
                        MinValIndx = i;
                    }
                }
                TembTable.Add(this.CurrentSimulationTable[MinValIndx]);
                CurrentSimulationTable.RemoveAt(MinValIndx);
            }
            this.CurrentSimulationTable = TembTable;
        }

        /*set the First Failur Cullomn in proposed simulation Table*/
        private void SetFirstFailur()
        {
            for(int i=0; i<ProposedSimulationTable.Count; i++)
            {
                int FirstFailur = ProposedSimulationTable[i].Bearings[0].Hours;
                for(int j=1; j<ProposedSimulationTable[i].Bearings.Count; j++)
                {
                    if (FirstFailur > ProposedSimulationTable[i].Bearings[j].Hours)
                        FirstFailur = ProposedSimulationTable[i].Bearings[j].Hours;
                }
                ProposedSimulationTable[i].FirstFailure = FirstFailur;
            }
        }
        /*set the Accumulatied life Cullomn in proposed simulation Table*/
        private void SetAccuLife()
        {
            this.ProposedSimulationTable[0].AccumulatedHours = this.ProposedSimulationTable[0].FirstFailure;
            for(int i = 1; i < this.ProposedSimulationTable.Count; i++)
            {
                this.ProposedSimulationTable[i].AccumulatedHours = this.ProposedSimulationTable[i - 1].AccumulatedHours + this.ProposedSimulationTable[i].FirstFailure;
            }
        }

        /*Expend propsed simulation table*/
        private void Expend()
        {
            ProposedSimulationCase PropCase;
            //Loop from Min to Max

            int i = MinSizeOfCasesOf_N_Bearings;
            while (this.ProposedSimulationTable[this.ProposedSimulationTable.Count-1].AccumulatedHours<this.NumberOfHours)
            {
                PropCase = new ProposedSimulationCase();
                for (int j = 0; j < CasesOf_N_Bearings.Count; j++)
                {
                    if (i < (CasesOf_N_Bearings[j].Count))
                    {
                        PropCase.Bearings.Add(CasesOf_N_Bearings[j][i].Bearing);
                    }
                    else
                    {
                        Bearing newBearing = new Bearing();
                        newBearing.RandomHours = Generate_Random();
                        newBearing.Hours = GetTime(newBearing.RandomHours, this.BearingLifeDistribution);
                        newBearing.Index = j+1;
                        PropCase.Bearings.Add(newBearing);
                    }
                }
                i++;
                //set first failur
                int FirstFailur = PropCase.Bearings[0].Hours;
                for(int idx=1; idx< PropCase.Bearings.Count; idx++)
                {
                    if (PropCase.Bearings[idx].Hours < FirstFailur)
                        FirstFailur = PropCase.Bearings[idx].Hours;
                }
                PropCase.FirstFailure = FirstFailur;
                //set Accumulated Life
                PropCase.AccumulatedHours = this.ProposedSimulationTable[this.ProposedSimulationTable.Count - 1].AccumulatedHours + PropCase.FirstFailure;
                this.ProposedSimulationTable.Add(PropCase);
              
            }
           
        }

        /*sett rondom Delay and Delay in Proposed Simulation Table*/
        private void SetRondomDelayAndDelay_PropSimTable()
        {
            for(int i=0; i<ProposedSimulationTable.Count; i++)
            {
                this.ProposedSimulationTable[i].RandomDelay = Generate_Random();
                this.ProposedSimulationTable[i].Delay = GetTime(this.ProposedSimulationTable[i].RandomDelay, this.DelayTimeDistribution);
            }
        }
        ///////////// Generating OUTPUTS /////////////

        //This List Of Lists Needed In expresses Simulation 
        List<List<CurrentSimulationCase>> CasesOf_N_Bearings=new List<List<CurrentSimulationCase>>();
        private int getMinSizeOfCassesLists()
        {
            
            int Min=CasesOf_N_Bearings[0].Count;
            //find Min
            for(int i=1; i < CasesOf_N_Bearings.Count; i++) {
                if (CasesOf_N_Bearings[i].Count < Min)
                    Min = CasesOf_N_Bearings[i].Count;
            }
            return Min;
        }

        public void Fill_CurrentSimulationTable()
        {
            /*Fill Current Simulation Table*/
            List<CurrentSimulationCase> CasesOfOneBearing;
            for (int i = 0; i < this.NumberOfBearings; i++) {
                CasesOfOneBearing = new List<CurrentSimulationCase>();
                CasesOfOneBearing = Generate_BearingCases_CurSim(i + 1);
                CasesOf_N_Bearings.Add(CasesOfOneBearing);
                this.CurrentSimulationTable.AddRange(CasesOfOneBearing);
            }
            this.arrangeSimTable();
        }


        int MinSizeOfCasesOf_N_Bearings;
        public void Fill_ProposedSimulationTable()
        {
           
            MinSizeOfCasesOf_N_Bearings = getMinSizeOfCassesLists();
            ProposedSimulationCase PropCase;
            //Loop on Min value
            for(int i = 0; i < MinSizeOfCasesOf_N_Bearings; i++)
            {
                PropCase = new ProposedSimulationCase();
                for(int j=0; j<CasesOf_N_Bearings.Count; j++)
                {
                    PropCase.Bearings.Add(CasesOf_N_Bearings[j][i].Bearing);
                }
                this.ProposedSimulationTable.Add(PropCase);
            }
            SetFirstFailur();
            SetAccuLife();
            if (this.ProposedSimulationTable[this.ProposedSimulationTable.Count - 1].AccumulatedHours < this.NumberOfHours)
                Expend();
            SetRondomDelayAndDelay_PropSimTable();

        }


#region Performance Measeres Functions
        private int CalcBearingCost(List<ProposedSimulationCase> PropSimTable)
        {
            return (PropSimTable.Count *this.NumberOfBearings) * this.BearingCost;
        }
        private int CalcBearingCost(List<CurrentSimulationCase> CurSimTable)
        {
            return CurSimTable.Count * this.BearingCost;
        }
        public class DelayInfo
        {
            public DelayInfo()
            {

            }
            public DelayInfo(int TotalDelay, int DelayCost) {
                this.TotalDelay = TotalDelay;
                this.DelayCost = DelayCost;
            }
            public int TotalDelay;
            public int DelayCost;
        }
        private DelayInfo CalcDelayInf(List<ProposedSimulationCase> PropSimTable)
        {
            int TotalDelay = 0;
            for(int i=0;i<PropSimTable.Count; i++)
            {
                TotalDelay += PropSimTable[i].Delay;
            }
            int DelayCost = TotalDelay * this.DowntimeCost;

            return new DelayInfo(TotalDelay, DelayCost);
        }
        private DelayInfo CalcDelayInf(List<CurrentSimulationCase> CurSimTable)
        {
            int TotalDelay = 0;
            for (int i = 0; i < CurSimTable.Count; i++)
            {
                TotalDelay += CurSimTable[i].Delay;
            }
            int DelayCost = TotalDelay * this.DowntimeCost;

            return new DelayInfo(TotalDelay, DelayCost);
        }

        private int CalcDownTimeCost(List<CurrentSimulationCase> CurSimTable)
        {
            return (CurSimTable.Count * this.RepairTimeForOneBearing) * this.DowntimeCost;
        }
        private int CalcDownTimeCost(List<ProposedSimulationCase> PropSimTable)
        {
            return (PropSimTable.Count * this.RepairTimeForAllBearings) * this.DowntimeCost;
        }

        private Decimal CalcRepairPersomCost(List<ProposedSimulationCase> PropSimTable) {
            Decimal temb = (PropSimTable.Count * this.RepairTimeForAllBearings)* this.RepairPersonCost;
            Decimal Cost = temb/ 60;
            return Cost;
        }
        private Decimal CalcRepairPersomCost(List<CurrentSimulationCase> CurSimTable)
        {
            Decimal temb = (CurSimTable.Count * this.RepairTimeForOneBearing) * (this.RepairPersonCost);
            Decimal Cost = temb / 60;
            return Cost;
        }
        public DelayInfo DelayInfoOfPropSimTable = new DelayInfo();
        public void CalcProposedPerformanceMeasures()
        {
            this.ProposedPerformanceMeasures.BearingCost = CalcBearingCost(this.ProposedSimulationTable);
            DelayInfoOfPropSimTable = CalcDelayInf(this.ProposedSimulationTable);
            this.ProposedPerformanceMeasures.DelayCost = DelayInfoOfPropSimTable.DelayCost;
            this.ProposedPerformanceMeasures.DowntimeCost = CalcDownTimeCost(this.ProposedSimulationTable);
            this.ProposedPerformanceMeasures.RepairPersonCost = CalcRepairPersomCost(this.ProposedSimulationTable);
            this.ProposedPerformanceMeasures.TotalCost = this.ProposedPerformanceMeasures.BearingCost +
                                                         this.ProposedPerformanceMeasures.DelayCost +
                                                         this.ProposedPerformanceMeasures.DowntimeCost +
                                                         this.ProposedPerformanceMeasures.RepairPersonCost;
        }
        public DelayInfo DelayInfoOfCurSimTable = new DelayInfo();
        public void CalcCurrentPerformanceMeasures()
        {
            this.CurrentPerformanceMeasures.BearingCost = CalcBearingCost(this.CurrentSimulationTable);
            DelayInfoOfCurSimTable = CalcDelayInf(this.CurrentSimulationTable);
            this.CurrentPerformanceMeasures.DelayCost = DelayInfoOfCurSimTable.DelayCost;
            this.CurrentPerformanceMeasures.DowntimeCost = CalcDownTimeCost(this.CurrentSimulationTable);
            this.CurrentPerformanceMeasures.RepairPersonCost = CalcRepairPersomCost(this.CurrentSimulationTable);
            this.CurrentPerformanceMeasures.TotalCost = this.CurrentPerformanceMeasures.BearingCost +
                                                         this.CurrentPerformanceMeasures.DelayCost +
                                                         this.CurrentPerformanceMeasures.DowntimeCost +
                                                         this.CurrentPerformanceMeasures.RepairPersonCost;
        }
        #endregion

        ///////////// INPUTS /////////////
        public int DowntimeCost { get; set; }
        public int RepairPersonCost { get; set; }
        public int BearingCost { get; set; }
        public int NumberOfHours { get; set; }
        public int NumberOfBearings { get; set; }
        public int RepairTimeForOneBearing { get; set; }
        public int RepairTimeForAllBearings { get; set; }
        public List<TimeDistribution> DelayTimeDistribution { get; set; }
        public List<TimeDistribution> BearingLifeDistribution { get; set; }

        ///////////// OUTPUTS /////////////
        public List<CurrentSimulationCase> CurrentSimulationTable { get; set; }
        public PerformanceMeasures CurrentPerformanceMeasures { get; set; }
        public List<ProposedSimulationCase> ProposedSimulationTable { get; set; }
        public PerformanceMeasures ProposedPerformanceMeasures { get; set; }
    }
}
