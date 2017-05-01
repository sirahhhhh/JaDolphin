using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPanel : MonoBehaviour {

    public GameObject[] HPicons;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHPPanel(int PlayerHP)
    {
        for(int i = 0; i < HPicons.Length; i++)
        {
            if (i < PlayerHP) HPicons[i].SetActive(true);
            else              HPicons[i].SetActive(false);
        }
    }
}
