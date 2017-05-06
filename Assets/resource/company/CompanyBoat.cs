using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyBoat : MonoBehaviour {

	// 方向転換を起こすの割合(合計が100でなくてもよい)
	private int[] ActionRatio = {
		1000,     // 何もしない
		1,     // 上下方向の進行を逆転させる
		1      // 横方向の方向転換
	};

	bool isGoUP = false;		// 上方向に移動中かどうか
	bool isLeft = false;        // 左向いてるか

	// お邪魔船のSpriteRenderer反転用
	SpriteRenderer spRender;


	// Use this for initialization
	void Start () {
		// スプライトレンダラ取得
		// 画像の左右反転に使用する
		spRender = GetComponent<SpriteRenderer>();

		// ボートの向きをランダムに決める
		isLeft = RandomBool();
		spRender.flipX = isLeft;

		isGoUP = RandomBool ();
		
	}
	
	// Update is called once per frame
	void Update () {
		Act ();
		Move ();
	}


	// 船を進行方向へ移動させる
	void Move()
	{
		// 画面外に出そうなら反転
		if (transform.position.x >= 4.0f || transform.position.x <= -4.0f) Reverse();
		if (transform.position.y >= 4.0f || transform.position.y <= -4.0f) isGoUP = !isGoUP;

		// 移動距離の設定
		// 横方向
		float moveX = 0.01f;
		if (isLeft)	moveX = - moveX;

		// 縦方向
		float moveY = 0.01f;
		if (isGoUP)	moveY = -moveY;

		// 現在地から移動距離分だけ進める
		transform.position = new Vector3(
			transform.position.x + moveX,
			transform.position.y + moveY,
			transform.position.z
		);
	}

	// ボートの行動
	void Act()
	{
		switch(SelectAct())
		{
		// 何もしない、待機
		case 0:
			break;

		case 1:
			isGoUP = !isGoUP;
			break;

		case 2:
			Reverse ();
			break;		}
	}

	// ボートの行動を抽選
	private int SelectAct()
	{
		// 行動の抽選
		int totalRatio = 0;
		for(int i = 0; i < ActionRatio.Length; i++)
		{
			totalRatio += ActionRatio[i];
		}
		int ActIndex = 0;
		int ranVal = Random.Range(0, totalRatio + 1);
		for (int i = 0; i < ActionRatio.Length; i++)
		{
			ranVal -= ActionRatio[i];
			// 0以下で抽選決定
			if(ranVal <= 0)
			{
				ActIndex = i;
				break;
			}
		}
		return ActIndex;
	}


	// ボートを反転
	void Reverse()
	{
		isLeft = !isLeft;
		spRender.flipX = isLeft;
	}

	// bool値の乱数を返す関数
	private static bool RandomBool()
	{
		return Random.Range(0, 2) == 0;
	}
}
