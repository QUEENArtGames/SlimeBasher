using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class Tower : MonoBehaviour
    {
        private List<Collider> _colliders = new List<Collider>();
        private List<Renderer> _renderers = new List<Renderer>();
        private Color _colorPlaceable = new Color(0, 1, 0, 0.5f);
        private Color _colorNotPlaceable = new Color(1, 0, 0, 0.5f);
        private NavMeshObstacle obstacle;

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

            obstacle = GetComponent<NavMeshObstacle>();
            obstacle.size = new Vector3(1.5f, 2, 1.5f);
        }

        internal void SetPreviewMode(bool state)
        {
            if (state)
            {
                foreach (Collider c in _colliders)
                {
                    c.isTrigger = true;
                }
                obstacle.enabled = false;
            }
            else
            {
                foreach (Collider c in _colliders)
                {
                    c.isTrigger = false;
                }
                obstacle.enabled = true;
            }
        }

        internal void SetPlaceable(bool state)
        {
            if (state)
            {
                foreach (Renderer r in _renderers)
                {
                    foreach (Material m in r.materials)
                    {
                        m.SetColor("_Color", _colorPlaceable);
                    }
                }
            }
            else
            {
                foreach (Renderer r in _renderers)
                {
                    foreach (Material m in r.materials)
                    {
                        m.SetColor("_Color", _colorNotPlaceable);
                    }
                }
            }
        }

        internal void Kill()
        {
            GetComponent<TowerRessourceManagement>().DestroyTower();
            Destroy(gameObject);
        }
    }
}