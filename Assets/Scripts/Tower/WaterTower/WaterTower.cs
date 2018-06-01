using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTower : MonoBehaviour {

	private LineRenderer line;                           // Line Renderer
	public int lengthOfLineRenderer = 20;

	public Transform Target;
	public GameObject Enemy;

	public float firingAngle = 45.0f;
    public float gravity = 9.8f;

	private Vector3 targetPoint;
     private Quaternion targetRotation;

 
	public GameObject projec;

	public Vector3 raycastDir;

	GameObject bullet;

	public float _damage=10;

	public float delay = 0.05f;
	private bool build= false;
	private bool destroy= true;

	public float range=10;




	// Use this for initialization
	void Start () {
		// Add a Line Renderer to the GameObject
		line = GetComponent<LineRenderer>();

		//line = gameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
		// Set the width of the Line Renderer
		//line.SetWidth(0.05F, 0.05F);
		// Set the number of vertex fo the Line Renderer
		//line.SetVertexCount(2);
		//line.positionCount= lengthOfLineRenderer;

		//shot();
	}

	void Update(){

		if(Enemy ==null && !build){
			Enemy=FindClosestEnemy();
		}
		
		if(Enemy !=null && !build){
			Target= Enemy.transform;
			
			StartCoroutine(AddPoints());
			//Debug.Log("Buld");
			destroy=false;
			build=true;
		}else if(Enemy !=null){
			BuildWaterBeam();
		}else if(build && !destroy ){
			destroy=true;
			StartCoroutine(DestroyWaterBeam());
			//Debug.Log("Destroy");
		}

		
	}

	void BuildWaterBeam(){
			float target_Distance = Vector3.Distance(transform.position, Target.position);
			//Debug.Log(target_Distance);
			if(target_Distance >range){
				Enemy=null;
				return;
			}
			HitEnemy();



 
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


			for (int i = 0; i < line.positionCount; i++)
			{
				float elapseDelta = (flightDuration/lengthOfLineRenderer);
				float elapse =  elapseDelta*i;
				//   transform.position.y+(Vy - (gravity * (elapseDelta*i))) * elapseDelta*i
				//line.SetPosition(i, new Vector3(Vx * elapse+transform.position.x,   transform.position.y+(  Vy*elapse+ (0.5f*(-gravity)*elapse*elapse))    ,    transform.position.z));
				
				float x= transform.position.x + elapse *  v0 *  Mathf.Cos(a) * Mathf.Cos(b);
				float y = transform.position.y + (  v0 * Mathf.Sin(a)*elapse+ (0.5f*(-gravity)*elapse*elapse))+  ydiff/(lengthOfLineRenderer-1)*i;
				float z = transform.position.z +elapse * v0 * Mathf.Cos(a)*Mathf.Sin(b) ;


				line.SetPosition(i, new Vector3(x,  y     ,  z  ));
				//yield return new WaitForSeconds(delay);

			}
	}

	IEnumerator AddPoints(){
		for(int i = 0; i < lengthOfLineRenderer; i++)
		{
			line.SetVertexCount(i);
			BuildWaterBeam();

			yield return new WaitForSeconds(delay);
		}
		//build=false;
	}

	IEnumerator DestroyWaterBeam(){
		for(int i = lengthOfLineRenderer - 1; i > 0; i--)
        {
            line.SetVertexCount(i);

            yield return new WaitForSeconds(delay);
        }
		build=false;
	}


	void HitEnemy(){
		Enemy.GetComponent<SlimeScript>().hit(_damage*Time.deltaTime);
	}

	public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
	
}
