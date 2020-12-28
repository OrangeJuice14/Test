using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DevExpress.Spreadsheet;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate;
using System.IO;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using System.Transactions;
using System.Globalization;
using System.Web.Configuration;
using ERP_Core.Common;
using System.Text;
using System.Windows.Forms;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class Imports
    {
        private static SqlConnection _dbConn = new SqlConnection();
        public static string _connectionString = string.Empty;
        static Workbook _workbookSet = new Workbook();
        //static SpreadsheetGear.IWorkbookSet _workbookSet = SpreadsheetGear.Factory.GetWorkbookSet();
        public static string _directFile = string.Empty;

        static DataTable _dtBacDaoTao = new System.Data.DataTable();
        static DataTable _dtHeDaoTao = new System.Data.DataTable();
        static DataTable _dtBacHeDaoTao = new System.Data.DataTable();

        public static string _collegeID = string.Empty;
        public static string _namHoc = string.Empty;
        public static int _namThucHoc = 0;

        public static string _strSuccess = string.Empty;
        public static string _strError = string.Empty;


        #region Luu()
        static int Luu(string XML)
        {
            int result = 0;
            SqlCommand dbCmd = _dbConn.CreateCommand();
            try
            {
                string xmlTMP = XML.Replace("&", " ");

                _dbConn.Open();
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.CommandText = "HRM_ThongTinDaoTao_Save";

                System.Data.SqlClient.SqlParameter sPrmIn = dbCmd.CreateParameter();
                sPrmIn.ParameterName = "@XmlData";
                sPrmIn.SqlDbType = SqlDbType.NText;
                sPrmIn.Direction = ParameterDirection.Input;
                sPrmIn.Value = xmlTMP;

                System.Data.SqlClient.SqlParameter sPrmOut = dbCmd.CreateParameter();
                sPrmOut.ParameterName = "@ReVal";
                sPrmOut.SqlDbType = SqlDbType.Int;
                sPrmOut.Direction = ParameterDirection.Output;
                sPrmOut.Value = DBNull.Value;


                dbCmd.Parameters.Add(sPrmIn);
                dbCmd.Parameters.Add(sPrmOut);
                dbCmd.CommandTimeout = 500;
                dbCmd.ExecuteNonQuery();

                result = int.Parse(sPrmOut.Value.ToString());

                dbCmd.Parameters.Clear();
                dbCmd.Connection.Close();


            }
            catch
            {
                result = -1;
            }
            finally
            {
                dbCmd.Parameters.Clear();
                dbCmd.Connection.Close();
                _dbConn.Close();
            }
            return result;
        }
        #endregion

        #region ImportData
        public static void ImportDataScienceResearch()
        {
            try
            {
                SessionManager.DoWork(session =>
                {

                    string deleteAll = string.Format("DELETE FROM {0}", "KPI_ScienceResearchData");
                    session.CreateSQLQuery(deleteAll).ExecuteUpdate();



                    _workbookSet = new Workbook();
                    _workbookSet.LoadDocument(_directFile, DocumentFormat.OpenXml);


                    var file = _workbookSet;
                    int indexSheet = 1;



                    int startIndexSheet = 0;
                    int countSheet = _workbookSet.Worksheets.Count;

                    for (int j = startIndexSheet; j <= countSheet; j++)
                    {
                        indexSheet = j;

                        #region //Sheet 0 -- Thong Ke Tuyen Moi
                        if (indexSheet == 0)
                        {

                            int i = 3;
                            while (file.Worksheets[indexSheet].Cells["A" + i.ToString()].Value.ToString().Trim() != "")
                            {
                                string staffCode = file.Worksheets[indexSheet].Cells["A" + i.ToString()].Value.ToString().Trim();
                                string name = file.Worksheets[indexSheet].Cells["B" + i.ToString()].Value.ToString().Trim();
                                string manageCode = file.Worksheets[indexSheet].Cells["C" + i.ToString()].Value.ToString().Trim();
                                string studyTerm = file.Worksheets[indexSheet].Cells["D" + i.ToString()].Value.ToString().Trim();
                                string studyYear = file.Worksheets[indexSheet].Cells["E" + i.ToString()].Value.ToString().Trim();
                                double record = (file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDouble(file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim()));
                                ScienceResearchData srd = new ScienceResearchData()
                                {
                                    Id = Guid.NewGuid(),
                                    StaffCode = file.Worksheets[indexSheet].Cells["A" + i.ToString()].Value.ToString().Trim(),
                                    Name = file.Worksheets[indexSheet].Cells["B" + i.ToString()].Value.ToString().Trim(),
                                    ManageCode = file.Worksheets[indexSheet].Cells["C" + i.ToString()].Value.ToString().Trim(),
                                    StudyYear = file.Worksheets[indexSheet].Cells["D" + i.ToString()].Value.ToString().Trim(),
                                    StudyTerm = file.Worksheets[indexSheet].Cells["E" + i.ToString()].Value.ToString().Trim(),
                                    Record = (file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDouble(file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim()))
                                };
                                session.Save(srd);
                                i++;
                            }
                        }
                    }
                });

                #endregion
            }
            catch { }
            finally
            {
                _workbookSet = null;
                _directFile = null;
                if (File.Exists(_directFile))
                    File.Delete(_directFile);
                _dbConn.Close();
            }
        }
         /*
        public static int ImportThuNhapKhac(Guid kyTinhLuong)
        {
            using (DataTable dt = HamDungChung.GetDataTable(_directFile, "[Sheet1$A2:H]"))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    BangThuNhapKhac_Factory factory = BangThuNhapKhac_Factory.New();
                    ChiTietThuNhapKhac_Factory ccfac = ChiTietThuNhapKhac_Factory.New();
                    HoSo_Factory hsfac = HoSo_Factory.New();
                    LoaiThuNhapKhac_Factory loaifac = LoaiThuNhapKhac_Factory.New();
                    StringBuilder mainLog = new StringBuilder();
                    StringBuilder detailLog;
                    //
                    foreach (DataRow dr in dt.Rows)
                    {
                        int idx_MaQuanLy = 1;
                        int idx_LoaiThuNhapKhac = 4;
                        int idx_NgayLap = 5;
                        int idx_SoTien = 6;
                        int idx_Ghichu = 7;
                        //
                        detailLog = new StringBuilder();
                        //
                        #region Mã quản lý
                        String maQuanLyText = dr[idx_MaQuanLy].ToString();
                        HoSo nhanVien = hsfac.GetAll().Where(x => x.MaQuanLy.Equals(maQuanLyText)).SingleOrDefault();
                        if (nhanVien != null)
                        {
                            #region Loại thu nhập khác
                            String loaiThuNhapKhacText = dr[idx_LoaiThuNhapKhac].ToString();
                            if(string.IsNullOrEmpty(loaiThuNhapKhacText))
                            {
                                return 2;
                            }
                            LoaiThuNhapKhacOld loaiThuNhap = loaifac.GetByLoaiThuNhapKhac(loaiThuNhapKhacText);
                            if (loaiThuNhap == null)
                            {
                                loaiThuNhap = loaifac.CreateManagedObject();
                                loaiThuNhap.Oid = Guid.NewGuid();
                                loaiThuNhap.MaQuanLy = loaiThuNhapKhacText;
                                loaiThuNhap.TenLoaiThuNhapKhac = loaiThuNhapKhacText;
                                loaifac.SaveChanges();
                            }

                            //Kiểm tra đã có bảng thu nhập khác chưa
                            BangThuNhapKhacOld qlcc = factory.GetByKyTinhLuongLoaiThuNhapKhac(kyTinhLuong, loaiThuNhap.Oid);

                                //Nếu chưa có BangThuNhapKhac
                                if (qlcc == null)
                                {
                                    qlcc = factory.CreateManagedObject();
                                    qlcc.Oid = Guid.NewGuid();
                                    qlcc.KyTinhLuong = kyTinhLuong;
                                    qlcc.LoaiThuNhapKhacOld = loaiThuNhap.Oid;
                                    qlcc.NgayLap = DateTime.Now;
                                    factory.SaveChanges();
                                }

                                //Có BangThuNhapKhac và chưa có chi tiết
                                ChiTietThuNhapKhacOld cctnk = ccfac.ExistByBangThuNhapKhacIDNV(qlcc.Oid, nhanVien.Oid);
                                if (cctnk == null)
                                {
                                    cctnk = ccfac.CreateManagedObject();
                                    cctnk.Oid = Guid.NewGuid();
                                    cctnk.BangThuNhapKhacOld = qlcc.Oid;
                                    cctnk.BoPhan = nhanVien.NhanVien.BoPhan;
                                    cctnk.ThongTinNhanVien = nhanVien.Oid;
                                }
                                #region Ngày lập
                                string ngayChiText = dr[idx_NgayLap].ToString();
                                if (!string.IsNullOrEmpty(ngayChiText))
                                {
                                    try
                                    {
                                        DateTime ngayChi = Convert.ToDateTime(ngayChiText);
                                        //
                                        cctnk.NgayLap = ngayChi;
                                    }
                                    catch (Exception ex)
                                    {
                                        detailLog.AppendLine(" + Ngày chi không đúng định dạng: " + ngayChiText);
                                    }
                                }
                                else
                                {
                                    detailLog.AppendLine(" + Ngày chi không được trống.");
                                }
                                #endregion

                                #region Số tiền
                                String soTienText = dr[idx_SoTien].ToString();
                                if (!string.IsNullOrEmpty(soTienText))
                                {
                                    try
                                    {
                                        cctnk.SoTien = Convert.ToDecimal(soTienText);
                                    }
                                    catch { detailLog.AppendLine(" + Số tiền không đúng định dạng: " + soTienText); }
                                }
                                else
                                {
                                    detailLog.AppendLine(" + Số tiền không được trống.");
                                }
                                #endregion

                                #region Ghi chú
                                String ghiChuText = dr[idx_Ghichu].ToString();
                                if (!string.IsNullOrEmpty(ghiChuText))
                                {
                                    cctnk.GhiChu = ghiChuText;
                                }
                                else
                                {
                                    detailLog.AppendLine(" + Diễn giải không được trống.");
                                }
                                #endregion
                                //
                                #region Ghi File log
                                //Đưa thông tin bị lỗi vào blog
                                if (detailLog.Length > 0)
                                {
                                    mainLog.AppendLine(string.Format("- Không import thu nhập khác của cán bộ [{0}] vào phần mềm được: ", nhanVien.HoTen));
                                    mainLog.AppendLine(detailLog.ToString());
                                    //
                                    return 2;
                                }
                                else
                                {
                                    ccfac.SaveChanges();
                                }
                                #endregion                         
                            #endregion
                        }
                        else
                        {
                            mainLog.AppendLine(" - Không có cán bộ nào có mã quản lý [" + maQuanLyText + "]");
                            //
                            return 2;
                        }
                        #endregion
                    }

                    if (mainLog.Length > 0)
                    {

                    }
                }
            }
            return 1;
        }
        public static int ImportDataThuNhap(Guid kyTinhLuong)
        {
            int result = 1;
            SessionManager.DoWork(session =>
            {

                _workbookSet = new Workbook();
                _workbookSet.LoadDocument(_directFile, DocumentFormat.OpenXml);


                var file = _workbookSet;
                int indexSheet = 1;
                BangThuNhapKhac_Factory factory = BangThuNhapKhac_Factory.New();
                ChiTietThuNhapKhac_Factory ccfac = ChiTietThuNhapKhac_Factory.New();
                LoaiThuNhapKhac_Factory loaifac = LoaiThuNhapKhac_Factory.New();
                HoSo_Factory hsfac = HoSo_Factory.New();
                IEnumerable<HoSo> HoSoList = hsfac.GetAll_GCRecordIsNull();

                int startIndexSheet = 0;
                int countSheet = _workbookSet.Worksheets.Count;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(300)))
                {
                    try
                    {
                        for (int j = startIndexSheet; j <= countSheet; j++)
                        {
                            indexSheet = j;
                            if (indexSheet == 0)
                            {

                                int i = 2;
                                while (file.Worksheets[indexSheet].Cells["B" + i.ToString()].Value.ToString().Trim() != "")
                                {
                                    string maQuanly = file.Worksheets[indexSheet].Cells["B" + i.ToString()].Value.ToString().Trim();
                                    if (maQuanly != null && maQuanly != "")
                                    {
                                        HoSo hs = HoSoList.Where(h => h.MaQuanLy == maQuanly).FirstOrDefault();
                                        if (hs != null)
                                        {
                                            string LoaiThuNhapKhac = file.Worksheets[indexSheet].Cells["E" + i.ToString()].Value.ToString().Trim();
                                            Guid loaiThuNhap = Guid.Empty; //loaifac.GetByName(LoaiThuNhapKhac);

                                            //Kiểm tra đã có bảng thu nhập khác chưa
                                            BangThuNhapKhacOld qlcc = factory.GetByKyTinhLuongLoaiThuNhapKhac(kyTinhLuong, loaiThuNhap);

                                            //Nếu chưa có BangThuNhapKhac
                                            if (qlcc == null)
                                            {
                                                qlcc = factory.CreateManagedObject();
                                                qlcc.Oid = Guid.NewGuid();
                                                qlcc.KyTinhLuong = kyTinhLuong;
                                                qlcc.LoaiThuNhapKhacOld = loaiThuNhap;
                                                qlcc.NgayLap = DateTime.Now;
                                                factory.SaveChanges();
                                            }
                                            //Có BangThuNhapKhac và chưa có chi tiết
                                            bool tonTai = false;//ccfac.ExistByBangThuNhapKhacIDNV(qlcc.Oid, hs.Oid);
                                            if (tonTai == false)
                                            {
                                                string NgayLap = file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim();
                                                DateTime? NgayLapFormat = null;
                                                try
                                                {
                                                    NgayLapFormat = DateTime.ParseExact(NgayLap, "d/M/yyyy h:m:s tt", System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception e)
                                                {
                                                }
                                                try
                                                {
                                                    NgayLapFormat = DateTime.ParseExact(NgayLap, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception e) { }

                                                decimal SoTien = (file.Worksheets[indexSheet].Cells["G" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["G" + i.ToString()].Value.ToString().Trim()));
                                                decimal SoTienChiuThue = (file.Worksheets[indexSheet].Cells["H" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["H" + i.ToString()].Value.ToString().Trim()));
                                                string GhiChu = file.Worksheets[indexSheet].Cells["I" + i.ToString()].Value.ToString().Trim();

                                                ChiTietThuNhapKhacOld cc = new ChiTietThuNhapKhacOld();
                                                cc = ccfac.CreateManagedObject();
                                                cc.Oid = Guid.NewGuid();
                                                cc.BangThuNhapKhacOld = qlcc.Oid;
                                                cc.BoPhan = hs.NhanVien.BoPhan;
                                                cc.ThongTinNhanVien = hs.Oid;
                                                cc.NgayLap = NgayLapFormat;
                                                cc.SoTien = SoTien;
                                                cc.SoTienChiuThue = SoTienChiuThue;
                                                cc.GhiChu = GhiChu;

                                                ccfac.SaveChanges();
                                            }

                                        }
                                        i++;
                                    }
                                }
                            }
                        }
                        scope.Complete();
                    }
                    catch
                    {
                        result = 0;
                        scope.Dispose();
                    }
                    finally
                    {
                        _workbookSet = null;
                        _directFile = null;
                        if (File.Exists(_directFile))
                            File.Delete(_directFile);
                        _dbConn.Close();
                    }

                }
            });
            return result;
        }
        */
        #endregion
        #region xoa
        public static int Xoa()
        {
            int result = 0;
            _dbConn = new SqlConnection(_connectionString);
            SqlCommand dbCmd = _dbConn.CreateCommand();
            try
            {

                _dbConn.Open();
                dbCmd.CommandType = CommandType.StoredProcedure;

                dbCmd.CommandText = "sp_uis_DeleteData";


                System.Data.SqlClient.SqlParameter sPrmOut = dbCmd.CreateParameter();
                sPrmOut.ParameterName = "@ReVal";
                sPrmOut.SqlDbType = SqlDbType.Int;
                sPrmOut.Direction = ParameterDirection.Output;
                sPrmOut.Value = DBNull.Value;

                System.Data.SqlClient.SqlParameter sPrmInCollege = dbCmd.CreateParameter();
                sPrmInCollege.ParameterName = "@CollegeID";
                sPrmInCollege.SqlDbType = SqlDbType.VarChar;
                sPrmInCollege.Direction = ParameterDirection.Input;
                sPrmInCollege.Value = _collegeID;

                System.Data.SqlClient.SqlParameter sPrmYear = dbCmd.CreateParameter();
                sPrmYear.ParameterName = "@Year";
                sPrmYear.SqlDbType = SqlDbType.Int;
                sPrmYear.Direction = ParameterDirection.Input;
                sPrmYear.Value = _namThucHoc;

                System.Data.SqlClient.SqlParameter sPrmYearStudy = dbCmd.CreateParameter();
                sPrmYearStudy.ParameterName = "@YearStudy";
                sPrmYearStudy.SqlDbType = SqlDbType.VarChar;
                sPrmYearStudy.Direction = ParameterDirection.Input;
                sPrmYearStudy.Value = _namHoc;
                dbCmd.Parameters.Add(sPrmYearStudy);

                dbCmd.Parameters.Add(sPrmYear);
                dbCmd.Parameters.Add(sPrmInCollege);
                dbCmd.Parameters.Add(sPrmOut);
                dbCmd.CommandTimeout = 500;
                dbCmd.ExecuteNonQuery();

                result = int.Parse(sPrmOut.Value.ToString());

                dbCmd.Parameters.Clear();
                dbCmd.Connection.Close();


            }
            catch
            {
                result = -1;
            }
            finally
            {
                dbCmd.Parameters.Clear();
                dbCmd.Connection.Close();
                _dbConn.Close();
            }
            return result;
        }
        #endregion

    }
}