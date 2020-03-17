# EyeHacker引継ぎ

## ネットワーク

![network](network.png)

* カメラ入力
  * `Insta360Air`オペレータ側がInsta360Airを使うときのオペレータ群，`THETA`オペレータ側がTHETAを使うときのオペレータ群になっている．繋いだカメラに応じて適宜バイパス・`switch1`オペレータの`Index`変更をする．
    * Insta360Airオペレータ側では，`toEquirectangular`オペレータによる画像形式変換が入る．
  * `transform2`オペレータで，提示映像の正面を合わせておくと後が楽．
* 録画・再生
  * `replicator1`オペレータで`item[1-5]`オペレータが複製されている．
  * `item[1-5]`オペレータ中，`videoMaster`オペレータは録画を，`playerMaster`は動画再生を担う．
    * `videoMaster`オペレータは`moviefileout1`オペレータによって録画するとともに，録画中であることを示すフラグを`playerMaster`オペレータに送る．
    * `playerMaster`オペレータは`moviefilein1`オペレータによって動画を再生する．
      * `videoPlayer`オペレータから送られてきたフラグを元に，録画終了の一定時間後に自動reload処理を入れる．
      * `switch1`オペレータの`Index`を0ではなく1にすると，dual-fisheye形式動画をequirectangular形式動画に変換する処理が挟まる．dual-fisheye形式動画を録画データとして持っているときに操作する．
  * `item0`オペレータは，`itemMaster`オペレータから録画・動画の再生機能を抜いて，リアルタイム映像を流したもの．
* レイヤー映像
  * 
* 音周り
* 視線位置取得
* マスク画像
* 映像混合・再生
* risk計算
* 頭部速度取得
* risk/threshold比較
* 自動混合
* ユーティリティ

## 積み残したこと

* マスク画像の自動生成
  * `blendTextures`オペレータに入力する`maskTexture`を変えることで出来そう
* risk計算の改善
  * `calculateRisk`オペレータの内部を変えることで出来そう
* 音周りの改善
  * 音周りに関連したオペレータ群をいじることになりそう
  * ちなみにTouchDesigner 2020には`Audio File Out CHOP`が[追加された](https://derivative.ca/community-post/2020-official-update)ので，これも使えると良いかも
* 映像を切り替えた瞬間からフレーム指定して再生開始
  * 録画・再生に関連したオペレータ群をいじることになりそう
* 実験の組み立て
  * `OldProjects/ExperimentController/itoMasterThesis`内のプロジェクトが，ログの取り方等の参考になる

## 参考文献

* [Visual Thinking with TouchDesigner
プロが選ぶリアルタイムレンダリング＆プロトタイピングの極意](http://www.bnn.co.jp/books/8842/)
  * これをなんとなくでも良いから理解しないと何も始まらない．最初から最後まで読むべき．
  * 特に2章全て・3-2節・3-3節・3-7節・3-9節・3-11節・3-14節・3-15節・AppendixのPython周りの節の話を多用している．
* [TouchDesignerのオペレータをPythonで繋ぐ](https://qiita.com/radi_bow/items/24f7384d9bfaafdd5d3c)
  * 映像の録画・再生やレイヤー映像指定のところでこのテクニックを使っている．
* [[TouchDesigner] UnityのMonoBehaviour的なノードを見つけたという話](https://qiita.com/kodai100/items/9b1b4be6f07c2fad1657)
  * risk計算でこのテクニックを使っている．
* [TouchDesigner 複数カメラをボタンでスイッチング](https://qiita.com/atsonic/items/8aeb32c4933b9f05673b)
  * 内容自体はあまり関係ないが，ラジオボタン周りやReplicator COMPの知見が得られる．
* [TouchDesigner 処理が重くなっている個所を見つける方法](https://qiita.com/narumin256/items/cf18280156ed12101943)
  * Palette内のTools->probeを使って調べようねというお話．
* [TouchDesignerでのVR開発で頭部に追従するカーソルを実装する](https://qiita.com/radi_bow/items/02c722592b987ddc1752)
  * 頭部方向・視線位置を適切に取得するところでこのテクニックを使っている．
* ショートカットキーリスト
![shortcuts_01](shortcuts_01.jpg)
![shortcuts_02](shortcuts_02.jpg)
