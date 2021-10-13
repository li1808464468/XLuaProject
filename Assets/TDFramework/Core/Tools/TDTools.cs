using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TDFramework.Tool
{
    public static class TDTools
    {
        
        
        /// <summary>
        /// 获取距离下一天凌晨还有多少时间
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetNextDayTimeSpan()
        {
            string dateDiff = null;
            //获取当前时间
            DateTime DateTime1 = DateTime.Now;
            //第二天的0点00分00秒
            DateTime DateTime2 = DateTime.Now.AddDays(1).Date;
            //把2个时间转成TimeSpan,方便计算
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            //时间比较，得出差值
            TimeSpan ts = ts1.Subtract(ts2).Duration();
        
            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
        
        
            return ts;
        }
        
        /// <summary>
        /// 距离传入时间还有多长时间
        /// </summary>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        public static TimeSpan GetTargetTimeSpan(DateTime targetTime)
        {
            string dateDiff = null;
            //获取当前时间
            DateTime DateTime1 = DateTime.Now;
           //第二天的0点00分00秒
            DateTime DateTime2 = targetTime;
            //把2个时间转成TimeSpan,方便计算
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            //时间比较，得出差值
            TimeSpan ts = ts1.Subtract(ts2).Duration();
        
            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
        
        
            return ts;
        }
        
        /// <summary>
        /// 检测是不是同一天
        /// </summary>
        /// <param name="curTime"></param>
        /// <returns></returns>
        public static bool IsToday(DateTime curTime)
        {
            DateTime date = curTime;
            DateTime dateToday = DateTime.Now;
            //Debug.Log("isToday========" + dateToday + "date" + date + "ate.Year.ToString()=" + date.Year.ToString() + "date.Month.ToString()=" + date.Month.ToString() + "date.Date.ToString()=" + date.Day.ToString());
            return date.Year.ToString() == dateToday.Year.ToString() && date.Month.ToString() == dateToday.Month.ToString() && date.Day.ToString() == dateToday.Day.ToString();
        }
    
        /// <summary>
        /// 检测是不是同一天
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static bool IsToday(long timeStamp)
        {
            var today = DateTime.Now.ToString("yyyy/MM/dd");
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
            var dt = startTime.AddMilliseconds(timeStamp);
            var day = dt.ToString("yyyy/MM/dd");
            var todayArr = today.Split('/');
            var dayArr = day.Split('/');
            for (var i = 0; i < todayArr.Length; ++i)
            {
                if (todayArr[i] != dayArr[i])
                {
                    return false;
                }
            }
            return true;
        }
        
        
        /// <summary>
        /// 获取贝塞尔曲线参数
        /// </summary>
        /// <param name="p0">起始点</param>
        /// <param name="p1">偏移点1</param>
        /// <param name="p2">偏移点2</param>
        /// <param name="p3">结束点</param>
        /// <param name="pointNum"></param>
        /// 使用方式 transform.DOLocalPath(points, 0.7f, PathType.CatmullRom).SetEase(Ease.InQuad)
        /// <returns>贝塞尔曲线</returns>
        public static Vector3[] GetPointsFromBezier(Vector3 p0,Vector3 p1,Vector3 p2 ,Vector3 p3,int pointNum)
        {
            var points = new Vector3[pointNum +1];
            var deltaT = 1f / pointNum;
            var count = 0;
            for (var t = 0f; t <= 1f; t = t + deltaT)
            {
                points[count] = p0 * (1 - t) * (1 - t) * (1 - t) + 3 * p1 * t * (1 - t) * (1 - t) + 3 * p2 * t * t * (1 - t) + p3 * t * t * t;
                ++count;
            }

            return points;
        }
        
        
        /// <summary>
        /// 获取B物体转换到A物体上的坐标
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>转换坐标</returns>
        public static Vector3 TransformBToTransformALocalPosition(Transform A , Transform B)
        {
            Vector3 v = B.transform.TransformPoint(Vector3.zero);
            return A.transform.InverseTransformPoint(v);
        }
        
    
        
       
        /// <summary>
        /// 两点之间的距离 x ,y 二维坐标
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetTwoPositionDistance(Vector2 p1, Vector2 p2)
        {
            return Math.Sqrt(Math.Abs(p1.x - p2.x) * Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y) * Math.Abs(p1.y - p2.y));
        }
    
    
        /// <summary>
        /// 获取两点角度
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float GetTwoPosintAngle(Vector2 p1, Vector2 p2)
        {
            double angleOfLine = Math.Atan2((p2.y - p1.y), (p2.x - p1.x)) * 180 / Math.PI;
            return (float)angleOfLine;
        }
        
        
        /// <summary>
        /// 保留小数点后指定位数
        /// </summary>
        /// <param name="value">操作数</param>
        /// <param name="count">保留位数</param>
        /// <returns></returns>
        public static float FloatRetainN(float value, int count = 2)
        {
            int i =(int)(value * Math.Pow(10, count));
        
            return (float) ((i*1.0f)/Math.Pow(10,count));
        }
        
        /// <summary>
        /// 求方差函数
        /// </summary>
        /// <param name="values">数组</param>
        /// <returns>方差</returns>
        public static double CalculateStdDev(List<int> values)
        {
            var array = values.ToArray().Select(Convert.ToInt64);

            double ret = 0;

            if (values.Count() > 0)
            {
                // 计算平均数
                double avg = values.Average();
                // 计算个数值与平均数的差值的平方，然后求和
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                // 开方
                ret = Math.Sqrt(sum / values.Count());
            }

            return ret;
        }
        
        
        
    }
}

