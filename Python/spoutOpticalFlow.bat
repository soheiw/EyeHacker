call activate sr

start python spoutOpticalFlow.py --receive_name RecordPlayer --send_name PastOpticalFlow
start python spoutOpticalFlow.py --receive_name ThetaEqCh1 --send_name RealtimeOpticalFlow