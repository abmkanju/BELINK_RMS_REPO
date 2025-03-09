using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_Patient
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string gender { get; set; }
        public DateTime? birth_date { get; set; }
        public int practice_id { get; set; }
        //public bool manual { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string patient_id { get; set; }
        public string phone_office { get; set; }
        public string phone_home { get; set; }
        public int? age_y { get; set; }
        public int? age_m { get; set; }
        public int? age_d { get; set; }
        public bool is_admin_created { get; set; } // default  : true
        public int remaining_balance { get; set; } // discuss with ali
        public string animal { get; set; } // nill
        public string breed { get; set; }// nill
        public string pet_name { get; set; } // nill
        public string occupation { get; set; } // nill
        public string referred_by { get; set; } // send refferred by text field data when creatring patient
        //public DateTime follow_up { get; set; } // nill
        public int biannual { get; set; } // nill
        public string doctor_name { get; set; } // send doctor name  :  select doctor name from drop down
        //public string pt_signature { get; set; } // nill
        //public string dr_signature { get; set; } // nill
        public bool is_ported { get; set; } // nill
        public string treatment { get; set; } // nill
        public DateTime date { get; set; } // registration date :  just like stock date
        //public string field_name { get; set; } // nill
        public string father_name { get; set; } // nill
        public string father_occupation { get; set; } // nill
        public string mother_name { get; set; } // nill
        public string mother_occupation { get; set; } // nill
        //public int insurance_id { get; set; } // nill
        //public string insurance_number { get; set; } // nill
        //public string medical_history { get; set; } // nill
        public DateTime? deleted_at { get; set; } 
        public string cnic { get; set; } 
        public string country_code_1 { get; set; }  //+92
        public string country_code_2 { get; set; } //+92
        public string country_code_3 { get; set; } //+92
        //public bool follow_up_read { get; set; } //nill
        //public bool is_insurance { get; set; } //nill
        //public int insurance_holder { get; set; } //nill
        //public int family_member_relation { get; set; } //nill
        //public int insurance_holder_id { get; set; } //nill
        //public int user_id { get; set; }
        //public string employer_name { get; set; } //nill
        //public int employee_id { get; set; } // nill
        //public int qb_id { get; set; } // discuss with habib
        //public bool english_sms_preference { get; set; } //nill
        //public string blood_group { get; set; } // add field
        //public DateTime follow_up_notification { get; set; } //nill
        public int status { get; set; } // nill
        public int which_type { get; set; } // default : 0
        public string registration_no { get; set; } // nill
        //public string designation { get; set; } // nill
        //public string department { get; set; } // nill
        //public float annual_allowance { get; set; } // nill
        public int patient_type { get; set; } // default : 0  
        //public DateTime date_of_death { get; set; } //nill
        //public string cause_of_death { get; set; } // nill
        //public bool deceased { get; set; } //nill
        //public string employee_name { get; set; } //nill
        public int blood_type { get; set; } // enum : create field
        //public int created_by_id { get; set; } // nill
        public int pending_dues { get; set; } // discuss with ali
        //public int visit_id { get; set; }         // nill
        //public bool is_new { get; set; }
        //public bool is_update { get; set; }
        //public bool _destroy { get; set; }
    }
}
