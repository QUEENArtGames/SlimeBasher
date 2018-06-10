﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class Tower : MonoBehaviour
    {
        public int Hitpoints = 100;
        public float destructiveForceMin = 2;
        public float destructiveForceMax = 4;

        private List<Collider> _colliders = new List<Collider>();
        private List<Renderer> _renderers = new List<Renderer>();
        private Color _colorPlaceable = new Color(0, 1, 0, 0.5f);
        private Color _colorNotPlaceable = new Color(1, 0, 0, 0.5f);
        private NavMeshObstacle obstacle;
        public List<GameObject> _parts = new List<GameObject>();
        private bool _isDying = false, _isDead = false;

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

        private void Update()
        {
            if (Hitpoints <= 0 && !_isDead)
            {
                _isDying = true;
            }

            if (_isDying)
            {
                this.Kill();
                _isDying = false;
                _isDead = true;
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
            //Destroy(gameObject, 5);
            _parts[0].GetComponentInParent<Animator>().enabled = false;
            foreach (GameObject part in _parts)
            {
                Vector3 scale = part.transform.parent.lossyScale;
                part.transform.SetParent(null);
                part.transform.localScale = scale;
                part.AddComponent<CapsuleCollider>();
                var rb = part.AddComponent<Rigidbody>();
                rb.AddForce(new Vector3(Random.Range(0, 0), Random.Range(destructiveForceMin, destructiveForceMax), Random.Range(0, 0)),ForceMode.VelocityChange);
            }            
        }
    }
}