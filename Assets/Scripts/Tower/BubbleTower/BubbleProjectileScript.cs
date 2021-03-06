﻿using UnityEngine;
namespace Assets.Scripts
{

    public class BubbleProjectileScript : MonoBehaviour
    {


        public bool colliderIsEnabled = true;
        public float angle;
        public Transform spawnpoint;
        public GameObject splashEffect;

        public float maxScale = 2;
        private float growSpeed;
        private float tremble = 0;

        public float speed = 1f;
        public float range = 5;
        public float dmg = 10;
        public int timeToFinish = 100;

        private Vector3 startPosition;

        void Start()
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                angle + 90,
                transform.eulerAngles.z
            );
            startPosition = transform.position;
            growSpeed = maxScale / timeToFinish;
        }

        void Update()
        {

            if (spawnpoint == null)
            {
                Destroy(this.gameObject);
                return;
            }


            float dist = Vector3.Distance(startPosition, transform.position);
            //print("Distance to other: " + dist);
            if (dist > range)
            {
                //Debug.Log(dist);
                Die();
            }
            //Debug.Log(startPosition.x-transform);

            GrowAndMove();
        }

        public void GrowAndMove()
        {
            if (transform.localScale.x <= maxScale)
            {
                //tremble = (Random.value - 0.5f) * 0.01f

                transform.localPosition = new Vector3(spawnpoint.position.x + tremble, spawnpoint.position.y + tremble, spawnpoint.position.z + tremble) +
                                                 (spawnpoint.forward * transform.localScale.x / 2);
                transform.localScale += new Vector3(growSpeed, growSpeed, growSpeed);
            }
            else
            {
                transform.localPosition -= transform.forward * speed * Time.deltaTime;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Enemy")
            {
                collision.transform.GetComponent<SlimeScript>().TakeDamage(dmg);
                Die();
            }
        }

        private void Die()
        {
            var splash = Instantiate(splashEffect, transform.position, transform.rotation);
            Destroy(splash.gameObject, 1);
            Destroy(gameObject);
        }
    }
}
