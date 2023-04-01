using NpcGenEditor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;

namespace NpcGenEditor.Classes
{
    public class Extensions
    {
        public static string[] HexToDecimal2(string octetsData)
        {
            List<string> final = new List<string>();
            var valor = Enumerable.Range(0, octetsData.Length / 8)
        .Select(i => octetsData.Substring(i * 8, 8)).ToArray();

            foreach (var val in valor)
            {
                var output = new SoapHexBinary(SoapHexBinary.Parse(val).Value.Reverse().ToArray())
            .ToString();
                final.Add(output);
            }

            return final.ToArray();
        }
        public static string[] DivideEm2(string divide)
        {
            return Enumerable.Range(0, divide.Length / 2)
        .Select(i => divide.Substring(i * 2, 2)).ToArray();
        }
        public static byte[] DivideEm2Byte(string divide)
        {
            return Enumerable.Range(0, divide.Length / 2)
        .Select(i => Convert.ToByte(divide.Substring(i * 2, 2), 16)).ToArray();
        }
        public static string[] DivideEm4(string divide)
        {
            return Enumerable.Range(0, divide.Length / 4)
        .Select(i => divide.Substring(i * 4, 4)).Reverse().ToArray();
        }

        public static string[] DivideEm8(string divide)
        {
            return Enumerable.Range(0, divide.Length / 8)
        .Select(i => divide.Substring(i * 8, 8)).ToArray();
        }

        public static List<int> HexToDecimal(string octetsData)
        {
            List<int> final = new List<int>();
            string[] valor = Enumerable.Range(0, octetsData.Length)
                             .Where(x => x % 4 == 0)
                             .Select(x => Convert.ToString(octetsData.Substring(x, 4))).ToArray();

            foreach (var val in valor)
            {
                var output = new SoapHexBinary(SoapHexBinary.Parse(val).Value.Reverse().ToArray())
            .ToString();
                final.Add(Convert.ToInt32(output, 16));
            }
            return final;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static byte[] StringToByteArrayReverse(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .Reverse()
                             .ToArray();
        }

        public static string ByteArray_to_HexString(byte[] value)
        {
            return BitConverter.ToString(value);
        }

        public static string HexStringToFloat(string hexString)
        {
            byte[] s = StringToByteArray(hexString);
            Array.Reverse(s);
            return BitConverter.ToSingle(s, 0).ToString("f6");
        }

        public static int HexStringToInt32(string hexString)
        {
            return Convert.ToInt32(hexString, 16);
        }

        public static int HexStringToInt16(string hexString)
        {
            return Convert.ToInt16(hexString, 8);
        }
        public static byte[] HexString_to_ByteArray(string value)
        {
            char[] chArray = new char[]
            {
                '-'
            };
            string[] strArray = value.Split(chArray);
            byte[] numArray = new byte[strArray.Length];
            for (int index = 0; index < strArray.Length; index++)
            {
                numArray[index] = Convert.ToByte(strArray[index], 16);
            }
            return numArray;
        }

        public static string ByteArray_to_GbkString(byte[] text)
        {
            Encoding encoding = Encoding.GetEncoding("GBK");
            char[] array = new char[1];
            char[] chArray = array;
            return encoding.GetString(text).Split(chArray)[0];
        }
        public static byte[] GbkString_to_ByteArray(string text, int length)
        {
            Encoding encoding = Encoding.GetEncoding("GBK");
            byte[] numArray = new byte[length];
            byte[] bytes = encoding.GetBytes(text);
            if (numArray.Length > bytes.Length)
            {
                Array.Copy(bytes, numArray, bytes.Length);
            }
            else
            {
                byte[] numArray2 = bytes;
                byte[] numArray3 = numArray;
                int length2 = numArray3.Length;
                Array.Copy(numArray2, numArray3, length2);
            }
            return numArray;
        }
        public static byte[] GbkString_to_ByteArray2(string text, int length)
        {
            Encoding enc = Encoding.GetEncoding("GBK");
            byte[] target = new byte[length];
            byte[] source = enc.GetBytes(text);
            if (target.Length > source.Length)
            {
                Array.Copy(source, target, source.Length);
            }
            else
            {
                Array.Copy(source, target, target.Length);
            }
            return target;
        }

        public static string ByteArray_to_UnicodeString(byte[] text)
        {
            Encoding encoding = Encoding.GetEncoding("Unicode");
            char[] array = new char[1];
            char[] chArray = array;
            return encoding.GetString(text).Split(chArray)[0];
        }

        public static string ByteArray_to_UTF8String(byte[] text)
        {
            Encoding encoding = Encoding.UTF8;
            char[] array = new char[1];
            char[] chArray = array;
            return encoding.GetString(text).Split(chArray)[0];
        }
        public static byte[] UnicodeString_to_ByteArray(string text, int length)
        {
            Encoding encoding = Encoding.GetEncoding("Unicode");
            byte[] numArray = new byte[length];
            byte[] bytes = encoding.GetBytes(text);
            if (numArray.Length > bytes.Length)
            {
                Array.Copy(bytes, numArray, bytes.Length);
            }
            else
            {
                byte[] numArray2 = bytes;
                byte[] numArray3 = numArray;
                int length2 = numArray3.Length;
                Array.Copy(numArray2, numArray3, length2);
            }
            return numArray;
        }
        public static byte[] UnicodeString_to_ByteArray2(string text, int length)
        {
            Encoding enc = Encoding.GetEncoding("Unicode");
            byte[] target = new byte[length];
            byte[] source = enc.GetBytes(text);
            if (target.Length > source.Length)
            {
                Array.Copy(source, target, source.Length);
            }
            else
            {
                Array.Copy(source, target, target.Length);
            }
            return target;
        }

        public static string ItemPropsSecondsToString(uint time)
        {
            string result = string.Empty;
            uint time1 = time;
            uint days = time / 86400;
            time = time - (days * 86400);
            uint hours = time / 3600;
            time = time - (hours * 3600);
            uint minutes = time / 60;
            uint seconds = time - (minutes * 60);
            if (time1 == 60) seconds = 60;
            if (time1 == 3600) minutes = 60;
            if (time1 == 86400) hours = 60;
            if (time1 <= 60) result = seconds.ToString() + "segundos";
            if (time1 > 60 && time1 <= 3600) result = minutes.ToString() + "minutos" + " " + seconds.ToString() + "segundos";
            if (time1 > 3600 && time1 <= 86400) result = hours.ToString() + "horas" + " " + minutes.ToString() + "minutos";
            if (time1 > 86400) result = days.ToString() + "dias" + " " + hours.ToString() + "horas";
            return result;
        }

        public static string ItemPropsSecondsToString2(uint time)
        {
            string result = string.Empty;
            uint time1 = time;
            uint hours = time / 3600;
            time = time - (hours * 3600);
            uint minutes = time / 60;
            uint seconds = time - (minutes * 60);
            if (time1 < 60) result = seconds.ToString() + "s";
            if (time1 >= 60 && time1 < 3600) result = minutes.ToString() + "m" + (seconds == 0 ? string.Empty : " " + seconds.ToString() + "s");
            if (time1 >= 3600) result = hours.ToString() + "h" + (minutes == 0 ? string.Empty : " " + minutes.ToString() + "m");
            return result;
        }

        public static string UnixTimeStampToDateTime(uint unixTimeStamp)
        {
            if (unixTimeStamp == 0) return null;
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime.ToString();
        }

        // Formato mm/dd/aaaa hh:mm:ss
        public static long StringDateTimeToUnixTimeStamp(string data)
        {
            DateTime foo = Convert.ToDateTime(data);
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
            return unixTime;
        }

        public static string SecondsToString(uint time)
        {
            uint days = time / 86400;
            time = time - (days * 86400);
            uint hours = time / 3600;
            time = time - (hours * 3600);
            uint minutes = time / 60;
            uint seconds = time - (minutes * 60);
            return (days.ToString("D2") + "-" + hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2"));
        }
        public static uint StringToSecond(string time)
        {
            char[] chArray = new char[]
            {
                '-', ':'
            };
            string[] times = time.Split(chArray);
            return (86400 * Convert.ToUInt32(times[0])) + (3600 * Convert.ToUInt32(times[1])) + (60 * Convert.ToUInt32(times[2])) + Convert.ToUInt32(times[3]);
        }

        public static int StringToInt32(string text)
        {
            return int.TryParse(text.Trim(), out int result) ? result : 0;
        }

        public static string ConvertToClientX(float x)
        {
            double cx = 400 + Math.Truncate(x * 0.1);
            return cx.ToString();
        }
        public static string ConvertToClientY(float y)
        {
            double cy = Math.Truncate(y * 0.1);
            return cy.ToString();
        }
        public static string ConvertToClientZ(float z)
        {
            double cz = 550 + Math.Truncate(z * 0.1);
            return cz.ToString();
        }

        public static int DigitNumberToInt32(object value)
        {
            string result = Convert.ToString(value).Replace(string.Empty + (char)160, string.Empty).Replace(string.Empty + (char)32, string.Empty);
            return Convert.ToInt32(result);
        }

        public static float PercentNumberToSingle(object value, bool EnableShowPercents)
        {
            if (EnableShowPercents == true)
            {
                float result = Convert.ToSingle(Convert.ToString(value).Replace("%", string.Empty));
                return Convert.ToSingle(result * 0.01);
            }
            else
            {
                float result = Convert.ToSingle(value);
                return result;
            }
        }

        public static float ReverseFloat(string value)
        {
            byte[] bytes = BitConverter.GetBytes(Convert.ToSingle(value));
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }

        public static short ReverseShort(string value)
        {
            byte[] bytes = BitConverter.GetBytes(Convert.ToInt16(value));
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        public static string ColorClean(string line)
        {
            if (line == string.Empty || line.Length <= 1) { return string.Empty; }
            string[] blocks = line.Split(new char[] { '^' });
            if (blocks.Length > 1)
            {
                string result = string.Empty;

                if (blocks[0] != string.Empty)
                {
                    result += blocks[0];
                }
                for (int i = 1; i < blocks.Length; i++)
                {
                    if (blocks[i] != string.Empty)
                    {
                        result += blocks[i].Substring(6);
                    }
                }

                return result;
            }
            else
            {
                return line;
            }
        }

        public static Bitmap GetItemImage(int id, int gender = 0, string listname = "_ESSENCE")
        {
            for (int l = 0; l < MainWindow.eLC.Lists.Length; l++)
            {
                if (l != MainWindow.eLC.ConversationListIndex)
                {
                    if (!MainWindow.eLC.Lists[l].listName.Contains(listname))
                        continue;
                    int pos2 = -1;
                    for (int i = 0; i < MainWindow.eLC.Lists[l].elementFields.Length; i++)
                    {
                        if (MainWindow.eLC.Lists[l].elementFields[i] == "file_icon" || MainWindow.eLC.Lists[l].elementFields[i] == "file_icon1")
                        {
                            pos2 = i;
                        }
                        if (pos2 != -1) { break; }
                    }

                    for (int e = 0; e < MainWindow.eLC.Lists[l].elementValues.Length; e++)
                    {
                        if (MainWindow.eLC.Lists[l].elementFields[0] == "ID")
                        {
                            if (MainWindow.eLC.GetValue(l, e, 0) == id.ToString())
                            {
                                Bitmap img = Resources.blank1;
                                string path = Path.GetFileName(MainWindow.eLC.GetValue(l, e, pos2));
                                img = MainWindow.images(path, gender);
                                return img;
                            }
                        }
                        else
                        {
                            return Resources._0;
                        }
                    }
                }
            }
            return Resources._0;
        }

        public static string GetItemName(int id, string listName = "_ESSENCE")
        {
            if (!MainWindow.loadedData) return string.Empty;
            for (int l = 0; l < MainWindow.eLC.Lists.Length; l++)
            {
                if (l != MainWindow.eLC.ConversationListIndex)
                {
                    if (!MainWindow.eLC.Lists[l].listName.Contains(listName))
                        continue;
                    // Find Position for Name
                    int pos = -1;
                    for (int i = 0; i < MainWindow.eLC.Lists[l].elementFields.Length; i++)
                    {
                        if (MainWindow.eLC.Lists[l].elementFields[i] == "Name")
                        {
                            pos = i;
                        }
                        if (pos != -1) { break; }
                    }

                    for (int e = 0; e < MainWindow.eLC.Lists[l].elementValues.Length; e++)
                    {
                        if (MainWindow.eLC.Lists[l].elementFields[0] == "ID")
                        {
                            if (MainWindow.eLC.GetValue(l, e, 0) == id.ToString())
                                return MainWindow.eLC.GetValue(l, e, pos);
                        }
                        else

                        {
                            return string.Empty;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            try
            {

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);

                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
            catch (Exception)
            {


            }
            return destImage;
        }

        public static Bitmap RotateImage(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                //move rotation point to center of image
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                //rotate
                g.RotateTransform(angle);
                //move image back
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                //draw passed in image onto graphics object
                g.DrawImage(b, new Point(0, 0));
                returnBitmap.SetResolution(96, 96);
            }
            return returnBitmap;
        }

        public static float ReturnAnglePosition(float x, float y, float z)
        {
            float vx = (float)Math.Round(x, 1, MidpointRounding.AwayFromZero);
            float vy = (float)Math.Round(y, 1, MidpointRounding.AwayFromZero);
            float vz = (float)Math.Round(z, 1, MidpointRounding.AwayFromZero);
            if ((vx > -0.5f && vx < 0.5f) && (vy >= 0.0f && vy < 0.5f) && (vz >= 0.5f && vz < 1.0f))
            {
                return 0f;
            }
            else if ((vx >= 0.5f && vx < 1.0f) && (vy > -0.5f && vy < 0.5f) && (vz >= 0.5f && vz < 1.0f))
            {
                return 45f;
            }
            else if ((vx >= 0.5f && vx < 1.0f) && (vy > -0.5f && vy < 0.5f) && (vz > -0.5f && vz < 0.5f))
            {
                return 90f;
            }
            else if ((vx >= 0.5f && vx < 1.0f) && (vy > -0.5f && vy < 0.5f) && (vz <= -0.5f && vz > -1.0f))
            {
                return 135f;
            }
            else if ((vx > -0.5f && vx < 0.5f) && (vy > -0.5f && vy < 0.5f) && (vz < -0.5f && vz >= -1.0f))
            {
                return 180f;
            }
            else if ((vx <= -0.5f && vx > 1.0f) && (vy > -0.5f && vy < 0.5f) && (vz <= -0.5f && vz > 1.0f))
            {
                return 225f;
            }
            else if ((vx <= -0.5f && vx > 1.0f) && (vy > -0.5f && vy < 0.5f) && (vz > -0.5f && vz < 0.5f))
            {
                return 270f;
            }
            else if ((vx <= -0.5f && vx > 1.0f) && (vy > -0.5f && vy < 0.5f) && (vz >= -0.5f && vz < 0.5f))
            {
                return 315f;
            }
            else return 0f;
        }

    }
}
