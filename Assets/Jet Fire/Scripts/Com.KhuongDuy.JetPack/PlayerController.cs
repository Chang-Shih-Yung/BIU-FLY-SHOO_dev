using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;

    public GameObject explosion;

    public GameObject[]
        bulletType1,
        bulletType2,
        bulletType3,
        bulletType4,
        bulletType5,
        bulletType6;

    public Rigidbody2D rg2D;

    public Animator anim;

    public float
        jumpForce,
        distanceFactor,
        speedBulletType1,
        //上一發到下一發的間隔時間
        fireRateBulletType1,
        speedBulletType2,
        fireRateBulletType2,
        speedBulletType3,
        fireRateBulletType3,
        speedBulletType4,
        fireRateBulletType4,
        speedBulletType5,
        fireRateBulletType5,
        speedBulletType6,
        fireRateBulletType6;

    public GameObject
        groundCheck,
        firePoint,
        smoke;

    [SerializeField] int typeBullet;

    private float nextFire;

    private float distanceMove;

    private int
        flyHash,
        shootHash,
        layerMaskWalkable,
        HP;

    private bool
        isJump,
        isFire;

    // Behaviour messages
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        //PlayerPrefs.GetInt 方法從存儲中讀取一個整數值。這個值是之前存儲的玩家選擇的角色ID。
        //Constants.CHARACTER_SELECT 是一個字符串常量，用來標識這個存儲的值。
        //如果沒有找到這個鍵值對，則使用默認值 1。
        int characterSelect = PlayerPrefs.GetInt(Constants.CHARACTER_SELECT, 1);

        //通過將字符串 "Animation/Animators/C" 和 characterSelect 的值拼接，構建出動畫控制器的資源路徑。
        string animControllerPath = "Animation/Animators/C" + characterSelect;//C1 C2 C3 C4 C5 C6 C7 C8 
        Debug.Log("Loading animator controller from path: " + animControllerPath);

        //RuntimeAnimatorController 可以綁定動畫參數（如浮點數、整數、布爾值和觸發器），這些參數可以用來控制動畫狀態的變化。
        //根據玩家的角色選擇'動態加載'並設置對應的動畫控制器(導入上面加載的動畫控制器名稱)，以便該角色能夠使用正確的動畫。
        anim.runtimeAnimatorController = Resources.Load(animControllerPath) as RuntimeAnimatorController;


        // 如果你要讓子彈類型跟選角色有關，那麼這裡的 typeBullet = characterSelect; 就是對的
        //但這裡的子彈模式是靠吃道具所以暫時不用這個

        typeBullet = characterSelect;
        // Debug.Log("Initial bullet type set to: " + typeBullet);
    }

    // Behaviour messages
    void Start()
    {
        HP = 1500;

        //以下是初始化抓取飛行與碰撞圖層的狀態，即在游戏对象启用后调用一次的方法
        //Animator.StringToHash 方法將字符串轉換為整數值，這樣可以更快地訪問動畫狀態。
        //其實就是Fly的動畫狀態
        flyHash = Animator.StringToHash("Fly");
        shootHash = Animator.StringToHash("Shoot");
        //LayerMask.GetMask 方法返回一個整數值，該值包含所有傳入的層的位掩碼。
        //以便在物理碰撞檢測中使用。
        layerMaskWalkable = LayerMask.GetMask("Walkable");
        //初始移動距離
        distanceMove = 0.0f;

    }

    public void JumpBtnDown()
    {
        isJump = true;
    }

    public void JumpBtnUp()
    {
        isJump = false;
    }

    public void FireBtnDown()
    {
        isFire = true;
    }

    public void FireBtnUp()
    {
        isFire = false;
    }

    private void Shoot()
    {
        Debug.Log("Current bullet type: " + typeBullet);
        if (typeBullet == 1)
        {
            Type_1_2_5_6(bulletType1, fireRateBulletType1);
        }
        else if (typeBullet == 2)
        {
            Type_1_2_5_6(bulletType2, fireRateBulletType2);
        }
        else if (typeBullet == 3)
        {
            Type_3_4(bulletType3, fireRateBulletType3);
        }
        else if (typeBullet == 4)
        {
            Type_3_4(bulletType4, fireRateBulletType4);
        }
        else if (typeBullet == 5)
        {
            Type_1_2_5_6(bulletType5, fireRateBulletType5);
        }
        else if (typeBullet == 6)
        {
            Type_1_2_5_6(bulletType6, fireRateBulletType6);
        }
        //動畫只播放一次，bool就變成false
        anim.SetBool(shootHash, true);

    }

    private void Type_1_2_5_6(GameObject[] bulletType, float fireRate)
    {
        if (Time.time >= nextFire)
        {
            for (int i = bulletType.Length - 1; i >= 0; i--)
            {
                if (!bulletType[i].activeInHierarchy)
                {
                    bulletType[i].transform.position = firePoint.transform.position;
                    bulletType[i].SetActive(true);

                    SoundManager.Instance.PlaySound(Constants.PLAYER_SHOOT_SOUND);

                    nextFire = Time.time + fireRate;
                    break;
                }
            }
        }
    }

    private void Type_3_4(GameObject[] bulletType, float fireRate)
    {
        if (Time.time >= nextFire)
        {
            for (int i = bulletType.Length - 1; i >= 0; i--)
            {
                if (!bulletType[i].transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    bulletType[i].transform.position = firePoint.transform.position;

                    for (int j = bulletType[i].transform.childCount - 1; j >= 0; j--)
                    {
                        bulletType[i].transform.GetChild(j).gameObject.SetActive(true);
                    }

                    SoundManager.Instance.PlaySound(Constants.PLAYER_SHOOT_SOUND);

                    nextFire = Time.time + fireRate;
                    break;
                }
            }
        }
    }

    private void CheckGround()
    {
        if (Physics2D.OverlapCircle(groundCheck.transform.position, 0.1f, layerMaskWalkable))
        {
            anim.SetBool(flyHash, false);
            smoke.SetActive(false);
        }
    }

    private void UpdateDistance()
    {
        distanceMove += Time.deltaTime * distanceFactor;
        UIManager.Instance.UpdateDistance(Mathf.Round(distanceMove));
    }

    // Behaviour messages
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with tag: " + collision.tag + " and name: " + collision.name);
        //吃金幣
        if (collision.tag == "Coin")
        {
            collision.gameObject.SetActive(false);

            GameController.Instance.CreateCoinEffect(collision.transform.position);
        }
        //吃道具
        else if (collision.tag == "TypeFly")
        {
            HandleTypeFly(collision);
        }
        //吃敵人子彈
        else if (collision.tag == "EBullet")
        {
            HP -= 10;
            HandleHurt(10, collision, true);

            CheckDie();
        }
        //標籤Hurt是站在地上的敵人、EnemyFly是飛行的敵人
        else if (collision.tag == "Hurt" || collision.tag == "EnemyFly")
        {
            //EBomb敵人的炸彈
            if (collision.name != "EBomb")
            {
                HP -= 20;
                HandleHurt(20, collision, true);
            }
            else
            {
                HP -= 30;
                HandleHurt(30, collision, false);
            }

            CheckDie();
        }
        else if (collision.tag == "Trap")
        {
            HP -= 20;
            UIManager.Instance.UpdatePlayerHP(-20);

            GameController.Instance.CreateExplosion(true, transform.position);

            CheckDie();
        }
    }

    //根据飞行物的名字，切换玩家的子弹类型，并在特定情况下增加玩家的 HP（生命值）。碰撞发生后，飞行物被禁用，并生成一个金币效果
    //就是一個吃道具改變武器種類的方法
    private void HandleTypeFly(Collider2D collision)
    {
        if (collision.name == "1")
        {
            typeBullet = 1;
        }
        else if (collision.name == "2")
        {
            typeBullet = 2;
        }
        else if (collision.name == "3")
        {
            typeBullet = 3;
        }
        else if (collision.name == "4")
        {
            typeBullet = 4;
        }
        else if (collision.name == "5")
        {
            typeBullet = 5;
        }
        else if (collision.name == "6")
        {
            typeBullet = 6;
            HP += 100;
            UIManager.Instance.UpdatePlayerHP(10);
        }
        else if (collision.name == "7")
        {
            HP += 300;
            UIManager.Instance.UpdatePlayerHP(30);
        }
        // Debug.Log("Bullet type switched to: " + typeBullet);
        GameController.Instance.CreateCoinEffect(collision.transform.position);
        collision.gameObject.SetActive(false);
    }

    private void HandleHurt(int subtractAmount, Collider2D collision, bool smallExplosion)
    {
        UIManager.Instance.UpdatePlayerHP(-subtractAmount);

        GameController.Instance.CreateExplosion(smallExplosion, collision.transform.position);
        collision.gameObject.SetActive(false);
    }

    private void CheckDie()
    {
        if (HP <= 0)
        {
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
            UIManager.Instance.UpdateScore(Mathf.Round(distanceMove));
            GameController.Instance.GameOver();
            this.gameObject.SetActive(false);
        }
    }
    // Behaviour messages
    void Update()
    {
        if (isJump)
        {
            //给玩家一个向上的速度，使其跳起来
            //在控制面板給數值：4.7
            rg2D.velocity = new Vector2(0.0f, jumpForce);
            //设置动画状态为飞行
            anim.SetBool(flyHash, true);
            smoke.SetActive(true);
        }

        if (isFire)
        {
            Shoot();
        }else{
            anim.SetBool(shootHash, false);
        }
        //检查玩家是否在地面上
        CheckGround();
        //更新玩家移动的距离
        UpdateDistance();
    }
}
