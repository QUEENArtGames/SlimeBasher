using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Attack : MonoBehaviour
    {
        private float damage = 17.0f;
        public bool dealDamage = false;

        private void Update()
        {
            /*if (transform.parent != null && transform.parent.parent != null)
            {
                if (transform.parent.parent.name == "Holder3")
                {
                    dealDamage = true;
                    GetComponent<Collider>().isTrigger = true;
                }
            }
            else
            {
                dealDamage = false;
                GetComponent<Collider>().isTrigger = false;
            }*/
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("dealDamage: " + dealDamage + " is enemey: " + other.gameObject.tag == "Enemy");
            if (other.gameObject.tag == "Enemy" && dealDamage)
            {
                other.gameObject.GetComponent<SlimeScript>().TakeDamage(damage);
            }
        }

        public void EnableAttack()
        {
            if (transform.parent.parent.name == "Holder3")
            {
                dealDamage = true;
                GetComponent<Collider>().isTrigger = true;
            }
        }

        public void DisableAttack()
        {
            dealDamage = false;
            GetComponent<Collider>().isTrigger = false;
        }
    }
}