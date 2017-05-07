using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatDamage : MonoBehaviour {
	bool isDead = false;
	bool isDamage = false;      // ダメージ時間中か
	float DamageTime;           // ダメージ時間
	public float MaxDamageTime;         // ダメージ時間の最大時間
	public int MaxHP;
	int HitPoint;

	Animator anime;

	// Use this for initialization
	void Start () {
		DamageTime  = MaxDamageTime;
		HitPoint    = MaxHP;

		// ボートのアニメーション開始
		anime = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// ダメージ中の処理
	public bool Damage()
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
