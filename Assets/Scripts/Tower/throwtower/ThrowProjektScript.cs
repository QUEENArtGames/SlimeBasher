﻿using System.Collections;
using UnityEngine;
namespace Assets.Scripts
{
    public class ThrowProjektScript : MonoBehaviour
    {


        private Transform Projectile;

        public Transform Target;
        private Transform copyTarget;
        public float firingAngle = 45.0f;
        public float gravity = 9.8f;
        public float _damage = 50;

        public GameObject puddle;

        //public float flightDuration;

        private Rigidbody rb;

        // Use this for initialization
        void Start()
        {
            //rb = GetComponent<Rigidbody>();

            copyTarget = Target;
            if (copyTarget != null)
            {
                StartCoroutine(SimulateProjectile());
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("Target not found: DESTROYED");
            }
        }


        IEnumerator SimulateProjectile()
        {

            //yield return new WaitForSeconds(0.5f);

            Projectile = transform;
            //Debug.Log("test");
            // Calculate distance to target
            float target_Distance = Vector3.Distance(transform.position, copyTarget.position);

            // Calculate the velocity needed to throw the object to the target at specified angle.
            float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

            // Extract the X  Y componenent of the velocity
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

            // Calculate flight time.
            float flightDuration = target_Distance / Vx;

            // Rotate projectile to face the target.
            transform.rotation = Quaternion.LookRotation(copyTarget.position - transform.position);


            float elapse_time = 0;

            while (elapse_time < flightDuration)
            {
                transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

                elapse_time += Time.deltaTime;

                yield return null;
            }
            SpwanPuddle();
            //Destroy(gameObject);
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                Debug.Log("hit");
                other.gameObject.GetComponent<SlimeScript>().TakeDamage(_damage);
            }
            SpwanPuddle();

            //Destroy(gameObject);
        }



        void SpwanPuddle()
        {

            Destroy(gameObject);
            GameObject obj = Instantiate(puddle) as GameObject;
            obj.transform.position = transform.position;
        }
    }
}