using UnityEngine;

public class destroyDebugHit : MonoBehaviour
{
    public float destroyAfter = 25.0f;
    public float createdTime = 0.0f;

    private void Start()
    {
        createdTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (Time.time - createdTime > destroyAfter)
        {
            Destroy(this.gameObject);
        }
    }
}
