using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {
    // ボートの種類
    public enum eBOAT_TYPE
    {
        NORMAL,         // 通常の漁船
        YELLOW,         // 黄色漁船
        BIGGER,         // 漁船(大)
        BOAT_TYPE_MAX,
    }

    enum eBOAT_ACT
    {
        IDLE,                      // なにもしない
//        SHOT_SPEAR,                // 銛を撃つ
        MAX_ACT,
    }

	//private GeneralFunc generalFunc;
	private MoveX moveX;
	private bool isLeft;

    // 銛発射のためのObj
    public GameObject boatSpear;
    BoatSpear boatSpearScript;

	// ダメージ処理用
	private BoatDamage boatDamage;

	// 一斉攻撃のタイミング取得用
	// BoatManagerはGameController経由で取得
	GameObject gameControllerObj;
	GameController gameCtrl;
	private GameObject boatManagerObj;
	private BoatManager boatManager;

	private SpearManager spearManager;	// 銛関係のマネージャ

    public float StartActTime;          // 行動を開始する時間
    public float MaxAttackIntervalTime; // 攻撃間隔の最大時間
    public float MaxActIntervalTime;    // 行動間隔の最大時間
    public eBOAT_TYPE boatType; // 漁船の種類

	float attackIntervalTime;   // 攻撃間隔の時間
	float actIntervalTime;      // 行動間隔の時間
	bool isAttack = false;
    bool isMovingUp = false;    // 上行くか
	bool isActed = false;       // 行動時間中か
	bool isStartAct = false;    // 行動開始するか

	// Use this for initialization
	void Start () {
		// 他のクラスでも使う関数を切り出した
		//generalFunc = new GeneralFunc ();

		// 左右の向きを親クラスから取得
		moveX = this.GetComponentInParent<MoveX>();
		isLeft = moveX.GetIsLeft ();

        // コピー用銛をDeactiveに
        boatSpear.SetActive(false);

		// 一斉攻撃のタイミング取得用
		// BoatManagerはGameController経由で取得
		gameControllerObj = GameObject.FindWithTag("GameController");
		gameCtrl = gameControllerObj.GetComponent<GameController>();
		boatManager = gameCtrl.GetBoatManager ();


		// 設定値取得等の初期設定
		InitSetting();

		// ダメージの確認用
		boatDamage = this.GetComponent<BoatDamage> ();

		// 銛関係のマネージャ
		GameObject obj = new GameObject("SpearManger");
		SpearManager smAddComponet = obj.AddComponent<SpearManager> ();
		spearManager = smAddComponet;
    }
	
	// Update is called once per frame
	void Update () {
        // 行動開始するまでの待機処理
		StandBy();

		isLeft = moveX.GetIsLeft ();

        // 攻撃中なら攻撃間隔時間を減らして0以下になれば
        // 次の攻撃が出来る
        if (isAttack) AttackStandByTime();

        // 行動開始しているなら
        if (isStartAct)
        {
            // 行動後時間中待機処理
            if (isActed) AttackStandByTime();
            else Act();	// 行動後時間中でなければ行動
        }

        // ダメージ時間処理
        bool IsDamage = boatDamage.Damage();

        // 一斉攻撃待機中か確認
        bool AllOutAttackStandBy = boatManager.AllOutAttackStandBy();
		if (AllOutAttackStandBy) return; // 一斉攻撃待機中は普通の攻撃はしないので抜ける

        // ダメージ中の処理
		// ダメージ時間中でなければ攻撃する
		if(!IsDamage)
        {
			isAttack = spearManager.Fire (
				false,
				isAttack,
				isLeft,
				this.gameObject.transform,
				boatSpear.gameObject.transform.position.x,
				boatSpear.gameObject.transform.position.y,
				boatSpear,
                boatType
            );
		}

		// 削除された銛をListから削除
		spearManager.DeleteSpears();

	}

	// 設定値取得等の初期設定
	void InitSetting()
	{
		attackIntervalTime = MaxAttackIntervalTime;
		actIntervalTime = MaxActIntervalTime;
	}

    // ボート行動
    void Act()
    {
        // ダメージ時間中でも行動するか要検討

        // 行動番号をランダムに引く
        int ran = Random.Range(0, (int)eBOAT_ACT.MAX_ACT);
        switch(ran)
        {
            // 何もしない、待機
            case (int)eBOAT_ACT.IDLE:
                break;

            //// 銛を撃つ
            //case (int)eBOAT_ACT.SHOT_SPEAR:
            //    // ダメージ時間中でなければ攻撃する
            //    SpearAttack();
            //    break;

        }
        isActed = true;
    }

	// スタンバイ
	void StandBy()
	{
		// 行動開始するまでの待機処理
		if (!isStartAct)
		{
			StartActTime -= Time.deltaTime;
			if (StartActTime <= 0.0f)
			{
				// 行動開始
				isStartAct = true;
			}
		}
	}

	// 攻撃スタンバイ時間の処理
	void AttackStandByTime()
	{
		// 攻撃中なら攻撃間隔時間を減らして0以下になれば
		// 次の攻撃が出来る
		attackIntervalTime -= Time.deltaTime;
		if (attackIntervalTime <= 0.0f)
		{
			attackIntervalTime = MaxAttackIntervalTime;
			isAttack = false;
		}
	}

	// 一斉攻撃
	public void AllOutAttack()
	{
		bool gomiFlag;
		for(int i=0; i<2; i++){
			gomiFlag = spearManager.Fire (
				true,
				false,
				isLeft,
				this.gameObject.transform,
				boatSpear.gameObject.transform.position.x,
				boatSpear.gameObject.transform.position.y,
				boatSpear,
				boatType
			);
			gomiFlag = spearManager.Fire (
				true,
				false,
				!isLeft,
				this.gameObject.transform,
				boatSpear.gameObject.transform.position.x,
				boatSpear.gameObject.transform.position.y,
				boatSpear,
				boatType
		);
		}
	}
}