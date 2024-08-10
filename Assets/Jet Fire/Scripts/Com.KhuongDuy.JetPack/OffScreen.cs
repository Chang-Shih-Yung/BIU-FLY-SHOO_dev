using UnityEngine;
using System.Collections;

public class OffScreen : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Behaviour messages
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (!GeometryUtility.TestPlanesAABB(planes, spriteRenderer.bounds))
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            if (this.name != "Layer 3")
            {
                spriteRenderer.enabled = true;
            }
        }
    }
}
