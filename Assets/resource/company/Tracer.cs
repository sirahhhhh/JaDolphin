using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour {
	// イルカの位置取得用
	private GameObject dolphinControllerObj;
	private DolphinController dolphinCtrl;

	// 横反転のためのSpriteRenderer
	SpriteRenderer spRender;

	// Use this for initialization
	void Start () {
		// スプライトレンダラ取得
		// 画像の左右反転に使用する
		spRender = GetComponent<SpriteRenderer>();

		// イルカの位置取得用
		dolphinControllerObj = GameObject.FindWithTag ("Player");
		dolphinCtrl = dolphinControllerObj.GetComponent<DolphinController> ();
	}

	// Update is called once per frame
	void Update () {
		Move (CheckPosX(),CheckPosY());
	}

	private void Move(float moveX,float moveY)
	{
		// 現在地から移動距離分だけ進める
		transform.position = new Vector3(
			transform.position.x + moveX,
			transform.position.y + moveY,
			transform.position.z
		);
	}

	private float CheckPosX()
	{
		// 移動距離の設定
		// 横方向
		float moveX = 0.01f;
		if (transform.position.x < dolphinCtrl.transform.position.x )
		{
			spRender.flipX = false;
		}
		else if (transform.position.x > dolphinCtrl.transform.position.x )
		{
			spRender.flipX = true;
			moveX = - moveX;
		}
		else
		{
			moveX = 0.0f;
		}
		return moveX;
	}

	private float CheckPosY()
	{
		// 移動距離の設定
		float moveY = 0.01f;
		if (transform.position.y < dolphinCtrl.transform.position.y )
		{
		}
		else if (transform.position.y > dolphinCtrl.transform.position.y )
		{
			moveY = - moveY;
		}
		else
		{
			moveY = 0.0f;
		}
		return moveY;
	}
}
