using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Business.Predefined
{
    public static class HinhThucNghiConst
    {
        //E609A972-0D11-48AD-B3AA-0845EE549C40	H	Nghỉ hè	H
        //A24CDBDA-9F44-4C95-B3A1-1F875714E07B	Ro	Nghi không phép	Ro
        //C70EBAFA-1A1B-4625-98FB-68FE64489B46	P	Nghỉ phép	P
        //3E0D2543-DC65-4E0D-A887-EB5CB4871E54	1/2	Nghỉ nửa ngày	1/2
        //0E731720-7CBC-403C-BF20-FF9C63721E8F	TS	Nghỉ thai sản	TS
        public static Guid NghiHeId = new Guid("E609A972-0D11-48AD-B3AA-0845EE549C40");
        public static String NghiHeKyHieu = "H";
        public static Guid NghiKhongPhepRoId = new Guid("A24CDBDA-9F44-4C95-B3A1-1F875714E07B");
        public static String NghiKhongPhepRoKyHieu = "Ro";
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
            HinhThucNghiDictionaryKyHieuToId.Add(NghiHeKyHieu, NghiHeId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiKhongPhepRoKyHieu, NghiKhongPhepRoId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiKhongPhepRoKyHieu.ToUpper(), NghiKhongPhepRoId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiPhepKyHieu, NghiPhepId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiNuaNgayKyHieu, NghiNuaNgayId);
            HinhThucNghiDictionaryKyHieuToId.Add(NghiThaiSanKyHieu, NghiThaiSanId);
            HinhThucNghiDictionaryKyHieuToId.Add(LamCaNgayKyHieu, Guid.Empty);
            HinhThucNghiDictionaryKyHieuToId.Add(KhongXacDinhKyHieu, KhongXacDinhId);

            HinhThucNghiDictionaryIdToKyHieu = new Dictionary<Guid, string>();
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiHeId, NghiHeKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiKhongPhepRoId, NghiKhongPhepRoKyHieu);
            
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiPhepId, NghiPhepKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiNuaNgayId, NghiNuaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(NghiThaiSanId, NghiThaiSanKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(Guid.Empty, LamCaNgayKyHieu);
            HinhThucNghiDictionaryIdToKyHieu.Add(KhongXacDinhId, KhongXacDinhKyHieu);

        }
    }
}