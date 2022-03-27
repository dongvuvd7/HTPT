using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TH_GTTrungBinh
{
    class Program
    {
        static void Main(string[] args)
        {
            //Đây là code để làm bài giữa kì (tức là điền đáp án chứ không phải nộp code) nên có sẵn input đề bài cho
            //Còn nếu là bài thực hành thì cần kết nối wsdl để get input về

            String format = "yyyy-MM-dd HH:mm:ss.fff";
            String[] input =
            {
                "2021-11-02 00:54:23.020",
                "2021-11-02 00:52:20.030",
                "2021-11-02 00:55:23.030",
                "2021-11-02 00:53:20.040",
                "2021-11-02 00:54:22.990",
                "2021-11-02 00:52:20.060",
                "2021-11-02 00:55:22.970",
                "2021-11-02 00:52:20.000",
                "2021-11-02 00:54:22.960",
                "2021-11-02 00:52:20.090",
                "2021-11-02 00:55:22.940",
                "2021-11-02 00:54:19.990",
                "2021-11-02 00:54:22.930",
            };

            Console.WriteLine(input.Length);

            //Chuyển các time (tiến trình) sang ms
            List<long> listDateLongFormat = convertToListTicks(input);
            //Duyệt từng time một
            foreach(long date in listDateLongFormat)
            {
                //copy mảng listDateLongFormat sang mảng listCopy để tiến hành tính thời gian của mỗi tiến trình
                //Tính bằng cách tính trung bình các tiến trình (loại đi 3 tiến trình là: chính nó, nhỏ nhất, lớn nhất)
                //VD: input có 13 time thì time kết quả của mỗi tiến trình sẽ  = tổng (13 - 3) time / 10

                List<long> listCopy = new List<long>(listDateLongFormat);
                //Xóa chính nó
                listCopy.Remove(listCopy[listCopy.IndexOf(date)]);
                //Sắp xếp lại mảng để xóa phần tử nhỏ nhất và lớn nhất
                listCopy.Sort();
                listCopy.Remove(listCopy[0]);
                listCopy.Remove(listCopy[listCopy.Count - 1]);

                //Tính giá thời gian mới cho tiến trình đang xét
                long tong = 0; //tổng giá trị các time sau khi bỏ 3 tiến trình kia
                long trungBinh = 0; //giá trị trung bình, chính là kết quả (sau khi đổi lại từ ms ra datetime)
                int length = 0; //tính ++ thế này hoặc lấy input.Length - 3 cũng được
                foreach(long lc in listCopy)
                {
                    tong += lc;
                    length++;
                }
                trungBinh = tong / length; //chú ý làm tròn theo ý thầy

                //Chuyển đổi lại từ ms ra datetime để nộp kết quả (thầy yêu cầu nộp là gì thì chuyển thành thế)
                DateTime resultInDateTimeFormat = new DateTime(trungBinh);
                String final = resultInDateTimeFormat.ToString(format);
                Console.WriteLine(final);

                //Nếu là bài thực hành thì cần làm mảng final để submit (add từng giá trị sau khi tính ra vào mảng)

            }
            //Nếu là thực hành thì viết câu lệnh submit ở đây
            Console.ReadKey();

        }

        /// <summary>
        /// Hàm chuyển đổi từ string sang Ms (miligiây, 1s = 1000ms)
        /// </summary>
        /// <param name="input">Mảng chứa các time(tiến trình) đầu vào</param>
        /// <returns>Mảng ms tương ứng các time đầu vào</returns>
        private static List<long> convertToListTicks(String[] input)
        {
            List<long> result = new List<long>();
            foreach(String str in input)
            {
                DateTime item = DateTime.Parse(str); //chuyển string sang datetime
                long itemTicks = item.Ticks; //chuyển datetime ra ms
                result.Add(itemTicks);
            }
            return result;
        }
    }
}
