# SRProject-EyeTracking

## What is this?

2つの全天周動画，例えばリアルタイム映像と過去映像とを，気づかれないタイミングを見計らって自動的に切り替えるシステム．

全天周映像のどこを見ているのかという視線情報を取得し，その周辺における動画の動きの情報をもとに，動画を切り替えるか否かを決定する．

![overview.png](https://github.com/inamilab/EyeHacker/blob/develop/images/overview.png)

![blend.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/blend.png)

## How To Use

### 動作環境

* Windows10 64bit
* Unity 2018.1.6f1
* HTC VIVE Pro
* Pupil Labs
* Insta360 Air
* ZOOM H2n
* TouchDesigner 099 Educational 2018.27910
* Python 3.5.6

### 使用アセット

* [KlakSpout v0.2.4](https://github.com/keijiro/KlakSpout)
* [uOSC](https://github.com/hecomi/uOSC)
* [warapuriさん作天球モデル](http://warapuri.com/post/131599525953/unity%E3%81%A8oculus%E3%81%A7360%E5%BA%A6%E3%83%91%E3%83%8E%E3%83%A9%E3%83%9E%E5%85%A8%E5%A4%A9%E5%91%A8%E5%8B%95%E7%94%BB%E3%82%92%E8%A6%8B%E3%82%8B%E6%96%B9%E6%B3%95%E7%84%A1%E6%96%99%E7%B7%A8)
* [SteamVR v1.2.3](https://github.com/ValveSoftware/steamvr_unity_plugin/releases/tag/1.2.3)
* [pupil_plugin v0.6](https://github.com/pupil-labs/hmd-eyes/releases)

### 使用方法

* UnityでSRWithGazePlayerを，TouchDesignerでVideoServerをそれぞれ開く．

![system.png](https://github.com/inamilab/EyeHacker/blob/develop/images/system.png)

#### Server(TouchDesigner)

* カメラとマイクを繋いで，マイクの電源を入れる．TouchDesignerのperform画面に`Realtime`に映像が映らない場合は，編集画面で`videodevin1`オペレータを探し，`Library`と`Device`を適宜指定し直す．
* spoutBackgroundSubstractorForTD.batを起動し，Pythonの画像処理を走らせる．

![Server.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/Server.png)

* 各`FileName`フィールドでファイル名の変更．
* 各`Begin`，`End`スライダで映像のトリミング．
* 各`Roll`，`Pitch`，`Yaw`スライダで映像の回転を調整するOSC信号をUnityに送信．
* 各`Black`，`Gamma`フィールドで映像の明るさ調整．
* `Threshold`スライダでThresholdの値調整．
* `RotFactor`，`Radius`スライダでWholeTiskの計算におけるパラメータを操作．
* `WaitingTime`スライダで待ち時間を調整．
* `RewindSpeed`スライダで巻き戻りの速度を調整．
* `Auto/Switch`トグルを`Auto`にすると映像自動切り替え，`Manual`にすると映像手動切り替え．
  * `Manual`のときは，`PlayingBuffer`トグルで映像を選択．
* `Auto switch is disabled`トグルを押すと`Time`のスライダが動きだし，スライダが振り切れると映像が切り替わる．
* `Calibration`スイッチを押すとUnity側でPupil Labsのキャリブレーションがコントロール可能（後述）． 
* `View Ray`トグルで体験者へのrayの表示/非表示を切り替え．
* `RayCast`トグルを`Gaze`にするとPupilLabsのデータが視線位置に，`HMD`にするとHMDの向いている方向が視線位置になる．
* `Method`トグルでアルゴリズムを選択．
* 各`Play`ボタンで録画映像の再生・停止．
* 各`Rec`ボタンで録画開始・終了．
* `G-Roll`，`G-Pitch`，`G-Yaw`スライダで全ての映像の回転を同時に調整するOSC信号をUnityに送信．

#### Controller(iOS, optional)

* [Chromeリモートデスクトップ](https://apps.apple.com/jp/app/chrome-%E3%83%AA%E3%83%A2%E3%83%BC%E3%83%88-%E3%83%87%E3%82%B9%E3%82%AF%E3%83%88%E3%83%83%E3%83%97/id944025852)をPC/iOSの両方に入れ，同じアカウントで両方にログインする．PC画面を共有することで，iOS側からPC画面を直接操作する．

#### Player(Unity)

![Player.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/Player.png)

* Calibration Sceneから起動する．
* Pupil Serviceを立ち上げる．
* TouchDesignerの`Calibration`スイッチを押すと，瞳孔キャリブレーション開始．
  * 正常にキャリブレーションが終わると，自動でMain Sceneに移り映像が提示される．
  * キャリブレーションの精度が悪いとやり直しになる．もう一度`Calibration`スイッチを押す．
* Main Scene移行後に`Calibration`スイッチを押すと，瞳孔キャリブレーション開始前の場面に戻る．さらに`Calibration`スイッチを押すと，再度瞳孔キャリブレーション開始．
