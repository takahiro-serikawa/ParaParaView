using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace ParaParaView
{
    class ExifInfo
    {
        static class ExifType
        {
            public const int BYTE = 1;    // Lne 1	8ビット符合なし整数
            public const int ASCII = 2;   //          ASCII文字の集合'0.H'で終端
            public const int SHORT = 3;	// Len 2	16ビット符合なし整数
            public const int LONG = 4; 	// Len 4	32ビット符合なし整数
            public const int RATIONAL = 5; // Len 8	LONG２つ、分子／分母
            public const int UNDEFINED = 7;
            public const int SLONG = 9;    // Len 4	32ビット符合あり整数
            public const int SRATIONAL = 10;  // Len 8
        }

        static class ExifId
        {
            public const int ImageDescription = 0x010E; // 画像タイトル	
            public const int Maker = 0x010F;    // 画像入力機器のメーカー名
            public const int Model = 0x0110;    // 画像入力機器のモデル名
            public const int Orientation = 0x0112;  // Short 画像方向
            public const int Software = 0x0131; // Ascii ソフトウェア
            public const int DateTime = 0x0132; // Ascii 20 ファイル変更日時
            public const int ExposureTime = 0x829A; // Rational 露出時間
            public const int FNumber = 0x829D;  // Rational	Fナンバー
            public const int ExposureProgram = 0x8822;  // Short 露出プログラム

            public const int PhotographicSensitivity = 0x8827;  // Short 撮影感度
            public const int SensitivityType = 0x8830;  // Short 感度種別
            public const int BrightnessValue = 0x9203;  // SRational 輝度値
            public const int ExposureBiasValue = 0x9204;    // SRational 露光補正値
            public const int FocalLength = 0x920A;  // Rational レンズ焦点距離
            public const int MakerNote = 0x927C;    // Undefined メーカノート
        }

#if XXX
        // ExifOrientation
            1	そのまま
2	上下反転(上下鏡像?)
3	180度回転
4	左右反転
5	上下反転、時計周りに270度回転
6	時計周りに90度回転
7	上下反転、時計周りに90度回転
8	時計周りに270度回転
#endif
        public static int GetOrientation(Image image)
        {
            try {
                var item = image.PropertyItems.First(x => x.Id == ExifId.Orientation);
                return (item.Value[1] << 8) | item.Value[0];
            } catch (Exception ) {
                return 0;   // EXIFなし
            }
        }

        public static string MakeExifStr(Image image)
        {
            string s = "";
            const int NEWLINE = -1, BLANK = -2;

            int[] id_template = new int[]{
                ExifId.DateTime, NEWLINE,
                ExifId.FocalLength, BLANK, ExifId.FNumber, NEWLINE,
                ExifId.ExposureTime, BLANK, ExifId.ExposureBiasValue, BLANK, ExifId.PhotographicSensitivity, NEWLINE,
                NEWLINE,
                ExifId.Model, BLANK, ExifId.Software, NEWLINE,
                ExifId.Maker, NEWLINE//,
                // ExifId.Orientation
            };
            var items = image.PropertyItems;
            foreach (var id in id_template) {
                if (id == NEWLINE)
                    s += "\r\n";
                else if (id == BLANK)
                    s += " ";
                else {
                    string svalue = "?";
                    int ivalue = 0, a = 0, b = 0;
                    float fvalue = 0;

                    try {
                        var item = items.First(x => x.Id == id);
                        switch (item.Type) {
                        case ExifType.ASCII:
                            svalue = Encoding.ASCII.GetString(item.Value).Trim(' ', '\0');
                            break;
                        case ExifType.SHORT:
                            ivalue = (item.Value[1] << 8) | item.Value[0];
                            svalue = ivalue.ToString();
                            break;
                        case ExifType.RATIONAL:
                            a = (item.Value[3] << 24) | (item.Value[2] << 16) | (item.Value[1] << 8) | item.Value[0];
                            b = (item.Value[7] << 24) | (item.Value[6] << 16) | (item.Value[5] << 8) | item.Value[4];
                            fvalue = (float)a / b;
                            svalue = ((double)a / b).ToString();
                            break;
                        }

                        switch (id) {
                        case ExifId.DateTime:
                            DateTime date = DateTime.ParseExact(svalue, "yyyy:MM:dd HH:mm:ss", null);
                            svalue = "("+date.DayOfWeek.ToString().Substring(0, 3)+")" + date.ToString(" yyyy.MM.dd HH:mm:ss");
                            s += svalue;
                            break;
                        case ExifId.Model:
                        case ExifId.Software:
                        case ExifId.Maker:
                            s += svalue;
                            break;
                        case ExifId.FocalLength:
                            s += fvalue + "mm";
                            break;
                        case ExifId.FNumber:
                            s += "F" + fvalue.ToString();
                            break;
                        case ExifId.ExposureTime:
                            if (a == 1 && b > 1)
                                s += string.Format("{0}/{1}", a, b);
                            else
                                s += fvalue.ToString();
                            s += "sec";
                            break;
                        case ExifId.PhotographicSensitivity:
                            s +=  "ISO " + svalue;
                            break;
                        case ExifId.ExposureBiasValue:
                            s += fvalue.ToString("+#;-#; ") + "(" + fvalue.ToString() + ")";
                            break;
                        case ExifId.Orientation:
                            s += "orientation:" + svalue + "\r\n";
                            break;
#if XXX
                    case ExifId.ExposureProgram:
                        s += "ExposureProgram:" + svalue + "\r\n";
                        break;

                    case ExifId.SensitivityType:
                        s += "SensitivityType:" + svalue + "\r\n";
                        break;
                    case ExifId.BrightnessValue:
                        s += "BrightnessValue:" + fvalue + "\r\n";
                        break;
#endif
                        default:
                            Console.WriteLine("id=0x{0:X}({1}), type={2}, len={3}", item.Id, item.Id, item.Type, item.Len);
                            break;
                        }
                    } catch (Exception ex) {
                        Console.WriteLine("exif id{0}: {1}", id, ex.Message);
                    }
                }
            }
            return s;
        }
    }
}
