using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UFrame.Util
{
    public class MD5Util
    {
        public static string FileMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    // Convert the byte array to hexadecimal string
                    byte[] hashBytes = md5.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
        }

        //public static string String2MD5Hex(string str)
        //{
        //    if (string.IsNullOrEmpty(str))
        //    {
        //        return null;
        //    }

        //    MD5 md5 = MD5.Create();

        //    byte[] hashBytes = md5.ComputeHash(BabelTime.Net.ByteArrayFunc.string2ByteArray(str));
        //    StringBuilder sb = new StringBuilder();
        //    //StringBuilder sb = StringBuilderPool.Shared.Rent(16);
        //    for (int i = 0, iMax = hashBytes.Length; i < iMax; ++i)
        //    {
        //        sb.AppendFormat("{0:x2}", hashBytes[i]);
        //    }

        //    //return sb.ToString();
        //    string result = sb.ToString();
        //    //StringBuilderPool.Shared.Return(sb);

        //    return result;
        //}

        //public static byte[] string2MD5Byte(string str)
        //{
        //    if (string.IsNullOrEmpty(str))
        //    {
        //        return null;
        //    }

        //    MD5 md5 = MD5.Create();

        //    return md5.ComputeHash(BabelTime.Net.ByteArrayFunc.string2ByteArray(str));
        //}

        //public static string Byte2MD5Hex(byte[] bytes)
        //{
        //    if (bytes == null)
        //    {
        //        return null;
        //    }
        //    int iMax = bytes.Length;
        //    if (iMax <= 0)
        //    {
        //        return null;
        //    }
        //    MD5 md5 = MD5.Create();

        //    //StringBuilder sb = new StringBuilder();
        //    StringBuilder sb = StringBuilderPool.Shared.Rent(16);
        //    for (int i = 0; i < iMax; ++i)
        //    {
        //        sb.AppendFormat("{0:x2}", bytes[i]);
        //    }

        //    //return sb.ToString();
        //    string result = sb.ToString();
        //    StringBuilderPool.Shared.Return(sb);

        //    return result;
        //}

        //public static byte[] Byte2MD5Byte(byte[] bytes)
        //{
        //    if (bytes == null || bytes.Length <= 0)
        //    {
        //        return null;
        //    }

        //    MD5 md5 = MD5.Create();
        //    return md5.ComputeHash(bytes);
        //}

    }
}

