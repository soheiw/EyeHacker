#include <stdio.h>
#include <signal.h>
#include <stdlib.h>
#include <thread>
#include "SRanipal.h"
#include "SRanipal_Eye.h"
#include "SRanipal_Enums.h"
#pragma comment (lib, "SRanipal.lib")
using namespace ViveSR;

std::thread *t = nullptr;
bool EnableEye = false;
bool looping = false;
void streaming() {
	ViveSR::anipal::Eye::EyeData eye_data;
	int result = ViveSR::Error::WORK;
	while (looping) {
		if (EnableEye) {
			int result = ViveSR::anipal::Eye::GetEyeData(&eye_data);
			if (result == ViveSR::Error::WORK) {
				float *origin = eye_data.verbose_data.combined.eye_data.gaze_origin_mm.elem_;
				float *gaze = eye_data.verbose_data.combined.eye_data.gaze_direction_normalized.elem_;
				// printf("[Eye] Origin: %.4f %.4f %.4f\n", origin[0], origin[1], origin[2]);
				printf("[Eye] Gaze: %.4f %.4f %.4f\n", gaze[0], gaze[1], gaze[2]);
			}
		}
	}
}

void callback()
{
	// do nothing
}

int main() {
	printf("SRanipal Sample\n\nPlease refer the below hotkey list to try apis.\n");
	printf("[`] Exit this program.\n");
	printf("[0] Release all anipal engines.\n");
	printf("[1] Initial Eye engine\n");
	printf("[2] Start Calibration\n");
	printf("[3] Launch a thread to query data.\n");
	printf("[4] Stop the thread.\n");
	
	if(!ViveSR::anipal::Eye::IsViveProEye()){
		printf("\n\nthis device does not have eye-tracker, please change your HMD\n");
		return 0;
	}
	char str = 0;
	int error, handle = NULL;
	while (true) {
		if (str != '\n' && str != EOF) { printf("\nwait for key event :"); }
		str = getchar();
		if (str == '`') break;
		else if (str == '0') {
			error = ViveSR::anipal::Release(ViveSR::anipal::Eye::ANIPAL_TYPE_EYE);
			printf("Successfully release all anipal engines.\n");
			EnableEye = false;
		}
		else if (str == '1') {
			error = ViveSR::anipal::Initial(ViveSR::anipal::Eye::ANIPAL_TYPE_EYE, NULL);
			if (error == ViveSR::Error::WORK) { EnableEye = true; printf("Successfully initialize Eye engine.\n"); }
			else if (error == ViveSR::Error::RUNTIME_NOT_FOUND) printf("please follows SRanipal SDK guide to install SR_Runtime first\n");
			else printf("Fail to initialize Eye engine. please refer the code %d of ViveSR::Error.\n", error);
		}
		else if (str == '2') {
			int result = ViveSR::anipal::Eye::LaunchEyeCalibration(callback);
			printf("Calibration result: %d", result);
		}
		else if (str == '3') {
			looping = true;
			t = new std::thread(streaming);
		}
		else if (str == '4') {
			if (t == nullptr) continue;
			looping = false;
			t->join();
			delete t;
			t = nullptr;
		}
	}
	ViveSR::anipal::Release(ViveSR::anipal::Eye::ANIPAL_TYPE_EYE);
}