using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinController : MonoBehaviour {

    const float MOVE_X_VALUE = 1.0f;
    const float MOVE_Y_VALUE = 1.0f;

    const float MAX_ACCEL_X = 0.15f;
    const float MAX_ACCEL_Y = 0.15f;
    const float MIN_ACCEL_X = -MAX_ACCEL_X;
    const float MIN_ACCEL_Y = -MAX_ACCEL_Y;

    const float ADD_ACCEL_X = 0.01f;
    const float ADD_ACCEL_Y = 0.01f;
    const float ATTENUATION_X = 0.005f;
    const float ATTENUATION_Y = 0.005f;

    public int MaxHitPoint;
    public int MaxAttackPower;

    public GameObject sprite;

    GameObject gameControllerObj;
    GameController gameCtrl;
    Animator DolphinAnimator;
    AudioSource hitSE;

    public float moveMinX, moveMaxX;
    public float moveMinY, moveMaxY;
    public float MaxDamageTime;

    float accelX, accelY;
    float DamageTime;
    int HitPoint;
    int AttackPower;
    bool isAlive;
    bool isDamage;

    void Awake()
    {
        DolphinAnimator = sprite.GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () {
        // ゲームコントローラーOBJ取得設定
        gameControllerObj = GameObject.FindWithTag("GameController");
        gameCtrl = gameControllerObj.GetComponent<GameController>();

        // SE用コンポーネント取得
        hitSE = GetComponent<AudioSource>();

        HitPoint = MaxHitPoint;
        AttackPower = MaxAttackPower;
        DamageTime = MaxDamageTime;

        isAlive = true;
        isDamage = false;
    }
	
	// Update is called once per frame
	void Update () {
        Move();

        // ダメージ時間処理
        if(isDamage)
        {
            DamageTime -= Time.deltaTime;
            if(DamageTime <= 0.0f)
            {
                DamageTime = MaxDamageTime;
                isDamage = false;
                DolphinAnimator.SetBool("IsDamage", false);
            }
        }
    }

    // 移動処理
    void Move()
    {
        float moveX = 0.0f;
        float moveY = 0.0f;

        // 横方向
        if (Input.GetKey("left"))
        {
            //moveX = -MOVE_X_VALUE;
            accelX += -ADD_ACCEL_X;
            if (accelX < MIN_ACCEL_X) accelX = MIN_ACCEL_X;
            DolphinAnimator.SetBool("IsLeft", true);
        }
        else if (Input.GetKey("right"))
        {
            //moveX = MOVE_X_VALUE;
            accelX += ADD_ACCEL_X;
            if (accelX > MAX_ACCEL_X) accelX = MAX_ACCEL_X;
            DolphinAnimator.SetBool("IsLeft", false);
        }
        // 加速度減衰
        else
        {
            if (accelX > 0)
            {
                accelX += -ATTENUATION_X;
                if (accelX < 0.0f) accelX = 0.0f;
            }
            else
            {
                accelX += ATTENUATION_X;
                if (accelX > 0.0f) accelX = 0.0f;
            }
        }

        // 縦方向
        if (Input.GetKey("up"))
        {
            //moveY = MOVE_Y_VALUE;
            accelY += ADD_ACCEL_Y;
            if (accelY > MAX_ACCEL_Y) accelY = MAX_ACCEL_Y;
        }
        else if (Input.GetKey("down"))
        {
            //moveY = -MOVE_Y_VALUE;
            accelY += -ADD_ACCEL_Y;
            if (accelY < MIN_ACCEL_Y) accelY = MIN_ACCEL_Y;
        }
        // 加速度減衰
        else
        {
            if (accelY > 0)
            {
                accelY += -ATTENUATION_Y;
                if (accelY < 0.0f) accelY = 0.0f;
            }
            else
            {
                accelY += ATTENUATION_Y;
                if (accelY > 0.0f) accelY = 0.0f;
            }
        }

        // 移動範囲制限
        float setX = transform.position.x;
        float setY = transform.position.y;

        setX += accelX;
        setY += accelY;

        if (setX < moveMinX) setX = moveMinX;
        if (setX > moveMaxX) setX = moveMaxX;
        if (setY < moveMinY) setY = moveMinY;
        if (setY > moveMaxY) setY = moveMaxY;

        // 移動
        transform.position = new Vector3(setX, setY, 0.0f);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //　敵の攻撃なら処理しない
        if (collision.gameObject.tag == "EnemyAttack") return;

        BoatController BoatCtrl = collision.gameObject.GetComponent<BoatController>();

        // ダメージ処理でfalseが帰ってきたらまだダメージ時間中
        if (!BoatCtrl.DamageBoat(AttackPower)) return;
        // ヒットSE再生
        hitSE.Play();

        // 沈め処理
        if (BoatCtrl.IsDead())
        {
            // 船沈め
            gameCtrl.DownBoat();
            //collision.gameObject.SetActive(false);
            // ボート破棄
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ダメージ時間中なら処理しない
        if (isDamage) return;

        // 敵の攻撃
        if (collision.gameObject.tag == "EnemyAttack")
        {
            BoatSpear boatSpearScript = collision.gameObject.GetComponent<BoatSpear>();
            int enemyAttackPower = boatSpearScript.GetAttackPower();

            //HitPoint -= enemyAttackPower;
            HitPoint -= 1;
            isDamage = true;
            DolphinAnimator.SetBool("IsDamage", true);
            if (HitPoint <= 0)
            {
                isAlive = false;
                HitPoint = 0;
                Debug.Log("you are dead.");
            }
        }
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public int GetHP()
    {
        return HitPoint;
    }
}
