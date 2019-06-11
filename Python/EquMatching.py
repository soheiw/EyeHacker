# -*- coding: utf-8 -*-
# refer: https://gist.github.com/muminoff/25f7a86f28968eb89a4b722e960603fe
# refer: https://stackoverflow.com/questions/29678510/convert-21-equirectangular-panorama-to-cube-map

import sys
from PIL import Image
from math import pi,sin,cos,tan,atan2,hypot,floor
import cv2
import numpy as np
#from numpy import clip
from matplotlib import pyplot as plt

img = cv2.imread('equirectangular.jpg')
img_gray = cv2.cvtColor(img, cv2.COLOR_RGB2GRAY)
template = cv2.imread('template.jpg', 0)
w, h = template.shape[::-1]

# apply template Matching
res = cv2.matchTemplate(img_gray, template, eval('cv2.TM_CCOEFF'))
min_val, max_val, min_loc, max_loc = cv2.minMaxLoc(res)
top_left = max_loc
bottom_right = (top_left[0] + w, top_left[1] + h)

#the center position of the marker 
x_center = top_left[0] + w / 2
y_center = top_left[1] + h / 2

print(x_center, y_center)

cv2.rectangle(img, top_left, bottom_right, (0, 0, 255), 3)

cv2.startWindowThread()

cv2.imshow('result', img)

plt.show()

cv2.waitKey(0)
cv2.destroyAllWindows()
