using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinController : MonoBehaviour {

    // イルカの最大、最小加速度
    const float MAX_ACCEL_X = 0.15f;
    const float MAX_ACCEL_Y = 0.15f;
    const float MIN_ACCEL_X = -MAX_ACCEL_X;
    const float MIN_ACCEL_Y = -MAX_ACCEL_Y;

    // 加速度
    const float ADD_ACCEL_X = 0.01f;
    const float ADD_ACCEL_Y = 0.01f;

    // 減衰値
    const float ATTENUATION_X = 0.005f;
    const float ATTENUATION_Y = 0.005f;

    // Inspectorから設定
    public int MaxHitPoint;     // 最大HP
    public int MaxAttackPower;  // 最大攻撃力

    public GameObject sprite;

    GameObject gameControllerObj;
    GameController gameCtrl;
    Animator DolphinAnimator;
    AudioSource hitSE;          // 攻撃時SE

    // 移動制限値
    public float moveMinX, moveMaxX;
    public float moveMinY, moveMaxY;

    public float MaxDamageTime;     // ダメージ時間の最大時間(設定用)

    float accelX, accelY;
    float DamageTime;
    int HitPoint;
    int AttackPower;
    bool isAlive;       // 生存フラグ
    bool isDamage;      // ダメージ時間中か

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
                // ダメージ時間終了
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
            accelX += -ADD_ACCEL_X;
            if (accelX < MIN_ACCEL_X) accelX = MIN_ACCEL_X;
            DolphinAnimator.SetBool("IsLeft", true);
        }
        else if (Input.GetKey("right"))
        {
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
            accelY += ADD_ACCEL_Y;
            if (accelY > MAX_ACCEL_Y) accelY = MAX_ACCEL_Y;
        }
        else if (Input.GetKey("down"))
        {
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

        //*********************************************
        // 移動範囲制限
        float setX = transform.position.x;
        float setY = transform.position.y;

        setX += accelX;
        setY += accelY;

        if (setX < moveMinX)        setX = moveMinX;
        else if (setX > moveMaxX)   setX = moveMaxX;
        if (setY < moveMinY)        setY = moveMinY;
        else if (setY > moveMaxY)   setY = moveMaxY;
        //*********************************************

        // 移動
        transform.position = new Vector3(setX, setY, 0.0f);
    }

    // 攻撃時の判定にしか使ってません
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

    // ダメージ時(攻撃を受けたとき)の判定にしか使ってません
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

            // HPなくなったら
            if (HitPoint <= 0)
            {
                isAlive = false;
                HitPoint = 0;
                Debug.Log("you are dead.");
            }
        }
    }

    // 生存フラグ取得
    public bool GetIsAlive()
    {
        return isAlive;
    }

    // HP取得
    public int GetHP()
    {
        return HitPoint;
    }
}
