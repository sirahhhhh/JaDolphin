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

	private GeneralFunc generalFunc;

	// ボート生成時の最大、最小座標
	private float createMinX, createMaxX;
	private float createMinY, createMaxY;
	private GameObject[] japBoats;  // ボートObj

	private int maxJapBoats = 10;	// 漁船の最大数
	private List<GameObject> lists = new List<GameObject>();	// ボートobjを保存しておくリスト

	float itemDropRatio = 1 / 5.0f;	// アイテムドロップ率設定

	public void Init(float minX, float maxX, float minY, float maxY, GameObject[] boats)
	{
		createMinX = minX;
		createMaxX = maxX;
		createMinY = minY;
		createMaxY = maxY;
		japBoats = boats;

		passTime = Time.time;       // 経過時間に現在の時間を設定
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
		// ボート生成
		this.CreateBoat();

		// 沈められたボートListの削除
		this.DeleteBoats ();
	}
}
