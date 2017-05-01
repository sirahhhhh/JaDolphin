using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

    // 銛発射のためのObj
    public GameObject boatSpear;
    BoatSpear boatSpearScript;

    public float MaxDamageTime;
    public int MaxHP;

    float DamageTime;
    int HitPoint;
    bool isDead;
    bool isDamage;  // ダメージ時間中か
    bool isAttack;
    bool isLeft;    // 左向いてるか

    Animator anime;

	// Use this for initialization
	void Start () {
        DamageTime  = MaxDamageTime;
        HitPoint    = MaxHP;
        isDead      = false;
        isDamage    = false;
        isAttack    = false;
        isLeft      = false;

        anime = GetComponent<Animator>();
        boatSpearScript = boatSpear.GetComponent<BoatSpear>();
    }
	
	// Update is called once per frame
	void Update () {
        // ダメージ時間減衰
        // ダメージ受けて一定時間は次のダメージを受けない
        if (isDamage)
        {
            float DamageDecTime = 0.1f;
            DamageTime -= DamageDecTime;
            if (DamageTime < 0.0f)
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

        boatSpearScript.ShotSpear();
        isAttack = true;
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
}
