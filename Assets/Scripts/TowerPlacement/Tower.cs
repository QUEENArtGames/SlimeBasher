using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Tower : MonoBehaviour
{
    public float _hp;

    private List<Collider> _colliders = new List<Collider>();
    private List<Renderer> _renderers = new List<Renderer>();
    private Color _colorPlaceable = new Color(0, 1, 0, 0.5f);
    private Color _colorNotPlaceable = new Color(1, 0, 0, 0.5f);
    private Color _colorNotBuildable = new Color(1, 1, 0, 0.5f);

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

    internal void SetPlaceable(int state)
    {
        switch (state)
        {
            case 0:
                foreach (Renderer r in _renderers)
                {
                    r.material.SetColor("_Color", _colorPlaceable);
                }
                break;

            case 1:
                foreach (Renderer r in _renderers)
                {
                    r.material.SetColor("_Color", _colorNotPlaceable);
                }
                break;

            case 2:
                foreach (Renderer r in _renderers)
                {
                    r.material.SetColor("_Color", _colorNotBuildable);
                }
                break;
        }
    }

    public void attacked(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            //TODO: destory + remove from List
        }
    }

    internal void Kill()
    {
        GetComponent<TowerRessourceManagement>().DestroyTower();
        Destroy(gameObject);
    }
}
