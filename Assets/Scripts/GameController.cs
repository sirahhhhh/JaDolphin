using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    const float CREATE_TIME = 1.0f;
    float passTime;
    int DownBoats;

    public float createMinX, createMaxX;
    public float createMinY, createMaxY;
    public GameObject japBoat;

    public Text ScoreLabel;
    public Text GameOverLabel;
    public Button RetryButton;

    DolphinController dolphinCtrl;
    bool IsGameOver;
    bool HasPushRetry;

    // Use this for initialization
    void Start () {
        passTime = Time.time;
        japBoat.SetActive(false);

        GameObject dolpObj = GameObject.FindWithTag("Player");
        dolphinCtrl = dolpObj.GetComponent<DolphinController>();

        IsGameOver = false;
        HasPushRetry = false;
    }

    // Update is called once per frame
    void Update () {
        //if (IsGameOver) return;

        // ボート生成
        CreateBoat();

        // スコア表示
        ScoreLabel.text = "しずめた数 : " + DownBoats;

        if (!dolphinCtrl.GetIsAlive())
        {
            if (!IsGameOver)
            {
                GameOver();
            }
        }
	}

    void CreateBoat()
    {
        float nowTime = Time.time;
        if ((Time.time - passTime) < CREATE_TIME) return;
        passTime = nowTime;

        float createX = Random.Range(createMinX, createMaxX);
        float createY = Random.Range(createMinY, createMaxY);

        GameObject Obj = (GameObject)Instantiate(
            japBoat,
            new Vector3(createX, createY, 0.0f),
            Quaternion.identity);
        Obj.SetActive(true);
    }

    // ゲームオーバー処理
    void GameOver()
    {
        if (HasPushRetry) return;

        Time.timeScale = 0.0f;
        IsGameOver = true;
        GameOverLabel.gameObject.SetActive(true);
        RetryButton.gameObject.SetActive(true);
    }

    public void DownBoat()
    {
        DownBoats++;
        Debug.Log("downs : " + DownBoats);
    }

    public void PushRetry()
    {
        Time.timeScale = 1.0f;
        IsGameOver = false;
        HasPushRetry = true;
        Application.LoadLevel("DolphinJapan");
    }
}
