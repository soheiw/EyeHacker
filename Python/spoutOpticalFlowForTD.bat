call activate sr

start python spoutOpticalFlow.py --receive_name SpoutSender1 --send_name SpoutReceiver1
start python spoutOpticalFlow.py --receive_name SpoutSender2 --send_name SpoutReceiver2