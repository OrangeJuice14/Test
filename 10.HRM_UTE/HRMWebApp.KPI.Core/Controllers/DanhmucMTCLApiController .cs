using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using System.Web.Configuration;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class DanhmucMTCLApiController : ApiController
    {

        [Authorize]
        [Route("")]
        public IEnumerable<StudyYearDTO> GetListStudyYear()
        {
            List<StudyYearDTO> result = new List<StudyYearDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<StudyYear>().ToList().Map<StudyYearDTO>();
            });
            return result;
        }
        public StaffDTO ParseStaff(Staff staff)
        {
            StaffDTO sd = new StaffDTO();
            sd.Id = staff.Id;
            sd.Name = staff.StaffProfile.Name;
            sd.ManageCode = staff.StaffInfo.ManageCode;
            sd.DepartmentName = staff.Department.Name;
            sd.PositionName = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.Name : "";
            return sd;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<StaffDTO> GetBGH()
        {
            List<StaffDTO> result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                List<Staff> templist = new List<Staff>();
                templist = session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && (s.StaffInfo.Position.AgentObjectType.Id == 10 || s.StaffInfo.Position.AgentObjectType.Id == 11)).OrderBy(s => s.StaffInfo.Position.AgentObjectType.Id).ToList();
                foreach (Staff s in templist)
                {
                    StaffDTO sd = new StaffDTO();
                    sd = ParseStaff(s);
                    result.Add(sd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentDTO> GetBoPhan()
        {
            List<DepartmentDTO> result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Department>().Where(r => r.DepartmentType == 1).ToList().Map<DepartmentDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DanhMucMTCLDTO> GetListDanhMuc(string managecode)
        {
            List<DanhMucMTCLDTO> result = new List<DanhMucMTCLDTO>();
            SessionManager.DoWork(session =>
            {
                List<DanhMucMTCL> listanhmuc = new List<DanhMucMTCL>();
                if (managecode == "" || managecode == null)
                {
                    listanhmuc = session.Query<DanhMucMTCL>().ToList();
                }
                else
                    listanhmuc = session.Query<DanhMucMTCL>().Where(r => r.OidDanhMucCha.Id == managecode).ToList();
                foreach (DanhMucMTCL item in listanhmuc)
                {
                    DanhMucMTCLDTO newitem = new DanhMucMTCLDTO();
                    newitem.Id = item.Id;
                    newitem.OidDanhMucCha = item.OidDanhMucCha;
                    newitem.MaDanhMuc = item.MaDanhMuc;
                    newitem.TenDanhMuc = item.TenDanhMuc;
                    newitem.DonViPhuTrach = new Department() { Id = item.DonViPhuTrach.Id };
                    newitem.CapDanhMuc = item.CapDanhMuc;
                    newitem.BGHPhuTrach = new Staff() { Id = item.BGHPhuTrach.Id };
                    newitem.OidDanhMucCha = new ManageCode() { Id = item.OidDanhMucCha.Id };
                    result.Add(newitem);
                }
                //result = session.Query<DanhMucMTCL>().ToList().Map<DanhMucMTCLDTO>();
            });
            return result.OrderBy(r=>r.MaDanhMuc);
        }
        [Authorize]
        [Route("")]
        public DanhMucMTCLDTO GetListByDanhMucId(Guid Id)
        {
            DanhMucMTCLDTO result = new DanhMucMTCLDTO();
            SessionManager.DoWork(session =>
            {
                List<DanhMucMTCL> listanhmuc = session.Query<DanhMucMTCL>().Where(r => r.Id == Id).ToList();
                foreach (DanhMucMTCL item in listanhmuc)
                {
                    //DanhMucMTCLDTO newitem = new DanhMucMTCLDTO();
                    result.Id = item.Id;
                    result.OidDanhMucCha = item.OidDanhMucCha;
                    result.MaDanhMuc = item.MaDanhMuc;
                    result.TenDanhMuc = item.TenDanhMuc;
                    result.DonViPhuTrach = new Department() { Id = item.DonViPhuTrach.Id };
                    result.OidDanhMucCha = new ManageCode() { Id = item.OidDanhMucCha.Id };
                    result.CapDanhMuc = item.CapDanhMuc;
                    result.BGHPhuTrach = new Staff() { Id = item.BGHPhuTrach.Id };

                    result.StudyYearIds = new List<Guid>();
                    foreach (var n in item.NamHoc)
                    {
                        result.StudyYearIds.Add(n.Id);
                    }
                  //  result.Add(newitem);

                }

                //result = session.Query<DanhMucMTCL>().ToList().Map<DanhMucMTCLDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public DanhMucMTCLDTO Put(DanhMucMTCLDTO obj)
        {
            DanhMucMTCL rerult = new DanhMucMTCL();
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();

            SessionManager.DoWork(session =>
            {
                rerult.Id = obj.Id;
                rerult.MaDanhMuc = obj.MaDanhMuc;
                rerult.TenDanhMuc = obj.TenDanhMuc;
                rerult.CapDanhMuc = obj.CapDanhMuc;
                rerult.OidDanhMucCha = new ManageCode() { Id = obj.OidDanhMucCha.Id };
                rerult.DonViPhuTrach = new Department() { Id = obj.DonViPhuTrach.Id };
                rerult.BGHPhuTrach = new Staff() { Id = obj.BGHPhuTrach.Id };
                rerult.NamHoc = new List<StudyYear>();
                if (obj.StudyYearIds != null)
                {
                    foreach (var id in obj.StudyYearIds)
                    {
                        rerult.NamHoc.Add(new StudyYear() { Id = id });
                    }
                }
                session.SaveOrUpdate(rerult);

            });
            return obj;
        }

        [Authorize]
        [Route("")]
        public List<DanhMucMTCL> Delete(List<DanhMucMTCL> obj)
        {
            foreach (DanhMucMTCL i in obj)
            {
                SessionManager.DoWork(session => session.Delete(i));
            }
            return obj;
        }
    }
}
