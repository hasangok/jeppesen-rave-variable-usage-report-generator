using JeppesenRaveVariableUsageReportGenerator.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JeppesenRaveVariableUsageReportGenerator
{
    public static class Helper
    {
        //regex to skip non alphanumeric files
        private static Regex fileNameRegex = new Regex("^[a-zA-Z0-9._]*$");
        //regex to catch rave variables
        private static Regex variableRegex = new Regex("([A-z])\\w+[.]\\%(.*?)\\%|\\%(.*?)\\%");

        public static Dictionary<string, VariableInfo> ExportVariablesFromModules(string carmusrPath)
        {
            Dictionary<string, VariableInfo> variableUsage = new Dictionary<string, VariableInfo>();

            //get module files from carmusr directory
            string[] moduleFiles = Directory.GetFiles(string.Format("{0}/crc/modules", carmusrPath), "*.*", SearchOption.AllDirectories).Where(path => !fileNameRegex.IsMatch(path) && !path.Contains('~')).ToArray();

            //iterate files to extract variables as <module_name>.<variable_name>
            foreach (string file in moduleFiles)
            {
                //get file content
                string content = File.ReadAllText(file);

                //match variables                
                MatchCollection matches = variableRegex.Matches(content);

                //iterate all matches
                for (int i = 0; i < matches.Count; i++)
                {
                    string variableName = FormatVariableName(file, matches[i].ToString());

                    //skip non alphanumeric variable names
                    if (!fileNameRegex.IsMatch(variableName))
                    {
                        continue;
                    }

                    //add variable to dict
                    if (!variableUsage.ContainsKey(variableName))
                    {
                        VariableInfo variableInfo = new VariableInfo(0, new List<string>() { });
                        variableUsage.Add(variableName, variableInfo);
                    }
                }
            }

            return variableUsage;
        }

        public static Dictionary<string, VariableInfo> GetVariableUsage(Dictionary<string, VariableInfo> variableUsage, string carmusrPath)
        {
            //get all files from carmusr directory
            string[] carmusrFiles = Directory.GetFiles(carmusrPath, "*.*", SearchOption.AllDirectories).Where(path => !fileNameRegex.IsMatch(path) && !path.Contains('~')).ToArray();

            //iterate files to extract variables as <module_name>.<variable_name>
            foreach (string file in carmusrFiles)
            {
                Console.Write(string.Format("Processing {0,-50}\t\t", Path.GetFileNameWithoutExtension(file)));

                //get file content
                string content = File.ReadAllText(file);

                //match variables
                MatchCollection matches = variableRegex.Matches(content);

                Console.WriteLine("Found " + matches.Count + " matches");

                //iterate all matches
                for (int i = 0; i < matches.Count; i++)
                {
                    string variableName = FormatVariableName(file, matches[i].ToString());

                    //skip non alphanumeric variable names
                    if (!fileNameRegex.IsMatch(variableName))
                    {
                        continue;
                    }

                    if (variableUsage.ContainsKey(variableName))
                    {
                        //increment usage count
                        variableUsage[variableName].UsageCount++;
                        if (!variableUsage[variableName].FoundIn.Contains(file.Replace("\\", "/")))
                        {
                            variableUsage[variableName].FoundIn.Add(file.Replace("\\", "/"));
                        }
                    }

                }

            }

            return variableUsage;
        }

        public static void WriteHTMLReport(Dictionary<string, VariableInfo> variableUsage)
        {
            //generate html file
            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<table border='1'><tr><th>#</th><th>Variable</th><th>Usage Count</th><th>Found In</th></tr>");
            int count = 0;
            foreach (var item in variableUsage.OrderByDescending(p => p.Value.UsageCount))
            {
                Console.WriteLine(string.Format("Adding {0, -5}", item.Key));
                count++;
                htmlContent.Append("<tr valign='top'>");
                htmlContent.AppendFormat("<td>{0}</td>", count);
                htmlContent.AppendFormat("<td>{0}</td>", item.Key);
                htmlContent.AppendFormat("<td>{0}</td>", item.Value.UsageCount);
                htmlContent.Append("<td>");
                foreach (string foundIn in item.Value.FoundIn)
                {
                    htmlContent.AppendFormat("{0}<br />", foundIn);
                }
                htmlContent.Append("</td>");
                htmlContent.Append("</tr>");
            }
            htmlContent.Append("</table>");

            //write to file
            StreamWriter streamWriter = new StreamWriter("variableusage.html");
            streamWriter.WriteLine(htmlContent.ToString());
            streamWriter.Close();
        }

        public static string FormatVariableName(string file, string variableName)
        {
            variableName = variableName.Replace("%", "");

            if (!variableName.Contains("."))
            {
                variableName = string.Format("{0}.{1}", Path.GetFileNameWithoutExtension(file), variableName);
            }

            return variableName;
        }
    }
}
