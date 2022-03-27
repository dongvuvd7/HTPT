using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            //Nếu là thực hành (phải kết nối wsdl để gọi hàm get input về)
            //ExamForNtp01 ex = new ExamForNtp01();
            //long t4 = 0;
            //byte[] ntpData = new byte[48];
            //ex.GetNtpMessage("username", "password", 442, 31832, ref t4, ref ntpData);

            //Nếu là giữa kì (đề cho input, tính toán điền kết quả)
            byte[] bytes =
            {
                28,03,00,233,00,00,00,114,
                00,00,12,160,25,66,230,04,
                225,72,40,195,105,242,38,249,
                00,00,00,00,00,00,00,00,
                225,72,40,208,41,242,12,33,
                225,72,40,208,41,242,52,101
            };
            String formatDate = "dd/MM/yyyy HH:mm:ss.fff";
            byte Position = 16; //Vi o vi tri 24 bang 0
            DateTime t1 = convertToDate(bytes, Position).ToLocalTime(); //Đề yêu cầu là quy đổi ra thời gian địa phương nên dùng ToLocalTime
            //DateTime t1 = convertToDate(bytes, Position).ToUniversalTime(); //Nếu đề yêu cầu dùng UTC thì dùng hàm này
            Position = 32;
            DateTime t2 = convertToDate(bytes, Position).ToLocalTime();
            Position = 40;
            DateTime t3 = convertToDate(bytes, Position).ToLocalTime();
            DateTime t4 = new DateTime(2019, 10, 09, 16, 37, 29, 229); // t4 là thời gian nhận được bản tin,
                                                                       // đang là giữa kì nên đề bài cho
                                                                       // còn thực hành phải dùng lệnh: DateTime T4 = new DateTime(t4).ToLocalTime();

            //độ lệch thời gian
            long theta = (long)Math.Round(((t2.Ticks - t1.Ticks) + (t3.Ticks - t4.Ticks)) / 2.0, 0, MidpointRounding.AwayFromZero); 
            //thời gian mới của máy khách = t4 + độ lệch
            DateTime final = t4.AddTicks(theta); 

            Console.WriteLine("T1: " + t1.ToString(formatDate));
            Console.WriteLine("T2: " + t2.ToString(formatDate));
            Console.WriteLine("T3: " + t3.ToString(formatDate));
            Console.WriteLine("T4: " + t4.ToString(formatDate));
            Console.WriteLine("Do lech thoi gian: " + theta);
            Console.WriteLine("Thoi gian moi cho may khach: " + final.ToString(formatDate));

            //Nếu thực hành thì nộp bài
            //ex.Submit("username", "password", 442, 31832, t1, t2, t3, t4, theta, final);

            Console.ReadKey(); //phải có dòng này
        }

        static DateTime convertToDate(byte[] bytes, byte Position)
        {
            DateTime dt = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            byte[] array = new byte[4];
            Array.Copy(bytes, Position, array, 0, 4);
            Array.Reverse(array);
            ulong nguyen = BitConverter.ToUInt32(array, 0);
            for (int i = 0; i < 4; i++)
            {
                array[i] = bytes[Position + i + 4];
            }
            ulong du = BitConverter.ToUInt32(array, 0);

            ulong miliseconds = nguyen * 1000 + (du * 1000) / UInt32.MaxValue;
            dt = dt.AddMilliseconds(miliseconds);
            return dt;
        }
    }
}
