using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Transactions;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
      
        public IEnumerable<DTO_QuanLyViPham_Find> QuanLyViPham_Find(String publicKey, String token,int ngay, int thang, int nam, Guid boPhanId)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_QuanLyViPham_Factory factory = new CC_QuanLyViPham_Factory();
                IEnumerable<DTO_QuanLyViPham_Find> list = null;
                list = factory.FindForQuanLyViPham(ngay,thang, nam, boPhanId);
                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_QuanLyViPham_Find> QuanLyViPham_FindXemBangChamCong(String publicKey, String token, int ngay, int thang, int nam, Guid boPhanId,Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_QuanLyViPham_Factory factory = new CC_QuanLyViPham_Factory();
                IEnumerable<DTO_QuanLyViPham_Find> list = null;
                list = factory.FindForQuanLyViPhamXemBangChamCong(ngay, thang, nam, boPhanId, idNhanVien);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String QuanLyViPham_Find_Json(String publicKey, String token,int ngay, int thang, int nam, Guid boPhanId)
        {//DANG SD
            IEnumerable<DTO_QuanLyViPham_Find> list = QuanLyViPham_Find(publicKey, token, ngay, thang, nam, boPhanId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyViPham_FindXemBangChamCong_Json(String publicKey, String token, int ngay, int thang, int nam, Guid boPhanId, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_QuanLyViPham_Find> list = QuanLyViPham_FindXemBangChamCong(publicKey, token, ngay, thang, nam, boPhanId,idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public bool QuanLyViPham_GuiMail(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                for (int i=1;i<=10;i++)
                {
                    string to = "kekkashi3105@gmail.com"; //To address    
                string from = "ute.test.test@gmail.com"; //From address    
                MailMessage message = new MailMessage(from, to);

                string mailbody = "<span style='display: block; background: red; '>In this article you will learn how to send a email using Asp.Net & C#</span>";
                message.Subject = "Thông báo";
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential("ute.test.test@gmail.com", "ute@2016");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(message);
                }

                catch (Exception ex)
                {
                    return false;
                }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
