using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TDFramework.Tool
{
    public static class StringTools
    {
        /// <summary>
        /// 获取字符串对应的唯一数字 类似于string => MD5
        /// </summary>
        /// <param name="text"></param>
        /// <returns>返回计算唯一字符</returns>
        public static string GenerateStringID(string text)
        {
            long sum = 0;
            byte overflow;
            for (int i = 0; i < text.Length; i++)
            {
                sum = (long)((16 * sum) ^ Convert.ToUInt32(text[i]));
                overflow = (byte)(sum / 4294967296);
                sum = sum - overflow * 4294967296;
                sum = sum ^ overflow;
            }

            if (sum > 2147483647)
                sum = sum - 4294967296;
            else if (sum >= 32768 && sum <= 65535)
                sum = sum - 65536;
            else if (sum >= 128 && sum <= 255)
                sum = sum - 256;

            sum = Math.Abs(sum);


            return sum.ToString();
        }
        
        /// <summary>
        /// 将string 按照指定字符分割成 int 数组
        /// </summary>
        /// <param name="targetString"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static int[] SplitStringToIntArray(string targetString, char symbol)
        {
            string[] array1 = targetString.Split(symbol);
        
            int[] array = Array.ConvertAll(array1 , int.Parse);

            return array;
        }
    }

}

