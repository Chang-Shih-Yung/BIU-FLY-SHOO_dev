using UnityEngine;
using System.Collections;

public class MoveToLeftSide : MonoBehaviour
{
    public float speedMove;

    public Vector2 positionLimit;

    private float m_boundLayerSize;

    private Sprite[] treeSprites;

    private SpriteRenderer treeRenderer;

    // Behaviour messages
    void Start()
    {
        m_boundLayerSize = GetComponent<SpriteRenderer>().bounds.size.x;

        if (this.name == "Tree")
        {
            treeRenderer = GetComponent<SpriteRenderer>();

            treeSprites = new Sprite[3];
            treeSprites[0] = Resources.Load<Sprite>("Platform/Tree And Tile set/1");
            treeSprites[1] = Resources.Load<Sprite>("Platform/Tree And Tile set/2");
            treeSprites[2] = Resources.Load<Sprite>("Platform/Tree And Tile set/3");
        }
    }

    // Behaviour messages
    void Update()
    {
        if (this.name == "Ground" || this.name == "Tree")
        {
            transform.position -= new Vector3(GameController.Instance.playerSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
        else
        {
            transform.position -= new Vector3(speedMove * Time.deltaTime, 0.0f, 0.0f);
        }

        if (transform.position.x <= positionLimit.x)
        {
            if (this.name == "Tree")
            {
                transform.position = new Vector3(11, positionLimit.y, 0.0f);

                SetUpTree();
            }
            else
            {
                transform.position = new Vector3(transform.position.x + (m_boundLayerSize * 2.0f), positionLimit.y, 0.0f);
            }
        }
    }

    private void SetUpTree()
    {
        // Hide or show tree
        if (Random.value <= 0.7f)
        {
            treeRenderer.enabled = true;
        }
        else
        {
            treeRenderer.enabled = false;
        }

        // Change tree model
        if (0.0f <= Random.value && Random.value <= 0.3f)
        {
            treeRenderer.sprite = treeSprites[0];
        }
        else if (0.4f <= Random.value && Random.value <= 0.6f)
        {
            treeRenderer.sprite = treeSprites[1];
        }
        else
        {
            treeRenderer.sprite = treeSprites[2];
        }
    }
}
