from __future__ import print_function
import sys
sys.path.insert(0, 'src')
import transform, numpy as np, vgg, pdb, os
import scipy.misc
import tensorflow as tf
from utils import save_img, get_img, exists, list_files
from argparse import ArgumentParser
#from collections import defaultdict
import time,sys
#import json
#import subprocess
import cv2
#import matplotlib.pyplot as plt

#Spoutforpython
sys.path.append('./Library')
import SpoutSDK
import pygame
from pygame.locals import *
from OpenGL.GL import *
from OpenGL.GLU import *

BATCH_SIZE = 1
DEVICE = '/gpu:0'

def build_parser():
    parser = ArgumentParser()
    parser.add_argument('--checkpoint', type=str,
                        dest='checkpoint_dir',
                        help='dir or .ckpt file to load checkpoint from',
                        metavar='CHECKPOINT', default = 'ckpt_line_c100')
    parser.add_argument('--device', type=str,
                        dest='device',help='device to perform compute on',
                        metavar='DEVICE', default=DEVICE)

    parser.add_argument('--batch_size', type=int,
                        dest='batch_size',help='batch size for feedforwarding',
                        metavar='BATCH_SIZE', default=BATCH_SIZE)
    
    parser.add_argument('--receiver_size', nargs = 2, type=int,default=[1280,720], help='Width and height of the spout receiver')   

    parser.add_argument('--spout_name', type=str, default='LadybugSender', help='Spout receiving name - the name of the sender you want to receive')  


    return parser

def check_opts(opts):
    exists(opts.checkpoint_dir, 'Checkpoint not found!')



def main():
    parser = build_parser()
    opts = parser.parse_args()
    check_opts(opts)
    checkpoint_dir = opts.checkpoint_dir
    device_t=opts.device
    batch_size=opts.batch_size

    # window details
    spoutReceiverWidth = opts.receiver_size[0]
    spoutReceiverHeight = opts.receiver_size[1]
    spoutSenderWidth = min(spoutReceiverWidth,1280)
    spoutSenderHeight = min(spoutReceiverHeight,720)
    display = (spoutSenderWidth,spoutSenderHeight)

    # window setup
    pygame.init() 
    pygame.display.set_caption('STSender')
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
    receiverName = opts.spout_name 
    
    # create spout receiver
    spoutReceiver = SpoutSDK.SpoutReceiver()

	# Its signature in c++ looks like this: bool pyCreateReceiver(const char* theName, unsigned int theWidth, unsigned int theHeight, bool bUseActive);
    spoutReceiver.pyCreateReceiver(receiverName,spoutReceiverWidth,spoutReceiverHeight, False)

    # init spout sender
    spoutSender = SpoutSDK.SpoutSender()
   
    # Its signature in c++ looks like this: bool CreateSender(const char *Sendername, unsigned int width, unsigned int height, DWORD dwFormat = 0);
    spoutSender.CreateSender('STSender', spoutSenderWidth, spoutSenderHeight, 0)

    # create texture for spout receiver
    textureReceiveID = glGenTextures(1)    
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

    # neural net init
    g = tf.Graph()    
    soft_config = tf.ConfigProto(allow_soft_placement=True)
    soft_config.gpu_options.allow_growth = True
    #soft_config.gpu_options.per_process_gpu_memory_fraction=0.5 # 最大値の50%まで
        

    with g.as_default(), g.device(opts.device), \
        tf.Session(config=soft_config) as sess:
        
        img_shape = (spoutSenderHeight, spoutSenderWidth, 3)
        batch_shape = (batch_size,) + img_shape
        img_placeholder = tf.placeholder(tf.float32, shape=batch_shape, name='img_placeholder')
        
        preds = transform.net(img_placeholder)
        
        saver = tf.train.Saver()
    
        if os.path.isdir(checkpoint_dir):
            ckpt = tf.train.get_checkpoint_state(checkpoint_dir)
            if ckpt and ckpt.model_checkpoint_path:
                saver.restore(sess, ckpt.model_checkpoint_path)
            else:
                raise Exception("No checkpoint found...")
        else:
            saver.restore(sess, checkpoint_dir)
        
        while True:
            begin = time.perf_counter()
         
            # receive texture
            # Its signature in c++ looks like this: bool pyReceiveTexture(const char* theName, unsigned int theWidth, unsigned int theHeight, GLuint TextureID, GLuint TextureTarget, bool bInvert, GLuint HostFBO);
            spoutReceiver.pyReceiveTexture(receiverName, spoutReceiverWidth, spoutReceiverHeight, textureReceiveID, GL_TEXTURE_2D, False, 0)
            
            glBindTexture(GL_TEXTURE_2D, textureReceiveID)
            glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE)
            glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE)
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST)
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST)
            
            # copy pixel byte array from received texture - this example doesn't use it, but may be useful for those who do want pixel info      
            image = glGetTexImage(GL_TEXTURE_2D, 0, GL_RGB, GL_UNSIGNED_BYTE, outputType=None)  #Using GL_RGB can use GL_RGBA 
            glBindTexture(GL_TEXTURE_2D, 0)
            # swap width and height data around due to oddness with glGetTextImage. http://permalink.gmane.org/gmane.comp.python.opengl.user/2423
            image.shape = (image.shape[1], image.shape[0], image.shape[2])
            image = cv2.resize(image, (spoutSenderWidth, spoutSenderHeight))
            if image.shape[0] > 720:
                image = cv2.resize(image,(image.shape[1],720))                
            if image.shape[1] > 1280:
                image = cv2.resize(image,(1280,image.shape[0])) 
            #cv2.imshow("Received", cv2.cvtColor(image, cv2.COLOR_RGB2BGR))    
            #print(image.shape) 
            # setup window to draw to screen
            glActiveTexture(GL_TEXTURE0)

            # clean start
            glClear(GL_COLOR_BUFFER_BIT  | GL_DEPTH_BUFFER_BIT )
            # reset drawing perspective
            glLoadIdentity()
        
                      
            #style transfer
            X = image[np.newaxis,:]
            transfered = sess.run(preds, feed_dict={img_placeholder:X})[0]
            #float32 To uint8
            transfered = np.uint8(np.clip(transfered,0,255))
            
            #cv2.imshow("Transfer", cv2.cvtColor(transfered, cv2.COLOR_RGB2BGR)
        
            for event in pygame.event.get():
                if event.type == pygame.QUIT:
                    pygame.quit()
                    cv2.destroyAllWindows()
                    quit()

            if cv2.waitKey(1) >= 0:
                cv2.imwrite("image.png", transfered)
                img = cv2.imread("image.png")
                break
            
         
        
            # setup the texture so we can load the stylised output into it
            glBindTexture(GL_TEXTURE_2D, senderTextureID)
            glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE)
            glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE)
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST)
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST)
            
            # copy output into texture
            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, spoutSenderWidth, spoutSenderHeight, 0, GL_RGB, GL_UNSIGNED_BYTE, transfered)
            
            # setup window to draw to screen
            glActiveTexture(GL_TEXTURE0)

            # clean start
            glClear(GL_COLOR_BUFFER_BIT  | GL_DEPTH_BUFFER_BIT )
            # reset drawing perspective
            glLoadIdentity()

            # draw texture on screen
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

            # update display 
            pygame.display.flip()

            # send texture to Spout
            # Its signature in C++ looks like this: bool SendTexture(GLuint TextureID, GLuint TextureTarget, unsigned int width, unsigned int height, bool bInvert=true, GLuint HostFBO = 0);
            spoutSender.SendTexture(senderTextureID, GL_TEXTURE_2D, spoutSenderWidth, spoutSenderHeight, False, 0)
        
            end = time.perf_counter()
            print('\r','FPS:',  1.0/(end-begin),end='')
            sys.stdout.flush()

    cv2.destroyAllWindows()
 
if __name__ == '__main__':
    main()


