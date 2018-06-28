using UnityEngine;

namespace Assets.Scripts
{
    public class GasSlime : MonoBehaviour
    {

        private float _maxHitpoints;

        private GasSlimeTransformScript[] _transformEffects;
        public float _hitpoints;

        void Start()
        {

            _maxHitpoints = _hitpoints;
            _transformEffects = this.GetComponents<GasSlimeTransformScript>();
        }

        // Update is called once per frame
        void Update()
        {

        }


        void transformSlime()
        {

            foreach (GasSlimeTransformScript transformEffect in _transformEffects)
            {
                transformEffect.setHpPercent((100 / _maxHitpoints) * _hitpoints);
            }
        }

        public void TakeDamage(int damage)
        {
            _hitpoints -= damage;
            transformSlime();
        }

        public void Kill()
        {
            Destroy(this.gameObject);
            GetComponent<SlimeRessourceManagement>().DropRessources();
        }
    }
}
