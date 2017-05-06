using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFunc{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	// bool値の乱数を返す関数
	public bool RandomBool()
	{
		return Random.Range(0, 2) == 0;
	}
}
