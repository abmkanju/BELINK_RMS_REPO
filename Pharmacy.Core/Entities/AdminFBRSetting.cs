using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AdminFBRSetting
    {
        public int AdminFBRSettingId { get; set; }
        public string POSID { get; set; }
        public string Tocken { get; set; }

        [NotMapped]
        public int POSIDEnc
        {
            get 
            {
                if (!string.IsNullOrEmpty(POSID))
                    return SharedFunctions.StringToInt((CryptopEngine.Decrypt(POSID, GlobalSharing.Salt)));
                else
                    return  0; 
            }
        }

        [NotMapped]
        public string  TockenEnc
        {
            get 
            {
                if (!string.IsNullOrEmpty(Tocken))
                    return SharedFunctions.ObjectToString((CryptopEngine.Decrypt(Tocken, GlobalSharing.Salt)));
                else
                    return  ""; 
            }
        }

    }
}
