using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Business.Predefined
{
    public static class HinhThucNghiConst
    { 
        public static Guid NghiOmId = new Guid("00000000-0000-0000-0000-000000000001");
        public static String NghiOmKyHieu = "Ô";
        public static Guid NghiConOmId = new Guid("00000000-0000-0000-0000-000000000002");
        public static String NghiConOmKyHieu = "CÔ";
        public static Guid NghiThaiSanId = new Guid("00000000-0000-0000-0000-000000000003");
        public static String NghiThaiSanKyHieu = "TS";
        public static Guid NghiTaiNanId = new Guid("00000000-0000-0000-0000-000000000004");
        public static String NghiTaiNanKyHieu = "T";
        public static Guid NghiPhepId = new Guid("00000000-0000-0000-0000-000000000005");
        public static String NghiPhepKyHieu = "P";
        public static Guid NghiCongTacId = new Guid("00000000-0000-0000-0000-000000000006");
        public static String NghiCongTacKyHieu = "C";
        public static Guid NghiHoiNghi_HocTapId = new Guid("00000000-0000-0000-0000-000000000007");
        public static String NghiHoiNghi_HocTapKyHieu = "H";
        public static Guid NghiBuId = new Guid("00000000-0000-0000-0000-000000000008");
        public static String NghiBuKyHieu = "Nb";
        public static Guid NghiLeId = new Guid("00000000-0000-0000-0000-000000000009");
        public static String NghiLeKyHieu = "L";
        public static Guid NghiKhongLuongId = new Guid("00000000-0000-0000-0000-000000000010");
        public static String NghiKhongLuongKyHieu = "No";
        public static Guid NgungViecId = new Guid("00000000-0000-0000-0000-000000000011");
        public static String NgungViecKyHieu = "N";
        public static Guid LaoDongNghiaVuId = new Guid("00000000-0000-0000-0000-000000000012");
        public static String LaoDongNghiaVuKyHieu = "Lđ";
        public static Guid HocNuocNgoaiId = new Guid("00000000-0000-0000-0000-000000000013");
        public static String HocNuocNgoaiKyHieu = "HNN";
        public static Guid KhongXacDinhId = new Guid("00000000-0000-0000-0000-000000000014");
        public static String KhongXacDinhKyHieu = "";
        //
        public static String LamCaNgayKyHieu = "+";

        public static System.Collections.Generic.Dictionary<String, Guid> HinhThucNghiDictionaryKyHieuToId;
        public static System.Collections.Generic.Dictionary<Guid, String> HinhThucNghiDictionaryIdToKyHieu;

        static HinhThucNghiConst()
        {

            //Id
            HinhThucNghiDictionaryIdToKyHieu = new Dictionary<Guid, string>();
            HinhThucNghiDictionaryIdToKyHieu.Add(KhongXacDinhId, KhongXacDinhKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiOmId, NghiOmKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiConOmId, NghiConOmKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiThaiSanId, NghiThaiSanKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiTaiNanId, NghiTaiNanKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiPhepId, NghiPhepKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiCongTacId, NghiCongTacKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHoiNghi_HocTapId, NghiHoiNghi_HocTapKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiBuId, NghiBuKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiLeId, NghiLeKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiKhongLuongId, NghiKhongLuongKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NgungViecId, NgungViecKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(LaoDongNghiaVuId, LaoDongNghiaVuKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(HocNuocNgoaiId, HocNuocNgoaiKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Guid.Empty, LamCaNgayKyHieu);
            //

            //Ký hiệu
            HinhThucNghiDictionaryKyHieuToId = new Dictionary<string, Guid>();
            HinhThucNghiDictionaryKyHieuToId.Add(KhongXacDinhKyHieu, KhongXacDinhId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiOmKyHieu, NghiOmId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiConOmKyHieu, NghiConOmId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiThaiSanKyHieu, NghiThaiSanId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiTaiNanKyHieu, NghiTaiNanId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiPhepKyHieu, NghiPhepId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiCongTacKyHieu, NghiCongTacId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHoiNghi_HocTapKyHieu, NghiHoiNghi_HocTapId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiBuKyHieu, NghiBuId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiLeKyHieu, NghiLeId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiKhongLuongKyHieu, NghiKhongLuongId);
            HinhThucNghiDictionaryKyHieuToId.Add(NgungViecKyHieu, NgungViecId);
            HinhThucNghiDictionaryKyHieuToId.Add(LaoDongNghiaVuKyHieu, LaoDongNghiaVuId);
            HinhThucNghiDictionaryKyHieuToId.Add(HocNuocNgoaiKyHieu, HocNuocNgoaiId);
            HinhThucNghiDictionaryKyHieuToId.Add(LamCaNgayKyHieu, Guid.Empty);

        }
    }
}