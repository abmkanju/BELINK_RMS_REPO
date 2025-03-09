using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AdminPrintFormatSetting
    {
        public int AdminPrintFormatSettingId { get; set; }
        public int HeaderFooterPref { get; set; }
        public int PatientDetailsPref { get; set; }
        public string TermsConditions { get; set; }
        public int PrintFormatIndex { get; set; }

    }
}