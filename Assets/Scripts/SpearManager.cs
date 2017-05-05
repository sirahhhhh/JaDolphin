using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearManager : MonoBehaviour {


	// 銛発射のためのObj
	//public GameObject boatSpear;
	BoatSpear boatSpearScript;

	private List<GameObject> spears = new List<GameObject>();	// 銛objを保存しておくリスト
	private int maxSpears = 2;	// 同時に発射できる銛の最大数

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// ボートの槍攻撃
	public bool Fire(bool isAttack,bool isLeft,Transform trans,float posX,float posY,GameObject boatSpear)
	{
		// 発射可能な最大数の銛を撃っていたら攻撃しない
		if (spears.Count >= maxSpears) return isAttack;
		// 攻撃中なら中断
		if (isAttack) return isAttack;

		// 左向きの場合、コピー元の銛から少し左にずらす
		float adjustPosX = 0.0f;
		if (isLeft) adjustPosX = -1.0f;

		// 銛の生成
		GameObject createObj = (GameObject)Instantiate(
			boatSpear,
			new Vector3(
				posX + adjustPosX,
				posY,
				0.0f),
			Quaternion.identity);
		createObj.transform.parent = trans; // 生成した銛の親を生成元ボートに設定

		boatSpearScript = createObj.GetComponent<BoatSpear>();
		boatSpearScript.ShotSpear(isLeft);
		isAttack = true;


		// 銛のオブジェクトをListに入れておく
		spears.Add(createObj);

	return isAttack;
	}

	// 削除された銛をListから削除
	public void DeleteSpears()
	{
		for (int i = spears.Count - 1; i >= 0; i--) {
			if (spears [i] == null) {
				spears.RemoveAt (i);
			}
		}
	}
}
