using System;
using Microsoft.AspNet.Identity;


namespace HRMWebApp.KPI.Core.Security
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, 
    //please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    
    public class ApplicationUser : IUser
    {
        public DateTime CreateDate { get; set; }
        public DateTime BirthDate { get; set; }

        public ApplicationUser()
        {
            CreateDate = DateTime.Now;           
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ThongTinNhanVien { get; set; }
        public string AgentObjectTypeId { get; set; }
        public string DepartmentId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HoVaTen { get; set; }
        public string WebGroupId { get; set; }
        public bool IsKPIs { get; set; }
        public string  SubPositionId { get; set; }
        public string DepartmentName { get; set; }
    }
 
}