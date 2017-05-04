using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

    enum eBOAT_ACT
    {
        IDLE,                      // なにもしない
		CHANGE_DIRECTION,			// 向きを変える
//        CHANGE_DIRECTION_LEFT,     // 左を向く
//        CHANGE_DIRECTION_RIGHT,    // 右を向く
//        SHOT_SPEAR,                // 銛を撃つ
        MAX_ACT,
    }

    // 銛発射のためのObj
    public GameObject boatSpear;
    BoatSpear boatSpearScript;

    public float StartActTime;          // 行動を開始する時間
    public float MaxDamageTime;         // ダメージ時間の最大時間
    public float MaxAttackIntervalTime; // 攻撃間隔の最大時間
    public float MaxActIntervalTime;    // 行動間隔の最大時間
    public int MaxHP;

	float DamageTime;           // ダメージ時間
	float attackIntervalTime;   // 攻撃間隔の時間
	float actIntervalTime;      // 行動間隔の時間
	int HitPoint;
	bool isDead = false;
	bool isDamage = false;      // ダメージ時間中か
	bool isAttack = false;
	bool isLeft = false;        // 左向いてるか
	bool isActed = false;       // 行動時間中か
	bool isStartAct = false;    // 行動開始するか

    Animator anime;

	// Use this for initialization
	void Start () {
        // コピー用銛をDeactiveに
        boatSpear.SetActive(false);

		// 設定値取得等の初期設定
		InitSetting();

		// ボートのアニメーション開始
        anime = GetComponent<Animator>();

		// ボートの向きをランダムに決める
		isLeft = RandomBool();
		anime.SetBool("IsLeft", isLeft);
    }
	
	// Update is called once per frame
	void Update () {
        // 行動開始するまでの待機処理
		StandBy();

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
		// 船を移動させる
		Move ();

        // ダメージ中の処理
		// ダメージ時間中でなければ攻撃する
		if(!Damage()) SpearAttack();
	}

	// 設定値取得等の初期設定
	void InitSetting()
	{
		DamageTime  = MaxDamageTime;
		HitPoint    = MaxHP;
		attackIntervalTime = MaxAttackIntervalTime;
		actIntervalTime = MaxActIntervalTime;
	}

    // ボートの槍攻撃
    void SpearAttack()
    {
        // 攻撃中なら中断
        if (isAttack) return;

		// 左向きの場合、コピー元の銛から少し左にずらす
		float adjustPosX = 0.0f;
		if (isLeft) adjustPosX = -1.0f;

        // 銛の生成
		GameObject createObj = (GameObject)Instantiate(
            boatSpear,
				new Vector3(
					boatSpear.gameObject.transform.position.x + adjustPosX,
					boatSpear.gameObject.transform.position.y,
                    0.0f),
            Quaternion.identity);
        createObj.transform.parent = this.gameObject.transform; // 生成した銛の親を生成元ボートに設定

        boatSpearScript = createObj.GetComponent<BoatSpear>();
		boatSpearScript.ShotSpear(isLeft);
		isAttack = true;
    }

	// ダメージ中の処理
	bool Damage()
	{
		// ダメージ時間減衰
		// ダメージ受けて一定時間は次のダメージを受けない
		if (isDamage)
		{
			DamageTime -= Time.deltaTime;
			if (DamageTime <= 0.0f)
			{
				DamageTime = 0.0f;
				isDamage = false;
				anime.SetBool("IsDamage", isDamage);    // ダメージ中アニメに切り替え
			}
			return true;
		}
		return false;
	}

	// 船を進行方向へ移動させる
	void Move()
	{
		// 画面外に出そうなら反転
		if (transform.position.x >= 4.0f || transform.position.x <= -4.0f) Reverse();

		// 移動距離の設定
		float moveX = 0.01f;
		if (isLeft)	moveX = - moveX;

		// 現在地から移動距離分だけ進める
		transform.position = new Vector3(
			transform.position.x + moveX,
			transform.position.y,
			transform.position.z
		);
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

			// 向きを変える
			case (int)eBOAT_ACT.CHANGE_DIRECTION:
				Reverse ();
				break;

            //// 銛を撃つ
            //case (int)eBOAT_ACT.SHOT_SPEAR:
            //    // ダメージ時間中でなければ攻撃する
            //    SpearAttack();
            //    break;

        }
        isActed = true;
    }

	// ボートの反転
	void Reverse()
	{
		isLeft = !isLeft;
		anime.SetBool("IsLeft", isLeft);
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

    // ボートダメージ処理
    // @return true     ダメージ処理できた
    //         false    ダメージ時間中
    public bool DamageBoat( int AttackPower )
    {
        // ダメージ時間中なら処理しない
        if (isDamage) return false;

        // ダメージ時間
        isDamage = true;
        DamageTime = MaxDamageTime;
        anime.SetBool("IsDamage", isDamage);

        // ダメージ処理
        HitPoint -= AttackPower;
        if(HitPoint <= 0)
        {
            isDead = true;
        }

        return true;
    }

    // 死亡フラグ取得
    public bool IsDead()
    {
        return isDead;
    }

    public float GetPosX()
    {
        return transform.position.x;
    }

    public float GetPosY()
    {
        return transform.position.y;
    }

	// bool値の乱数を返す関数
	private static bool RandomBool()
	{
		return Random.Range(0, 2) == 0;
	}
	public static int getID()
	{
		return 9;
	}
}