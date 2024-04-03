#pragma once

#include "pch.h"

using namespace std;
using namespace cv;
	
extern "C" __declspec(dllexport) void CvAdaptiveThreshold(BYTE * input, BYTE * output, int width, int height, double maxValue, bool isGausian, bool isBinary, int blockSize, double C)
{
	if (input == nullptr || output == nullptr)
		return;

	Mat image = Mat(height, width, CV_8UC3, input).clone(); // make a copy
	Mat dst;
	int adaptiveMethod = isGausian ? AdaptiveThresholdTypes::ADAPTIVE_THRESH_GAUSSIAN_C : AdaptiveThresholdTypes::ADAPTIVE_THRESH_MEAN_C;
	int threasholdType = isBinary ? ThresholdTypes::THRESH_BINARY : ThresholdTypes::THRESH_BINARY_INV;
	adaptiveThreshold(image, dst, maxValue, adaptiveMethod, threasholdType, blockSize, C);

	/*if (sizeof(output) != sizeof(dst.data))
		return;*/
	Size size = dst.size();
	memcpy(output, dst.data, (size.height * size.width));
}

//__declspec(dllexport) void CvAdaptiveThreshold(string path, BYTE* output, int width, int height, double maxValue, bool isGausian, bool isBinary, int blockSize, double C);
//{
//	if (output == nullptr)
//		return;
//
//	Mat image = imread(path, IMREAD_GRAYSCALE);
//	Mat dst;
//	int adaptiveMethod = isGausian ? AdaptiveThresholdTypes::ADAPTIVE_THRESH_GAUSSIAN_C : AdaptiveThresholdTypes::ADAPTIVE_THRESH_MEAN_C;
//	int threasholdType = isBinary ? ThresholdTypes::THRESH_BINARY : ThresholdTypes::THRESH_BINARY_INV;
//	adaptiveThreshold(image, dst, maxValue, adaptiveMethod, threasholdType, blockSize, C);
//	Size size = dst.size();
//	memcpy(output, dst.data, (size.height * size.width));
//}

extern "C" __declspec(dllexport) int TestInput(int input)
{
	return input;
}
