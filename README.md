# SRProject-EyeTracking

## What is this?

2つの全天周動画，例えばリアルタイム映像と過去映像とを自動的に切り替えるシステム．

全天周映像のどこを見ているのかという視線情報を取得し，その周辺における動画の動きの情報をもとに，動画を切り替えるか否かを決定する．

![overview.png](https://github.com/inamilab/EyeHacker/blob/develop/images/overview.png)

![blend.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/blend.png)

## How To Use

### 動作環境

* Windows10 64bit
* Unity 2018.1.6f1
* HTC VIVE
* Pupil Labs
* RICOH THETA S
* TouchDesigner 099 Educational 2018.27910
* Python 3.5.6

### 使用アセット

* [KlakSpout v0.1.2](https://github.com/keijiro/KlakSpout)
* [uOSC](https://github.com/hecomi/uOSC)
* [warapuriさん作天球モデル](http://warapuri.com/post/131599525953/unity%E3%81%A8oculus%E3%81%A7360%E5%BA%A6%E3%83%91%E3%83%8E%E3%83%A9%E3%83%9E%E5%85%A8%E5%A4%A9%E5%91%A8%E5%8B%95%E7%94%BB%E3%82%92%E8%A6%8B%E3%82%8B%E6%96%B9%E6%B3%95%E7%84%A1%E6%96%99%E7%B7%A8)
* [SteamVR v1.2.3](https://github.com/ValveSoftware/steamvr_unity_plugin/releases/tag/1.2.3)
* [pupil_plugin v0.6](https://github.com/pupil-labs/hmd-eyes/releases)

### 使用方法

* UnityでSRWithGazePlayerを，TouchDesignerでVideoServerをそれぞれ開く．

![system.png](https://github.com/inamilab/EyeHacker/blob/develop/images/system.png)

#### Server(TouchDesigner)

* THETAを繋いでLiveモードで起動する．`RealtimeImage`に映像が映らない場合は，編集画面で`videodevin1`オペレータを探し，`Library`を`DirectShow (WDM)`に，`Device`を`THETA UVC FullHD Blender`に設定する．
* spoutOpticalFlowForTD.batかspoutBackgroundSubstractorForTD.batを起動し，Pythonの画像処理を走らせる．

![Server.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/Server.png)

* 各`FileName`フィールドでファイル名の変更．
* 各`Play`ボタンで録画映像の再生・停止．
* 各`Rec`ボタンで録画開始・終了．
* 各`Begin`，`End`スライダで映像のトリミング．
* `BaseLine`や`RotFactor`，`Radius`スライダでWholeTiskやThresholdの計算式におけるパラメータが操作可能．
* `RayCast`トグルを`Gaze`にするとPupilLabsのデータが視線位置に，`HMD`にするとHMDの向いている方向が視線位置になる．
* `Switch`トグルを`Auto`にすると映像自動切り替え，`Manual`にすると映像手動切り替え．
  * `Manual`のときは，`ManualVideo`トグルで映像を選択．
* `Method`トグルでアルゴリズムを選択

#### Controller(iOS, optional)

* UnityでiOSControllerをビルドしてiPhoneに移す．
* iOSアプリ右上のフィールドでPC側のIPアドレスを指定する．
* TouchDesignerの`IPOut`オペレータでiOS側のIPアドレスを指定する．

#### Player(Unity)

* Calibration Sceneから起動する．
* Pupil Serviceを立ち上げる．
* `c`キーを押すと，瞳孔キャリブレーション開始．
* 右コントローラのグリップを押すと，体験者へのrayの表示/非表示を切り替え．

![Player.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/Player.png)
