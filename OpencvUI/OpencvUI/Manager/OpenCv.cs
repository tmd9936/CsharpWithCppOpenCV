using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpencvUI.Manager
{
    public class OpenCv
    {
        [DllImport("CppOpenCV.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TestInput(int input);

        [DllImport("CppOpenCV.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CvAdaptiveThreshold([In, Out][MarshalAs(UnmanagedType.LPArray)] byte[] input, int width, int height, double maxValue, bool isGausian, bool isBinary, int blockSize, double C, out int outSize);

        [DllImport("CppOpenCV.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SearchBlob([In, Out][MarshalAs(UnmanagedType.LPArray)] byte[] input, int width, int height, out int outSize);

        [DllImport("CppOpenCV.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeMemory(IntPtr array);

        [DllImport("CppOpenCV.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SearchBlob([In, Out][MarshalAs(UnmanagedType.LPArray)] byte[] input, int width, int height, bool collectContours, bool filterByArea, bool filterByCircularity, bool filterByColor, bool filterByConvexity, bool filterByInertia,
             float minArea, float maxArea, float minCircularity, float maxCircularity, float minConvexity, float maxConvexity, float minInertiaRatio, float maxInertiaRatio,
             float minThreshold, float maxThreshold, float minRepeatability, float minDistBetweenBlobs, float thresholdStep, out int outSize);
    }
}
