using HRMWebApp.KPI.Core.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using System.Xml.Serialization;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using System.Configuration;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ExternalServicesApiController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public string PostEofficeData(Dictionary<string, object> resultDic)
        {
            string result = "Helloworld";

            CreateDocument(resultDic["Data"].ToString());
            return result;
        }

        [Authorize]
        [Route("")]
        public int CreateDocument(string document)
        {
            int result=0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    int year = DateTime.Now.Year;
                    string pathVirtal = System.Configuration.ConfigurationManager.AppSettings["VanBanGuiDen"];
                    string pathPhysical = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["VanBanGuiDen"]);
                    string virtualFilePath = ConfigurationManager.AppSettings["FilesPath"] + "/" + year;
                    string filePath = HttpContext.Current.Server.MapPath(virtualFilePath);
                    VanbanGui vbg = null;
                    Guid id = Guid.NewGuid();
                    // Tạo đường dẫn chứa file văn bản
                    string Virtalpath = pathPhysical + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
                    //string path = HttpContext.Current.Server.MapPath(Virtalpath);
                    if (!System.IO.Directory.Exists(Virtalpath))
                        System.IO.Directory.CreateDirectory(Virtalpath);
                    
                    // Lưu file xml vào thư mục
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(document);
                    xdoc.Save(Virtalpath + id.ToString() + ".xml");

                    // Đọc file xml parse thành đối tượng VanBanGui
                    XmlSerializer xmlser = new XmlSerializer(typeof(VanbanGui));
                    using (System.IO.Stream filetream = new FileStream(Virtalpath + id.ToString() + ".xml", FileMode.Open))
                    {
                        filetream.Position = 0;
                        vbg = (VanbanGui)xmlser.Deserialize(filetream);
                    }
                    // Insert văn bản vào database
                    if (vbg != null)
                    {

                        foreach(To to in vbg.Messageheader.To)
                        {
                            string did = to.OrganId;

                            StaffApiController sc = new StaffApiController();

                            //Guid leaderId = new Guid(sc.GetDepartmentLeaderId(new Guid(did)));
                            Guid toDepartmentId = session.Query<EOfficeHRMDepartment>().SingleOrDefault(h => h.EOfficeDepartmentId.ToString() ==  did).DepartmentId;

                            PlanStaff plst = session.Query<PlanStaff>().SingleOrDefault(p => p.Department.Id == toDepartmentId && p.PlanKPI.StartTime.Month == DateTime.Now.Month && p.PlanKPI.StartTime.Year == DateTime.Now.Year);
                      
                            PlanKPIDetail planDetail = new PlanKPIDetail();
                            planDetail.TargetDetail = vbg.Subject;
                            planDetail.Id = Guid.NewGuid();
                            planDetail.Name = vbg.Messageheader.From.Name;
                            planDetail.PlanStaff = plst;
                            planDetail.CreateTime = DateTime.Now;
                            planDetail.IsAddition = true;
                            planDetail.IsFromEoffice = true;
                            planDetail.StartTime = plst.PlanKPI.StartTime;
                            planDetail.EndTime = plst.PlanKPI.EndTime;
                            Method method = new Method();
                            method.StartTime = plst.PlanKPI.StartTime;
                            method.EndTime = plst.PlanKPI.EndTime;
                            planDetail.Methods = new List<Method>();
                            planDetail.Methods.Add(method);
                            
                            session.Save(planDetail);
                            foreach (Attachment item in vbg.Document.Attach)
                            {                                                              
                                FileAttachment fa = new FileAttachment();
                                fa.Id = planDetail.Id;
                                fa.Name = item.Name;
                                fa.Extension = System.IO.Path.GetExtension(item.Name);

                                string PathFileResource = filePath +"/"+ fa.Id.ToString() + fa.Extension;
                                File.WriteAllBytes(PathFileResource, item.Value);

                                if (!System.IO.File.Exists(PathFileResource))
                                    System.IO.File.WriteAllBytes(PathFileResource, item.Value);
                                fa.Path = virtualFilePath + "/" + fa.Id.ToString() + fa.Extension;
                                fa.PlanKPIDetail = new PlanKPIDetail() { Id = planDetail.Id };
                                fa.CreationTime = DateTime.Now;
                                session.Save(fa);
                            }
                        }
                    }
                    else
                    {
                        //Không đọc dc xml
                        result=-1;
                    }
                });
            }
            catch
            {
                result= -2; // lỗi khác
            }
            return result;
        }


        [EnableCors(origins: "http://localhost:53235", headers: "*", methods: "*")]
        public string Get()
        {
            string result = "Helloworld";
            return result;
        }
    }
}
