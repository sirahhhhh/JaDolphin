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

	private BoatManager	boatManager; // 漁船関係をまとめるクラス

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
		boatManager = new BoatManager ();
		boatManager.Init (
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
		boatManager.Run ();

		// スコア表示
 		ScoreLabel.text = "しずめた数 : " + boatManager.GetDownBoat();

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
	public void DownBoat(GameObject obj,float posX, float posY)
    {
		// 沈めた数を加算
		boatManager.AddDownBoat ();
		// 爆発エフェクト
		CreateExplosionEffect(posX,posY);
		// アイテムドロップ
		DropItem(posX, posY);
		// ボート破棄
		Destroy(obj);
    }

    // リトライボタン押下時
    public void PushRetry()
    {
        Time.timeScale = 1.0f;
        IsGameOver = false;
        HasPushRetry = true;
		SceneManager.LoadScene("DolphinJapan");	
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
        //ExplosionEffect expEffScript = createObj.GetComponent<ExplosionEffect>();

    }

}