# SRProject-EyeTracking

## What is this?

2つの全天周動画，例えばリアルタイム映像と過去映像とを自働的に切り替えるシステム．

全天周映像のどこを見ているのかという視線情報を取得し，その周辺における動画の動きの情報をもとに，動画を切り替えるか否かを決定する．
![Blend.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/Blend.png)

## How To Use

### 動作環境

* Windows10 64bit
* Unity 2018.1.6f1
* HTC VIVE
* Pupil Labs
* RICOH THETA S
* TouchDesigner 099 2018.27300
* Python 3.5.6

### 使用アセット

* [KlakSpout v0.1.2](https://github.com/keijiro/KlakSpout)
* [FFmpegOut v0.1.4](https://github.com/keijiro/FFmpegOut)
* [uOSC](https://github.com/hecomi/uOSC)
* [warapuriさん作天球モデル](http://warapuri.com/post/131599525953/unity%E3%81%A8oculus%E3%81%A7360%E5%BA%A6%E3%83%91%E3%83%8E%E3%83%A9%E3%83%9E%E5%85%A8%E5%A4%A9%E5%91%A8%E5%8B%95%E7%94%BB%E3%82%92%E8%A6%8B%E3%82%8B%E6%96%B9%E6%B3%95%E7%84%A1%E6%96%99%E7%B7%A8)
* [SteamVR v1.2.3](https://github.com/ValveSoftware/steamvr_unity_plugin/releases/tag/1.2.3)
* [pupil_plugin v0.6](https://github.com/pupil-labs/hmd-eyes/releases)

### 使用方法

* ServerとPlayer，TouchDesignerを同時に開く．さらに，PythonでspoutOpticalflow.pyかspoutBackgroundSubtractor.pyを起動しておく．

#### Server

* THETAを繋いでLiveモードで起動する．
![Server.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/Server.png)

#### Player

* Calibration Sceneから起動する．
* Pupil Serviceを立ち上げる．
* 右コントローラのタッチパッドを押すと瞳孔キャリブレーション開始．
* 右コントローラのグリップを押すとrayの表示/非表示を切り替え．

#### TouchDesigner

![TouchDesigner.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/TouchDesigner.png)

* `Play`ボタンで録画映像の再生・停止．
* `Rec`ボタンで録画開始・終了．
* `HMDdirection`ボタンで視線位置をPupilLabsのデータにするかHMDのdirectionに固定するかを決定．
