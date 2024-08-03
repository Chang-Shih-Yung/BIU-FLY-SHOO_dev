using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rg2D;

    private float
        speedBulletType3,
        speedBulletType4;

    // Behaviour messages
    void Awake()
    {
        if (tag != "EBullet")
        {
            if (transform.parent.parent.name == "Type 3" || transform.parent.parent.name == "Type 4")
            {
                rg2D = GetComponent<Rigidbody2D>();
            }
        }

        if (tag != "EBullet")
        {
            if (transform.parent.parent.name == "Type 3")
            {
                speedBulletType3 = PlayerController.Instance.speedBulletType3;
            }
            if (transform.parent.parent.name == "Type 4")
            {
                speedBulletType4 = PlayerController.Instance.speedBulletType4;
            }
        }
    }

    // Behaviour messages
    void OnEnable()
    {
        if (tag != "EBullet")
        {
            if (transform.parent.parent.name == "Type 3")
            {
                transform.localPosition = Vector3.zero;

                rg2D.AddRelativeForce(transform.right * speedBulletType3);
            }

            if (transform.parent.parent.name == "Type 4")
            {
                transform.localPosition = Vector3.zero;

                rg2D.AddRelativeForce(transform.right * speedBulletType4);
            }
        }
    }

    // Behaviour messages
    void Update()
    {
        if (tag != "EBullet")
        {
            if (transform.parent.parent.name != "Type 3" && transform.parent.parent.name != "Type 4")
            {
                transform.position += new Vector3(PlayerController.Instance.speedBulletType1 * Time.deltaTime, 0.0f, 0.0f);
            }

            if (transform.position.x >= 9.5f)
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            transform.position -= new Vector3(PlayerController.Instance.speedBulletType1 * Time.deltaTime, 0.0f, 0.0f);

            if (transform.position.x <= -9.5f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    // Behaviour messages
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hurt" || collision.tag == "EnemyFly" || collision.tag == "EGunStay")
        {
            if (this.tag == "PBullet1" || this.tag == "PBullet2" || this.tag == "PBullet3" || this.tag == "PBullet4")
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                ObstacleScript enemyFly = collision.GetComponent<ObstacleScript>();

                if (enemy != null && enemy.HP > 50 || enemyFly != null && enemyFly.HP > 50)
                {
                    if (enemy != null)
                    {
                        enemy.HP -= 50;
                    }
                    else
                    {
                        enemyFly.HP -= 50;
                    }
                    GameController.Instance.CreateExplosion(true, collision.transform.position);
                }
                else
                {
                    GameController.Instance.CreateExplosion(false, collision.transform.position);
                    collision.gameObject.SetActive(false);
                }
                this.gameObject.SetActive(false);
            }
            else if (this.tag == "PBullet5" || this.tag == "PBullet6")
            {
                GameController.Instance.CreateExplosion(false, collision.transform.position);
                collision.gameObject.SetActive(false);
            }

            this.gameObject.SetActive(false);
        }
        else if (collision.tag == "EBullet")
        {
            if (this.tag == "PBullet1" || this.tag == "PBullet2" || this.tag == "PBullet3"
                || this.tag == "PBullet4" || this.tag == "PBullet6")
            {
                GameController.Instance.CreateExplosion(true, transform.position);

                collision.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            }
            else if (this.tag == "PBullet5")
            {
                GameController.Instance.CreateExplosion(true, transform.position);

                collision.gameObject.SetActive(false);
            }
        }
    }
}
