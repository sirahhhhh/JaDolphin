using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeart : MonoBehaviour
{
    // 回復SEをこちらに持たせようと思ったけど
    // アイテムを取得させてスプライトを消すのと
    // SE再生が両立しないので要調査

    public DolphinController dolphinCtrl;
    public GameObject itemHeart;

    int healHitPoint;   // 回復量

    // Use this for initialization
    void Start()
    {
        healHitPoint = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // アイテム取得
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 接触したのがイルカなら
        // 回復してアイテムを消す
        if(collision.gameObject.tag == "Player")
        {
            dolphinCtrl.IncreaseHP(healHitPoint);
            Destroy(itemHeart);
        }
    }
}
