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
        public static String NghiConOmKyHieu = "Cô";

        public static Guid NghiTaiNanId = new Guid("00000000-0000-0000-0000-000000000003");
        public static String NghiTaiNanKyHieu = "T";

        public static Guid NghiHocTapId = new Guid("00000000-0000-0000-0000-000000000004");
        public static String NghiHocTapKyHieu = "H";

        public static Guid NghiNgungViecId = new Guid("00000000-0000-0000-0000-000000000005");
        public static String NghiNgungViecKyHieu = "N";

        public static Guid NghiBuId = new Guid("00000000-0000-0000-0000-000000000006");
        public static String NghiBuKyHieu = "NB";

        public static Guid NghiLaoDongId = new Guid("00000000-0000-0000-0000-000000000007");
        public static String NghiLaoDongKyHieu = "LĐ";

        public static Guid DiCongTacId = new Guid("00000000-0000-0000-0000-000000000008");
        public static String DiCongTacKyHieu = "CT";

        public static Guid NghiKhongPhepRoId = new Guid("A24CDBDA-9F44-4C95-B3A1-1F875714E07B");
        public static String NghiKhongPhepRoKyHieu = "Ro";

        public static Guid LamNuaNgayId = new Guid("32EDECC3-9B58-4ED5-B359-34B5416BB3C8");
        public static String LamNuaNgayKyHieu = "P/2";

        public static Guid NghiPhepId = new Guid("C70EBAFA-1A1B-4625-98FB-68FE64489B46");
        public static String NghiPhepKyHieu = "P";

        public static Guid NghiNuaNgayId = new Guid("3E0D2543-DC65-4E0D-A887-EB5CB4871E54");
        public static String NghiNuaNgayKyHieu = "1/2";

        public static Guid NghiThaiSanId = new Guid("0E731720-7CBC-403C-BF20-FF9C63721E8F");
        public static String NghiThaiSanKyHieu = "TS";

        public static String LamCaNgayKyHieu = "+";

        public static Guid KhongXacDinhId = new Guid("DAA9EA01-92F7-4EC8-928E-9065ACD156F5");
        public static String KhongXacDinhKyHieu = "";

        public static System.Collections.Generic.Dictionary<String, Guid> HinhThucNghiDictionaryKyHieuToId;
        public static System.Collections.Generic.Dictionary<Guid, String> HinhThucNghiDictionaryIdToKyHieu;

        static HinhThucNghiConst()
        {//khoi tao dictionary
            HinhThucNghiDictionaryKyHieuToId = new Dictionary<string, Guid>();
            HinhThucNghiDictionaryKyHieuToId.Add(NghiOmKyHieu, NghiOmId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiConOmKyHieu, NghiConOmId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiTaiNanKyHieu, NghiTaiNanId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHocTapKyHieu, NghiHocTapId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiNgungViecKyHieu, NghiNgungViecId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiBuKyHieu, NghiBuId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiLaoDongKyHieu, NghiLaoDongId);
            HinhThucNghiDictionaryKyHieuToId.Add(DiCongTacKyHieu, DiCongTacId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiKhongPhepRoKyHieu, NghiKhongPhepRoId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiKhongPhepRoKyHieu.ToUpper(), NghiKhongPhepRoId);
            HinhThucNghiDictionaryKyHieuToId.Add(LamNuaNgayKyHieu, LamNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiPhepKyHieu, NghiPhepId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiNuaNgayKyHieu, NghiNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiThaiSanKyHieu, NghiThaiSanId);
            HinhThucNghiDictionaryKyHieuToId.Add(LamCaNgayKyHieu, Guid.Empty);
            HinhThucNghiDictionaryKyHieuToId.Add(KhongXacDinhKyHieu, KhongXacDinhId);

            HinhThucNghiDictionaryIdToKyHieu = new Dictionary<Guid, string>();
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiOmId, NghiOmKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiConOmId, NghiConOmKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiTaiNanId, NghiTaiNanKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHocTapId, NghiHocTapKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiNgungViecId, NghiNgungViecKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiBuId, NghiBuKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiLaoDongId, NghiLaoDongKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(DiCongTacId, DiCongTacKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(LamNuaNgayId, LamNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiKhongPhepRoId, NghiKhongPhepRoKyHieu);

            HinhThucNghiDictionaryIdToKyHieu.Add(NghiPhepId, NghiPhepKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiNuaNgayId, NghiNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiThaiSanId, NghiThaiSanKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Guid.Empty, LamCaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(KhongXacDinhId, KhongXacDinhKyHieu);

        }
    }
}