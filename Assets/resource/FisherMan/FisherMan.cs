using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisherMan : MonoBehaviour {
	// 挿し替えようSpriteの定義
	public Sprite fisherman1;
	public Sprite fisherman2;
	public Sprite fisherman3;

	private MoveX moveX;
	private bool isLeft;

	// 漁師のSpriteRenderer反転用
	SpriteRenderer spRender;

	// Use this for initialization
	void Start () {
		// 漁師の絵を選択
		spRender = 	this.GetComponent<SpriteRenderer> ();
		spRender.sprite = SelectFisherManType ();

		// 左右の向きを親クラスから取得
		moveX = this.GetComponentInParent<MoveX>();
		isLeft = moveX.GetIsLeft ();
		spRender.flipX = isLeft;
	}
	
	// Update is called once per frame
	void Update () {
		// 方向転換した時に通知するのが面倒なのでヘタレる
		isLeft = moveX.GetIsLeft ();
		spRender.flipX = isLeft;
		
	}

	private Sprite SelectFisherManType(){
		int ran = Random.Range(1, 4);
		switch(ran)
		{
		case 1:
			return fisherman1;

		case 2:
			return fisherman2;

		case 3:
			return fisherman3;
		}
		return fisherman1;
	}
}
