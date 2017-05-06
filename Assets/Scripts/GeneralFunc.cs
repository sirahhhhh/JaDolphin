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
		
	// 抽選
	public int SelectInt(int[] ActionRatio)
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
}
