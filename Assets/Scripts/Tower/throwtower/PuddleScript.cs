using UnityEngine;
namespace Assets.Scripts
{
    public class PuddleScript : MonoBehaviour
    {

        public float _damage = 10;
        public float _livetime = 0.5f;

        private float currundtime = 0;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (currundtime > _livetime)
            {
                Destroy(gameObject);
            }
            else
            {
                currundtime += Time.deltaTime;
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {

                other.gameObject.GetComponent<SlimeScript>().TakeDamage(_damage);
            }
        }

    }
}
