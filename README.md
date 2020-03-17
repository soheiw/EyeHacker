# SR-Project/EyeHacker

## What is this?

2つの全天球映像，例えばリアルタイム映像と過去映像を，体験者に気付かれにくいタイミングを見計らって自動的に切り替えるシステム．

全天球映像のどこを見ているのかという視線情報を取得し，その周辺における映像の画像処理情報をもとに，映像を切り替えるか否かを決定する．

![overview.png](https://github.com/inamilab/EyeHacker/blob/develop/images/overview.png)

![blend.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/blend.png)

## How To Use

### 動作環境

* Windows10 64bit
* Unity 2019.1.14f1
* TouchDesigner 099 Educational 2019.18360
* HTC VIVE Pro Eye
* Insta360Air または THETA Z1
* ZOOM H1n

### Unity使用アセット

* [uOSC](https://github.com/hecomi/uOSC)
* [Eye Tracking SDK (SRanipal)](https://developer.vive.com/resources/knowledgebase/vive-sranipal-sdk/)

### 使用方法

* TouchDesignerでEyeHackerを，UnityでGazeDirectionSenderを起動．

#### EyeHacker (TouchDesigner)

* カメラとマイクをPCに繋いで，マイクの電源を入れる．
  * TouchDesignerのperform画面の`Realtime`に映像が映らない/映像が変な場合は，`cameraImageIn`オペレータ内の`Video Device In`TOPの`Device`が正しいかを確認する．

![EyeHacker.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/EyeHacker.png)

* 映像セット
  * `FileName`フィールドでファイル名の変更．
  * `Rec`ボタンで録画開始・終了．
  * `Loop`トグルでループするかを指定．
  * `Reload`ボタンで映像をリロード．
  * `Begin`，`End`スライダで映像のトリミング．
  * `Yaw`スライダで映像の回転を調整するOSC信号をUnityに送信．
  * `Black`，`Gamma`フィールドで映像の明るさ調整．
* Area[1-2]
  * `Play`ボタンで今選択している映像の再生・停止．
  * セット番号ボタンで再生する映像の選択．
* Blend Ratio
  * `Ratio goes to 0/1 now`トグルで`Ratio`を0/1まで自動で動かす．
  * `Ratio`スライダでアルファブレンドの比率を直接操作可能．
* Blend Shape
  * `whole`/`circle`/`rectangle`ボタンでmaskの形状を指定．
    * モードに応じてMaskに関するパラメータが白枠の中に出てくる．適宜調整．
* Blend Time
  * `MaxBlendTime`スライダで映像切り替えにかける時間を調整．
    * `BlendTimeAdjustment`トグルがONのときは，実際の切り替え時間は，HMDの回転速度を反映した`Actual BlendTime`の値になる．
* Decision Algorithm
  * `InnerRisk D`/`OuterRisk D`スライダでrisk計算で使う領域の直径を指定．
  * `Threshold`スライダでthreshold調整．
  * `Judge`トグルでriskとthresholdのどちらが大きいときに映像を切り替えるか指定．
* Decision Time
  * `WaitingTime`スライダで待ち時間を調整．
  * `RewindSpeed`スライダで巻き戻りの速度を調整．
  * `Activate Auto Switch`トグルを押すと`Time`のスライダが動きだし，スライダが振り切れると映像が切り替わる．
* Audio
  * `RecNoise`ボタンで環境音を録音開始・終了．
  * `NoiseVal`スライダで環境音の音量調整．
* Utilities
  * `View Ray`トグルで体験者へのrayの表示/非表示を切り替え．
  * `RayCast`トグルを`Gaze`にするとPupilLabsのデータが視線位置に，`HMD`にするとHMDの向いている方向が視線位置になる．

#### GazeDirectionSender (Unity)

![GazeDirectionSender.png](https://github.com/inamilab/SRProject-EyeTracking/blob/develop/images/GazeDirectionSender.png)

（適宜ビルドしてから）起動する．OSCで視線位置情報を送信し続ける．
  
### 実装の詳細

[specification.md](specification/specification.md)に記載．
