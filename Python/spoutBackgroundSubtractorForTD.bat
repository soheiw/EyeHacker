call activate sr

start python spoutBackgroundSubtractor.py --receive_name SpoutSender1 --send_name SpoutReceiver1 --spout_size 3008 1504
start python spoutBackgroundSubtractor.py --receive_name SpoutSender2 --send_name SpoutReceiver2 --spout_size 3008 1504