using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeart : MonoBehaviour
{
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
            itemHeart.SetActive(false);
        }
    }
}
