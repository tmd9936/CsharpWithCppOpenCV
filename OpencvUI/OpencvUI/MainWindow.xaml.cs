using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpencvUI
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Drawing.Image img = null;
        MemoryStream ms = null;
        byte[] bytes = null;


        public MainWindow()
        {
            InitializeComponent();

            try
            {
                ms = new MemoryStream();
                img = System.Drawing.Image.FromFile("C:\\test\\sudoku.png");

                string pf = img.PixelFormat.ToString();
                string rf = img.RawFormat.ToString();

                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                bytes = ms.ToArray();
                int outSize = 0;

                IntPtr resultArray = CvAdaptiveThreshold(bytes, img.Width, img.Height, 255, true, true, 5, 5, out outSize);

                byte[] resultBytes = new byte[outSize];
                Marshal.Copy(resultArray, resultBytes, 0, outSize);

                FreeMemory(resultArray);

                ms = new MemoryStream(resultBytes);

                img = System.Drawing.Image.FromStream(ms);

                img.Save("C:\\test\\out111.png");
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }
        }

        [DllImport("CppOpenCV.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TestInput(int input);

        [DllImport("CppOpenCV.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CvAdaptiveThreshold([In, Out][MarshalAs(UnmanagedType.LPArray)] byte[] input, int width, int height, double maxValue, bool isGausian, bool isBinary, int blockSize, double C, out int outSize);

        [DllImport("CppOpenCV.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeMemory(IntPtr array);
    }
}
