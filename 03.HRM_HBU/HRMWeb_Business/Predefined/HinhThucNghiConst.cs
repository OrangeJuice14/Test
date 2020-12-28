using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Business.Predefined
{
    public static class HinhThucNghiConst
    {
        //00000000-0000-0000-0000-000000000001	Ô Nghỉ ốm, điều dưỡng
        //00000000-0000-0000-0000-000000000002	Cô Nghỉ con ốm
        //00000000-0000-0000-0000-000000000003	T Nghỉ tai nạn
        //00000000-0000-0000-0000-000000000004	Ho Nghỉ hội nghị, học tập
        //00000000-0000-0000-0000-000000000005	N Nghỉ ngừng việc
        //00000000-0000-0000-0000-000000000006	NB Nghỉ bù
        //00000000-0000-0000-0000-000000000007	LĐ Nghỉ lao động nghĩa vụ
        //E609A972-0D11-48AD-B3AA-0845EE549C40 H   Nghỉ hè
        //A24CDBDA-9F44-4C95-B3A1-1F875714E07B Ro  Nghỉ không lương(không phép)
        //32EDECC3-9B58-4ED5-B359-34B5416BB3C8	P/2	Làm nửa ngày
        //C70EBAFA-1A1B-4625-98FB-68FE64489B46 P   Nghỉ phép
        //DAA9EA01-92F7-4EC8-928E-9065ACD156F5 Không xác định
        //3E0D2543-DC65-4E0D-A887-EB5CB4871E54	1/2	Nghỉ nửa ngày
        //0E731720-7CBC-403C-BF20-FF9C63721E8F TS  Nghỉ thai sản
        public static Guid NghiOmId = new Guid("00000000-0000-0000-0000-000000000001");
        public static String NghiOmKyHieu = "Ô";

        public static Guid NghiConOmId = new Guid("00000000-0000-0000-0000-000000000002");
        public static String NghiConOmKyHieu = "CÔ";

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

        public static Guid DiCTNuaNgayId = new Guid("00000000-0000-0000-0000-000000000009");
        public static String DiCTNuaNgayKyHieu = "CT/2";

        public static Guid NghiRoNuaNgayId = new Guid("00000000-0000-0000-0000-000000000010");
        public static String NghiRoNuaNgayKyHieu = "RO/2";

        public static Guid NghiLeId = new Guid("00000000-0000-0000-0000-000000000011");
        public static String NghiLeKyHieu = "L";

        public static Guid NghiLeNuaNgayId = new Guid("00000000-0000-0000-0000-000000000012");
        public static String NghiLeNuaNgayKyHieu = "L/2";

        public static Guid NghiHocNuaNgayId = new Guid("00000000-0000-0000-0000-000000000013");
        public static String NghiHocNuaNgayKyHieu = "H/2";

        public static Guid NghiHuongLuongId = new Guid("00000000-0000-0000-0000-000000000014");
        public static String NghiHuongLuongKyHieu = "HL";

        public static Guid NghiHLNuaNgayId = new Guid("00000000-0000-0000-0000-000000000015");
        public static String NghiHLNuaNgayKyHieu = "HL/2";

        public static Guid NghiHeId = new Guid("E609A972-0D11-48AD-B3AA-0845EE549C40");
        public static String NghiHeKyHieu = "HE";

        public static Guid NghiKhongPhepRoId = new Guid("A24CDBDA-9F44-4C95-B3A1-1F875714E07B");
        public static String NghiKhongPhepRoKyHieu = "RO";

        public static Guid LamNuaNgayId = new Guid("32EDECC3-9B58-4ED5-B359-34B5416BB3C8");
        public static String LamNuaNgayKyHieu = "1/2";

        public static Guid NghiTSNuaNgayId = new Guid("5ABDE47D-8EF5-4966-95F4-2C8D29F8AC86");
        public static String NghiTSNuaNgayKyHieu = "TS/2";

        public static Guid NghiPhepId = new Guid("C70EBAFA-1A1B-4625-98FB-68FE64489B46");
        public static String NghiPhepKyHieu = "P";

        public static Guid NghiNuaNgayId = new Guid("3E0D2543-DC65-4E0D-A887-EB5CB4871E54");
        public static String NghiNuaNgayKyHieu = "P/2";

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
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHeKyHieu, NghiHeId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiKhongPhepRoKyHieu, NghiKhongPhepRoId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiTSNuaNgayKyHieu, NghiTSNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(LamNuaNgayKyHieu, LamNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiPhepKyHieu, NghiPhepId);
            HinhThucNghiDictionaryKyHieuToId.Add(DiCTNuaNgayKyHieu, DiCTNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiNuaNgayKyHieu, NghiNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiThaiSanKyHieu, NghiThaiSanId);
            HinhThucNghiDictionaryKyHieuToId.Add(LamCaNgayKyHieu, Guid.Empty);
            HinhThucNghiDictionaryKyHieuToId.Add(KhongXacDinhKyHieu, KhongXacDinhId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiRoNuaNgayKyHieu, NghiRoNuaNgayId);

            HinhThucNghiDictionaryKyHieuToId.Add(NghiLeKyHieu, NghiLeId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiLeNuaNgayKyHieu, NghiLeNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHocNuaNgayKyHieu, NghiHocNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHuongLuongKyHieu, NghiHuongLuongId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHLNuaNgayKyHieu, NghiHLNuaNgayId);

            HinhThucNghiDictionaryIdToKyHieu = new Dictionary<Guid, string>();
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiOmId, NghiOmKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiConOmId, NghiConOmKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiTaiNanId, NghiTaiNanKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHocTapId, NghiHocTapKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiNgungViecId, NghiNgungViecKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiBuId, NghiBuKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiLaoDongId, NghiLaoDongKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(DiCongTacId, DiCongTacKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHeId, NghiHeKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(LamNuaNgayId, LamNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiKhongPhepRoId, NghiKhongPhepRoKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiTSNuaNgayId, NghiTSNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(DiCTNuaNgayId, DiCTNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiPhepId, NghiPhepKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiNuaNgayId, NghiNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiThaiSanId, NghiThaiSanKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Guid.Empty, LamCaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(KhongXacDinhId, KhongXacDinhKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiRoNuaNgayId, NghiRoNuaNgayKyHieu);

            HinhThucNghiDictionaryIdToKyHieu.Add(NghiLeId, NghiLeKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiLeNuaNgayId, NghiLeNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHocNuaNgayId, NghiHocNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHuongLuongId, NghiHuongLuongKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHLNuaNgayId, NghiHLNuaNgayKyHieu);
        }
    }
}