using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    // ボート生成時の最大、最小座標
    public float createMinX, createMaxX;
    public float createMinY, createMaxY;
    public GameObject japBoat;      // ボートObj
    public GameObject itemHeart;    // ハートアイテムObj
    public GameObject explosion;    // 爆発エフェクトObj

	private FishingBoat fishingBoat; // 漁船関係をまとめるクラス

    public Text ScoreLabel;         // スコア
	public Text GameOverLabel;      // ゲームオーバーテキスト
    public Button RetryButton;      // リトライボタン
    public HPPanel hpPanelScript;   // HP表示スクリプト

    DolphinController dolphinCtrl;

    float itemDropRatio;            // アイテムドロップ率
    bool IsGameOver;
    bool HasPushRetry;

    // Use this for initialization
    void Start () {
		// 漁船関係をまとめたオブジェクトを作成
		fishingBoat = new FishingBoat ();
		fishingBoat.Start (
			createMinX,
			createMaxX,
			createMinY,
			createMaxY,
			japBoat
		);



       japBoat.SetActive(false);   // コピー元Objをdeactive

		// ゲームオーバー用にイルカの生存フラグをみる
        GameObject dolpObj = GameObject.FindWithTag("Player");
        dolphinCtrl = dolpObj.GetComponent<DolphinController>();

        itemDropRatio = 1 / 5.0f;   // アイテムドロップ率設定
        IsGameOver = false;
        HasPushRetry = false;

    }

    // Update is called once per frame
    void Update () {
		// 漁船関係の動作
		fishingBoat.Run ();

		// スコア表示
 		ScoreLabel.text = "しずめた数 : " + fishingBoat.GetDownBoat();

        // HP表示
        hpPanelScript.UpdateHPPanel(dolphinCtrl.GetHP());

        if (!dolphinCtrl.GetIsAlive())
        {
            if (!IsGameOver)
            {
                GameOver();
            }
        }
	}



    // ゲームオーバー処理
    void GameOver()
    {
        if (HasPushRetry) return;

        Time.timeScale = 0.0f;
        IsGameOver = true;
        GameOverLabel.gameObject.SetActive(true);
		GameOverLabel.text = "m9(^Д^)";
        RetryButton.gameObject.SetActive(true);
    }

    // ボートが沈んだら
    // 現状、沈めたボート数を加算する
    public void DownBoat()
    {
		fishingBoat.AddDownBoat ();
    }

    // リトライボタン押下時
    public void PushRetry()
    {
        Time.timeScale = 1.0f;
        IsGameOver = false;
        HasPushRetry = true;
        //Application.LoadLevel("DolphinJapan");	// 非推奨らしい
		SceneManager.LoadScene("DolphinJapan");		// 今後はこっちで
    }

    // アイテムドロップ
    public void DropItem(float dropPosX, float dropPosY)
    {
        // 0.0～1.0からの値をランダムで取得して
        // 指定割合以下ならアイテムドロップする
        if (Random.value > itemDropRatio) return;

        // ボートが居た場所にドロップする
        GameObject itemObj =
            (GameObject)Instantiate(
                itemHeart,
                new Vector3(dropPosX, dropPosY, 0.0f),
                Quaternion.identity);
        itemObj.SetActive(true);
    }

    // 爆発エフェクト生成
    public void CreateExplosionEffect(float posX, float posY)
    {
        float adjustY = 0.1f;   // 少し下側に表示する

        GameObject createObj =
            (GameObject)Instantiate(
                explosion,
                new Vector3(posX, posY - adjustY, 0.0f),
                Quaternion.identity);

        createObj.SetActive(true);  // 有効に
        ExplosionEffect expEffScript = createObj.GetComponent<ExplosionEffect>();

        // 爆発アニメ開始
        expEffScript.StartAnime();
    }

	// 漁船関係をまとめたクラス
	private class FishingBoat
	{
		private const float CREATE_TIME = 1.0f; // ボート生成時間
		private float passTime; // 生成時に使う経過時間
		private int DownBoats;  // 沈めたボート数

		// ボート生成時の最大、最小座標
		private float createMinX, createMaxX;
		private float createMinY, createMaxY;
		private GameObject japBoat;      // ボートObj

		private int maxJapBoats = 10;	// 漁船の最大数
		private List<GameObject> lists = new List<GameObject>();	// ボートobjを保存しておくリスト

		public void Start(float minX, float maxX, float minY, float maxY, GameObject boat)
		{
			createMinX = minX;
			createMaxX = maxX;
			createMinY = minY;
			createMaxY = maxY;
			japBoat = boat;

			passTime = Time.time;       // 経過時間に現在の時間を設定
		}

		// 削除されたボートをListから削除
		private void DeleteBoats()
		{
			for (int i = this.lists.Count -1; i >= 0; i--) {
				if (this.lists[i] == null) {
					this.lists.RemoveAt (i);
				}
			}

		}

		// ボート生成
		private void CreateBoat()
		{
			// 漁船数が最大値ならそれ以上作成しない
			if (this.lists.Count >= this.maxJapBoats) return;

			// 生成時間を過ぎたらつくる
			float nowTime = Time.time;
			if ((Time.time - passTime) < CREATE_TIME) return;
			passTime = nowTime;

			// 座標はランダムで決定
			float createX = Random.Range(createMinX, createMaxX);
			float createY = Random.Range(createMinY, createMaxY);

			GameObject Obj = (GameObject)Instantiate(
				japBoat,
				new Vector3(createX, createY, 0.0f),
				Quaternion.identity);
			Obj.SetActive(true);
			this.lists.Add (Obj);
		}

		public void Run()
		{
			// ボート生成
			this.CreateBoat();

			// 沈められたボートListの削除
			this.DeleteBoats ();
		}

		// 沈めたボートの数をカウント
		public void AddDownBoat()
		{
			DownBoats++;
			//Debug.Log("downs : " + DownBoats);
		}

		// 沈めたボートの数を返す
		public int GetDownBoat()
		{
			return DownBoats;

		}
	}
}