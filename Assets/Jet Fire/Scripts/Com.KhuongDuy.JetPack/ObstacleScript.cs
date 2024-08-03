using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour
{
    public Vector2 positionLimit;

    private Vector3 originalPos;

    public int HP { get; set; }

    // Behaviour messages
    void Start()
    {
        originalPos = transform.localPosition;
    }

    // Behaviour messages
    void Update()
    {
        if (this.gameObject.tag != "EnemyFly" && this.gameObject.tag != "TypeFly")
        {
            transform.position -= new Vector3(GameController.Instance.playerSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
        else
        {
            if (this.gameObject.tag == "EnemyFly")
            {
                transform.position -= new Vector3((GameController.Instance.playerSpeed + 3.0f) * Time.deltaTime, 0.0f, 0.0f);
            }
            else if (this.gameObject.tag == "TypeFly")
            {
                transform.position -= new Vector3((GameController.Instance.playerSpeed + 1.3f) * Time.deltaTime, 0.0f, 0.0f);
            }
        }

        if (this.gameObject.tag != "EnemyFly" && this.gameObject.tag != "TypeFly")
        {
            if (transform.position.x <= positionLimit.x + 5.0f)
            {
                GameController.Instance.RespawnObstacle();
            }
        }

        if (transform.position.x <= positionLimit.x)
        {
            transform.position = originalPos;
            this.gameObject.SetActive(false);
        }
    }

    // Behaviour messages
    void OnDisable()
    {
        if (this.gameObject.tag != "EnemyFly" && this.gameObject.tag != "TypeFly")
        {
            GameController.Instance.IsRespawnObstacle = false;

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (!transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

    // Behaviour messages
    void OnEnable()
    {
        if (this.gameObject.tag == "EnemyFly")
        {
            transform.position = new Vector3(10.5f, Random.Range(-1.5f, 5.5f), 0.0f);

            HP = 100;
        }
        else if (this.gameObject.tag == "TypeFly")
        {
            transform.position = new Vector3(10.5f, Random.Range(-1.5f, 4.5f), 0.0f);
        }
    }
}
