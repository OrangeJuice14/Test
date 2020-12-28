using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DevExpress.Spreadsheet;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using System.Transactions;
using NHibernate;
using System.IO;


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
        public static int ImportDataChamCong(Guid kyTinhLuong, Guid thongTinTruong)
        {
            int result = 1;
            SessionManager.DoWork(session =>
            {

                _workbookSet = new Workbook();
                _workbookSet.LoadDocument(_directFile, DocumentFormat.OpenXml);


                var file = _workbookSet;
                int indexSheet = 1;
                QuanLyChamCongNhanVien_Factory factory = QuanLyChamCongNhanVien_Factory.New();
                CC_QuanLyChamCong qlcc = factory.GetByKyTinhLuong(kyTinhLuong);
                CC_ChiTietChamCong_Factory ccfac = CC_ChiTietChamCong_Factory.New();
                //Nếu đã có QLCC
                if (qlcc != null)
                {                   
                    bool tonTai = ccfac.ExistByQuanLyChamCong(qlcc.Oid);

                    //Có QLCC và chưa có chi tiết CC
                    if (tonTai == false)
                    {
                        int startIndexSheet = 0;
                        int countSheet = _workbookSet.Worksheets.Count;

                        KyTinhLuong_Factory kyfac = KyTinhLuong_Factory.New();
                        KyTinhLuong ky = kyfac.GetObjById(kyTinhLuong);
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(120)))
                        {
                            try
                            {
                                for (int j = startIndexSheet; j <= countSheet; j++)
                                {
                                    indexSheet = j;
                                    if (indexSheet == 0)
                                    {
                                        HoSo_Factory hsfac = HoSo_Factory.New();
                                        List<HoSo> HoSoList = hsfac.GetAll_HoSo().ToList();
                                        List<CC_XepLoaiDanhGia> XepLoaiList = hsfac.GetAll_XepLoai().ToList();    
                                        int i = 2;
                                        while (file.Worksheets[indexSheet].Cells["A" + i.ToString()].Value.ToString().Trim() != "")
                                        {
                                            string maQuanly = file.Worksheets[indexSheet].Cells["A" + i.ToString()].Value.ToString().Trim();
                                            if (maQuanly != null && maQuanly != "")
                                            {                                              
                                                HoSo hs = HoSoList.Where(h=>h.MaQuanLy==maQuanly).FirstOrDefault();
                                                if (hs != null)
                                                {
                                                    decimal NgayCongChuan = (file.Worksheets[indexSheet].Cells["D" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["D" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayCongThucTe = (file.Worksheets[indexSheet].Cells["E" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["E" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayCongCangTra = (file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayCongNghi = (file.Worksheets[indexSheet].Cells["G" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["G" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayCongSuaChua = (file.Worksheets[indexSheet].Cells["H" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["H" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayCongLamLe = (file.Worksheets[indexSheet].Cells["I" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["I" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayCongNghiLe = (file.Worksheets[indexSheet].Cells["J" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["J" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayCongAnCa = (file.Worksheets[indexSheet].Cells["K" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["K" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayCongDocHai = (file.Worksheets[indexSheet].Cells["L" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["L" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayCongLamDem = (file.Worksheets[indexSheet].Cells["M" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["M" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayNghiPhep = (file.Worksheets[indexSheet].Cells["N" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["N" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayNghiKhongPhep = (file.Worksheets[indexSheet].Cells["O" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["O" + i.ToString()].Value.ToString().Trim()));
                                                    decimal NgayNghiThaiSan = (file.Worksheets[indexSheet].Cells["P" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["P" + i.ToString()].Value.ToString().Trim()));
                                                    string XepLoaiCanBo = file.Worksheets[indexSheet].Cells["Q" + i.ToString()].Value.ToString().Trim();
                                                    decimal HeSoXepLoai = (file.Worksheets[indexSheet].Cells["R" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["R" + i.ToString()].Value.ToString().Trim()));
                                                    decimal HeSoNgayCong = (file.Worksheets[indexSheet].Cells["S" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["S" + i.ToString()].Value.ToString().Trim()));

                                                    CC_ChiTietChamCong cc = new CC_ChiTietChamCong();
                                                    cc = ccfac.CreateManagedObject();
                                                    cc.Oid = Guid.NewGuid();
                                                    cc.QuanLyChamCong = qlcc.Oid;
                                                    cc.BoPhan = hs.NhanVien.Department;
                                                    cc.ThongTinNhanVien = hs.Oid;
                                                    cc.NgayCongChuan = NgayCongChuan;
                                                    cc.NgayCongThucTe = NgayCongThucTe;
                                                    cc.NgayCongCangTra = NgayCongCangTra;
                                                    cc.NgayCongNghi = NgayCongNghi;
                                                    cc.NgayCongSuaChua = NgayCongSuaChua;
                                                    cc.NgayCongLamLe = NgayCongLamLe;
                                                    cc.NgayCongNghiLe = NgayCongNghiLe;
                                                    cc.NgayCongAnCa = NgayCongAnCa;
                                                    cc.NgayCongDocHai = NgayCongDocHai;
                                                    cc.NgayCongLamDem = NgayCongLamDem;
                                                    cc.NgayNghiPhep = NgayNghiPhep;
                                                    cc.NgayNghiKhongPhep = NgayNghiKhongPhep;
                                                    cc.NgayNghiThaiSan = NgayNghiThaiSan;
                                                    cc.XepLoaiCanBo = XepLoaiCanBo == "Không xác định" ? (Guid?)null : XepLoaiList.Where(h => h.TenXepLoai == XepLoaiCanBo).FirstOrDefault().Oid;
                                                    cc.HeSoXepLoai = HeSoXepLoai;
                                                    cc.HeSoNgayCong = HeSoNgayCong;
                                                    ccfac.SaveChanges();

                                                    //string insert = string.Format("INSERT INTO ChiTietChamCongNhanVien(Oid,QuanLyChamCongNhanVien,BoPhan,ThongTinNhanVien,NgayCongChuan,NgayCongThucTe,NgayCongCangTra,NgayCongNghi,NgayCongSuaChua", "ChiTietChamCongNhanVien");
                                                    //session.CreateSQLQuery(insert).ExecuteUpdate();
                                                    i++;
                                                }
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
                    }

                }
                else
                {
                    int startIndexSheet = 0;
                    int countSheet = _workbookSet.Worksheets.Count;

                    KyTinhLuong_Factory kyfac = KyTinhLuong_Factory.New();
                    KyTinhLuong ky = kyfac.GetObjById(kyTinhLuong);


                    CC_QuanLyChamCong ql = new CC_QuanLyChamCong();
                    ql = factory.CreateManagedObject();
                    ql.Oid = Guid.NewGuid();
                    ql.KyTinhLuong = kyTinhLuong;
                    ql.NgayLap = DateTime.Today;
                    ql.CompanyInfo = thongTinTruong;
                    factory.SaveChanges();

                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            for (int j = startIndexSheet; j <= countSheet; j++)
                            {
                                indexSheet = j;
                                if (indexSheet == 0)
                                {

                                    int i = 2;
                                    while (file.Worksheets[indexSheet].Cells["A" + i.ToString()].Value.ToString().Trim() != "")
                                    {
                                        string maQuanly = file.Worksheets[indexSheet].Cells["B" + i.ToString()].Value.ToString().Trim();
                                        if (maQuanly != null && maQuanly != "")
                                        {
                                            HoSo_Factory hsfac = HoSo_Factory.New();
                                            HoSo hs = hsfac.GetByManageCode(maQuanly);
                                            if (hs != null)
                                            {
                                                decimal NgayCongChuan = (file.Worksheets[indexSheet].Cells["D" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["D" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayCongThucTe = (file.Worksheets[indexSheet].Cells["E" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["E" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayCongCangTra = (file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayCongNghi = (file.Worksheets[indexSheet].Cells["G" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["G" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayCongSuaChua = (file.Worksheets[indexSheet].Cells["H" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["H" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayCongLamLe = (file.Worksheets[indexSheet].Cells["I" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["I" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayCongNghiLe = (file.Worksheets[indexSheet].Cells["J" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["J" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayCongAnCa = (file.Worksheets[indexSheet].Cells["K" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["K" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayCongDocHai = (file.Worksheets[indexSheet].Cells["L" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["L" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayCongLamDem = (file.Worksheets[indexSheet].Cells["M" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["M" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayNghiPhep = (file.Worksheets[indexSheet].Cells["N" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["N" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayNghiKhongPhep = (file.Worksheets[indexSheet].Cells["O" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["O" + i.ToString()].Value.ToString().Trim()));
                                                decimal NgayNghiThaiSan = (file.Worksheets[indexSheet].Cells["P" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["P" + i.ToString()].Value.ToString().Trim()));
                                                string XepLoaiCanBo = file.Worksheets[indexSheet].Cells["Q" + i.ToString()].Value.ToString().Trim();
                                                //decimal HeSoXepLoai = (file.Worksheets[indexSheet].Cells["R" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["R" + i.ToString()].Value.ToString().Trim()));
                                                //decimal HeSoNgayCong = (file.Worksheets[indexSheet].Cells["S" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(file.Worksheets[indexSheet].Cells["S" + i.ToString()].Value.ToString().Trim()));

                                                CC_ChiTietChamCong cc = new CC_ChiTietChamCong();
                                                cc = ccfac.CreateManagedObject();
                                                cc.Oid = Guid.NewGuid();
                                                cc.QuanLyChamCong = qlcc.Oid;
                                                cc.BoPhan = hs.NhanVien.Department1.Oid;
                                                cc.ThongTinNhanVien = hs.Oid;
                                                cc.NgayCongChuan = NgayCongChuan;
                                                cc.NgayCongThucTe = NgayCongThucTe;
                                                cc.NgayCongCangTra = NgayCongCangTra;
                                                cc.NgayCongNghi = NgayCongNghi;
                                                cc.NgayCongSuaChua = NgayCongSuaChua;
                                                cc.NgayCongLamLe = NgayCongLamLe;
                                                cc.NgayCongNghiLe = NgayCongNghiLe;
                                                cc.NgayCongAnCa = NgayCongAnCa;
                                                cc.NgayCongDocHai = NgayCongDocHai;
                                                cc.NgayCongLamDem = NgayCongLamDem;
                                                cc.NgayNghiPhep = NgayNghiPhep;
                                                cc.NgayNghiKhongPhep = NgayNghiKhongPhep;
                                                cc.NgayNghiThaiSan = NgayNghiThaiSan;
                                                cc.XepLoaiCanBo = hsfac.GetIdXepLoaiByName(XepLoaiCanBo).Oid;
                                                //cc.HeSoXepLoai = HeSoXepLoai;
                                                //cc.HeSoNgayCong = HeSoNgayCong;
                                                ccfac.SaveChanges();

                                                //string insert = string.Format("INSERT INTO ChiTietChamCongNhanVien(Oid,QuanLyChamCongNhanVien,BoPhan,ThongTinNhanVien,NgayCongChuan,NgayCongThucTe,NgayCongCangTra,NgayCongNghi,NgayCongSuaChua", "ChiTietChamCongNhanVien");
                                                //session.CreateSQLQuery(insert).ExecuteUpdate();
                                                i++;
                                            }
                                        }
                                    }
                                }
                            }
                            scope.Complete();
                        }
                        catch
                        {
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
                }
            });
            return result;
        }
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

    }
}