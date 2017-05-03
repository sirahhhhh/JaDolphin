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

    // Use this for initialization
    void Start () {
        // 銛の速度は現状、等加速度
		SpearSpeed = MaxSpearSpeed + Random.Range(0.0f, 0.03f);
		if(isLeft) SpearSpeed = - SpearSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isAlive) return;

        // 銛移動処理
        float posX = transform.position.x;
        float posY = transform.position.y;

		posX += SpearSpeed;

        SpearAliveTime -= Time.deltaTime;
        if(SpearAliveTime <= 0.0f)
        {
            // 銛削除
            isAlive = false;
            SpearAliveTime = 0.0f;
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

        transform.position = new Vector3(posX, posY, 0.0f);
    }

    // 銛発射
    // @param a_isLeft  左向きか
	public void ShotSpear(bool a_isLeft)
	{
        this.gameObject.SetActive(true);
        isAlive = true;
        isLeft = a_isLeft;

        // AwakeやStartより早くここを通るので
        // ここで設定しておく
        anime = GetComponent<Animator>();
		anime.SetBool("IsLeft", isLeft);

        SpearAliveTime = MaxSpearAliveTime;
    }

    // 攻撃(銛)の攻撃力取得
    public int GetAttackPower()
    {
        return AttackPower;
    }

}
