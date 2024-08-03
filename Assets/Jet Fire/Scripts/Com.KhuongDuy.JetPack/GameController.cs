using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;

    public GameObject[]
        obstacles,
        enemiesFly,
        enemiesRun;

    public GameObject[]
        coinEffect,
        smallExplosion,
        bigExplosion,
        itemTypes;

    public float
        playerSpeed,
        enemyFireRate,
        coinPoint;

    public SpriteRenderer[] bgLayers;

    private float coinAmount;

    public bool IsRespawnObstacle { get; set; }

    // Behaviour messages
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
		Application.targetFrameRate = 60;
    }

    // Behaviour messages
    void Start()
    {
        coinAmount = PlayerPrefs.GetFloat(Constants.COIN, 0);
        UIManager.Instance.UpdateCoin(coinAmount);

        RandomBackGround();

        StartCoroutine(StartSpawnObstacle());
        StartCoroutine(StartSpawnEnemyFly());
        StartCoroutine(StartSpawnEnemyRun());
    }

    private void RandomBackGround()
    {
        if (0.0f <= Random.value && Random.value <= 0.25f)
        {
            bgLayers[0].sprite = Resources.Load<Sprite>("Background/1/Layer1");
            bgLayers[1].sprite = Resources.Load<Sprite>("Background/1/Layer1");
            bgLayers[2].sprite = Resources.Load<Sprite>("Background/1/Layer2");
            bgLayers[3].sprite = Resources.Load<Sprite>("Background/1/Layer2");
            
        }
        else if (0.26f <= Random.value && Random.value <= 0.5f)
        {
            bgLayers[0].sprite = Resources.Load<Sprite>("Background/2/Layer01");
            bgLayers[1].sprite = Resources.Load<Sprite>("Background/2/Layer01");
            bgLayers[2].sprite = Resources.Load<Sprite>("Background/2/Layer02");
            bgLayers[3].sprite = Resources.Load<Sprite>("Background/2/Layer02");
        }
        else if (0.51f <= Random.value && Random.value <= 0.75f)
        {
            bgLayers[0].sprite = Resources.Load<Sprite>("Background/3/Layer01");
            bgLayers[1].sprite = Resources.Load<Sprite>("Background/3/Layer01");
            bgLayers[2].sprite = Resources.Load<Sprite>("Background/3/Layer02");
            bgLayers[3].sprite = Resources.Load<Sprite>("Background/3/Layer02");
            bgLayers[4].sprite = Resources.Load<Sprite>("Background/3/Layer03");
            bgLayers[5].sprite = Resources.Load<Sprite>("Background/3/Layer03");

            bgLayers[4].gameObject.SetActive(true);
            bgLayers[5].gameObject.SetActive(true);
        }
        else
        {
            bgLayers[0].sprite = Resources.Load<Sprite>("Background/4/Layer01");
            bgLayers[1].sprite = Resources.Load<Sprite>("Background/4/Layer01");
            bgLayers[2].sprite = Resources.Load<Sprite>("Background/4/Layer02");
            bgLayers[3].sprite = Resources.Load<Sprite>("Background/4/Layer02");
        }

        bgLayers[0].gameObject.SetActive(true);
        bgLayers[1].gameObject.SetActive(true);
        bgLayers[2].gameObject.SetActive(true);
        bgLayers[3].gameObject.SetActive(true);
    }

    private IEnumerator StartSpawnObstacle()
    {
        yield return new WaitForSeconds(3.5f);
        obstacles[0].SetActive(true);
    }

    private IEnumerator StartSpawnEnemyFly()
    {
        yield return new WaitForSeconds(25.0f);
        StartCoroutine(SpawnEnemyFly());
    }

    private IEnumerator SpawnEnemyFly()
    {
        while (true)
        {
            float waitTime = 0.0f;
            waitTime = Random.Range(1.7f, 5.0f);
            yield return new WaitForSeconds(waitTime);

            int randomIndex = 0;
            do
            {
                randomIndex = (int)Mathf.Round(Random.Range(0.0f, 13.4f));
            } while (enemiesFly[randomIndex].activeInHierarchy);

            enemiesFly[randomIndex].SetActive(true);
        }
    }

    private IEnumerator StartSpawnEnemyRun()
    {
        yield return new WaitForSeconds(30.0f);
        StartCoroutine(SpawnEnemyRun());
    }

    private IEnumerator SpawnEnemyRun()
    {
        while (true)
        {
            float waitTime = 0.0f;
            waitTime = Random.Range(2.5f, 5.0f);
            yield return new WaitForSeconds(waitTime);

            int randomIndex = 0;
            do
            {
                randomIndex = (int)Mathf.Round(Random.Range(0.0f, 5.4f));
            } while (enemiesRun[randomIndex].activeInHierarchy);

            enemiesRun[randomIndex].SetActive(true);

            // Spaw random bullet types
            if (0.65 <= Random.value && Random.value <= 0.8)
            {
                int randomIndex2 = 0;
                do
                {
                    randomIndex2 = (int)Mathf.Round(Random.Range(0.0f, 5.4f));
                } while (itemTypes[randomIndex2].activeInHierarchy);

                itemTypes[randomIndex2].SetActive(true);
            }

            // Spaw heal item
            if (0.4 <= Random.value && Random.value <= 0.5)
            {
                if (!itemTypes[6].activeInHierarchy)
                {
                    itemTypes[6].SetActive(true);
                }
            }
        }
    }

    public void RespawnObstacle()
    {
        if (!IsRespawnObstacle)
        {
            IsRespawnObstacle = true;

            int randomIndex = 0;
            do
            {
                randomIndex = (int)Mathf.Round(Random.Range(0.0f, 5.4f));
            } while (obstacles[randomIndex].activeInHierarchy);

            obstacles[randomIndex].SetActive(true);
        }
    }

    public void CreateExplosion(bool small, Vector3 position)
    {
        if (small)
        {
            for (int i = smallExplosion.Length - 1; i >= 0; i--)
            {
                if (!smallExplosion[i].activeInHierarchy)
                {
                    smallExplosion[i].transform.position = position;
                    smallExplosion[i].SetActive(true);
                    break;
                }
            }
        }
        else
        {
            for (int i = bigExplosion.Length - 1; i >= 0; i--)
            {
                if (!bigExplosion[i].activeInHierarchy)
                {
                    bigExplosion[i].transform.position = position;
                    bigExplosion[i].SetActive(true);
                    break;
                }
            }
        }

        SoundManager.Instance.PlaySound(Constants.EXPLOSION_SOUND);
    }

    public void CreateCoinEffect(Vector3 position)
    {
        SoundManager.Instance.PlaySound(Constants.COIN_SOUND);

        for (int i = coinEffect.Length - 1; i >= 0; i--)
        {
            if (!coinEffect[i].activeInHierarchy)
            {
                coinEffect[i].transform.position = position;
                coinEffect[i].SetActive(true);

                coinAmount += coinPoint;
                UIManager.Instance.UpdateCoin(coinAmount);
                break;
            }
        }
    }

    public void GameOver()
    {
        PlayerPrefs.SetFloat(Constants.COIN, coinAmount);

        StartCoroutine(WaitBeforeGameOver());
    }

    private IEnumerator WaitBeforeGameOver()
    {
        yield return new WaitForSeconds(1.0f);
        UIManager.Instance.GameOverShow();
    }
}
