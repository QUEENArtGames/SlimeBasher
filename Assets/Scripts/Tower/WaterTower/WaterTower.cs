using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts{

public class WaterTower : MonoBehaviour {

	private LineRenderer [] line;                           // Line Renderer
	public int lengthOfLineRenderer = 20;

	public Transform Target;
	public GameObject Enemy;

	public Transform waterpoint;

	public float firingAngle = 45.0f;
    public float gravity = 9.8f;

	private Vector3 targetPoint;
     private Quaternion targetRotation;

 
	//public GameObject projec;

	public Vector3 raycastDir;

	GameObject bullet;

	public float _damage=10;

	public float delay = 0.05f;
	private bool build= false;
	private bool destroy= true;

        private TowerSounds towersounds;

	public float range=10;
    
	 List<Vector3> paths = new List<Vector3>();

    // needed to only rotate the wateringCan while shooting
    public Transform rotationReset;

    // interpolate rotation for wateringCan
    public float speed = 45.0F;

    // Use this for initialization
    void Start () {
            // Add a Line Renderer to the GameObject
            towersounds = GetComponent<TowerSounds>();
		line = gameObject.GetComponentsInChildren<LineRenderer>();

		//line = gameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
		// Set the width of the Line Renderer
		//line.SetWidth(0.05F, 0.05F);
		// Set the number of vertex fo the Line Renderer
		//line.SetVertexCount(2);
		//line.positionCount= lengthOfLineRenderer;
		line[0].positionCount=0;
		paths.Add(new Vector3(0,  0     ,  0  ));
		//shot();

	}

	void Update(){
        if (Enemy ==null && !build){
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
			
			StartCoroutine(DestroyWaterBeam());
			destroy=true;
			//Debug.Log("Destroy");
		}
		
	}

	void Beam(){
		//Debug.Log(paths.Count-1);
		//Debug.Log(line.positionCount);
		for (int i = 0; i < paths.Count-1; i++){

				//paths[i]=new Vector3(x,  y     ,  z  );
				line[0].SetPosition(i,paths[i]);
				//yield return new WaitForSeconds(delay);

		}
	}

	void BuildWaterBeam(){
            if (Target != null)
            {
                
                float target_Distance = Vector3.Distance(waterpoint.position, Target.position);
                //Debug.Log(target_Distance);
                if (target_Distance > range)
                {
                    Enemy = null;
                    return;
                }
                if (Enemy != null)
                {
                    
                    HitEnemyWater();
                }

                // Calculate the velocity needed to throw the object to the target at specified angle.
                float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);


                // Extract the X  Y componenent of the velocity
                float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
                float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

                // Calculate flight time.
                float flightDuration = target_Distance / Vx;

                //Debug.Log(flightDuration);



                //Rotate wateringCan to face the target.
                Quaternion targetRot = Quaternion.LookRotation(Target.position - rotationReset.position);

                //wateringCan.transform.LookAt(Target);
                Vector3 eulerAngles = targetRot.eulerAngles;
                eulerAngles.x = 0;
                //eulerAngles.y = eulerAngles.y + 90;
                eulerAngles.z = 0;

                targetRot = Quaternion.Euler(eulerAngles);

                // Set the altered rotation back
                Quaternion rotationSooth = Quaternion.RotateTowards(rotationReset.transform.rotation, targetRot, Time.deltaTime * speed);
                //wateringCan.transform.rotation = rotationSooth.eulerAngles;
                rotationReset.transform.rotation = rotationSooth;

                float a = (firingAngle) * Mathf.Deg2Rad;
                // Rotate projectile to face the target.
                Quaternion rotation = rotationSooth;
                // rotation y
                float b = Mathf.Abs(rotation.eulerAngles.y - 360 - 90) * Mathf.Deg2Rad;
                float v0 = Mathf.Sqrt(projectile_Velocity);

                //Debug.Log(rotation.eulerAngles.y + " radians are equal to " + b + " degrees.");

                float tmp = (2 * v0 * Mathf.Sin(a)) / gravity;

                float ydiff = Target.position.y - waterpoint.position.y;

                for (int i = 0; i < paths.Count - 1; i++)
                {
                    float elapseDelta = (flightDuration / lengthOfLineRenderer);
                    float elapse = elapseDelta * i;
                    //   transform.position.y+(Vy - (gravity * (elapseDelta*i))) * elapseDelta*i
                    //line.SetPosition(i, new Vector3(Vx * elapse+transform.position.x,   transform.position.y+(  Vy*elapse+ (0.5f*(-gravity)*elapse*elapse))    ,    transform.position.z));

                    float x = waterpoint.position.x + elapse * v0 * Mathf.Cos(a) * Mathf.Cos(b);
                    float y = waterpoint.position.y + (v0 * Mathf.Sin(a) * elapse + (0.5f * (-gravity) * elapse * elapse)) + ydiff / (lengthOfLineRenderer - 1) * i;
                    float z = waterpoint.position.z + elapse * v0 * Mathf.Cos(a) * Mathf.Sin(b);

                    paths[i] = new Vector3(x, y, z);
                    Beam();
                    //line.SetPosition(i,paths[i]);
                    //yield return new WaitForSeconds(delay);

                }
            }
			
	}

	IEnumerator AddPoints(){
		for(int i = 0; i < lengthOfLineRenderer; i++)
		{

			line[0].positionCount+=1;
			paths.Add(new Vector3(0,  0     ,  0  ));
			BuildWaterBeam();
			//Beam();
			//line.SetVertexCount(i);


			yield return new WaitForSeconds(delay);
		}
		//build=false;
	}

	IEnumerator DestroyWaterBeam(){
		for(int i = lengthOfLineRenderer - 1; i >= 0; i--)
        {
			Beam();
			paths.RemoveAt(0);
			line[0].positionCount-=1;
			
			
            yield return new WaitForSeconds(delay);
        }
		Debug.Log("Destroy");
		build=false;

            towersounds.ContinousSource.Stop();
	}


	void HitEnemyWater(){
		Enemy.GetComponent<SlimeScript>().TakeDamage(_damage*Time.deltaTime);
            if (!towersounds.ContinousSource.isPlaying && Target != null)
                towersounds.ContinousSource.Play();
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
}
