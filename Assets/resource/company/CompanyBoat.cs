using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyBoat : MonoBehaviour {

	// 方向転換を起こすの割合(合計が100でなくてもよい)
	private int[] ActionRatio = {
		1000,     // 何もしない
		1,     // 上下方向の進行を逆転させる
	};

	private GeneralFunc generalFunc;

	bool isGoUP = false;		// 上方向に移動中かどうか


	// Use this for initialization
	void Start () {
		generalFunc = new GeneralFunc ();

		// 上下どちらに移動するか決定
		isGoUP = generalFunc.RandomBool();
		
	}
	
	// Update is called once per frame
	void Update () {
		// 縦方向の進行方向を変えるか選択
		Act ();
		// 縦方向の移動
		Move ();
	}


	// 船を進行方向へ移動させる
	void Move()
	{
		// 画面外に出そうなら反転
		if (transform.position.y >= 4.0f || transform.position.y <= -4.0f) isGoUP = !isGoUP;


		// 縦方向
		float moveY = 0.01f;
		if (isGoUP)	moveY = -moveY;

		// 現在地から移動距離分だけ進める
		transform.position = new Vector3(
			transform.position.x,
			transform.position.y + moveY,
			transform.position.z
		);
	}

	// ボートの行動
	void Act()
	{
		switch (generalFunc.SelectInt (ActionRatio)) {
		// 何もしない、待機
		case 0:
			break;

		case 1:
			isGoUP = !isGoUP;
			break;
		}
	}
}
