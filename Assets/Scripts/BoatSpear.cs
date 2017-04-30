using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSpear : MonoBehaviour {

    public float MaxSpearSpeed;
    public float MaxSpearAliveTime;
    public int AttackPower;

    bool isAlive;
    float SpearSpeed;
    float SpearAliveTime;

    // Use this for initialization
    void Start () {
        SpearSpeed = MaxSpearSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isAlive) return;

        float posX = transform.position.x;
        float posY = transform.position.y;

        posX += SpearSpeed;
        SpearAliveTime -= Time.deltaTime;
        if(SpearAliveTime <= 0.0f)
        {
            isAlive = false;
            SpearAliveTime = 0.0f;
            this.gameObject.SetActive(false);
        }

        transform.position = new Vector3(posX, posY, 0.0f);
    }

    public void ShotSpear()
    {
        isAlive = true;
        SpearAliveTime = MaxSpearAliveTime;
        this.gameObject.SetActive(true);
    }

    public int GetAttackPower()
    {
        return AttackPower;
    }
}
