using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour {

    public Animator explosionAnim;  // 爆発アニメ
    AudioSource explosionSE;        // 爆発SE

    void Awake()
    {
        explosionSE = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        // 爆発アニメーションが終了したら自分で削除
        float normalizedTime = explosionAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (normalizedTime >= 1.0f)
        {
            //****************************************
            // SE再生待ちするとエフェクトが残る
            // あと再生後にノイズが乗る
            //
            //  // SE再生中ならまだ削除しない
            //  //if (explosionSE.isPlaying) return;
            //****************************************

            Destroy(this.gameObject);
        }
    }

    // 爆発アニメ開始
    public void StartAnime()
    {
        explosionAnim.SetTrigger("OnceAnim");
        explosionSE.Play();
    }
}
