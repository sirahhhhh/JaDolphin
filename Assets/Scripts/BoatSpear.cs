using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ボートの攻撃(銛)クラス
public class BoatSpear : MonoBehaviour {

    public float MaxSpearSpeed;     // 銛の最大速度
    public float MaxSpearAliveTime; // 銛の存在する最大時間
    public int AttackPower;         // 銛攻撃力

    Animator anime; // 銛の左右切り替えアニメ
    float SpearSpeed;               // 銛速度
    float SpearAliveTime;           // 銛存在時間
    bool isAlive;                   // 銛存在フラグ
    bool isLeft;                    // 左向きか

	private MoveX moveX;


    // Use this for initialization
    void Start () {
        // 銛の速度と進む向きを決定
		SpearSpeed = MaxSpearSpeed + Random.Range(0.0f, 0.03f);
		if(isLeft) SpearSpeed = - SpearSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isAlive) return;
        // 銛移動処理
		transform.position = new Vector3(
			transform.position.x + SpearSpeed,
			transform.position.y,
			0.0f);

		// 銛の生存時間を減算
        SpearAliveTime -= Time.deltaTime;
		// 生存時間が終わったら銛を削除
		if (SpearAliveTime <= 0.0f) DeleteSpear ();

    }

    // 銛発射
    // @param a_isLeft  左向きか
	public void ShotSpear()
	{
        this.gameObject.SetActive(true);
        isAlive = true;

		// 左右の向きを親クラスから取得
		moveX = this.GetComponentInParent<MoveX>();
		isLeft = moveX.GetIsLeft ();

        // AwakeやStartより早くここを通るので
        // ここで設定しておく
        anime = GetComponent<Animator>();
		anime.SetBool("IsLeft", isLeft);

        SpearAliveTime = MaxSpearAliveTime;
    }

	// 銛の削除
	void DeleteSpear()
	{
		isAlive = false;
		SpearAliveTime = 0.0f;
		//this.gameObject.SetActive(false);
		Destroy(this.gameObject);
	}

    // 攻撃(銛)の攻撃力取得
    public int GetAttackPower()
    {
        return AttackPower;
    }

	public bool GetAlive()
	{
		return isAlive;
	}

	// 他のクラスからの
	public bool GetIsLeft()
	{
		return isLeft;
	}
}
