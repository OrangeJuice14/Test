using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_DanhGiaChiTietApiController : ApiController
    {
        //public float TinhDiemTuDong(ABC_DanhGiaChiTiet item, ABC_DanhGia objDanhGia, ISession session)
        //{
        //    float DiemAuto = 0;
        //    try
        //    {
        //        List<ABC_DieuKienTieuChi> ListDieuKienTieuChi = session.Query<ABC_DieuKienTieuChi>().Where(e => e.TieuChi.Id == item.TieuChi.Id).ToList();
        //        if (ListDieuKienTieuChi.Count != 0)
        //        {
        //            switch (item.TieuChi.DieuKienLoaiDiem.Value)
        //            {
        //                case 1: // Điểm theo tiêu chí
        //                    {
        //                        ABC_BoTieuChi_Role ObjDieuKienHoanThanhBoTieuChiRole = session.Query<ABC_BoTieuChi_Role>().
        //                                                                                                Where(e => e.BoTieuChi.Id == ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi.Id && e.GroupTuDanhGia.Id == objDanhGia.UserDuocDanhGia.GroupDanhGia.Id).
        //                                                                                                OrderBy(e => e.GroupDanhGia.STT).FirstOrDefault();
        //                        if (ObjDieuKienHoanThanhBoTieuChiRole == null)
        //                        {
        //                            ObjDieuKienHoanThanhBoTieuChiRole = session.Query<ABC_BoTieuChi_Role>().OrderBy(e => e.GroupDanhGia.STT).FirstOrDefault(e => e.BoTieuChi.Id == ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi.Id);
        //                        }
        //                        ABC_BoTieuChi DieuKienBoTieuChi = ListDieuKienTieuChi.Select(e => e.DieuKienDiemBoTieuChi).Distinct().SingleOrDefault();

        //                        if (item.TieuChi.DieuKienDiemNhanVien.Value == 1) // 1: Theo đúng người; 
        //                        {
        //                            switch (item.TieuChi.DieuKienThoiGian) // 0: Năm; 12: 12 tháng; 1: đúng tháng; 2: Tính theo danh sách tháng đã chọn
        //                            {
        //                                case 12:
        //                                    {
        //                                        List<ABC_DanhGia> ListDanhGiaOfBoTieuChiDieuKien = session.Query<ABC_DanhGia>().
        //                                                                                            Where(e => e.BoTieuChi.Id == DieuKienBoTieuChi.Id &&
        //                                                                                                            e.UserDuocDanhGia.WebUser.Id == objDanhGia.UserDuocDanhGia.WebUser.Id &&
        //                                                                                                            e.UserDanhGia_Group.Id == ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Id &&
        //                                                                                                            e.IsLock == true && e.KyDanhGia.Nam == objDanhGia.KyDanhGia.Nam && e.KyDanhGia.Loai == 3).
        //                                                                                            ToList();
        //                                        if (ListDanhGiaOfBoTieuChiDieuKien.Count == 12)
        //                                        {
        //                                            foreach (ABC_DanhGia ObjDanhGiaOfBoTieuChiDieuKien in ListDanhGiaOfBoTieuChiDieuKien)
        //                                            {
        //                                                foreach (ABC_DieuKienTieuChi ObjDieuKienTieuChi in ListDieuKienTieuChi)
        //                                                {
        //                                                    ABC_DanhGiaChiTiet ObjDanhGiaChiTiet = session.Query<ABC_DanhGiaChiTiet>().
        //                                                                                                    Single(e => e.TieuChi.Id == ObjDieuKienTieuChi.DieuKienDiemTieuChi.Id &&
        //                                                                                                                e.DanhGia.Id == ObjDanhGiaOfBoTieuChiDieuKien.Id &&
        //                                                                                                                e.DanhGia.IsLock == true);
        //                                                    DiemAuto += ObjDanhGiaChiTiet.Diem.Value;
        //                                                }
        //                                            }
        //                                            DiemAuto = DiemAuto / 12;
        //                                        }
        //                                        break;
        //                                    }
        //                                case 1:
        //                                    {
        //                                        ABC_DanhGia ObjDanhGiaOfBoTieuChiDieuKien = session.Query<ABC_DanhGia>().
        //                                                                                            SingleOrDefault(e => e.BoTieuChi.Id == DieuKienBoTieuChi.Id &&
        //                                                                                                            e.UserDuocDanhGia.Id == objDanhGia.UserDanhGia.Id &&
        //                                                                                                            e.UserDanhGia_Group.Id == ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Id &&
        //                                                                                                            e.IsLock == true && e.KyDanhGia.Id == objDanhGia.KyDanhGia.Id);

        //                                        foreach (ABC_DieuKienTieuChi ObjDieuKienTieuChi in ListDieuKienTieuChi)
        //                                        {
        //                                            ABC_DanhGiaChiTiet ObjDanhGiaChiTiet = session.Query<ABC_DanhGiaChiTiet>().
        //                                                                                            Single(e => e.TieuChi.Id == ObjDieuKienTieuChi.DieuKienDiemTieuChi.Id &&
        //                                                                                                        e.DanhGia.Id == ObjDanhGiaOfBoTieuChiDieuKien.Id &&
        //                                                                                                        e.DanhGia.IsLock == true);
        //                                            DiemAuto += ObjDanhGiaChiTiet.Diem.Value;
        //                                        }
        //                                        break;
        //                                    }
        //                                default: // 0: Năm
        //                                    {
        //                                        ABC_DanhGia ObjDanhGiaOfBoTieuChiDieuKien = session.Query<ABC_DanhGia>().
        //                                                                                            SingleOrDefault(e => e.BoTieuChi.Id == DieuKienBoTieuChi.Id &&
        //                                                                                                            e.UserDuocDanhGia.Id == objDanhGia.UserDanhGia.Id &&
        //                                                                                                            e.UserDanhGia_Group.Id == ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Id &&
        //                                                                                                            e.IsLock == true && e.KyDanhGia.Id == objDanhGia.KyDanhGia.Id);

        //                                        foreach (ABC_DieuKienTieuChi ObjDieuKienTieuChi in ListDieuKienTieuChi)
        //                                        {
        //                                            ABC_DanhGiaChiTiet ObjDanhGiaChiTiet = session.Query<ABC_DanhGiaChiTiet>().
        //                                                                                            Single(e => e.TieuChi.Id == ObjDieuKienTieuChi.DieuKienDiemTieuChi.Id &&
        //                                                                                                        e.DanhGia.Id == ObjDanhGiaOfBoTieuChiDieuKien.Id &&
        //                                                                                                        e.DanhGia.IsLock == true);
        //                                            DiemAuto += ObjDanhGiaChiTiet.Diem.Value;
        //                                        }
        //                                        break;
        //                                    }
        //                            }
        //                        }
        //                        else if (item.TieuChi.DieuKienDiemNhanVien.Value == 2) //2: Trung bình tất cả các thành viên trong đơn vị
        //                        {
        //                            List<ABC_User> ListAllUserRoleInDonVi = new List<ABC_User>();
        //                            List<ABC_DanhGia> ListDanhGiaCuaCapTren = new List<ABC_DanhGia>();
        //                            if (item.TieuChi.IsTeacher.HasValue && item.TieuChi.IsTeacher.Value)
        //                            {
        //                                //ListAllUserRoleInDonVi = GetListUserInDonVi(session, objDanhGia.UserDuocDanhGia.Department, objDanhGia.KyDanhGia, ObjDieuKienHoanThanhBoTieuChiRole.GroupTuDanhGia, objDanhGia.UserDuocDanhGia.Subject);
        //                                ListDanhGiaCuaCapTren = GetListDanhGiaCuaCapTren(session, ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia, objDanhGia.UserDuocDanhGia.Department, ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi, objDanhGia.KyDanhGia, objDanhGia.UserDuocDanhGia.Subject);
        //                            }
        //                            else
        //                            {
        //                                // ListAllUserRoleInDonVi = GetListUserInDonVi(session, objDanhGia.UserDuocDanhGia.Department, objDanhGia.KyDanhGia, ObjDieuKienHoanThanhBoTieuChiRole.GroupTuDanhGia);

        //                                ListDanhGiaCuaCapTren = GetListDanhGiaCuaCapTren(session, ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia, objDanhGia.UserDuocDanhGia.Department, ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi, objDanhGia.KyDanhGia);
        //                            }

        //                            //if (ListDanhGiaCuaCapTren.Count != 0 && ListAllUserRoleInDonVi.Count == ListDanhGiaCuaCapTren.Count) // nếu bằng nhau thì cấp trên đã đánh giá toàn bộ nhân viên trong đơn vị
        //                            //{
        //                            foreach (ABC_DanhGia DanhGiaCuaCapTren in ListDanhGiaCuaCapTren)
        //                            {
        //                                List<ABC_DanhGiaChiTiet> DanhGiaChiTietCuaCapTren = new List<ABC_DanhGiaChiTiet>();
        //                                foreach (ABC_DieuKienTieuChi ObjDieuKienTieuChi in ListDieuKienTieuChi)
        //                                {
        //                                    ABC_DanhGiaChiTiet TempDanhGiaChiTietCuaCapTren = session.Query<ABC_DanhGiaChiTiet>().Single(e => e.DanhGia.Id == DanhGiaCuaCapTren.Id && e.TieuChi.Id == ObjDieuKienTieuChi.DieuKienDiemTieuChi.Id);
        //                                    DiemAuto += TempDanhGiaChiTietCuaCapTren.Diem.Value;
        //                                }
        //                            }
        //                            DiemAuto = DiemAuto / ListDanhGiaCuaCapTren.Count;
        //                            //}
        //                        }
        //                        else if (item.TieuChi.DieuKienDiemNhanVien.Value == 3) // 3: Trung bình tất cả các thành viên trong bộ môn
        //                        {
        //                            List<ABC_User> ListAllUserRoleInBoMon = new List<ABC_User>();
        //                            List<ABC_DanhGia> ListDanhGiaCuaCapTren = new List<ABC_DanhGia>();

        //                            ListAllUserRoleInBoMon = GetListUserInBoMon(session, objDanhGia.UserDuocDanhGia.Subject, objDanhGia.KyDanhGia, ObjDieuKienHoanThanhBoTieuChiRole.GroupTuDanhGia);

        //                            ListDanhGiaCuaCapTren = GetListDanhGiaCuaCapTren(session, ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia, objDanhGia.UserDuocDanhGia.Department, ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi, objDanhGia.KyDanhGia, objDanhGia.UserDuocDanhGia.Subject);


        //                            //if (ListDanhGiaCuaCapTren.Count != 0 && ListAllUserRoleInBoMon.Count == ListDanhGiaCuaCapTren.Count) // nếu bằng nhau thì cấp trên đã đánh giá toàn bộ nhân viên trong đơn vị
        //                            //{
        //                            foreach (ABC_DanhGia DanhGiaCuaCapTren in ListDanhGiaCuaCapTren)
        //                            {
        //                                List<ABC_DanhGiaChiTiet> DanhGiaChiTietCuaCapTren = new List<ABC_DanhGiaChiTiet>();
        //                                foreach (ABC_DieuKienTieuChi ObjDieuKienTieuChi in ListDieuKienTieuChi)
        //                                {
        //                                    ABC_DanhGiaChiTiet TempDanhGiaChiTietCuaCapTren = session.Query<ABC_DanhGiaChiTiet>().Single(e => e.DanhGia.Id == DanhGiaCuaCapTren.Id && e.TieuChi.Id == ObjDieuKienTieuChi.DieuKienDiemTieuChi.Id);

        //                                    DiemAuto += TempDanhGiaChiTietCuaCapTren != null ? TempDanhGiaChiTietCuaCapTren.Diem.Value : 0;
        //                                }
        //                            }
        //                            DiemAuto = DiemAuto / ListDanhGiaCuaCapTren.Count;
        //                            //}
        //                        }
        //                        break;
        //                    }
        //                default: // Điểm tổng
        //                    {
        //                        ABC_BoTieuChi_Role ObjDieuKienHoanThanhBoTieuChiRole = session.Query<ABC_BoTieuChi_Role>().
        //                                                                                                      Where(e => e.BoTieuChi.Id == ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi.Id && e.GroupTuDanhGia.Id == objDanhGia.UserDuocDanhGia.GroupDanhGia.Id).
        //                                                                                                      OrderBy(e => e.GroupDanhGia.STT).FirstOrDefault();
        //                        if (ObjDieuKienHoanThanhBoTieuChiRole == null)
        //                        {
        //                            ObjDieuKienHoanThanhBoTieuChiRole = session.Query<ABC_BoTieuChi_Role>().OrderBy(e => e.GroupDanhGia.STT).FirstOrDefault(e => e.BoTieuChi.Id == ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi.Id);
        //                        }
        //                        if (item.TieuChi.DieuKienDiemNhanVien.Value == 1) // 1: Theo từng người;  
        //                        {
        //                            ABC_BoTieuChi DieuKienBoTieuChi = ListDieuKienTieuChi.Select(e => e.DieuKienDiemBoTieuChi).Distinct().SingleOrDefault();

        //                            switch (item.TieuChi.DieuKienThoiGian) // 0: Năm; 12: 12 tháng; 1: đúng tháng; 2: Tính theo danh sách tháng đã chọn
        //                            {
        //                                case 12:
        //                                    {
        //                                        List<ABC_DanhGia> ListDanhGiaOfBoTieuChiDieuKien = session.Query<ABC_DanhGia>().
        //                                                                                            Where(e => e.BoTieuChi.Id == DieuKienBoTieuChi.Id &&
        //                                                                                                            e.UserDuocDanhGia.WebUser.Id == objDanhGia.UserDuocDanhGia.WebUser.Id &&
        //                                                                                                            e.UserDanhGia_Group.Id == ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Id &&
        //                                                                                                            e.IsLock == true && e.KyDanhGia.Nam == objDanhGia.KyDanhGia.Nam && e.KyDanhGia.Loai == 3).
        //                                                                                            ToList();
        //                                        if (ListDanhGiaOfBoTieuChiDieuKien.Count == 12)
        //                                        {
        //                                            DiemAuto = ListDanhGiaOfBoTieuChiDieuKien.Sum(e => e.TongDiem.Value);
        //                                            //foreach (ABC_DanhGia ObjDanhGiaOfBoTieuChiDieuKien in ListDanhGiaOfBoTieuChiDieuKien)
        //                                            //{
        //                                            //    DiemAuto += ObjDanhGiaOfBoTieuChiDieuKien.TongDiem.Value;
        //                                            //}
        //                                            DiemAuto = DiemAuto / 12;
        //                                        }
        //                                        break;
        //                                    }
        //                                case 1:
        //                                    {
        //                                        break;
        //                                    }
        //                                default: // 0: Năm
        //                                    {
        //                                        List<ABC_DanhGia> ListDanhGiaOfBoTieuChiDieuKien = session.Query<ABC_DanhGia>().
        //                                                                                               Where(e => e.BoTieuChi.Id == DieuKienBoTieuChi.Id &&
        //                                                                                                               e.UserDuocDanhGia.Id == objDanhGia.UserDanhGia.Id &&
        //                                                                                                               e.UserDanhGia_Group.Id == ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Id &&
        //                                                                                                               e.IsLock == true && e.KyDanhGia.Id == objDanhGia.KyDanhGia.Id).
        //                                                                                               ToList();
        //                                        break;
        //                                    }
        //                            }
        //                        }
        //                        else if (item.TieuChi.DieuKienDiemNhanVien.Value == 2) //2: Trung bình tất cả các thành viên trong nhóm
        //                        {
        //                            List<ABC_User> ListAllUserRoleInDonVi = new List<ABC_User>();
        //                            List<ABC_DanhGia> ListDanhGiaCuaCapTren = new List<ABC_DanhGia>();
        //                            if (item.TieuChi.IsTeacher.HasValue && item.TieuChi.IsTeacher.Value)
        //                            {
        //                                ListAllUserRoleInDonVi = GetListUserInDonVi(session, objDanhGia.UserDuocDanhGia.Department, objDanhGia.KyDanhGia, ObjDieuKienHoanThanhBoTieuChiRole.GroupTuDanhGia, objDanhGia.UserDuocDanhGia.Subject);
        //                                ListDanhGiaCuaCapTren = GetListDanhGiaCuaCapTren(session, ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia, objDanhGia.UserDuocDanhGia.Department, ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi, objDanhGia.KyDanhGia, objDanhGia.UserDuocDanhGia.Subject);
        //                            }
        //                            else
        //                            {
        //                                ListAllUserRoleInDonVi = GetListUserInDonVi(session, objDanhGia.UserDuocDanhGia.Department, objDanhGia.KyDanhGia, ObjDieuKienHoanThanhBoTieuChiRole.GroupTuDanhGia);

        //                                ListDanhGiaCuaCapTren = GetListDanhGiaCuaCapTren(session, ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia, objDanhGia.UserDuocDanhGia.Department, ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi, objDanhGia.KyDanhGia);
        //                            }

        //                            //ABC_BoTieuChi DieuKienBoTieuChi = ListDieuKienTieuChi.Select(e => e.DieuKienDiemBoTieuChi).Distinct().SingleOrDefault();
        //                            //List<ABC_DanhGia> ListDanhGiaOfBoTieuChiDieuKien = session.Query<ABC_DanhGia>().
        //                            //                                                    Where(e => e.BoTieuChi.Id == DieuKienBoTieuChi.Id &&
        //                            //                                                                    e.UserDanhGia.Id == objDanhGia.UserDanhGia.Id &&
        //                            //                                                                    e.UserDuocDanhGia.Id == objDanhGia.UserDuocDanhGia.Id &&
        //                            //                                                                    e.IsLock == true).
        //                            //                                                    ToList();
        //                            //if (ListDanhGiaCuaCapTren.Count != 0 && ListAllUserRoleInDonVi.Count == ListDanhGiaCuaCapTren.Count) // nếu bằng nhau thì cấp trên đã đánh giá toàn bộ nhân viên trong đơn vị
        //                            //{
        //                            switch (item.TieuChi.DieuKienThoiGian) // 0: đúng Năm; 12: 12 tháng; 1: đúng tháng; 2: Tính theo danh sách tháng đã chọn
        //                            {
        //                                case 1:
        //                                    {
        //                                        DiemAuto = ListDanhGiaCuaCapTren.Sum(e => e.TongDiem.Value) / ListDanhGiaCuaCapTren.Count;
        //                                        //foreach (ABC_DanhGia DanhGiaCuaCapTren in ListDanhGiaCuaCapTren)
        //                                        //{
        //                                        //    DiemAuto += DanhGiaCuaCapTren.TongDiem.Value;
        //                                        //}
        //                                        //DiemAuto = DiemAuto / ListDanhGiaCuaCapTren.Count;
        //                                        break;
        //                                    }
        //                                case 12: // không vào trường hợp này 
        //                                    {
        //                                        break;
        //                                    }
        //                                default: // 0 :đúng năm
        //                                    {
        //                                        DiemAuto = ListDanhGiaCuaCapTren.Sum(e => e.TongDiem.Value) / ListDanhGiaCuaCapTren.Count;
        //                                        //foreach (ABC_DanhGia DanhGiaCuaCapTren in ListDanhGiaCuaCapTren)
        //                                        //{
        //                                        //    DiemAuto += DanhGiaCuaCapTren.TongDiem.Value;
        //                                        //}
        //                                        //DiemAuto = DiemAuto / ListDanhGiaCuaCapTren.Count;

        //                                        break;
        //                                    }
        //                            }
        //                            //}
        //                            //switch (item.TieuChi.DieuKienThoiGian) // 0: đúng Năm; 12: 12 tháng; 1: đúng tháng; 2: Tính theo danh sách tháng đã chọn
        //                            //{
        //                            //    case 1:
        //                            //        {
        //                            //            if (ListDanhGiaCuaCapTren.Count != 0 && ListAllUserRoleInDonVi.Count == ListDanhGiaCuaCapTren.Count) // nếu bằng nhau thì cấp trên đã đánh giá toàn bộ nhân viên trong đơn vị
        //                            //            {
        //                            //                //DiemAuto = ListDanhGiaCuaCapTren.Sum(e => e.TongDiem.Value) / ListDanhGiaCuaCapTren.Count;
        //                            //                foreach (ABC_DanhGia DanhGiaCuaCapTren in ListDanhGiaCuaCapTren)
        //                            //                {
        //                            //                    DiemAuto += DanhGiaCuaCapTren.TongDiem.Value;
        //                            //                }
        //                            //                DiemAuto = DiemAuto / ListDanhGiaCuaCapTren.Count;
        //                            //            }
        //                            //            break;
        //                            //        }
        //                            //    case 12: // không vào trường hợp này 
        //                            //        {
        //                            //            break;
        //                            //        }
        //                            //    default: // 0 :đúng năm
        //                            //        {
        //                            //            if (ListDanhGiaCuaCapTren.Count != 0 && ListAllUserRoleInDonVi.Count == ListDanhGiaCuaCapTren.Count) // nếu bằng nhau thì cấp trên đã đánh giá toàn bộ nhân viên trong đơn vị
        //                            //            {
        //                            //                foreach (ABC_DanhGia DanhGiaCuaCapTren in ListDanhGiaCuaCapTren)
        //                            //                {
        //                            //                    DiemAuto += DanhGiaCuaCapTren.TongDiem.Value;
        //                            //                }
        //                            //                DiemAuto = DiemAuto / ListDanhGiaCuaCapTren.Count;
        //                            //            }
        //                            //            break;
        //                            //        }
        //                            //}
        //                        }
        //                        else if (item.TieuChi.DieuKienDiemNhanVien.Value == 3) // 3: Trung bình tất cả các thành viên trong bộ môn
        //                        {
        //                            List<ABC_User> ListAllUserRoleInBoMon = new List<ABC_User>();
        //                            List<ABC_DanhGia> ListDanhGiaCuaCapTren = new List<ABC_DanhGia>();

        //                            ListAllUserRoleInBoMon = GetListUserInBoMon(session, objDanhGia.UserDuocDanhGia.Subject, objDanhGia.KyDanhGia, ObjDieuKienHoanThanhBoTieuChiRole.GroupTuDanhGia);

        //                            ListDanhGiaCuaCapTren = GetListDanhGiaCuaCapTren(session, ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia, objDanhGia.UserDuocDanhGia.Department, ListDieuKienTieuChi[0].DieuKienDiemBoTieuChi, objDanhGia.KyDanhGia, objDanhGia.UserDuocDanhGia.Subject);


        //                            //if (ListDanhGiaCuaCapTren.Count != 0 && ListAllUserRoleInBoMon.Count == ListDanhGiaCuaCapTren.Count) // nếu bằng nhau thì cấp trên đã đánh giá toàn bộ nhân viên trong đơn vị
        //                            //{
        //                                switch (item.TieuChi.DieuKienThoiGian) // 0: đúng Năm; 12: 12 tháng; 1: đúng tháng; 2: Tính theo danh sách tháng đã chọn
        //                                {
        //                                    case 1:
        //                                        {
        //                                            DiemAuto = ListDanhGiaCuaCapTren.Sum(e => e.TongDiem.Value) / ListDanhGiaCuaCapTren.Count;
        //                                            //foreach (ABC_DanhGia DanhGiaCuaCapTren in ListDanhGiaCuaCapTren)
        //                                            //{
        //                                            //    DiemAuto += DanhGiaCuaCapTren.TongDiem.Value;
        //                                            //}
        //                                            //DiemAuto = DiemAuto / ListDanhGiaCuaCapTren.Count;
        //                                            break;
        //                                        }
        //                                    case 12: // không vào trường hợp này 
        //                                        {
        //                                            break;
        //                                        }
        //                                    default: // 0 :đúng năm
        //                                        {
        //                                            DiemAuto = ListDanhGiaCuaCapTren.Sum(e => e.TongDiem.Value) / ListDanhGiaCuaCapTren.Count;
        //                                            //foreach (ABC_DanhGia DanhGiaCuaCapTren in ListDanhGiaCuaCapTren)
        //                                            //{
        //                                            //    DiemAuto += DanhGiaCuaCapTren.TongDiem.Value;
        //                                            //}
        //                                            //DiemAuto = DiemAuto / ListDanhGiaCuaCapTren.Count;

        //                                            break;
        //                                        }
        //                                }
        //                            //}
        //                        }
        //                        break;
        //                    }
        //            }
        //        }
        //        else
        //        {
        //            DiemAuto = item.Diem.HasValue ? item.Diem.Value : 0;
        //        }
        //    }
        //    catch (Exception ex) { Helper.ErrorLog("ABC_DanhGiaChiTietApi/TinhDiemTuDong ", ex); throw ex; }
        //    return float.Parse(Math.Round(DiemAuto, 2).ToString());
        //}

        //public List<ABC_User> GetListUserInDonVi(ISession session, Department department, ABC_KyDanhGia kyDanhGia, ABC_GroupDanhGia groupTuDanhGia, Department subject)
        //{
        //    List<ABC_User> result = new List<ABC_User>();
        //    result = session.Query<ABC_User>().
        //                    Where(e => e.Department.Id == department.Id &&
        //                                e.Subject.Id == subject.Id &&
        //                                (e.GroupDanhGia.Id == groupTuDanhGia.Id ||
        //                                !((e.Subject.ManageCode == "TBM" && subject.Name.StartsWith("Bộ môn ")) ||
        //                                  (e.Subject.ManageCode == "PTBM" && subject.Name.StartsWith("Bộ môn ")) ||
        //                                  (e.Position.ManageCode == "PTP" && department.Name.StartsWith("Phòng")) ||
        //                                  (e.Position.ManageCode == "TP" && department.Name.StartsWith("Phòng")) ||
        //                                  (e.Position.ManageCode == "PGĐ" && department.Name.StartsWith("Trung tâm ")) ||
        //                                  (e.Position.ManageCode == "GD" && department.Name.StartsWith("Trung tâm ")) ||
        //                                  (e.Position.ManageCode == "VT" && department.Name.StartsWith("Viện ")) ||
        //                                  (e.Position.ManageCode == "PVTr" && department.Name.StartsWith("Viện ")) ||
        //                                  (e.Position.ManageCode == "PTBT" && department.Name.StartsWith("Nhà xuất bản ")) ||
        //                                  (e.Position.ManageCode == "TBT" && department.Name.StartsWith("Nhà xuất bản ")))) &&
        //                                e.DeleteTime == null &&
        //                                e.KyDanhGia.Id == kyDanhGia.Id).
        //                    OrderBy(e => e.WebUser.UserName).ToList();
        //    return result;
        //}
        //public List<ABC_User> GetListUserInDonVi(ISession session, Department department, ABC_KyDanhGia kyDanhGia, ABC_GroupDanhGia groupTuDanhGia)
        //{
        //    List<ABC_User> result = new List<ABC_User>();
        //    result = session.Query<ABC_User>().
        //                    Where(e => e.Department.Id == department.Id &&
        //                                (e.GroupDanhGia.Id == groupTuDanhGia.Id ||
        //                                !((e.Subject.ManageCode == "TBM") ||
        //                                  (e.Subject.ManageCode == "PTBM") ||
        //                                  (e.Position.ManageCode == "PTP" && department.Name.StartsWith("Phòng")) ||
        //                                  (e.Position.ManageCode == "TP" && department.Name.StartsWith("Phòng")) ||
        //                                  (e.Position.ManageCode == "PGĐ" && department.Name.StartsWith("Trung tâm ")) ||
        //                                  (e.Position.ManageCode == "GD" && department.Name.StartsWith("Trung tâm ")) ||
        //                                  (e.Position.ManageCode == "VT" && department.Name.StartsWith("Viện ")) ||
        //                                  (e.Position.ManageCode == "PVTr" && department.Name.StartsWith("Viện ")) ||
        //                                  (e.Position.ManageCode == "PTBT" && department.Name.StartsWith("Nhà xuất bản ")) ||
        //                                  (e.Position.ManageCode == "TBT" && department.Name.StartsWith("Nhà xuất bản ")))) &&
        //                                e.DeleteTime == null &&
        //                                e.KyDanhGia.Id == kyDanhGia.Id).
        //                    ToList();
        //    return result;
        //}
        //public List<ABC_User> GetListUserInBoMon(ISession session, Department subject, ABC_KyDanhGia kyDanhGia, ABC_GroupDanhGia groupTuDanhGia)
        //{
        //    List<ABC_User> result = new List<ABC_User>();
        //    result = session.Query<ABC_User>().
        //                    Where(e => e.Subject.Id == subject.Id &&
        //                                e.GroupDanhGia.Id == groupTuDanhGia.Id &&
        //                                e.DeleteTime == null &&
        //                                e.KyDanhGia.Id == kyDanhGia.Id).
        //                    ToList();
        //    return result;
        //}
        //public List<ABC_DanhGia> GetListDanhGiaCuaCapTren(ISession session, ABC_GroupDanhGia userDanhGia_Group, Department department, ABC_BoTieuChi boTieuChi, ABC_KyDanhGia kyDanhGia, Department subject)
        //{
        //    List<ABC_DanhGia> result = new List<ABC_DanhGia>();
        //    result = session.Query<ABC_DanhGia>().
        //                    Where(e => e.GroupUserDanhGia.Id == userDanhGia_Group.Id &&
        //                                e.UserDuocDanhGia.Department.Id == department.Id &&
        //                                e.UserDuocDanhGia.Subject.Id == subject.Id &&
        //                                e.UserDuocDanhGia.DeleteTime == null &&
        //                                e.BoTieuChi.Id == boTieuChi.Id &&
        //                                e.IsLock == true && e.KyDanhGia.Id == kyDanhGia.Id).ToList();
        //    return result;
        //}
        //public List<ABC_DanhGia> GetListDanhGiaCuaCapTren(ISession session, ABC_GroupDanhGia userDanhGia_Group, Department department, ABC_BoTieuChi boTieuChi, ABC_KyDanhGia kyDanhGia)
        //{
        //    List<ABC_DanhGia> result = new List<ABC_DanhGia>();
        //    result = session.Query<ABC_DanhGia>().
        //                    Where(e => e.GroupUserDanhGia.Id == userDanhGia_Group.Id &&
        //                                e.UserDuocDanhGia.Department.Id == department.Id &&
        //                                e.UserDuocDanhGia.DeleteTime == null &&
        //                                e.BoTieuChi.Id == boTieuChi.Id &&
        //                                e.IsLock == true && e.KyDanhGia.Id == kyDanhGia.Id).ToList();
        //    return result;
        //}
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_DanhGiaChiTietVMDTO> GetListByDanhGiaId(Guid danhGiaId)
        {
            List<ABC_TieuChi> ListTieuChi = new List<ABC_TieuChi>();
            ABC_DanhGia objDanhGia = new ABC_DanhGia();
            List<ABC_DanhGiaChiTietVMDTO> listResult = new List<ABC_DanhGiaChiTietVMDTO>();

            SessionManager.DoWorkNoTransaction(session =>
            {
                objDanhGia = session.Query<ABC_DanhGia>().SingleOrDefault(e => e.Id == danhGiaId);
                List<ABC_DanhGiaChiTiet> ListDanhGiaChiTiet = session.Query<ABC_DanhGiaChiTiet>().
                                                                Where(e => e.DanhGia.Id == objDanhGia.Id && e.TieuChi.GCRecord == null).
                                                                OrderBy(e => e.TieuChi.STT).ToList();
                #region Nếu thêm tiêu chí sau khi có người vào đánh giá thì kiểm tra và thêm vào.

                List<ABC_TieuChi> ListTieuChiInBoTieuChiDanhGia = session.Query<ABC_TieuChi>().Where(e => e.BoTieuChi.Id == objDanhGia.BoTieuChi.Id && e.GCRecord == null).ToList();

                if (ListDanhGiaChiTiet  != null && ListDanhGiaChiTiet.Count != 0 && ListTieuChiInBoTieuChiDanhGia.Count != ListDanhGiaChiTiet.Count)
                {
                    IEnumerable<ABC_TieuChi> ListTieuChiNotInDanhGiaChiTiet = ListTieuChiInBoTieuChiDanhGia.Except(ListDanhGiaChiTiet.Select(e => e.TieuChi).Distinct());
                    foreach (ABC_TieuChi Obj in ListTieuChiNotInDanhGiaChiTiet)
                    {
                        ABC_DanhGiaChiTiet TempDanhGiaChiTiet = new ABC_DanhGiaChiTiet();
                        TempDanhGiaChiTiet.TieuChi = Obj;
                        TempDanhGiaChiTiet.DanhGia = objDanhGia;
                        TempDanhGiaChiTiet.Id = Guid.NewGuid();
                        session.Save(TempDanhGiaChiTiet);
                        ListDanhGiaChiTiet.Add(TempDanhGiaChiTiet);
                    }
                }

                #endregion

                foreach (ABC_DanhGiaChiTiet item in ListDanhGiaChiTiet)
                {
                    //Tính điêm tự động
                    //if (item.TieuChi.IsAutoScore.HasValue && item.TieuChi.IsAutoScore.Value == true && objDanhGia.IsLock != true) // nếu điểm này là điểm tự động tính dựa theo mục khác
                    //{
                    //    item.Diem = TinhDiemTuDong(item, objDanhGia, session);
                    //}
                    ABC_DanhGiaChiTietVMDTO obj = item.Map<ABC_DanhGiaChiTietVMDTO>();
                    obj.NoChild = item.TieuChi.Childrens.Where(e => e.GCRecord == null).ToList().Count == 0;
                    listResult.Add(obj);
                }
            });

            if (listResult.Count == 0)
            {
                SessionManager.DoWork(session =>
                {
                    ListTieuChi = session.Query<ABC_TieuChi>().Where(e => e.BoTieuChi.Id == objDanhGia.BoTieuChi.Id && e.GCRecord == null).ToList();
                    foreach (ABC_TieuChi item in ListTieuChi)
                    {
                        ABC_DanhGiaChiTiet obj = new ABC_DanhGiaChiTiet()
                        {
                            Id = Guid.NewGuid(),
                            DanhGia = new ABC_DanhGia() { Id = objDanhGia.Id },
                            TieuChi = item,
                            Diem = 0
                        };
                        session.Save(obj);
                    }

                    List<ABC_DanhGiaChiTiet> ListDanhGia = session.Query<ABC_DanhGiaChiTiet>().
                                                                    Where(e => e.DanhGia.Id == objDanhGia.Id).
                                                                    OrderBy(e => e.TieuChi.STT).
                                                                    ToList();
                    objDanhGia = session.Query<ABC_DanhGia>().SingleOrDefault(e => e.Id == danhGiaId);
                    foreach (ABC_DanhGiaChiTiet item in ListDanhGia)
                    {
                        //Tính điêm tự động
                        //if (item.TieuChi.IsAutoScore.HasValue && item.TieuChi.IsAutoScore.Value == true && objDanhGia.IsLock != true) // nếu điểm này là điểm tự động tính dựa theo mục khác
                        //{
                        //    item.Diem = TinhDiemTuDong(item, objDanhGia, session);
                        //}
                        ABC_DanhGiaChiTietVMDTO obj = item.Map<ABC_DanhGiaChiTietVMDTO>();
                        obj.NoChild = item.TieuChi.Childrens.Where(e => e.GCRecord == null).ToList().Count == 0;
                        listResult.Add(obj);
                    }
                });
            }

            return listResult;
        }
        [Authorize]
        [Route("")]
        //public int Put(List<ABC_DanhGiaChiTietDTO> list)
        public int Put(List<ABC_DanhGiaChiTietCreateDTO> list)
        {
            try
            {
                SessionManager.DoWork(session =>
                {
                    foreach (ABC_DanhGiaChiTietCreateDTO item in list)
                    {
                        ABC_DanhGiaChiTiet obj = new ABC_DanhGiaChiTiet();
                        obj = session.Query<ABC_DanhGiaChiTiet>().Single(e => e.Id == item.Id);
                        obj.DanhGia = new ABC_DanhGia() { Id = item.DanhGiaId.Value };
                        obj.Diem = item.Diem;
                        obj.TieuChi = new ABC_TieuChi() { Id = item.TieuChiId.Value };
                        session.Update(obj);
                    }
                });
                return 1;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_DanhGiaChiTietApi/Put", ex);
                throw ex;
            }
        }
    }
}
