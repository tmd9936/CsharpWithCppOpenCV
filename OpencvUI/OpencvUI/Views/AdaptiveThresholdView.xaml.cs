using OpencvUI.Manager;
using System;
using System.Collections.Generic;
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
    /// AdaptiveThresholdView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AdaptiveThresholdView : UserControl
    {
        System.Drawing.Image originImg = null;
        System.Drawing.Image resultImg = null;

        MemoryStream originMs = null;
        MemoryStream resultMs = null;
        byte[] bytes = null;

        public AdaptiveThresholdView()
        {
            InitializeComponent();

            Unloaded += AdaptiveThresholdView_Unloaded;
        }

        private void AdaptiveThresholdView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (originMs != null)
            {
                originMs.Dispose();
                originMs = null;
            }

            if (resultMs != null)
            {
                resultMs.Dispose();
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

        private void ChangeImage(object sender, RoutedEventArgs e)
        {
            try
            {
                originMs = new MemoryStream();
                originImg = System.Drawing.Image.FromFile("C:\\test\\sudoku.png");

                originImg.Save(originMs, System.Drawing.Imaging.ImageFormat.Png);

                BitmapImage originBitmap = new BitmapImage();
                originBitmap.BeginInit();
                originBitmap.StreamSource = originMs;
                originBitmap.EndInit();

                uiPicOrigin.Source = originBitmap;

                bytes = originMs.ToArray();
                int outSize = 0;

                int MaxValue = (int)(MaxValueSlider.Value);
                bool isGaussian = IsGaussianCheckBox.IsChecked.Value;
                bool isBinary = !IsInvBinaryCheckBox.IsChecked.Value;
                int blockSize = (int)(BlockSizeSlider.Value);
                int cValue = (int)(CSlider.Value);

                IntPtr resultArray = OpenCv.CvAdaptiveThreshold(bytes, originImg.Width, originImg.Height, MaxValue, isGaussian, isBinary, blockSize, cValue, out outSize);

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

                resultImg.Save("C:\\test\\out111.png");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Image Processing Error: " + ex.ToString());
            }
            finally
            {
            }
        }

        private void MaxValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MaxValueTextBlock != null)
                MaxValueTextBlock.Text = ((int)e.NewValue).ToString();
        }

        private void BlockSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BlockSizeTextBlock != null)
                BlockSizeTextBlock.Text = ((int)e.NewValue).ToString();
        }

        private void CChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CTextBlock != null)
                CTextBlock.Text = ((int)e.NewValue).ToString();
        }
    }
}
