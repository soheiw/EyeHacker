# SR-Project/EyeHacker

## What is this?

2つの全天球映像，例えばリアルタイム映像と過去映像を，体験者に気付かれにくいタイミングを見計らって自動的に切り替えるシステム．

全天球映像のどこを見ているのかという視線情報を取得し，その周辺における映像の画像処理情報をもとに，映像を切り替えるか否かを決定する．

![overview.png](https://github.com/inamilab/EyeHacker/blob/develop/images/overview.png)

![blend.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/blend.png)

## How To Use

### 動作環境

* Windows10 64bit
* Unity 2018.4.6f1
* TouchDesigner 099 Educational 2019.18360
* HTC VIVE Pro Eye
* Insta360 Air
* ZOOM H1n

### 使用アセット

* [KlakSpout v0.2.4](https://github.com/keijiro/KlakSpout)
* [uOSC](https://github.com/hecomi/uOSC)
* [warapuriさん作天球モデル](http://warapuri.com/post/131599525953/unity%E3%81%A8oculus%E3%81%A7360%E5%BA%A6%E3%83%91%E3%83%8E%E3%83%A9%E3%83%9E%E5%85%A8%E5%A4%A9%E5%91%A8%E5%8B%95%E7%94%BB%E3%82%92%E8%A6%8B%E3%82%8B%E6%96%B9%E6%B3%95%E7%84%A1%E6%96%99%E7%B7%A8)
* [Eye Tracking SDK (SRanipal)](https://developer.vive.com/resources/knowledgebase/vive-sranipal-sdk/)

### 使用方法

* TouchDesignerでEyeHackerMixerを，UnityでEyeHackerPlayerを起動．

#### EyeHackerMixer (TouchDesigner)

* カメラとマイクをPCに繋いで，マイクの電源を入れる．
  * TouchDesignerのperform画面の`Realtime`に映像が映らない場合は，編集画面で`videodevin1`オペレータを探し，`Library`を`Media Foundation`に，`Device`を`Video Control`に指定し直す．
  * 音がこの時点で聞こえない場合は，編集画面で`audiodevout1`オペレータを探し，`Device`を`VIVE Pro`に指定し直す．また，`realtimeaudio`オペレータを探し，`Active`をOFF/ONしてみる．
* spoutBackgroundSubstractorForTD.batを起動し，Pythonによる背景差分法を走らせる．

![Server.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/Server.png)

* 各`FileName`フィールドでファイル名の変更．
* 各`Begin`，`End`スライダで映像のトリミング．
* 各`Roll`，`Pitch`，`Yaw`スライダで映像の回転を調整するOSC信号をUnityに送信．
* 各`Black`，`Gamma`フィールドで映像の明るさ調整．
* `Threshold`スライダでThresholdの値調整．
* `Auto/Switch`トグルを`Auto`にすると映像自動切り替え，`Manual`にすると映像手動切り替え．
  * `Manual`のときは，`PlayingBuffer`トグルで映像を選択．
* `Auto switch is disabled`トグルを押すと`Time`のスライダが動きだし，スライダが振り切れると映像が切り替わる．
* `WaitingTime`スライダで待ち時間を調整．
* `RewindSpeed`スライダで巻き戻りの速度を調整．
* `MaxBlendTime`スライダで映像切り替えにかける時間を調整．
  * 実際の切り替え時間は，HMDの回転速度を反映した`BlendTime`の値になる．
* `RotFactor`，`RiskRadius`スライダでWholeRiskの計算におけるパラメータを操作．
* `MaskRadius`スライダでMaskモード時のMaskの大きさを調整．
* `G-Roll`，`G-Pitch`，`G-Yaw`スライダで全ての映像の回転を同時に調整するOSC信号をUnityに送信．
* 各`Play`ボタンで録画映像の再生・停止．
* 各`Rec`ボタンで録画開始・終了．
* `Calibration`スイッチを押すとUnity側でPupil Labsのキャリブレーションがコントロール可能（後述）． 
* `View Ray`トグルで体験者へのrayの表示/非表示を切り替え．
* `All/Mask`トグルを`All`にすると映像全体を切り替え，`Mask`にすると注視領域のMask内外の映像を切り替え．
  * `Mask`モード時は，再生中でないBufferの映像を変えることで，Mask外の映像を切り替え可能．
* `Method`トグルでアルゴリズムを選択．
* `RayCast`トグルを`Gaze`にするとPupilLabsのデータが視線位置に，`HMD`にするとHMDの向いている方向が視線位置になる．

#### Remote Controller(iOS, optional)

* [Chromeリモートデスクトップ](https://apps.apple.com/jp/app/chrome-%E3%83%AA%E3%83%A2%E3%83%BC%E3%83%88-%E3%83%87%E3%82%B9%E3%82%AF%E3%83%88%E3%83%83%E3%83%97/id944025852)をPC/iOSの両方に入れ，同じアカウントで両方にログインする．PC画面を共有することで，iOS側からPC画面を直接操作する．

#### EyeHackerPlayer (Unity)

![Player.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/Player.png)

* Calibration Sceneから起動する．
* Pupil Serviceを立ち上げる．
* TouchDesignerの`Calibration`スイッチを押すと，瞳孔キャリブレーション開始．
  * 正常にキャリブレーションが終わると，自動でMain Sceneに移り映像が提示される．
  * キャリブレーションの精度が悪いとやり直しになる．もう一度`Calibration`スイッチを押す．
* Main Scene移行後に`Calibration`スイッチを押すと，瞳孔キャリブレーション開始前の場面に戻る．さらに`Calibration`スイッチを押すと，再度瞳孔キャリブレーション開始．
