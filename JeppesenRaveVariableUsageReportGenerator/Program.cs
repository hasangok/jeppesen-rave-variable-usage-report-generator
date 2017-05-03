using JeppesenRaveVariableUsageReportGenerator.Model;
using JeppesenRaveVariableUsageReportGenerator.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JeppesenRaveVariableUsageReportGenerator
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.Run(new UI());
            }
            else
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Usage: JeppesenRaveVariableUsageReportGenerator [carmusr path]");
                    Environment.Exit(1);
                }

                string carmusrPath = args[0];

                Dictionary<string, VariableInfo> variableUsage = Helper.ExportVariablesFromModules(carmusrPath);
                Helper.GetVariableUsage(variableUsage, carmusrPath);
                Helper.WriteHTMLReport(variableUsage);
            }
        }
    }
}
