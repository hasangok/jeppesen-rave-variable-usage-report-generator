using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeppesenRaveVariableUsageReportGenerator.Model
{
    public class VariableInfo
    {
        public int UsageCount { get; set; }
        public List<string> FoundIn { get; set; }

        public VariableInfo(int usageCount, List<string> foundIn)
        {
            UsageCount = usageCount;
            FoundIn = foundIn;
        }
    }
}
