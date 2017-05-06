using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisherMan : MonoBehaviour {
	// 挿し替えようSpriteの定義
	public Sprite fisherman1;
	public Sprite fisherman2;
	public Sprite fisherman3;

	// 漁船のSpriteRenderer反転用
	SpriteRenderer fmRender;

	// Use this for initialization
	void Start () {
		fmRender = 	this.GetComponent<SpriteRenderer> ();
		fmRender.sprite = SelectFisherManType ();


	}
	
	// Update is called once per frame
	void Update () {
		
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
