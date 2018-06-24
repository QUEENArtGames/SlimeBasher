using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts{

public class BubbleTowerScript : MonoBehaviour {

	public GameObject projectile;

    public List<GameObject> spawnpoints;
    public GameObject ShootCylinder;
        public ParticleSystem BubbleParticleSystem;
    private TowerRessourceManagement _towerRessourceManagement;


	void Start () {
        _towerRessourceManagement = GetComponentInParent<TowerRessourceManagement>();
        spawnpoints = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++) 
        {
            if (transform.GetChild(i).tag == "SpawnpointBubbleTower")
            {
                spawnpoints.Add(transform.GetChild(i).GetChild(0).gameObject);
            }
        }
	}
	
	void Update () {
		
	}

	public void BubbleProjectile(int bubbleIndex) {

            // bubbleIndex 
            // 0 = top, 1 = right, 2 = bottom, 3 = left
            if (_towerRessourceManagement.AttachedScraps.Count - 1 < bubbleIndex)
                return;

        GameObject bubbleProjectile = Instantiate(projectile) as GameObject;
        //print(transform.eulerAngles.y);

        bubbleProjectile.GetComponent<BubbleProjectileScript>().angle = transform.eulerAngles.y;
        //Physics.IgnoreCollision(bubbleProjectile.GetComponent<Collider>(), GetComponent<Collider>());

        if (spawnpoints.Count > bubbleIndex)
                   bubbleProjectile.GetComponent<BubbleProjectileScript>().spawnpoint = spawnpoints[bubbleIndex].transform;




    }

        public void RemoveShootCylinder()
        {
            Destroy(ShootCylinder);
        }

        public void StartBubbleParticleEffect(){
            BubbleParticleSystem.Play();
        }


}
}
