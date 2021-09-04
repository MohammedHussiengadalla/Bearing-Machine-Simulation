using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BearingMachineTesting;
using BearingMachineModels;

namespace BearingMachineSimulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        SimulationSystem SimSys;
        private void Form_load(object Sender, EventArgs e)
        {

            SimSys = new SimulationSystem();
            string FileName = @"E:\1_ST 4YEAR\SIMULATION\task\Task 3_Simulation\[Students]_Template\BearingMachineSimulation\TestCases\TestCase3.txt";
            SimSys=FileReader.read(FileName);
            SimSys.Initialize();
            SimSys.Fill_CurrentSimulationTable();
            SimSys.Fill_ProposedSimulationTable();
            SimSys.CalcProposedPerformanceMeasures();
            SimSys.CalcCurrentPerformanceMeasures();
            string TestMsg = TestingManager.Test(SimSys, Constants.FileNames.TestCase3);
            MessageBox.Show(TestMsg);
            ShowCurTable(SimSys);
            ShowPropTable(SimSys);
           
        }

        void ShowCurTable(SimulationSystem SimSys)
        {
            DataGridView view = dataGridView1;
            view.Rows.Clear();
            view.ColumnCount = 7;
            view.Columns[0].Name = "Case";
            view.Columns[1].Name = "Bearing Index";
            view.Columns[2].Name = "Random Number For Life";
            view.Columns[3].Name = "Life";
            view.Columns[4].Name = "Accumulated Life";
            view.Columns[5].Name = "Random Number For Delay";
            view.Columns[6].Name = "Delay";

            for(int i=0; i < SimSys.CurrentSimulationTable.Count; i++)
            {
                string[] Row = new string[] {

                    (i+1).ToString(),
                    SimSys.CurrentSimulationTable[i].Bearing.Index.ToString(),
                    SimSys.CurrentSimulationTable[i].Bearing.RandomHours.ToString(),
                    SimSys.CurrentSimulationTable[i].Bearing.Hours.ToString(),
                    SimSys.CurrentSimulationTable[i].AccumulatedHours.ToString(),
                    SimSys.CurrentSimulationTable[i].RandomDelay.ToString(),
                    SimSys.CurrentSimulationTable[i].Delay.ToString(),

                };
                view.Rows.Add(Row);

            }
            CostOfBearings_txt.Text = SimSys.CurrentPerformanceMeasures.BearingCost.ToString();
            CostOfDelay_txt.Text    = SimSys.CurrentPerformanceMeasures.DelayCost.ToString();
            DowntimeCost_txt.Text = SimSys.CurrentPerformanceMeasures.DowntimeCost.ToString();
            Costofrepairpersons_txt.Text = SimSys.CurrentPerformanceMeasures.RepairPersonCost.ToString();
            TotalCost_txt.Text = SimSys.CurrentPerformanceMeasures.TotalCost.ToString();
            TotalDelay_txt.Text = SimSys.DelayInfoOfCurSimTable.TotalDelay.ToString();

        


        }

        void ShowPropTable(SimulationSystem SimSys)
        {
            DataGridView view = dataGridView3;
            view.Rows.Clear();
            view.ColumnCount = 4+SimSys.NumberOfBearings;
            view.Columns[0].Name = "Case";
            int j;
            for ( j=0; j<SimSys.NumberOfBearings; j++)
            {
                view.Columns[j+1].Name = string.Format("Bearing {0} Life",j+1);
            }
            
            view.Columns[j+1].Name = "Accumulated Life";
            view.Columns[j+2].Name = "Random Number For Delay";
            view.Columns[j+3].Name = "Delay";

            for (int i = 0; i < SimSys.ProposedSimulationTable.Count; i++)
            {
                string[] PropRow =new string[view.ColumnCount];
                PropRow[0] = (i + 1).ToString();
                int x;
                for (x = 0; x < SimSys.NumberOfBearings; x++)
                {
                    PropRow[x + 1] = SimSys.ProposedSimulationTable[i].Bearings[x].Hours.ToString();
                }
                PropRow[x + 1] = SimSys.ProposedSimulationTable[i].AccumulatedHours.ToString();
                PropRow[x + 2] = SimSys.ProposedSimulationTable[i].RandomDelay.ToString();
                PropRow[x + 3] = SimSys.ProposedSimulationTable[i].Delay.ToString();

                view.Rows.Add(PropRow);
            }


        }

        private void Policy_btn_Click(object sender, EventArgs e)
        {
            if(Policy_btn.Text== "Current Policy")
            {
                Policy_btn.Text = "Proposed Policy";
                dataGridView3.Show();
                // set Performance Measure 
                CostOfBearings_txt.Text = SimSys.ProposedPerformanceMeasures.BearingCost.ToString();
                CostOfDelay_txt.Text = SimSys.ProposedPerformanceMeasures.DelayCost.ToString();
                DowntimeCost_txt.Text = SimSys.ProposedPerformanceMeasures.DowntimeCost.ToString();
                Costofrepairpersons_txt.Text = SimSys.ProposedPerformanceMeasures.RepairPersonCost.ToString();
                TotalCost_txt.Text = SimSys.ProposedPerformanceMeasures.TotalCost.ToString();
                TotalDelay_txt.Text = SimSys.DelayInfoOfPropSimTable.TotalDelay.ToString();
            }
            else
            {
                Policy_btn.Text = "Current Policy";
                dataGridView3.Hide();
                CostOfBearings_txt.Text = SimSys.CurrentPerformanceMeasures.BearingCost.ToString();
                CostOfDelay_txt.Text = SimSys.CurrentPerformanceMeasures.DelayCost.ToString();
                DowntimeCost_txt.Text = SimSys.CurrentPerformanceMeasures.DowntimeCost.ToString();
                Costofrepairpersons_txt.Text = SimSys.CurrentPerformanceMeasures.RepairPersonCost.ToString();
                TotalCost_txt.Text = SimSys.CurrentPerformanceMeasures.TotalCost.ToString();
                TotalDelay_txt.Text = SimSys.DelayInfoOfCurSimTable.TotalDelay.ToString();
            }
            
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
