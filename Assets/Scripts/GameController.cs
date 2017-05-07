using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    // ボート生成時の最大、最小座標
    public float createMinX, createMaxX;
    public float createMinY, createMaxY;
    public GameObject[] japBoats;   // 漁船のコピー元Obj

	private BoatManager	boatManager; // 漁船関係をまとめるクラス

    public Text ScoreLabel;         // スコア
	public Text GameOverLabel;      // ゲームオーバーテキスト
    public Button RetryButton;      // リトライボタン
    public HPPanel hpPanelScript;   // HP表示スクリプト

    DolphinController dolphinCtrl;

    bool IsGameOver;
    bool HasPushRetry;

    // Use this for initialization
    void Start () {
		// 漁船関係をまとめたオブジェクトを作成
		GameObject obj = new GameObject("BoatManager");
		BoatManager bmAddComponet = obj.AddComponent<BoatManager> ();
		boatManager = bmAddComponet;
		boatManager.Init (
			createMinX,
			createMaxX,
			createMinY,
			createMaxY,
			japBoats
		);

        // 漁船のコピー元Objをdeactive
        for (int i = 0; i < japBoats.Length; i++)
        {
            japBoats[i].SetActive(false);
        }

		// ゲームオーバー用にイルカの生存フラグをみる
        GameObject dolpObj = GameObject.FindWithTag("Player");
        dolphinCtrl = dolpObj.GetComponent<DolphinController>();

        IsGameOver = false;
        HasPushRetry = false;

    }

    // Update is called once per frame
    void Update () {
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
	public void DownBoat()
    {
		// 沈めた数を加算
		boatManager.BoatDown ();

    }

    // リトライボタン押下時
    public void PushRetry()
    {
        Time.timeScale = 1.0f;
        IsGameOver = false;
        HasPushRetry = true;
		SceneManager.LoadScene("DolphinJapan");	
    }
}
