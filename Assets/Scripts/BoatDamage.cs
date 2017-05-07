using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatDamage : MonoBehaviour {
	public GameObject itemHeart;    // ハートアイテムObj
	public GameObject explosion;    // 爆発エフェクトObj

	bool isDead = false;
	bool isDamage = false;      // ダメージ時間中か
	float DamageTime;           // ダメージ時間
	public float MaxDamageTime;         // ダメージ時間の最大時間
	public int MaxHP;
	int HitPoint;

	float itemDropRatio = 1 / 5.0f;	// アイテムドロップ率設定

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

		// 破壊された時の処理
		if(HitPoint <= 0)
		{
			isDead = true;
			// 爆発エフェクト
			CreateExplosionEffect(explosion, this.transform.position.x, this.transform.position.y);
			// アイテムドロップ
			DropItem (itemHeart, this.transform.position.x, this.transform.position.y, this.GetComponent<BoatController>().boatType);
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

	// アイテムドロップ
	private void DropItem(GameObject itemHeart,float dropPosX, float dropPosY, BoatController.eBOAT_TYPE boatType)
	{
		// 漁船の種類が大でなければアイテムドロップ抽選
		if (boatType != BoatController.eBOAT_TYPE.BIGGER)
		{
			// 0.0～1.0からの値をランダムで取得して
			// 指定割合以下ならアイテムドロップする
			if (Random.value > itemDropRatio) return;
		}

		int itemDropNums = 1;   // アイテムドロップの数
		float adjustPosX = 0.5f;

		// 漁船(大)ならアイテムを複数ドロップ
		if (boatType == BoatController.eBOAT_TYPE.BIGGER) itemDropNums = 3;
		for (int i = 0; i < itemDropNums; i++)
		{
			// ボートが居た場所にドロップする
			GameObject itemObj =
				(GameObject)Instantiate(
					itemHeart,
					new Vector3(dropPosX + (adjustPosX * i), dropPosY, 0.0f),
					Quaternion.identity);
			itemObj.SetActive(true);
		}
	}

	// 爆発エフェクト生成
	private void CreateExplosionEffect(GameObject explosion, float posX, float posY)
	{
		float adjustY = 0.1f;   // 少し下側に表示する

		GameObject createObj =
			(GameObject)Instantiate(
				explosion,
				new Vector3(posX, posY - adjustY, 0.0f),
				Quaternion.identity);

		createObj.SetActive(true);  // 有効に
	}
}
