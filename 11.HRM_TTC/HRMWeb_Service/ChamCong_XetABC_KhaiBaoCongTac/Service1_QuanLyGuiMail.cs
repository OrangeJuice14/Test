using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Transactions;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public List<DTO_QuanLyGuiMail> QuanLyGuiEmail_Find(String publicKey, String token, DateTime tuNgay, DateTime denNgay,Guid idWebUser)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                List<DTO_QuanLyGuiMail> list = null;
                CC_EmailManager_Factory factory = CC_EmailManager_Factory.New();
                list = factory.GetDTOQuanLyGuiMail_ByTuNgayDenNgay(tuNgay, denNgay, idWebUser).ToList();
                //
                return list;
            }
            else
            {
                return null;
            }
        }
        public bool QuanLyGuiEmail_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {
            //
            List<CC_MailManager> objList = JsonConvert.DeserializeObject<List<CC_MailManager>>(jsonObjectList);
            return QuanLyGuiEmail_DeleteList(publicKey, token, objList);
        }
        public bool QuanLyGuiEmail_DeleteList(String publicKey, String token, List<CC_MailManager> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_EmailManager_Factory factory = CC_EmailManager_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_MailManager mail = factory.GetByID(obj.Oid);
                            if (mail != null)
                                factory.DeleteObject(mail);

                            //
                        }
                    }
                    //////////////
                    try
                    {
                        factory.SaveChanges();
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }


        public String QuanLyGuiEmail_Find_Json(String publicKey, String token, DateTime tuNgay, DateTime denNgay,Guid idWebUser)
        {
            List<DTO_QuanLyGuiMail> list = QuanLyGuiEmail_Find(publicKey, token, tuNgay, denNgay, idWebUser);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public bool QuanLyGuiEmail_SendMail(String publicKey, String token, string emailgui, string passgui, string emailnhan, string tieude, string noidung, string filename, Guid idWebUser, Guid congTy)
        {
            bool result = false;
            //
            if (Helper.TrustTest(publicKey, token))
            {
                //
                try
                {   //
                    bool sucess = false;
                    //Kiểm tra có bao nhiêu email nhận
                    string[] emailNhanList = emailnhan.Split(';');
                    //
                    if (emailNhanList.Count() > 0)
                    {
                        foreach (var item in emailNhanList) // Gửi nhiều email
                        {
                            //
                            sucess = GuiMail(tieude, noidung, emailgui, passgui, item.Trim(), filename,congTy);
                        }
                    }
                    //
                    if (sucess)
                    {
                        CC_EmailManager_Factory factory = new CC_EmailManager_Factory();
                        CC_MailManager sendMail = factory.CreateManagedObject();
                        sendMail.Oid = Guid.NewGuid();
                        sendMail.Title = tieude;
                        sendMail.Contents = noidung;
                        //
                        sendMail.SendEmail = emailgui;
                        sendMail.SendPass = passgui;
                        //
                        sendMail.ReceiverEmail = emailnhan;
                        sendMail.FileName = filename;
                        sendMail.SendDate = DateTime.Now.Date;
                        sendMail.IDWebUser = idWebUser;
                        //
                        factory.SaveChanges();
                        //
                        result = true;
                    }
                }
                catch (Exception ex) { return result; }
            }

            //
            return result;
        }

        private bool GuiMail(string tieude, string noidung, string emailgui, string passgui, string emailnhan, string filename, Guid congTy)
        {
            bool sucess = false;

            //Lấy email người gửi
            CC_CauHinhChamCong_Factory factory = CC_CauHinhChamCong_Factory.New();
            DTO_CC_CauHinhChamCong cauHinhChamCong = factory.GetCauHinhChamCongByCongTy(congTy).Map<DTO_CC_CauHinhChamCong>();
            if (cauHinhChamCong == null) return false;

            //
            //string email = cauHinhChamCong.EmailSender != "" ? cauHinhChamCong.EmailSender :  "pscerp@gmail.com";
            //string pass = cauHinhChamCong.PassSender != "" ? cauHinhChamCong.PassSender :  "pscvietnam";
            //
            var loginInfo = new NetworkCredential(emailgui, passgui);
            var msg = new System.Net.Mail.MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            //
            msg.To.Add(new MailAddress(emailnhan));
            //
            if (!string.IsNullOrEmpty(filename))
            {
                string path = HttpContext.Current.Server.MapPath("~/Uploads/" + filename);
                //
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(path);
                if (System.IO.File.Exists(path))
                {
                    attachment.Name = filename;
                    msg.Attachments.Add(attachment);
                }
            }
            //
            msg.From = new MailAddress(emailgui);
            //msg.To.Add(new MailAddress(tags.Text));
            msg.Subject = tieude;
            msg.Body = noidung;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            //smtpClient.Send(msg);
            try
            {
                smtpClient.Send(msg);
                sucess = true;
            }
            catch
            {
                try
                {
                    smtpClient.Send(msg);
                    sucess = true;
                }
                catch (Exception ex)
                {//
                    sucess = false;
                }
            }
            //
            return sucess;
        }

        public IEnumerable<DTO_WebMailTemplateType> QuanLyGuiEmail_FindMailTemplateType(String publicKey, String token)
        {
            try
            {
                IEnumerable<DTO_WebMailTemplateType> result = null;

                if (Helper.TrustTest(publicKey, token))
                {
                    result = WebMailTemplate_Factory.New().GetMailType().Map<DTO_WebMailTemplateType>();
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_QuanLyGuiMail/QuanLyGuiEmail_FindMailTemplateType", ex);
                throw ex;
            }
        }

        public DTO_WebMailTemplate QuanLyGuiEmail_FindMailTemplate(String publicKey, String token, Guid congTy, Guid loaiGuiMail)
        {
            try
            {
                DTO_WebMailTemplate result = null;

                if (Helper.TrustTest(publicKey, token))
                {
                    result = WebMailTemplate_Factory.New().GetByCongTyVaLoaiGuiMail(congTy, loaiGuiMail).Map<DTO_WebMailTemplate>();
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_QuanLyGuiMail/QuanLyGuiEmail_FindMailTemplate", ex);
                throw ex;
            }
        }

        public bool QuanLyGuiEmail_UpdateMailTemplate(String publicKey, String token, string tieuDeEmail, string noiDungEmail, Guid loaiGuiMail, Guid congTy)
        {
            try
            {
                bool result = false;

                if (Helper.TrustTest(publicKey, token))
                {
                    WebMailTemplate_Factory factory = new WebMailTemplate_Factory();
                    WebMailTemplate obj = factory.GetByCongTyVaLoaiGuiMail(congTy, loaiGuiMail);
                    if (obj == null)
                    {
                        obj = factory.CreateManagedObject();
                        obj.Oid = Guid.NewGuid();
                    }
                    obj.TieuDe = tieuDeEmail;
                    obj.NoiDung = noiDungEmail;
                    obj.LoaiGuiMail = loaiGuiMail;
                    obj.CongTy = congTy;

                    factory.SaveChanges();

                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_QuanLyGuiMail/QuanLyGuiEmail_UpdateMailTemplate", ex);
                throw ex;
            }
        }
    }
}
