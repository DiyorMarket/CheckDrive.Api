using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Domain.ResourceParameters
{
    public class OilMarkResourceParameters : ResourceParametersBase
    {
        public override string OrderBy { get; set; } = "OilMark"; 
    }
}
