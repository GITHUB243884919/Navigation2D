namespace UFrame.Util
{
    public class StringUtil
    {
        /// <summary>
        /// 字符串按规定长度切割
        /// 比如输入 12345，规定长度2，那么输出 [1,2] [3,4] [5]
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitLen"></param>
        /// <returns></returns>
        public string[] SplitString(string str, int splitLen)
        {
            int strLen = str.Length;
            int countPart = (strLen + splitLen - 1) / splitLen;
            string[] parts = new string[countPart];
            for (int i = 0; i < parts.Length; i++)
            {
                splitLen = splitLen <= str.Length ? splitLen : str.Length;
                parts[i] = str.Substring(0, splitLen);
                str = str.Substring(splitLen, str.Length - splitLen);
            }
            return parts;
        }
    }
}

