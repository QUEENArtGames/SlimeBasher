using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{

    public float timeToDestroy;

    void Update()
    {
        timeToDestroy -= Time.deltaTime;

        if (timeToDestroy <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
