using OpencvUI.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace OpencvUI.Views
{
    /// <summary>
    /// BlobSearchView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BlobSearchView : UserControl
    {
        System.Drawing.Image originImg = null;
        System.Drawing.Image resultImg = null;

        MemoryStream originMs = null;
        MemoryStream resultMs = null;

        byte[] bytes = null;

        public BlobSearchView()
        {
            InitializeComponent();

            originMs = new MemoryStream();
            resultMs = new MemoryStream();

            Unloaded += BlobSearchView_Unloaded;
        }

        private void BlobSearchView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (originMs != null)
            {
                originMs.Dispose();
                originMs.Close();
                originMs = null;
            }

            if (resultMs != null)
            {
                resultMs.Dispose();
                resultMs.Close();
                resultMs = null;
            }

            if (originImg != null)
            {
                originImg.Dispose();
                originImg = null;
            }

            if (resultImg != null)
            {
                resultImg.Dispose();
                resultImg = null;
            }
        }

        bool ttt = false;
        Stopwatch stopwatch = new Stopwatch();

        private void BtnExcute_Click(object sender, RoutedEventArgs e)
        {
            DefalutSearchBlob();
        }

        private void DefalutSearchBlob()
        {
            try
            {
                stopwatch.Restart();

                originMs = new MemoryStream();
                if (ttt)
                {
                    originImg = System.Drawing.Image.FromFile("C:\\test\\blob.png");
                }
                else
                {
                    originImg = System.Drawing.Image.FromFile("C:\\test\\blob2.png");
                }

                ttt = !ttt;

                originImg.Save(originMs, System.Drawing.Imaging.ImageFormat.Png);

                BitmapImage originBitmap = new BitmapImage();
                originBitmap.BeginInit();
                originBitmap.StreamSource = originMs;
                originBitmap.EndInit();

                uiPicOrigin.Source = originBitmap;

                bytes = originMs.ToArray();
                int outSize = 0;

                IntPtr resultArray = OpenCv.SearchBlob(bytes, originImg.Width, originImg.Height, out outSize);

                byte[] resultBytes = new byte[outSize];
                Marshal.Copy(resultArray, resultBytes, 0, outSize);

                OpenCv.FreeMemory(resultArray);

                resultMs = new MemoryStream(resultBytes);

                resultImg = System.Drawing.Image.FromStream(resultMs);

                BitmapImage resultBitmap = new BitmapImage();
                resultBitmap.BeginInit();
                resultBitmap.StreamSource = resultMs;
                resultBitmap.EndInit();

                uiPicResult.Source = resultBitmap;

                resultImg.Save("C:\\test\\blobOut.png");

                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Image Processing Error: " + ex.ToString());
            }
        }
    }
}
