using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTower : MonoBehaviour {

	private LineRenderer line;                           // Line Renderer
	public int lengthOfLineRenderer = 20;

	public Transform Target;
	public float firingAngle = 45.0f;
    public float gravity = 9.8f;

	private Vector3 targetPoint;
     private Quaternion targetRotation;

 
	public GameObject projec;

	public Vector3 raycastDir;

	GameObject bullet;


	// Use this for initialization
	void Start () {
		// Add a Line Renderer to the GameObject
		line = gameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
		// Set the width of the Line Renderer
		//line.SetWidth(0.05F, 0.05F);
		// Set the number of vertex fo the Line Renderer
		//line.SetVertexCount(2);
		line.positionCount= lengthOfLineRenderer;

		//shot();
	}

	void Update(){

		if(Target ==null){
			Target=FindClosestEnemy();
		}

		if(Target !=null){
			float target_Distance = Vector3.Distance(transform.position, Target.position);
 
			// Calculate the velocity needed to throw the object to the target at specified angle.
			float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

	
			// Extract the X  Y componenent of the velocity
			float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
			float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
	
			// Calculate flight time.
			float flightDuration = target_Distance / Vx;

			//Debug.Log(flightDuration);
	
			// Rotate projectile to face the target.
			Quaternion rotation = Quaternion.LookRotation(Target.position - transform.position);

			transform.rotation= rotation;

			


			float a = (firingAngle) * Mathf.Deg2Rad;

			// rotation y
			float b = Mathf.Abs(rotation.eulerAngles.y-360-90)*Mathf.Deg2Rad;
			float v0=Mathf.Sqrt(projectile_Velocity);

			//Debug.Log(rotation.eulerAngles.y + " radians are equal to " + b + " degrees.");

			float tmp = (2*v0*Mathf.Sin(a)) /gravity;

			float ydiff = Target.position.y-transform.position.y;		


			for (int i = 0; i < lengthOfLineRenderer; i++)
			{
				
				float elapseDelta = (flightDuration/lengthOfLineRenderer);
				float elapse =  elapseDelta*i;
				//   transform.position.y+(Vy - (gravity * (elapseDelta*i))) * elapseDelta*i
				//line.SetPosition(i, new Vector3(Vx * elapse+transform.position.x,   transform.position.y+(  Vy*elapse+ (0.5f*(-gravity)*elapse*elapse))    ,    transform.position.z));
				
				float x= transform.position.x + elapse *  v0 *  Mathf.Cos(a) * Mathf.Cos(b);
				float y = transform.position.y + (  v0 * Mathf.Sin(a)*elapse+ (0.5f*(-gravity)*elapse*elapse))+  ydiff/(lengthOfLineRenderer-1)*i;
				float z = transform.position.z +elapse * v0 * Mathf.Cos(a)*Mathf.Sin(b) ;


				line.SetPosition(i, new Vector3(x,  y     ,  z  ));
			}	
		}

		
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
	
}
