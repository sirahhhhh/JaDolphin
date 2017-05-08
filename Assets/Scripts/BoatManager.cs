using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 漁船関係をまとめたクラス
public class BoatManager : MonoBehaviour {
    // 作られる漁船の割合(合計が100でなくてもよい)
    private int[] CreateBoatRatio = {
        60,     // 普通の漁船
        30,     // 黄漁船
        10      // 大漁船
    };
	private const float CREATE_TIME = 1.0f; // ボート生成時間
	private float passTime; // 生成時に使う経過時間
	private int DownBoats;  // 沈めたボート数

	// 一斉攻撃用変数
	private bool AllOutAttackEnable = false;	// 一斉攻撃用フラグ
	private float StanByTime = 3.0f;	// 一斉攻撃までの猶予時間
	private float StanByStartTime;      // 猶予が開始した時刻
    float allOutDelayTime = 0.0f;       // 再一斉攻撃待機時間
    float maxAllOutDelayTime = 10.0f;   // 再一斉攻撃待機最大時間
    bool isAllOutAttackDelay = false;   // 一斉攻撃待機中か

	// 外のクラスにまとめた関数集
	private GeneralFunc generalFunc;

	// ボート生成時の最大、最小座標
	private float createMinX, createMaxX;
	private float createMinY, createMaxY;
	private GameObject[] japBoats;  // ボートObj

	private int maxJapBoats = 10;	// 漁船の最大数
	private List<GameObject> lists = new List<GameObject>();	// ボートobjを保存しておくリスト


	public void Init(float minX, float maxX, float minY, float maxY, GameObject[] boats)
	{
		createMinX = minX;
		createMaxX = maxX;
		createMinY = minY;
		createMaxY = maxY;
		japBoats = boats;

		passTime = Time.time;       // 経過時間に現在の時間を設定
        allOutDelayTime = maxAllOutDelayTime;   // 一斉攻撃待機時間設定
    }

	// 削除されたボートをListから削除
	private void DeleteBoats()
	{
		for (int i = this.lists.Count -1; i >= 0; i--) {
			if (this.lists[i] == null) this.lists.RemoveAt (i);
		}
	}

	// ボート生成
	private void CreateBoat()
	{
		// 漁船数が最大値ならそれ以上作成しない
		if (this.lists.Count >= this.maxJapBoats) return;

		// 生成時間を過ぎたらつくる
		float nowTime = Time.time;
		if ((Time.time - passTime) < CREATE_TIME) return;
		passTime = nowTime;

		// 座標はランダムで決定
		float createX = Random.Range(createMinX, createMaxX);
		float createY = Random.Range(createMinY, createMaxY);

		GameObject Obj = (GameObject)Instantiate(
			japBoats[generalFunc.SelectInt(CreateBoatRatio)],
			new Vector3(createX, createY, 0.0f),
			Quaternion.identity
		);
		Obj.SetActive(true);
		this.lists.Add (Obj);
	}

	// ボートが沈められた時の処理
	public void BoatDown()
	{
		// 沈めたボートの数をカウント
		DownBoats++;
	}

	// 沈めたボートの数を返す
	public int GetDownBoat()
	{
		return DownBoats;
	}
		
	// Use this for initialization
	void Start () {
		generalFunc = new GeneralFunc ();
	}

	// Update is called once per frame
	void Update () {
		// 沈められたボートListの削除
		this.DeleteBoats ();

        // 一斉攻撃待機処理
        DelayAllOutAttack();

		// 一斉攻撃に移るかチェック
		CheckAllOutAttack ();
		AllOutAttack ();

		// ボート生成
		this.CreateBoat();
	}

    // 一斉攻撃待機処理
    void DelayAllOutAttack()
    {
        // 一斉攻撃待機中でなければ処理しない
        if (!isAllOutAttackDelay) return;

        allOutDelayTime -= Time.deltaTime;
        if(allOutDelayTime <= 0.0f)
        {
            isAllOutAttackDelay = false;    // 待機解除
            allOutDelayTime = maxAllOutDelayTime; // 再設定
        }

    }

	// 一斉攻撃の準備に入るかチェック
	private void CheckAllOutAttack()
	{
        // 一斉攻撃待機中か
        if (isAllOutAttackDelay) return;
		// 一斉攻撃フラグが立ってたら以降の処理は不要
		if (AllOutAttackEnable) return;
		// ボート数が最大でなかったら一斉攻撃フラグお解除
		if (this.lists.Count != maxJapBoats ) {
			AllOutAttackEnable = false;
			return;
		}

		// ボート数が最大になっていたら待機開始
		AllOutAttackEnable = true;
		StanByStartTime = Time.time;
	}

	public bool AllOutAttackStandBy()
	{
		// 一斉攻撃フラグが立っていたら待機状態に以降
		if (AllOutAttackEnable) {
			return true;
		} else {
			return false;
		}
	}

	// 一斉攻撃
	public void AllOutAttack()
	{
		// 一斉攻撃フラグが立ってなかったら一斉攻撃に移らない
		if (!AllOutAttackEnable) return;

		// 待機時間が経過してなければ一斉攻撃には移らない
		if (Time.time - StanByStartTime <= StanByTime) return;

		// 一斉攻撃フラグが立ち待機時間が過ぎているので一斉攻撃開始
		for (int i = 0; i < this.lists.Count; i++) {
			this.lists [i].GetComponentInChildren<BoatController> ().AllOutAttack ();
		}
		// メソッドを抜ける前にフラグを戻しておく
		AllOutAttackEnable = false;
        // 一斉攻撃を待機する
        isAllOutAttackDelay = true;
	}
}
