using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TH_De3_Berkeley
{
    class Program
    {
        static void Main(string[] args)
        {
            ExamForBerkeley03 exam = new ExamForBerkeley03();
            const string DateFormat = "yyyy-MM-dd HH:mm:ss.fff";
            string[] str;
            string getResult = exam.GetInputData("username", "password", 443, 31965, out str); //gán các tham số thời gian vào mảng chuỗi str
            Console.WriteLine(getResult);

            //Truyền các giá trị thời gian từ str vào oldTimes
            DateTime[] oldTimes = new DateTime[str.Length];
            int j = 0;
            foreach(string i in str)
            {
                oldTimes[j] = DateTime.Parse(i);
                Console.WriteLine(oldTimes[j].ToString(DateFormat));
                j++;
            }

            //Tính trung bình độ lệch
            int dolechMs = CalDoLech(oldTimes);
            Console.WriteLine(dolechMs);

            //Thời gian sau khi đồng bộ (tính ra = time điều phối + độ lệch)
            DateTime finalTime = oldTimes[0].AddMilliseconds(dolechMs);
            Console.WriteLine(finalTime.ToString(DateFormat));

            //Tính thời gian cần điều chỉnh cho mỗi tiến trình
            int[] eachTimeMs = new int[oldTimes.Length];
            int k = 0;
            foreach(DateTime time in oldTimes)
            {
                eachTimeMs[k] = (int)((finalTime.Ticks - time.Ticks) / 10000);
                Console.WriteLine(eachTimeMs[k]);
                k++;
            }

            //Nộp bài
            string submitResult = exam.Submit("username", "password", 443, 31965, eachTimeMs, finalTime.ToString(DateFormat));
            Console.WriteLine(submitResult); //200 là nộp success thì phải
            Console.ReadKey(); //Bắt buộc phải có dòng này

        }

        /// <summary>
        /// Hàm tính độ lệch trung bình giữa time(tiến trình) điều phối và các time thành viên
        /// Tính bằng cách tính trung bình độ chênh giữa các time thành viên và time điều phối
        /// </summary>
        /// <param name="oldTimes">Mảng các thời gian ban đầu đề bài cho</param>
        /// <returns></returns>
        private static int CalDoLech(DateTime[] oldTimes)
        {
            DateTime master = oldTimes[0]; //time dieu phoi
            int total = 0;
            foreach(DateTime time in oldTimes)
            {
                total += (int)((time.Ticks - master.Ticks) / 10000); //tổng độ chênh
            }
            return (int)Math.Round(total / (double)oldTimes.Length); //Chú ý làm tròn theo ý thầy
        }
    }
}
