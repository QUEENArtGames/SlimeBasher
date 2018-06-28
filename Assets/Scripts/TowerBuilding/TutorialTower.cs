using System.Collections.Generic;
using UnityEngine;

public class TutorialTower : MonoBehaviour
{

    private List<Renderer> _renderers = new List<Renderer>();
    public GameObject TutorialUI;

    private Color color = new Color(0, 1, 0, 0.5f);

    // Use this for initialization
    void Awake()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            foreach (Material m in r.materials)
            {
                m.SetColor("_Color", color);
            }
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
