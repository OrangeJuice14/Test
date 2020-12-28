namespace HRMWebApp.ChamCong.Core.WebAPI.Models
{
    public class URMUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public bool Locked { get; set; }
        public string PW { get; set; }
        public string MicrosoftAccount { get; set; }
        public string GoogleAccount { get; set; }
        public string ActiveDirectory { get; set; }
        public int TypeID { get; set; }
        public int DepartmentID { get; set; }
        public string Department_ErpID { get; set; }

        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }

        public URMUser() { }
    }
}
