using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Vector2 positionLimit;

    private GameObject prefabBullet;

    private GameObject[] bullets;

    private Transform firePoint = null;

    private Vector3 originalPos;

    private float nextFire;

    public int HP { get; set; }

    // Behaviour messages
    void Awake()
    {
        if (transform.childCount != 0)
        {
            prefabBullet = Resources.Load<GameObject>("Prefabs/EBullet");

            bullets = new GameObject[2];

            bullets[0] = Instantiate(prefabBullet, prefabBullet.transform.position, prefabBullet.transform.rotation) as GameObject;
            bullets[1] = Instantiate(prefabBullet, prefabBullet.transform.position, prefabBullet.transform.rotation) as GameObject;

            firePoint = transform.GetChild(0);
        }

        originalPos = transform.localPosition;
    }

    // Behaviour messages
    void OnEnable()
    {
        HP = 100;
    }

    // Behaviour messages
    void Update()
    {
        if (transform.parent.name == "Run")
        {
            transform.position -= new Vector3((GameController.Instance.playerSpeed + 1.5f) * Time.deltaTime, 0.0f, 0.0f);

            if (transform.position.x <= positionLimit.x)
            {
                this.gameObject.SetActive(false);
            }
        }

        Shoot();
    }

    private void Shoot()
    {
        if (firePoint != null)
        {
            if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= 12.0f)
            {
                if (Time.time >= nextFire)
                {
                    for (int i = bullets.Length - 1; i >= 0; i--)
                    {
                        if (!bullets[i].activeInHierarchy)
                        {
                            bullets[i].transform.position = firePoint.position;
                            bullets[i].SetActive(true);

                            SoundManager.Instance.PlaySound(Constants.ENEMY_SHOOT_SOUND);

                            nextFire = Time.time + GameController.Instance.enemyFireRate;
                            break;
                        }
                    }
                }
            }
        }
    }

    // Behaviour messages
    void OnDisable()
    {
        transform.localPosition = originalPos;
    }
}
