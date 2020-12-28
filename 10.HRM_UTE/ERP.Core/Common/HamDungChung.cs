using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ERP_Core.Common
{
    public static class HamDungChung
    {

        /// <summary>
        /// Set Time for DateTime value (use in Tax calculator)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="style">0: start hour of day, 1: end hour of day, 2: start day of month, 3: end day of month, 4: start month of year, 5: end month of year</param>
        /// <returns></returns>
        public static DateTime SetTime(DateTime source, int style)
        {
            int hh, mm, ss;
            if (style == 0)
            {
                hh = source.Hour;
                mm = source.Minute;
                ss = source.Second;

                source = source.AddHours(-hh);
                source = source.AddMinutes(-mm);
                source = source.AddSeconds(-ss);
            }
            else if (style == 1)
            {
                hh = 23 - source.Hour;
                mm = 59 - source.Minute;
                ss = 59 - source.Second;

                source = source.AddHours(hh);
                source = source.AddMinutes(mm);
                source = source.AddSeconds(ss);
            }
            else if (style == 2)
            {
                source = new DateTime(source.Year, source.Month, 1);
                source = SetTime(source, 0);
            }
            else if (style == 3)
            {
                source = new DateTime(source.Year, source.Month, 1).AddMonths(1).AddDays(-1);
                source = SetTime(source, 1);
            }
            else if (style == 4)
            {
                source = new DateTime(source.Year, 1, 1);
                source = SetTime(source, 0);
            }
            else if (style == 5)
            {
                source = new DateTime(source.Year, 12, 31);
                source = SetTime(source, 1);
            }

            return source;
        }


        /// <summary>
        /// Đọc số tiền
        /// </summary>
        /// <param name="NumCurrency">số tiền</param>
        /// <returns></returns>
        public static string DocTien(decimal NumCurrency)
        {
            string SoRaChu = "";
            NumCurrency = Math.Abs(NumCurrency);
            if (NumCurrency == 0)
            {
                SoRaChu = "Không đồng";
                return SoRaChu;
            }

            string[] CharVND = new string[10] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string BangChu;
            int I;
            //As String, BangChu As String, I As Integer
            int SoLe, SoDoi;
            string PhanChan, Ten;
            string DonViTien, DonViLe;
            int NganTy, Ty, Trieu, Ngan;
            int Dong, Tram, Muoi, DonVi;

            SoDoi = 0;
            Muoi = 0;
            Tram = 0;
            DonVi = 0;

            Ten = "";
            DonViTien = "đồng";
            DonViLe = "xu";


            SoLe = (int)((NumCurrency - (Int64)NumCurrency) * 100); //'2 kí so^' le?
            PhanChan = ((Int64)NumCurrency).ToString().Trim();

            int khoangtrang = 15 - PhanChan.Length;
            for (int i = 0; i < khoangtrang; i++)
                PhanChan = "0" + PhanChan;

            NganTy = int.Parse(PhanChan.Substring(0, 3));
            Ty = int.Parse(PhanChan.Substring(3, 3));
            Trieu = int.Parse(PhanChan.Substring(6, 3));
            Ngan = int.Parse(PhanChan.Substring(9, 3));
            Dong = int.Parse(PhanChan.Substring(12, 3));

            if (NganTy == 0 & Ty == 0 & Trieu == 0 & Ngan == 0 & Dong == 0)
            {
                BangChu = String.Format("không {0} ", DonViTien);
                I = 5;
            }
            else
            {
                BangChu = "";
                I = 0;
            }

            while (I <= 5)
            {
                switch (I)
                {
                    case 0:
                        SoDoi = NganTy;
                        Ten = "ngàn tỷ";
                        break;
                    case 1:
                        SoDoi = Ty;
                        Ten = "tỷ";
                        break;
                    case 2:
                        SoDoi = Trieu;
                        Ten = "triệu";
                        break;
                    case 3:
                        SoDoi = Ngan;
                        Ten = "ngàn";
                        break;
                    case 4:
                        SoDoi = Dong;
                        Ten = DonViTien;
                        break;
                    case 5:
                        SoDoi = SoLe;
                        Ten = DonViLe;
                        break;
                }

                if (SoDoi != 0)
                {
                    Tram = (int)(SoDoi / 100);
                    Muoi = (int)((SoDoi - Tram * 100) / 10);
                    DonVi = (SoDoi - Tram * 100) - Muoi * 10;
                    BangChu = BangChu.Trim() + (BangChu.Length == 0 ? "" : " ") + (Tram != 0 ? CharVND[Tram].Trim() + " trăm " : "");
                    if (Muoi == 0 & Tram == 0 & DonVi != 0 & BangChu != "")
                        BangChu = BangChu + "không trăm lẻ ";
                    else if (Muoi != 0 & Tram == 0 & (DonVi == 0 || DonVi != 0) & BangChu != "")
                        BangChu = BangChu + "không trăm ";
                    else if (Muoi == 0 & Tram != 0 & DonVi != 0 & BangChu != "")
                        BangChu = BangChu + "lẻ ";
                    if (Muoi != 0 & Muoi > 0)
                        BangChu = BangChu + ((Muoi != 0 & Muoi != 1) ? CharVND[Muoi].Trim() + " mươi " : "mười ");

                    if (Muoi != 0 & DonVi == 5)
                        BangChu = String.Format("{0}lăm {1} ", BangChu, Ten);
                    else if (Muoi > 1 & DonVi == 1)
                        BangChu = String.Format("{0}mốt {1} ", BangChu, Ten);
                    else
                        BangChu = BangChu + ((DonVi != 0) ? String.Format("{0} {1} ", CharVND[DonVi].Trim(), Ten) : Ten + " ");
                }
                else
                    BangChu = BangChu + ((I == 4) ? DonViTien + " " : "");

                I = I + 1;
            }

            BangChu = BangChu[0].ToString().ToUpper() + BangChu.Substring(1);
            SoRaChu = BangChu;
            return SoRaChu;
        }

        public static string DocSo(string number)
        {
            if (number.Contains("."))
                number = number.Replace(".", "");

            Regex regex = new Regex(@"^\d+(\,?\d+)?$");
            if (regex.IsMatch(number))
            {
                string[] split = number.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Length == 2)
                    return String.Format("{0}phẩy {1}", ChuyenSo(split[0]), ChuyenSo(split[1]));
                else
                    return ChuyenSo(number);
            }
            return "";
        }

        private static string ChuyenSo(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỷ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv;
            bool state = false;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3)
                                    doc += RemoveEmptyString(cs[0]);
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0')
                                        doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3)
                                    doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0)
                                        k = 0;
                                    else
                                        k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += RemoveEmptyString(cs[1]);
                                }
                                break;
                            case '5':
                                if (j == n - 1)
                                    doc += "lăm ";
                                else
                                    doc += RemoveEmptyString(cs[5]);
                                break;
                            default:
                                doc += RemoveEmptyString(cs[(int)number[i + j] - 48]);
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += RemoveEmptyString(dv[n - j - 1]);
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (found != 0)
                        {
                            state = true;
                            for (k = 0; k < (len - i - n) / 9; k++)
                            {
                                doc += "tỷ ";
                            }
                        }
                        else
                        {
                            if (!state)
                                doc += "tỷ ";
                        }
                    }
                    else
                        if (found != 0)
                        doc += RemoveEmptyString(dv[((len - i - n + 1) % 9) / 3 + 2]);
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5')
                    return cs[(int)number[0] - 48] + " ";

            return doc;
        }

        private static string RemoveEmptyString(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return input + " ";
            return "";
        }

    }

}
