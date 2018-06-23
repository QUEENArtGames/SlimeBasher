using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts{

public class BubbleTowerScript : MonoBehaviour {

	public GameObject projectile;

    public List<GameObject> spawnpoints;


	void Start () {
        spawnpoints = new List<GameObject>();
        Debug.Log(transform.childCount);

        for (int i = 0; i < transform.childCount; i++) 
        {
            if (transform.GetChild(i).tag == "SpawnpointBubbleTower")
            {
                Debug.Log(transform.GetChild(i).name);
                spawnpoints.Add(transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        Debug.Log(spawnpoints);

        Debug.Log(spawnpoints.Count);
        
        Debug.Log(spawnpoints[0].tag);
	}
	
	void Update () {
		
	}

	public void BubbleProjectile(int bubbleIndex) {

        // bubbleIndex 
        // 0 = top, 1 = right, 2 = bottom, 3 = left

		GameObject bubbleProjectile = Instantiate(projectile) as GameObject;
        //print(transform.eulerAngles.y);

        bubbleProjectile.GetComponent<BubbleProjectileScript>().angle = transform.eulerAngles.y;
        //Physics.IgnoreCollision(bubbleProjectile.GetComponent<Collider>(), GetComponent<Collider>());

        if (spawnpoints.Count > bubbleIndex)
        {
            bubbleProjectile.GetComponent<BubbleProjectileScript>().spawnpoint = spawnpoints[bubbleIndex].transform;
        }



    }


}
}
