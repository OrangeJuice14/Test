using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DevExpress.Spreadsheet;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate.Linq;
using System.IO;
using HRMWebApp.KPI.Core.DTO;

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
                                // double record = (file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim() == string.Empty ? 0 : Convert.ToDouble(file.Worksheets[indexSheet].Cells["F" + i.ToString()].Value.ToString().Trim()));
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

        public static int ImportDataOtherActivity(NHibernate.ISession session)
        {
            int result = 0;
            try
            {
                string deleteAll = string.Format("DELETE FROM {0}", "KPI_OtherActivityDataDetail");
                session.CreateSQLQuery(deleteAll).ExecuteUpdate();

                _workbookSet = new Workbook();
                _workbookSet.LoadDocument(_directFile, DocumentFormat.OpenXml);


                var file = _workbookSet;
                int indexSheet = 1;


                int startIndexSheet = 0;
                int countSheet = _workbookSet.Worksheets.Count;


                for (int j = startIndexSheet; j < countSheet; j++)
                {
                    indexSheet = j;

                    #region //Sheet 0 -- Thong Ke Tuyen Moi
                    if (indexSheet == 0)
                    {
                        int rowIndex = 2;

                     //   List<OtherActivityData> listHDK = session.Query<OtherActivityData>().ToList();
                        while (file.Worksheets[indexSheet].Cells["A" + rowIndex.ToString()].Value.ToString().Trim() != "")
                        {
                            string MaCanBo = file.Worksheets[indexSheet].Cells["A" + rowIndex.ToString()].Value.ToString().Trim();
                            string TenCanBo = file.Worksheets[indexSheet].Cells["B" + rowIndex.ToString()].Value.ToString().Trim();
                            double SoLan = Convert.ToDouble(file.Worksheets[indexSheet].Cells["C" + rowIndex.ToString()].Value.ToString().Trim());

                            Staff staff = session.Query<Staff>().Where(s => s.StaffInfo.ManageCode == MaCanBo).SingleOrDefault();
                                if (staff != null)
                                {
                                OtherActivityDataDetail srd = new OtherActivityDataDetail()
                                {
                                    Id = Guid.NewGuid(),
                                    StaffCode = MaCanBo,
                                    StaffName = TenCanBo,
                                    NumberOfTime = SoLan,
                                    Khoa = new Department { Id = staff.Department.Id },
                                    IdCanBo = staff.Id,
                                    };
                                    session.Save(srd);
                                }
                                rowIndex++;
                        }
                        #endregion
                    }
                }
            }
            catch
            {
                result = 1;
            }
            finally
            {
                _workbookSet = null;
                _directFile = null;
                if (File.Exists(_directFile))
                    File.Delete(_directFile);
                _dbConn.Close();
            }
            return result;
        }

        public static int ImportDataMTCL(NHibernate.ISession session)
        {
            int result = 0;
            try
            {
                string deleteAll = string.Format("DELETE FROM {0}", "KPI_MTCLDetail");
                session.CreateSQLQuery(deleteAll).ExecuteUpdate();

                _workbookSet = new Workbook();
                _workbookSet.LoadDocument(_directFile, DocumentFormat.OpenXml);


                var file = _workbookSet;
                int indexSheet = 1;


                int startIndexSheet = 0;
                int countSheet = _workbookSet.Worksheets.Count;


                for (int j = startIndexSheet; j < countSheet; j++)
                {
                    indexSheet = j;

                    #region //Sheet 0
                    if (indexSheet == 0)
                    {
                        int rowIndex = 2;
                        while (file.Worksheets[indexSheet].Cells["A" + rowIndex.ToString()].Value.ToString().Trim() != "")
                        {
                            string MaCanBo = file.Worksheets[indexSheet].Cells["A" + rowIndex.ToString()].Value.ToString().Trim();
                            string TenCanBo = file.Worksheets[indexSheet].Cells["B" + rowIndex.ToString()].Value.ToString().Trim();
                            double SoDiem = Convert.ToDouble(file.Worksheets[indexSheet].Cells["C" + rowIndex.ToString()].Value.ToString().Trim());

                            Staff staff = session.Query<Staff>().Where(s => s.StaffInfo.ManageCode == MaCanBo).SingleOrDefault();
                            if (staff != null)
                            {
                                MTCLDetail srd = new MTCLDetail()
                                {
                                    Id = Guid.NewGuid(),
                                    MaCanBo = MaCanBo,
                                    TenCanBo = TenCanBo,
                                    SoDiem = SoDiem,
                                    Khoa = new Department { Id = staff.Department.Id },
                                    IdCanBo = new Staff { Id = staff.Id },
                            };
                                session.Save(srd);
                            }
                            rowIndex++;
                        }
                        #endregion
                    }
                }
            }
            catch
            {
                result = 1;
            }
            finally
            {
                _workbookSet = null;
                _directFile = null;
                if (File.Exists(_directFile))
                    File.Delete(_directFile);
                _dbConn.Close();
            }
            return result;
        }

        public static int ImportDataDiemthuongdiemtru(NHibernate.ISession session)
        {
            int result = 0;
            try
            {
                string deleteAll = string.Format("DELETE FROM {0}", "KPI_DiemThuongDiemTruMaster");
                session.CreateSQLQuery(deleteAll).ExecuteUpdate();

                _workbookSet = new Workbook();
                _workbookSet.LoadDocument(_directFile, DocumentFormat.OpenXml);


                var file = _workbookSet;
                int indexSheet = 1;


                int startIndexSheet = 0;
                int countSheet = _workbookSet.Worksheets.Count;


                for (int j = startIndexSheet; j < countSheet; j++)
                {
                    indexSheet = j;

                    #region //Sheet 0 
                    if (indexSheet == 0)
                    {
                        int rowIndex = 2;

                        while (file.Worksheets[indexSheet].Cells["A" + rowIndex.ToString()].Value.ToString().Trim() != "")
                        {
                            string MaCanBo = file.Worksheets[indexSheet].Cells["A" + rowIndex.ToString()].Value.ToString().Trim();
                            string TenCanBo = file.Worksheets[indexSheet].Cells["B" + rowIndex.ToString()].Value.ToString().Trim();
                            string NoiDung = file.Worksheets[indexSheet].Cells["C" + rowIndex.ToString()].Value.ToString().Trim();
                            double SoDiem = Convert.ToDouble(file.Worksheets[indexSheet].Cells["D" + rowIndex.ToString()].Value.ToString().Trim());

                            Staff staff = session.Query<Staff>().Where(s => s.StaffInfo.ManageCode == MaCanBo).SingleOrDefault();
                            
                                if (staff != null)
                                {
                                    DiemThuongDiemTruMaster srd = new DiemThuongDiemTruMaster()
                                    {
                                        Id = Guid.NewGuid(),
                                        MaCanBo = MaCanBo,
                                        TenCanBo = TenCanBo,
                                        NoiDung = NoiDung,
                                        SoDiem = SoDiem,
                                        Khoa = new Department { Id = staff.Department.Id },
                                    };
                                    session.Save(srd);
                                }
                                rowIndex++;
                        }
                        #endregion
                    }
                }
            }
            catch
            {
                result = 1;
            }
            finally
            {
                _workbookSet = null;
                _directFile = null;
                if (File.Exists(_directFile))
                    File.Delete(_directFile);
                _dbConn.Close();
            }
            return result;
        }

        public static int ImportDataGiangDay(NHibernate.ISession session)
        {
            int result = 0;
            try
            {
                string deleteAll = string.Format("DELETE FROM {0}", "KPI_DoDuLieuGiangDay");
                session.CreateSQLQuery(deleteAll).ExecuteUpdate();

                Workbook _workbookSet = new Workbook();
                _workbookSet.LoadDocument(_directFile, DocumentFormat.OpenXml);

                var file = _workbookSet;
                int indexSheet = 1;             

                int startIndexSheet = 0;
                int countSheet = _workbookSet.Worksheets.Count;
                for (int j = startIndexSheet; j <= countSheet + 1; j++)
                {
                    indexSheet = j;
                    
                    if (indexSheet == 0)
                    {
                        string NamHoc = file.Worksheets[indexSheet].Cells["B" + 2.ToString()].Value.ToString().Trim();
                        string HocKy = file.Worksheets[indexSheet].Cells["B" + 3.ToString()].Value.ToString().Trim();
                        string CreationTime = file.Worksheets[indexSheet].Cells["B" + 4.ToString()].Value.ToString().Trim();
                        string MaTieuChi = file.Worksheets[indexSheet].Cells["B" + 5.ToString()].Value.ToString().Trim();
                        string LoaiGiaTri = file.Worksheets[indexSheet].Cells["B" + 6.ToString()].Value.ToString().Trim();
                        string DonViCungCap = file.Worksheets[indexSheet].Cells["B" + 7.ToString()].Value.ToString().Trim();
                        string DuLieuMinhChung = file.Worksheets[indexSheet].Cells["B" + 8.ToString()].Value.ToString().Trim();

                        ProfessorCriterion professor = session.Query<ProfessorCriterion>().Where(r => MaTieuChi == r.ManageCode).FirstOrDefault();
                        List<CriterionDictionary> Criterion = session.Query<CriterionDictionary>().Where(r => r.TargetGroupDetail.Id == new Guid("00000000-0000-0000-0000-000000000001") || professor.Id == r.ProfessorCriterion.Id).ToList();
                        var datmuc = "";
                        var CriterionDictionaryId = new Guid();

                        int i = 11; //rowindex                           
                        while (file.Worksheets[indexSheet].Cells["A" + i.ToString()].Value.ToString().Trim() != "")
                        {
                            string MaCanBo = file.Worksheets[indexSheet].Cells["A" + i.ToString()].Value.ToString().Trim();
                            string TenCanBo = file.Worksheets[indexSheet].Cells["B" + i.ToString()].Value.ToString().Trim();
                            double GiaTriThuc = Convert.ToDouble(file.Worksheets[indexSheet].Cells["C" + i.ToString()].Value.ToString().Trim());
                            string GhiChu = file.Worksheets[indexSheet].Cells["D" + i.ToString()].Value.ToString().Trim();

                            var NhanVien = session.Query<StaffInfo>().Where(r => r.ManageCode == MaCanBo).Select(r => r.Id).FirstOrDefault();
                            Staff staff = session.Query<Staff>().Where(s => s.StaffInfo.ManageCode == MaCanBo).SingleOrDefault();
                            foreach (CriterionDictionary cri in Criterion)
                            {
                                if ((GiaTriThuc >= cri.DataRecord && GiaTriThuc <= cri.DataMaxRecord))
                                {
                                    datmuc = cri.Name;
                                    CriterionDictionaryId = cri.Id;
                                }
                            }

                            DoDuLieuDanhGia srd = new DoDuLieuDanhGia()
                            {
                                Id = Guid.NewGuid(),
                                MaCanBo = MaCanBo,
                                TenCanBo = TenCanBo,
                                IdNhanVien = new StaffInfo() { Id = NhanVien },
                                GiaTriThuc = GiaTriThuc,
                                GhiChu = GhiChu,
                                DonVi = new Department { Id = staff.Department.Id },
                                NamHoc = NamHoc,
                                HocKy = HocKy,
                                CreationTime = DateTime.Parse(CreationTime),
                                MaTieuChi = MaTieuChi,
                                LoaiGiaTri = LoaiGiaTri,
                                DonViCungCap = DonViCungCap,
                                DuLieuMinhChung = DuLieuMinhChung,
                                DatMuc = datmuc,
                                CriterionDictionaryId = new CriterionDictionary() { Id = CriterionDictionaryId },
                            };
                            session.Save(srd);
                            i++;

                        }
                    }
                    #endregion                   
                }

            }
            catch
            {
                result = 2;
            }
            finally
            {
                _workbookSet = null;
                _directFile = null;
                if (File.Exists(_directFile))
                    File.Delete(_directFile);
                _dbConn.Close();
            }
            return result;
        }
        

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