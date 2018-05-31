using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSlime : MonoBehaviour {

	// Use this for initialization
	public float hp;
	public float movementSpeed;
	public float finalDamage;
	public float bubbleProjectDamage;

	private float maxHp;

	private GasSlimeTransformScript[] transformEffects;

	void Start () {
		maxHp = hp;

		transformEffects = this.GetComponents<GasSlimeTransformScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision col)
	{
		Debug.Log("Collision: "+col.gameObject.name);
		if(col.gameObject.name == "bubbleProjectile")
		{
			
			hp -= bubbleProjectDamage;
			foreach(GasSlimeTransformScript transformEffect in transformEffects){
				transformEffect.setHpPercent ((100/maxHp) * hp);
			}

			if(hp <= 0){
				Destroy(this.gameObject);
			}

		}
	}
}
