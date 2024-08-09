// dllmain.cpp : DLL 애플리케이션의 진입점을 정의합니다.
#include "pch.h"
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>

using namespace std;
using namespace cv;


BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
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

//
extern "C" __declspec(dllexport) unsigned char* CvAdaptiveThreshold(BYTE* byteArray, int width, int height, double maxValue, bool isGausian, bool isBinary, int blockSize, double C, int& outSize)
{
	if (byteArray == nullptr)
		return 0;

	Mat image = Mat(1, width * height, CV_8U, byteArray); // make a copy
	cv::Mat decodedImage = cv::imdecode(image, cv::IMREAD_GRAYSCALE);

	Mat dst;
	int adaptiveMethod = isGausian ? AdaptiveThresholdTypes::ADAPTIVE_THRESH_GAUSSIAN_C : AdaptiveThresholdTypes::ADAPTIVE_THRESH_MEAN_C;
	int threasholdType = isBinary ? ThresholdTypes::THRESH_BINARY : ThresholdTypes::THRESH_BINARY_INV;
	adaptiveThreshold(decodedImage, dst, maxValue, adaptiveMethod, threasholdType, blockSize, C);

	//cv::imshow("Image", dst);
	//cv::waitKey(0);
	//try
	//{
	//	cv::imwrite("ssdflkajsdf.png", image);
	//}
	//catch (const std::exception& e)
	//{
	//}

	std::vector<uchar> encoded;
	cv::imencode(".png", dst, encoded);

	// 바이트 배열을 C#으로 반환
	outSize = encoded.size();
	unsigned char* returnArray = new unsigned char[outSize];
	std::copy(encoded.begin(), encoded.end(), returnArray);

	return returnArray;
}

extern "C" __declspec(dllexport) unsigned char* SearchBlob(BYTE * byteArray, int width, int height, int& outSize)
{
	if (byteArray == nullptr)
		return 0;

	Mat image = Mat(1, width * height, CV_8U, byteArray); // make a copy
	cv::Mat decodedImage = cv::imdecode(image, cv::IMREAD_GRAYSCALE);

	vector<KeyPoint> keypoints;

	SimpleBlobDetector::Params params;
	Ptr<SimpleBlobDetector> detector = SimpleBlobDetector::create(params);

	detector->detect(decodedImage, keypoints);

	Mat dst;

	drawKeypoints(decodedImage, keypoints, dst, Scalar(0, 0, 255), DrawMatchesFlags::DRAW_RICH_KEYPOINTS);

	std::vector<uchar> encoded;
	cv::imencode(".png", dst, encoded);

	// 바이트 배열을 C#으로 반환
	outSize = encoded.size();
	unsigned char* returnArray = new unsigned char[outSize];
	std::copy(encoded.begin(), encoded.end(), returnArray);

	return returnArray;
}

extern "C" __declspec(dllexport) unsigned char* SearchBlob(BYTE * byteArray, int width, int height, bool collectContours, bool filterByArea, bool filterByCircularity, bool filterByColor, bool filterByConvexity, bool filterByInertia,  
	float minArea, float maxArea, float minCircularity, float maxCircularity, float minConvexity, float maxConvexity,  float minInertiaRatio, float maxInertiaRatio, 
	float minThreshold, float maxThreshold, float minRepeatability,float minDistBetweenBlobs, float thresholdStep, int& outSize)
{
	if (byteArray == nullptr)
		return 0;

	Mat image = Mat(1, width * height, CV_8U, byteArray); // make a copy
	cv::Mat decodedImage = cv::imdecode(image, cv::IMREAD_GRAYSCALE);

	vector<KeyPoint> keypoints;

	SimpleBlobDetector::Params params;
	params.collectContours = collectContours;
	params.filterByArea = filterByArea;
	params.filterByCircularity = filterByCircularity;
	params.filterByColor = filterByColor;
	params.filterByConvexity = filterByConvexity;
	params.filterByInertia = filterByInertia;
	params.minArea = minArea;
	params.maxArea = maxArea;
	params.minCircularity = minCircularity;
	params.maxCircularity = maxCircularity;
	params.minConvexity = minConvexity;
	params.maxConvexity= maxConvexity;
	params.minInertiaRatio = minInertiaRatio;
	params.maxInertiaRatio = maxInertiaRatio;
	params.minThreshold = minThreshold;
	params.maxThreshold = maxThreshold;
	params.minRepeatability = minRepeatability;
	params.minDistBetweenBlobs = minDistBetweenBlobs;
	params.thresholdStep = thresholdStep;

	Ptr<SimpleBlobDetector> detector = SimpleBlobDetector::create(params);

	detector->detect(decodedImage, keypoints);

	Mat dst;
	drawKeypoints(decodedImage, keypoints, dst, Scalar(0, 0, 255), DrawMatchesFlags::DRAW_RICH_KEYPOINTS);

	std::vector<uchar> encoded;
	cv::imencode(".png", dst, encoded);

	// 바이트 배열을 C#으로 반환
	outSize = encoded.size();
	unsigned char* returnArray = new unsigned char[outSize];
	std::copy(encoded.begin(), encoded.end(), returnArray);

	return returnArray;
}

extern "C" __declspec(dllexport) void FreeMemory(unsigned char* array) {
	delete[] array;
}