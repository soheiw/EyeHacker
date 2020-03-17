# -*- coding: UTF-8 -*-
import cv2

if __name__ == '__main__':

    # 画像の読み込み
    img_src1 = cv2.imread("otome.jpg", cv2.IMREAD_GRAYSCALE)
    img_src2 = cv2.imread("otome_origin.jpg", cv2.IMREAD_GRAYSCALE)

    # fgbg = cv2.bgsegm.createBackgroundSubtractorMOG()
    #fgmask = fgbg.apply(img_src1)
    #fgmask = fgbg.apply(img_src2)

    absimage = cv2.absdiff(img_src1, img_src2)

    # 表示
    cv2.imshow('frame',absimage)

    # 検出画像
    bg_diff_path  = './diff.jpg'
    cv2.imwrite(bg_diff_path,absimage)

    cv2.waitKey(0)
    cv2.destroyAllWindows()