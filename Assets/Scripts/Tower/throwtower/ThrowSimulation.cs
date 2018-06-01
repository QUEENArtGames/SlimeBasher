using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSimulation : MonoBehaviour {

	private Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
 
    private Transform myTransform;

	public GameObject projec;

	void Awake()
    {
        myTransform = transform;      
    }

	void Start()
    {          
        StartCoroutine(SimulateProjectile());
    }

    public Transform FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go.transform;
                distance = curDistance;
            }
        }
        return closest;
    }
	
    IEnumerator SimulateProjectile()
    {

        Target=FindClosestEnemy();

        if(Target != null){
            // Short delay added before Projectile is thrown
        
            GameObject bullet = Instantiate(projec) as GameObject;

            bullet.GetComponent<ThrowProjektScript>().Target = Target;
            bullet.GetComponent<ThrowProjektScript>().gravity = gravity;
            bullet.GetComponent<ThrowProjektScript>().firingAngle = firingAngle;

            
            //Projectile = bullet.transform;
        
            // Move projectile to the position of throwing object + add some offset if needed.
            bullet.transform.position = myTransform.position + new Vector3(1f, 0.0f, 0);
        
            yield return new WaitForSeconds(4.0f);
            StartCoroutine(SimulateProjectile());

        }


        


    } 
	

}
