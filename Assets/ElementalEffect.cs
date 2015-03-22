using UnityEngine;
using System.Collections;

public class ElementalEffect : MonoBehaviour
{
	//The target for the effect to apply to
	private GameObject target;
	//How much damage is done each tick
	private float periodicDmg;
	//How often to tick
	private float periodicTick;
	//How long to tick
	private float duration;

	public void init(GameObject go, float dmg, float interval, float dur, string fx)
	{
		//Constructor one, for damagey things
		periodicDmg = dmg;
		periodicTick = interval;
		duration = dur;
		target = go;
		//This handles killing off the dot in the right time
		StartCoroutine(beginDestroy());
		//This handles doing things every tick seconds
		StartCoroutine(tick());
	}

	public void init(GameObject go, float dur, string fx)
	{
		//Constructor two, for stunny things
		target = go;
		duration = dur;
		//Starts a timer to turn back on enemy AI
		StartCoroutine(endFreeze());
	}

	IEnumerator tick()
	{
		//IEnum's are cool and in line with Unity's standards apparently
		//Better than doing an InvokeRepeating apparently
		while(true)
		{
			//This is a wait
			yield return new WaitForSeconds(periodicTick);
			//Nerf down enemy hp
			target.GetComponent<EnemyAI>().curHP -= periodicDmg;
		}
	}
	
	IEnumerator beginDestroy()
	{
		//This is a wait
		yield return new WaitForSeconds(duration);
		//Kills this elemental effect
		Destroy(this);
	}

	IEnumerator endFreeze()
	{
		//Give computer the dumb
		target.GetComponent<EnemyAI>().runAI = false;
		//This is a wait
		yield return new WaitForSeconds(duration);
		//Take back the dumb
		target.GetComponent<EnemyAI>().runAI = true;
		//Kills this elemental effect
		Destroy(this);
	}
}
