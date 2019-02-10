import cv2
import numpy as np

per_pixel = 8 #GCD(720,1280) = 80の約数
width = 1280
height = 720

cap = cv2.VideoCapture("ChangeWholeEnvironment.mp4")

ret, frame1 = cap.read() # frame1は(720,1280,3)のnumpy.ndarray
frame1_picked = np.zeros((int(height / per_pixel),int(width / per_pixel),3))

# for v in range(int(height / per_pixel)):
#     for u in range(int(width / per_pixel)):
#         frame1_picked[v,u,0] = frame1[v * per_pixel, u * per_pixel, 0]
#         frame1_picked[v,u,1] = frame1[v * per_pixel, u * per_pixel, 1]
#         frame1_picked[v,u,2] = frame1[v * per_pixel, u * per_pixel, 2]

frame1_picked = cv2.resize(frame1, (160,90))

fgbg = cv2.createBackgroundSubtractorMOG2(detectShadows = False) 

frame1_gray = np.uint8(frame1_picked)
prvs = cv2.cvtColor(frame1_gray,cv2.COLOR_BGR2GRAY)  
           
bgr = np.zeros_like(frame1_gray)
cv2.startWindowThread()

while(1):
    ret, frame2 = cap.read()
    frame2_picked = np.zeros((int(height / per_pixel),int(width / per_pixel),3))

    # for v in range(int(height / per_pixel)):
    #     for u in range(int(width / per_pixel)):
    #         frame2_picked[v,u,0] = frame2[v * per_pixel, u * per_pixel, 0]
    #         frame2_picked[v,u,1] = frame2[v * per_pixel, u * per_pixel, 1]
    #         frame2_picked[v,u,2] = frame2[v * per_pixel, u * per_pixel, 2]

    frame2_picked = cv2.resize(frame2, (160,90))

    fgmask = fgbg.apply(frame2_picked)

    frame2_gray = np.uint8(frame2_picked)
    next = cv2.cvtColor(frame2_gray, cv2.COLOR_BGR2GRAY)
    
    cv2.namedWindow('movie', cv2.WINDOW_NORMAL)
    cv2.imshow('movie', frame2_picked)

    flow = cv2.calcOpticalFlowFarneback(prvs,next, None, 0.1, 1, 15, 1, 5, 1.1, 0) #(720/per_pixel,1280/per_pixel,2)

    mag, ang = cv2.cartToPolar(flow[...,0], flow[...,1])
    mag_normalized = cv2.normalize(mag,None,0,255,cv2.NORM_MINMAX)
    bgr[...,0] = mag_normalized
    bgr[...,1] = mag_normalized
    bgr[...,2] = mag_normalized
    #bgr = cv2.cvtColor(hsv,cv2.COLOR_HSV2BGR)

    cv2.namedWindow('OpticalFlow', cv2.WINDOW_NORMAL)
    cv2.imshow('OpticalFlow',fgmask)
    k = cv2.waitKey(30) & 0xff
    if k == ord('q'):
        break
    elif k == ord('s'):
        cv2.imwrite('movie.png',frame2)
        cv2.imwrite('opticalflow.png',bgr)
    prvs = next

cap.release()
cv2.waitKey(1)
cv2.destroyAllWindows()
cv2.waitKey(1)
