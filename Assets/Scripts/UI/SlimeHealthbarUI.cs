using UnityEngine;

public class SlimeHealthbarUI : MonoBehaviour
{

    void Update()
    {
        //SlimeHealthbarUI should always look at the main camera, therefore the healthbar looks like 2-dimensional
        transform.LookAt(Camera.main.transform);
    }
}
