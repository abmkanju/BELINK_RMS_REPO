using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_User
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        //public string last_name { get; set; }
        //public string image_url { get; set; }
        public string first_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public int practice_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<m_userRole> user_practices { get; set; }

        //public string encrypted_password { get; set; }
        //public string reset_password_token { get; set; }
        //public DateTime reset_password_sent_at { get; set; }
        //public DateTime remember_created_at { get; set; }
        //public int sign_in_count { get; set; }
        //public DateTime current_sign_in_at { get; set; }
        //public DateTime last_sign_in_at { get; set; }
        //public string current_sign_in_ip { get; set; }
        //public string last_sign_in_ip { get; set; }
        //public string confirmation_token { get; set; }
        //public DateTime confirmed_at { get; set; }
        //public DateTime confirmation_sent_at { get; set; }
        //public string unconfirmed_email { get; set; }
        //public int failed_attempts { get; set; }
        //public string unlock_token { get; set; }
        //public DateTime locked_at { get; set; }
        //public bool is_admin { get; set; }
        //public string title { get; set; }
        //public string slug_fallback { get; set; }
        //public bool is_visible { get; set; }
        //public bool is_doctor { get; set; }
        //public bool is_verified { get; set; }
        //public int rating { get; set; }
        //public bool is_img { get; set; }
        //public DateTime? deleted_at { get; set; }
        //public DateTime schedule_time { get; set; }
        //public string pmdc { get; set; }
        //public bool ehr { get; set; }
        //public bool gender { get; set; }
        //public int sort_order { get; set; }
        //public bool is_ported { get; set; }
        //public string sms_phone { get; set; }
        //public int department_id { get; set; }
        //public bool is_token { get; set; }
        //public bool is_purchase_requisition { get; set; }
        //public bool print_payment { get; set; }
        //public bool phone_verified { get; set; }
        //public string phone_pin { get; set; }
        //public int hw_id { get; set; }
        //public DateTime birth_date { get; set; }
        //public int second_factor_attempts_count { get; set; }
        //public string encrypted_otp_secret_key { get; set; }
        //public string encrypted_otp_secret_key_iv { get; set; }
        //public string encrypted_otp_secret_key_salt { get; set; }
        //public string direct_otp { get; set; }
        //public DateTime direct_otp_sent_at { get; set; }
        //public DateTime totp_timestamp { get; set; }
        //public bool otp_login_required { get; set; }
        //public int platform { get; set; }
        //public int share_percentage { get; set; }
        //public string timezone { get; set; }
        //public bool video_consultations { get; set; }
        //public int schedule_type { get; set; }
        //public string signature { get; set; }
        //public bool print_signature { get; set; }
        //public bool send_operation_message { get; set; }
        //public string uuid { get; set; }
        //public int referral_percentage { get; set; }
        //public int sim_type { get; set; }
        //public int maximum_discount { get; set; }
        //public int radiology_referral_percentage { get; set; }     
        //public bool instant_booking { get; set; }
        //public string slug { get; set; }
        //public bool is_new { get; set; }
        //public bool is_update { get; set; }
        //public bool _destroy { get; set; }
    }
}