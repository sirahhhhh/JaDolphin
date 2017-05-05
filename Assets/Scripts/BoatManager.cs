using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 漁船関係をまとめたクラス
public class BoatManager : MonoBehaviour {
    private const int CREATE_BIGGER_BOAT_RATIO = 10;    // 漁船(大)が作られる割合
	private const float CREATE_TIME = 1.0f; // ボート生成時間
	private float passTime; // 生成時に使う経過時間
	private int DownBoats;  // 沈めたボート数

	// ボート生成時の最大、最小座標
	private float createMinX, createMaxX;
	private float createMinY, createMaxY;
	private GameObject[] japBoats;  // ボートObj

	private int maxJapBoats = 10;	// 漁船の最大数
	private List<GameObject> lists = new List<GameObject>();	// ボートobjを保存しておくリスト

	float itemDropRatio;            // アイテムドロップ率

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
            GetCreateBoatObj(),
			new Vector3(createX, createY, 0.0f),
			Quaternion.identity
		);
		Obj.SetActive(true);
		this.lists.Add (Obj);
	}

    // 生成するボートのコピー元ボートのGameObjectを取得する
    private GameObject GetCreateBoatObj()
    {
        // 生成する漁船を抽選
        int BoatIndex = Random.Range(1, CREATE_BIGGER_BOAT_RATIO + 1);
        BoatIndex /= CREATE_BIGGER_BOAT_RATIO;

        return japBoats[BoatIndex];
    }

	public void Run()
	{
		// ボート生成
		this.CreateBoat();

		// 沈められたボートListの削除
		this.DeleteBoats ();
	}

	// ボートが沈められた時の処理
	public void BoatDown(GameObject explosion, GameObject itemHeart,float posX, float posY, BoatController.eBOAT_TYPE boatType)
	{
		// 沈めたボートの数をカウント
		DownBoats++;
		//Debug.Log("downs : " + DownBoats);
		// 爆発エフェクト
		CreateExplosionEffect(explosion, posX,posY);
		// アイテムドロップ
		DropItem (itemHeart, posX, posY, boatType);
	}

	// 沈めたボートの数を返す
	public int GetDownBoat()
	{
		return DownBoats;
	}
		
	// Use this for initialization
	void Start () {
		itemDropRatio = 1 / 5.0f;   // アイテムドロップ率設定
	}

	// Update is called once per frame
	void Update () {
		
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
