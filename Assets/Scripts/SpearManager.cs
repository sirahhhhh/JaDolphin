using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearManager : MonoBehaviour {
	private MoveX moveX;
	private bool isLeft;

	// 銛発射のためのObj
	//public GameObject boatSpear;
	BoatSpear boatSpearScript;

	private List<GameObject> spears = new List<GameObject>();   // 銛objを保存しておくリスト
    // 同時に発射できる銛の最大数
    private int[] maxSpears = {
        2,  // NORMAL
        3,  // YELLOW
        6,  // BIGGER
    };

	// Use this for initialization
	void Start () {
		// 左右の向きを親クラスから取得
		moveX = this.GetComponentInParent<MoveX>();
		isLeft = moveX.GetIsLeft ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	// ボートの槍攻撃
	public bool Fire(bool isAttack,Transform trans,float posX,float posY,GameObject boatSpear, BoatController.eBOAT_TYPE boatType)
	{
		// 発射可能な最大数の銛を撃っていたら攻撃しない
		if (spears.Count >= maxSpears[(int)boatType]) return isAttack;
		// 攻撃中なら中断
		if (isAttack) return isAttack;

		// 左向きの場合、コピー元の銛から少し左にずらす
		float adjustPosX = 0.0f;
		if (isLeft) adjustPosX = -1.0f;
        // 漁船(大)の銛位置調整
        if (boatType == BoatController.eBOAT_TYPE.BIGGER)
        {
            float biggerAdjustPoxX = 0.7f;
            if (!isLeft) adjustPosX += biggerAdjustPoxX;
            else adjustPosX -= biggerAdjustPoxX;
        }

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
		boatSpearScript.ShotSpear();
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
