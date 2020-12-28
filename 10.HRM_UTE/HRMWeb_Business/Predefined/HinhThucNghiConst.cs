using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Business.Predefined
{
    public static class HinhThucNghiConst
    {
        public static Guid NghiConOmId = new Guid("71F67A43-FF76-4A87-B1C6-29B71718FD9E");
        public static String NghiConOmKyHieu = "CÔ";

        public static Guid NghiDamCuoiId = new Guid("FC5394B0-E7CD-4FF6-829E-2DFE09304906");
        public static String NghiDamCuoiKyHieu = "ĐC";

        public static Guid NghiViecRiengId = new Guid("B9178FBB-BDEF-4C8F-A141-3F20F8007332");
        public static String NghiViecRiengKyHieu = "R";

        public static Guid NghiHocNuocNgoaiId = new Guid("C66B90C6-CD10-46BA-BE7F-44ACC520D7E9");
        public static String NghiHocNuocNgoaiKyHieu = "N";

        public static Guid NghiHocNuocNgoaiKLId = new Guid("00000000-0000-0000-0000-000000000001");
        public static String NghiHocNuocNgoaiKLKyHieu = "NK";

        public static Guid NghiDamTangId = new Guid("1AAACCD5-2681-45D2-9902-59F9F4964CF2");
        public static String NghiDamTangKyHieu = "ĐT";

        public static Guid NghiNuaNgayId = new Guid("2A6811E7-D012-48D8-A18D-5A20E3012F6C");
        public static String NghiNuaNgayKyHieu = "-";

        public static Guid NghiLeId = new Guid("9677F388-706D-4F0A-B5E9-81F611B6054C");
        public static String NghiLeKyHieu = "L";

        public static Guid KhongXacDinhId = new Guid("DAA9EA01-92F7-4EC8-928E-9065ACD156F5");
        public static String KhongXacDinhKyHieu = "";   

        public static Guid NghiKhacId = new Guid("E3532E79-4A82-400F-9CE7-90C90FF328A9");
        public static String NghiKhacKyHieu = "K";

        public static Guid NghiPhepId = new Guid("E04E2C42-2EB1-4007-89C7-AD8A52BA4E74");
        public static String NghiPhepKyHieu = "P";

        public static Guid NghiPhepBuoiSangId = new Guid("BE69AD27-147E-4B30-A485-FD95971BAABD");
        public static String NghiPhepBuoiSangKyHieu = "P-";

        public static Guid NghiPhepBuoiChieuId = new Guid("84DBA03D-A4B3-4E7F-92BA-FDAF6540616A");
        public static String NghiPhepBuoiChieuKyHieu = "-P";

        public static Guid NghiOmId = new Guid("756195EE-F0B9-4684-B56B-AFE720B5E8D4");
        public static String NghiOmKyHieu = "Ô";

        public static Guid NghiKhamThaiId = new Guid("57A861D4-C344-4197-BFFB-B2143A931ABB");
        public static String NghiKhamThaiKyHieu = "KT";

        public static Guid TrucDemId = new Guid("7641C776-8ADF-48BA-8E1A-BD2C1C928B2B");
        public static String TrucDemKyHieu = "Đ";

        public static Guid NghiHocTrongNuocId = new Guid("78502FD1-7734-4EA1-A7DC-C360EAFA251B");
        public static String NghiHocTrongNuocKyHieu = "T";

        public static Guid NghiHocTrongNuocKLId = new Guid("00000000-0000-0000-0000-000000000002");
        public static String NghiHocTrongNuocKLKyHieu = "TK";

        public static Guid TamGiuLuongId = new Guid("C646F20D-4EB1-4334-831E-C3DBF58868A0");
        public static String TamGiuLuongKyHieu = "TGL";

        public static Guid NghiKhongPhepRoId = new Guid("2B5E4D2D-6CF6-4AA7-8C34-FBFC1ED64BEA");
        public static String NghiKhongPhepRoKyHieu = "O";
         
        public static Guid NghiThaiSanId = new Guid("5DB87591-C13E-475E-B4FF-FF730E7EA0B6");
        public static String NghiThaiSanKyHieu = "TS";

        public static Guid NghiVoSinhThuongId = new Guid("211E6140-0A75-45C8-9993-ED50E42DAF4D");
        public static String NghiVoSinhThuongKyHieu = "NVS-ST";

        public static Guid NghiVoSinhMoId = new Guid("DECE8614-D6D8-460B-B365-863B4F611324");
        public static String NghiVoSinhMoKyHieu = "NVS-SM";

        public static Guid Ca1Id = new Guid("7837A6A7-46A5-4642-A150-3558E85E58A3");
        public static String Ca1KyHieu = "C1";

        public static Guid Ca2Id = new Guid("AC2962DF-D394-4FFA-A6E9-88C1E91BD890");
        public static String Ca2KyHieu = "C2";

        public static Guid Ca3Id = new Guid("03B07BAC-01B7-4B48-8D48-A68C186CDBF5");
        public static String Ca3KyHieu = "C3";

        public static Guid Ca13Id = new Guid("8FEBC5A9-7529-4162-BF05-9A1D7C5A86B8");
        public static String Ca13KyHieu = "C1C3";

        public static Guid Ca12Id = new Guid("5F27ADC8-B439-4B9F-A226-F0AF24B26F9F");
        public static String Ca12KyHieu = "C1C2";

        public static Guid Ca23Id = new Guid("CECB59CC-3FF2-42AC-B057-E30C13BD36D1");
        public static String Ca23KyHieu = "C2C3";

        public static String LamCaNgayKyHieu = "+";

       

        public static System.Collections.Generic.Dictionary<String, Guid> HinhThucNghiDictionaryKyHieuToId;
        public static System.Collections.Generic.Dictionary<Guid, String> HinhThucNghiDictionaryIdToKyHieu;

        static HinhThucNghiConst()
        {//khoi tao dictionary
            HinhThucNghiDictionaryKyHieuToId = new Dictionary<string, Guid>();
            HinhThucNghiDictionaryKyHieuToId.Add(NghiConOmKyHieu, NghiConOmId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiDamCuoiKyHieu, NghiDamCuoiId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHocNuocNgoaiKLKyHieu, NghiHocNuocNgoaiKLId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiViecRiengKyHieu, NghiViecRiengId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHocNuocNgoaiKyHieu, NghiHocNuocNgoaiId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiDamTangKyHieu, NghiDamTangId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiNuaNgayKyHieu, NghiNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiLeKyHieu, NghiLeId);
            HinhThucNghiDictionaryKyHieuToId.Add(KhongXacDinhKyHieu, KhongXacDinhId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiKhacKyHieu, NghiKhacId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiPhepKyHieu, NghiPhepId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiPhepBuoiSangKyHieu, NghiPhepBuoiSangId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiPhepBuoiChieuKyHieu, NghiPhepBuoiChieuId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiOmKyHieu, NghiOmId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiKhamThaiKyHieu, NghiKhamThaiId);
            HinhThucNghiDictionaryKyHieuToId.Add(TrucDemKyHieu, TrucDemId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHocTrongNuocKyHieu, NghiHocTrongNuocId);
            HinhThucNghiDictionaryKyHieuToId.Add(TamGiuLuongKyHieu, TamGiuLuongId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHocTrongNuocKLKyHieu, NghiHocTrongNuocKLId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiKhongPhepRoKyHieu, NghiKhongPhepRoId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiThaiSanKyHieu, NghiThaiSanId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiVoSinhThuongKyHieu, NghiVoSinhThuongId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiVoSinhMoKyHieu, NghiVoSinhMoId);
            HinhThucNghiDictionaryKyHieuToId.Add(LamCaNgayKyHieu, Guid.Empty);
            //
            HinhThucNghiDictionaryKyHieuToId.Add(Ca1KyHieu, Ca1Id);
            HinhThucNghiDictionaryKyHieuToId.Add(Ca2KyHieu, Ca2Id);
            HinhThucNghiDictionaryKyHieuToId.Add(Ca3KyHieu, Ca3Id);
            HinhThucNghiDictionaryKyHieuToId.Add(Ca12KyHieu, Ca12Id);
            HinhThucNghiDictionaryKyHieuToId.Add(Ca13KyHieu, Ca13Id);
            HinhThucNghiDictionaryKyHieuToId.Add(Ca23KyHieu, Ca23Id);


            HinhThucNghiDictionaryIdToKyHieu = new Dictionary<Guid, string>();
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiConOmId, NghiConOmKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiDamCuoiId, NghiDamCuoiKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHocNuocNgoaiKLId, NghiHocNuocNgoaiKLKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiViecRiengId, NghiViecRiengKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHocNuocNgoaiId, NghiHocNuocNgoaiKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiDamTangId, NghiDamTangKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiNuaNgayId, NghiNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiLeId, NghiLeKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(KhongXacDinhId, KhongXacDinhKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiKhacId, NghiKhacKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiPhepId, NghiPhepKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiPhepBuoiSangId, NghiPhepBuoiSangKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiPhepBuoiChieuId, NghiPhepBuoiChieuKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiOmId, NghiOmKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiKhamThaiId, NghiKhamThaiKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(TrucDemId, TrucDemKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHocTrongNuocId, NghiHocTrongNuocKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(TamGiuLuongId, TamGiuLuongKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHocTrongNuocKLId, NghiHocTrongNuocKLKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiKhongPhepRoId, NghiKhongPhepRoKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiThaiSanId, NghiThaiSanKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiVoSinhThuongId, NghiVoSinhThuongKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiVoSinhMoId, NghiVoSinhMoKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Guid.Empty, LamCaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Ca1Id, Ca1KyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Ca2Id, Ca2KyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Ca3Id, Ca3KyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Ca12Id, Ca12KyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Ca13Id, Ca13KyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Ca23Id, Ca23KyHieu);
        }
    }
}