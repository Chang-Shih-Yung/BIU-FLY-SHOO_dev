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

        // Set the bullet type based on the selected character
        typeBullet = characterSelect;
        Debug.Log("Initial bullet type set to: " + typeBullet);
    }

    // Behaviour messages
    void Start()
    {
        HP = 100;

        flyHash = Animator.StringToHash("Fly");
        layerMaskWalkable = LayerMask.GetMask("Walkable");

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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            collision.gameObject.SetActive(false);

            GameController.Instance.CreateCoinEffect(collision.transform.position);
        }
        else if (collision.tag == "TypeFly")
        {
            HandleTypeFly(collision);
        }
        else if (collision.tag == "EBullet")
        {
            HP -= 10;
            HandleHurt(10, collision, true);

            CheckDie();
        }
        else if (collision.tag == "Hurt" || collision.tag == "EnemyFly")
        {
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
        }
        else if (collision.name == "7")
        {
            HP += 20;
            UIManager.Instance.UpdatePlayerHP(20);
        }
        Debug.Log("Bullet type switched to: " + typeBullet);
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
            rg2D.velocity = new Vector2(0.0f, jumpForce);
            anim.SetBool(flyHash, true);
            smoke.SetActive(true);
        }

        if (isFire)
        {
            Shoot();
        }

        CheckGround();
        UpdateDistance();
    }
}
