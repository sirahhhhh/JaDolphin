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

    public float MaxDamageTime;         // ダメージ時間の最大時間
    public float MaxAttackIntervalTime; // 攻撃間隔の最大時間
    public float MaxActIntervalTime;    // 行動間隔の最大時間
    public int MaxHP;

    float DamageTime;           // ダメージ時間
    float attackIntervalTime;   // 攻撃間隔の時間
    float actIntervalTime;      // 行動間隔の時間
    int HitPoint;
    bool isDead;
    bool isDamage;      // ダメージ時間中か
    bool isAttack;
    bool isLeft;        // 左向いてるか
    bool isActed;       // 行動時間中か

    Animator anime;

	// Use this for initialization
	void Start () {
        // コピー用銛をDeactiveに
        boatSpear.SetActive(false);

        DamageTime  = MaxDamageTime;
        HitPoint    = MaxHP;
        attackIntervalTime = MaxAttackIntervalTime;
        actIntervalTime = MaxActIntervalTime;

        isDead      = false;
        isDamage    = false;
        isAttack    = false;
        isLeft      = false;
        isActed     = false;

        anime = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        // 攻撃中なら攻撃間隔時間を減らして0以下になれば
        // 次の攻撃が出来る
        if (isAttack)
        {
            attackIntervalTime -= Time.deltaTime;
            if (attackIntervalTime <= 0.0f)
            {
                attackIntervalTime = MaxAttackIntervalTime;
                isAttack = false;
            }
        }

        // 行動後時間中待機処理
        if (isActed)
        {
            actIntervalTime -= Time.deltaTime;
            if(actIntervalTime <= 0.0f)
            {
                actIntervalTime = MaxActIntervalTime;
                isActed = false;
            }
        }
        else
        {
            // 行動後時間中でなければ行動
            Act();
        }
		// 船を移動させる
		Move ();

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
        }
        else
        {
            // ダメージ時間中でなければ攻撃する
            SpearAttack();
        }
    }

    // ボートの槍攻撃
    void SpearAttack()
    {
        // 攻撃中なら中断
        if (isAttack) return;

        // 右向きの場合、コピー元の銛と同じの座標に生成
        float createPosX = boatSpear.gameObject.transform.position.x;
        float createPosY = boatSpear.gameObject.transform.position.y;

        // 左向きの場合、コピー元の銛から少し左にずらす
        float adjustPosX = -1.0f;
        if (isLeft) { createPosX += adjustPosX; }

        GameObject createObj = (GameObject)Instantiate(
            boatSpear,
            new Vector3(createPosX,
                        createPosY,
                        0.0f),
            Quaternion.identity);
        createObj.transform.parent = this.gameObject.transform; // 生成した銛の親を生成元ボートに設定

        boatSpearScript = createObj.GetComponent<BoatSpear>();
		boatSpearScript.ShotSpear(isLeft);
		isAttack = true;
    }

	// 船を進行方向へ移動させる
	void Move()
	{
		// 移動距離の設定
		float moveX = 0.01f;
		if (isLeft)	{moveX = - moveX;}

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

        // 0～3のどれかをランダムに引く
        int ran = (int)(Random.value * 10 % (int)eBOAT_ACT.MAX_ACT);
        switch(ran)
        {
            // 何もしない、待機
            case (int)eBOAT_ACT.IDLE:
                break;

			// 向きを変える
			case (int)eBOAT_ACT.CHANGE_DIRECTION:
				isLeft = !isLeft;
				anime.SetBool("IsLeft", isLeft);
				break;

            //// 銛を撃つ
            //case (int)eBOAT_ACT.SHOT_SPEAR:
            //    // ダメージ時間中でなければ攻撃する
            //    SpearAttack();
            //    break;

        }
        isActed = true;
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
}
