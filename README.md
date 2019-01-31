# SRProject-EyeTracking

## What is this?

全天周映像のどこを見ているのかという視線情報を取得し，その部分のみ事前に録画した映像に切り替えるSRシステム．

## How To Use

### 動作環境
* Windows10 64bit
* Unity 2018.1.6f1
* HTC VIVE
* RICOH THETA S
* Pupil Labs

### 使用アセット
* [KlakSpout v0.1.2](https://github.com/keijiro/KlakSpout)/[KlakNDI v0.0.9](https://github.com/keijiro/KlakNDI)
* [FFmpegOut v0.1.4](https://github.com/keijiro/FFmpegOut)
* [uOSC](https://github.com/hecomi/uOSC)
* [warapuriさん作天球モデル](http://warapuri.com/post/131599525953/unity%E3%81%A8oculus%E3%81%A7360%E5%BA%A6%E3%83%91%E3%83%8E%E3%83%A9%E3%83%9E%E5%85%A8%E5%A4%A9%E5%91%A8%E5%8B%95%E7%94%BB%E3%82%92%E8%A6%8B%E3%82%8B%E6%96%B9%E6%B3%95%E7%84%A1%E6%96%99%E7%B7%A8)
* [SteamVR v1.2.3](https://github.com/ValveSoftware/steamvr_unity_plugin/releases/tag/1.2.3)
* [pupil_plugin v0.6](https://github.com/pupil-labs/hmd-eyes/releases)

### 使用方法
* ServerとPlayerを同時に開く．

#### Server
* THETAを繋いでLiveモードで起動する．
![Server.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/Server.png)
* `Space`キーで録画映像の再生．`P`キーで指定した場所から録画映像の再生．`L`キーで再生を停止．
* `R`キーで録画開始．`T`キーで録画終了．
* `M`キーでSpout/NDI切り替え．

#### Player
* Calibration Sceneから起動する．
* 視線位置とマスク位置を一致させるために，HMDを被ったユーザの場所でThetaの3軸とコントローラの3軸を合わせて左コントローラのトリガーを引く．
![position01.jpeg](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/position01.jpeg)
![position02.jpeg](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/position02.jpeg)
* Pupil Serviceを立ち上げる．
* 右コントローラのタッチパッドを押すと瞳孔キャリブレーション開始．
* 右コントローラのグリップを押すとrayの表示/非表示を切り替え．
* `M`キーでSpout/NDI切り替え．
