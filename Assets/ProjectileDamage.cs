using UnityEngine;
using System.Collections;

public class ProjectileDamage : MonoBehaviour
{
	//Damage gets assigned, oh happy day.
	public float dmg = 0;
	//Grabs the projectile for later destruction
	public GameObject projectile;
	//Stores all the effects!
	//Might be better to not invoke here and instead do when the object is created AND if fx are wanted.
	public ElementalEffectsWrapper fx = new ElementalEffectsWrapper();

	void OnTriggerEnter2D(Collider2D col)
	{
		//Destroy if what it hits isn't a player
		if(col.gameObject.tag != "Player" && col.gameObject.tag != "Weapon")
		{
			//Apply all assigned effects.
			//Effects are assigned in the place the projectile is created
			fx.ApplyEffects(col.gameObject);
			//Hurt the enemy
			col.gameObject.GetComponent<EnemyAI>().curHP -= dmg;
			//Kill this projectile cause penetration is bad
			Destroy(projectile, 0.01f);
		}
	}
}