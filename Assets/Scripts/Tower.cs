using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tower : MonoBehaviour
{
    private List<Collider> _colliders = new List<Collider>();
    private List<Renderer> _renderers = new List<Renderer>();
    private Color _colorPlaceable = new Color(0, 1, 0, 0.5f);
    private Color _colorNotPlaceable = new Color(1, 0, 0, 0.5f);

    // Use this for initialization
    void Awake()
    {
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            _colliders.Add(c);
        }

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            _renderers.Add(r);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetPreviewMode(bool state)
    {
        if (state)
        {
            foreach (Collider c in _colliders)
            {
                c.isTrigger = true;
            }
        }
        else
        {
            foreach (Collider c in _colliders)
            {
                c.isTrigger = false;
            }
        }
    }

    internal void SetPlaceable(bool state)
    {
        if (state)
        {
            foreach (Renderer r in _renderers)
            {
                r.material.SetColor("_Color", _colorPlaceable);
            }
        }
        else
        {
            foreach (Renderer r in _renderers)
            {
                r.material.SetColor("_Color", _colorNotPlaceable);
            }
        }
    }
}
