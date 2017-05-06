using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveX : MonoBehaviour {

	// 方向転換を起こすの割合(合計が100でなくてもよい)
	private int[] ActionRatio = {
		1000,     // 何もしない
		1      // 横方向の方向転換
	};

	private GeneralFunc generalFunc;

	bool isLeft = false;        // 左向いてるか

	// 横反転のためのSpriteRenderer
	SpriteRenderer spRender;

	// Use this for initialization
	void Start () {
		generalFunc = new GeneralFunc ();

		// スプライトレンダラ取得
		// 画像の左右反転に使用する
		spRender = GetComponent<SpriteRenderer>();

		// ボートの向きをランダムに決める
		isLeft = generalFunc.RandomBool();
		spRender.flipX = isLeft;
	}
	
	// Update is called once per frame
	void Update () {
		// 横方向の進行方向を変えるか選択
		Act ();
		// 横方向の移動
		Move ();
	}

	private void Move()
	{
		// 画面外に出そうなら反転
		if (transform.position.x >= 4.0f || transform.position.x <= -4.0f) Reverse();

		// 移動距離の設定
		// 横方向
		float moveX = 0.01f;
		if (isLeft)	moveX = - moveX;

		// 現在地から移動距離分だけ進める
		transform.position = new Vector3(
			transform.position.x + moveX,
			transform.position.y,
			transform.position.z
		);
	}

	// ボートの行動
	private void Act()
	{
		switch(generalFunc.SelectInt(ActionRatio))
		{
		// 何もしない、待機
		case 0:
			break;

		case 1:
			Reverse ();
		break;		}
	}
		
	// ボートを反転
	private void Reverse()
	{
		isLeft = !isLeft;
		spRender.flipX = isLeft;
	}
}
