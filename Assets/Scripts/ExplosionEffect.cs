using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour {

    public Animator explosionAnim;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        // 爆発アニメーションが終了したら自分で削除
        float normalizedTime = explosionAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (normalizedTime >= 1.0f)
        {
            Destroy(this.gameObject);
        }
    }

    // 爆発アニメ開始
    public void StartAnime()
    {
        explosionAnim.SetTrigger("OnceAnim");
    }
}
