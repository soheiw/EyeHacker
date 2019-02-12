import cv2
import numpy as np

per_pixel = 8 #GCD(720,1280) = 80の約数
width = 3840
height = 1920

cap = cv2.VideoCapture("LLK9.mp4")

fourcc = cv2.VideoWriter_fourcc('H','2','6','4')
out = cv2.VideoWriter('heatmap.avi',fourcc, 20.0, (480,240), False)

ret, frame1 = cap.read() # frame1は(720,1280,3)のnumpy.ndarray
#frame1_picked = np.zeros((int(height / per_pixel),int(width / per_pixel),3))
frame1_picked = cv2.resize(frame1, (480,240))

fgbg = cv2.bgsegm.createBackgroundSubtractorMOG()  
           
cv2.startWindowThread()

while(1):
    ret, frame2 = cap.read()
    #frame2_picked = np.zeros((int(height / per_pixel),int(width / per_pixel),3))
    frame2_picked = cv2.resize(frame2, (480,240))

    fgmask = fgbg.apply(frame2_picked)
    
    cv2.namedWindow('movie', cv2.WINDOW_NORMAL)
    cv2.imshow('movie', frame2_picked)

    cv2.namedWindow('OpticalFlow', cv2.WINDOW_NORMAL)
    cv2.imshow('OpticalFlow',fgmask)
    
    out.write(fgmask)

    k = cv2.waitKey(30) & 0xff
    if k == ord('q'):
        break
    elif k == ord('s'):
        cv2.imwrite('movie.png',frame2)
        cv2.imwrite('opticalflow.png',fgmask)
    prvs = next

cap.release()
cv2.waitKey(1)
cv2.destroyAllWindows()
cv2.waitKey(1)
