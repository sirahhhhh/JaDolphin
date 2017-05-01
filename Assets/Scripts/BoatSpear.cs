using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ボートの攻撃(銛)クラス
public class BoatSpear : MonoBehaviour {

    public float MaxSpearSpeed;     // 銛の最大速度
    public float MaxSpearAliveTime; // 銛の存在する最大時間
    public int AttackPower;         // 銛攻撃力

    bool isAlive;                   // 銛存在フラグ
    float SpearSpeed;               // 銛速度
    float SpearAliveTime;           // 銛存在時間

    // Use this for initialization
    void Start () {
        // 銛の速度は現状、等加速度
        SpearSpeed = MaxSpearSpeed;
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
            this.gameObject.SetActive(false);
        }

        transform.position = new Vector3(posX, posY, 0.0f);
    }

    // 銛発射
    public void ShotSpear()
    {
        isAlive = true;
        SpearAliveTime = MaxSpearAliveTime;
        this.gameObject.SetActive(true);
    }

    // 攻撃(銛)の攻撃力取得
    public int GetAttackPower()
    {
        return AttackPower;
    }
}
