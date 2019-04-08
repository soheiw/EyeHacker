# -*- coding: utf-8 -*-

# Add relative directory ../Library to import path, so we can import the SpoutSDK.pyd library. Feel free to remove these if you put the SpoutSDK.pyd file in the same directory as the python scripts.
import cv2
import numpy as np

import sys
sys.path.append('Spout-for-Python-master/Library')

import argparse
import SpoutSDK
import pygame
from pygame.locals import *
from OpenGL.GL import *
from OpenGL.GLU import *


"""parsing and configuration"""
def parse_args():
    desc = "Optical Flow with Spout"
    parser = argparse.ArgumentParser(description=desc)

    parser.add_argument('--spout_size', nargs = 2, type=int, default=[1280, 720], help='Width and height of the spout receiver')   

    parser.add_argument('--receive_name', type=str, default='ThetaEqCh1', help='Spout receiving name - the name of the sender you want to receive')  

    parser.add_argument('--pixel_ratio', type=int, default=8, help='pixel compression ratio')

    parser.add_argument('--send_name', type=str, default='RealtimeOpticalFlow', help='Spout sending name - the name of the sender you want to send')

    #parser.add_argument('--window_size', nargs = 2, type=int, default=[1280, 720], help='Width and height of the window')    

    return parser.parse_args()

"""main"""
def main():

    # parse arguments
    args = parse_args()
    
    # window details
    spoutReceiverWidth = args.spout_size[0]
    spoutReceiverHeight = args.spout_size[1]
    per_pixel = args.pixel_ratio #GCD(720,1280) = 80の約数
    spoutSenderWidth = int(spoutReceiverWidth / per_pixel) # min(spoutReceiverWidth,1280)
    spoutSenderHeight = int(spoutReceiverHeight / per_pixel) # min(spoutReceiverHeight,720)
    display = (spoutSenderWidth,spoutSenderHeight)

    #fourcc = cv2.VideoWriter_fourcc('M','J','P','G')
    #out1 = cv2.VideoWriter('movie.avi',fourcc,20.0,(spoutReceiverWidth,spoutReceiverHeight))
    #out2 = cv2.VideoWriter('opticalflow.avi',fourcc,20.0,(spoutSenderWidth,spoutSenderHeight))
    
    # window setup
    pygame.init() 
    pygame.display.set_caption(args.send_name)
    pygame.display.set_mode(display, DOUBLEBUF|OPENGL)

    # OpenGL init
    glMatrixMode(GL_PROJECTION)
    glLoadIdentity()
    glOrtho(0,spoutSenderWidth,spoutSenderHeight,0,1,-1)
    glMatrixMode(GL_MODELVIEW)
    glDisable(GL_DEPTH_TEST)
    glClearColor(0.0,0.0,0.0,0.0)
    glEnable(GL_TEXTURE_2D)

    # init spout receiver
    receiverName = args.receive_name 
    
    # create spout receiver
    spoutReceiver = SpoutSDK.SpoutReceiver()

	# Its signature in c++ looks like this: bool pyCreateReceiver(const char* theName, unsigned int theWidth, unsigned int theHeight, bool bUseActive);
    spoutReceiver.pyCreateReceiver(receiverName,spoutReceiverWidth,spoutReceiverHeight, False)

    # create texture for spout receiver
    textureReceiveID = glGenTextures(1)    

    # init spout sender
    spoutSender = SpoutSDK.SpoutSender()
    
    # Its signature in c++ looks like this: bool CreateSender(const char *Sendername, unsigned int width, unsigned int height, DWORD dwFormat = 0);
    spoutSender.CreateSender(args.send_name, spoutSenderWidth, spoutSenderHeight, 0)

    # init spout sender texture ID
    senderTextureID = glGenTextures(1)

    # initalise receiver texture
    glBindTexture(GL_TEXTURE_2D, textureReceiveID)
    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE)
    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE)
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST)
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST)

    # copy data into texture
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, spoutReceiverWidth, spoutReceiverHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, None) 
    glBindTexture(GL_TEXTURE_2D, 0)

    first_try = True # To get first frame

    # loop for graph frame by frame
    while(True):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                spoutReceiver.ReleaseReceiver()
                pygame.quit()
                quit()
        
        if cv2.waitKey(1) & 0xFF == ord('q'):
            #out1.release()
            #out2.release()
            cv2.destroyAllWindows()
            break

        # receive texture
        # Its signature in c++ looks like this: bool pyReceiveTexture(const char* theName, unsigned int theWidth, unsigned int theHeight, GLuint TextureID, GLuint TextureTarget, bool bInvert, GLuint HostFBO);
        spoutReceiver.pyReceiveTexture(receiverName, spoutReceiverWidth, spoutReceiverHeight, np.uint32(textureReceiveID).item(), GL_TEXTURE_2D, False, 0)
       
        glBindTexture(GL_TEXTURE_2D, textureReceiveID)

        # copy pixel byte array from received texture - this example doesn't use it, but may be useful for those who do want pixel info      
        data = glGetTexImage(GL_TEXTURE_2D, 0, GL_RGB, GL_UNSIGNED_BYTE, outputType=None)  #Using GL_RGB can use GL_RGBA 
        
        # swap width and height data around due to oddness with glGetTextImage. http://permalink.gmane.org/gmane.comp.python.opengl.user/2423
        data.shape = (data.shape[1], data.shape[0], data.shape[2]) #(720,1280,3)
        image = cv2.resize(data, (spoutReceiverWidth, spoutReceiverHeight))
        if image.shape[0] > 720:
                image = cv2.resize(image,(image.shape[1],720))
        if image.shape[1] > 1280:
            image = cv2.resize(image,(1280,image.shape[0])) 

        # setup window to draw to screen
        glActiveTexture(GL_TEXTURE0)

        # clean start
        glClear(GL_COLOR_BUFFER_BIT  | GL_DEPTH_BUFFER_BIT )
        # reset drawing perspective
        glLoadIdentity()
       
        #imageに対してのopticalflow処理
        width = spoutReceiverWidth
        height = spoutReceiverHeight

        if first_try == True:
            first_try = False
            frame1 = image
            frame1_picked = np.zeros((int(height / per_pixel),int(width / per_pixel),3)) #(90,160,3)
            for v in range(int(height / per_pixel)):
                for u in range(int(width / per_pixel)):
                    frame1_picked[v,u,0] = frame1[v * per_pixel, u * per_pixel, 0]
                    frame1_picked[v,u,1] = frame1[v * per_pixel, u * per_pixel, 1]
                    frame1_picked[v,u,2] = frame1[v * per_pixel, u * per_pixel, 2]
            frame1_gray = np.uint8(frame1_picked)
            prvs = cv2.cvtColor(frame1_gray,cv2.COLOR_BGR2GRAY)

            bgr = np.zeros_like(frame1_gray)
        else:
            frame2 = image

            #show = cv2.cvtColor(frame2, cv2.COLOR_BGR2RGB)
            
            #out1.write(show)

            frame2_picked = np.zeros((int(height / per_pixel),int(width / per_pixel),3))
            for v in range(int(height / per_pixel)):
                for u in range(int(width / per_pixel)):
                    frame2_picked[v,u,0] = frame2[v * per_pixel, u * per_pixel, 0]
                    frame2_picked[v,u,1] = frame2[v * per_pixel, u * per_pixel, 1]
                    frame2_picked[v,u,2] = frame2[v * per_pixel, u * per_pixel, 2]
            frame2_gray = np.uint8(frame2_picked)
            next = cv2.cvtColor(frame2_gray, cv2.COLOR_BGR2GRAY)

            cv2.namedWindow('movie', cv2.WINDOW_AUTOSIZE)
            show_movie = cv2.cvtColor(frame2_gray, cv2.COLOR_BGR2RGB)
            cv2.imshow('movie', show_movie)

            flow = cv2.calcOpticalFlowFarneback(prvs,next, None, 0.5, 1, 15, 1, 5, 1.1, 0) #(720/per_pixel,1280/per_pixel,2)

            mag, ang = cv2.cartToPolar(flow[...,0], flow[...,1])
            mag_normalized = cv2.normalize(mag,None,0,255,cv2.NORM_MINMAX)
            bgr[...,0] = mag_normalized
            bgr[...,1] = mag_normalized
            bgr[...,2] = mag_normalized

            #cv2.namedWindow('OpticalFlow', cv2.WINDOW_NORMAL)
            #cv2.imshow('OpticalFlow',bgr)

            #out2.write(bgr)

            prvs = next

        # setup the texture so we can load the optical flow output into it
        glBindTexture(GL_TEXTURE_2D, senderTextureID)
        glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE)
        glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE)
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST)
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST)
            
        # copy output into texture
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, spoutSenderWidth, spoutSenderHeight, 0, GL_RGB, GL_UNSIGNED_BYTE, bgr)

        # clean start
        glClear(GL_COLOR_BUFFER_BIT  | GL_DEPTH_BUFFER_BIT )
        # reset drawing perspective
        glLoadIdentity()

        # draw texture on screen
        # glPushMatrix() use these lines if you want to scale your received texture
        # glScale(0.3, 0.3, 0.3)
        glBegin(GL_QUADS)

        glTexCoord(0,0)        
        glVertex2f(0,0)

        glTexCoord(1,0)
        glVertex2f(spoutSenderWidth,0)

        glTexCoord(1,1)
        glVertex2f(spoutSenderWidth,spoutSenderHeight)

        glTexCoord(0,1)
        glVertex2f(0,spoutSenderHeight)
        
        glEnd()
        # glPopMatrix() make sure to pop your matrix if you're doing a scale        
        # update window
        pygame.display.flip()

        #bind our senderTexture and copy the window's contents into the texture
        glBindTexture(GL_TEXTURE_2D, senderTextureID)
        glCopyTexImage2D(GL_TEXTURE_2D,0,GL_RGBA,0,0,spoutSenderWidth,spoutSenderHeight,0)
        glBindTexture(GL_TEXTURE_2D, 0)

        # send texture to Spout
        # Its signature in C++ looks like this: bool SendTexture(GLuint TextureID, GLuint TextureTarget, unsigned int width, unsigned int height, bool bInvert=true, GLuint HostFBO = 0);
        spoutSender.SendTexture(np.uint32(senderTextureID).item(), GL_TEXTURE_2D, spoutSenderWidth, spoutSenderHeight, True, 0)

        #pygame.time.wait(10)

if __name__ == '__main__':
    main()
