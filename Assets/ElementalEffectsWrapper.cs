using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementalEffectsWrapper
{
	//A list of the elemental effects to apply
	private List<ElementalFxStorage> EleFx = new List<ElementalFxStorage>();

	//Nested class, data structure to hold elemental fx data
	private class ElementalFxStorage
	{
		//How much damage is done on hit, negative for healing
		public float Damage;
		//How often the effect ticks for dots
		public float Interval;
		//How long the effect lasts
		public float Duration;
		//This string is a placeholder for now, but will hold the visuals at some point.
		public string Effect;

		public ElementalFxStorage(float dmg, float intv, float dura, string fx)
		{
			//Constructor for damage
			Damage = dmg;
			Interval = intv;
			Duration = dura;
			Effect = fx;
		}

		public ElementalFxStorage(float dura, string fx)
		{
			//Constructor for stuns
			Duration = dura;
			Effect = fx;
		}
	}

	public void ApplyEffects(GameObject target)
	{
		//Iterates through each effect and applies it to the target
		foreach(ElementalFxStorage c in EleFx)
		{
			//This needs some work as floats default to 0
			//Basically if there's no damage listed the code assumes it's a stun
			//otherwise it's a dot or damagey thing
			if(c.Damage == 0)
				target.AddComponent<ElementalEffect>().init(target, c.Duration, c.Effect);
			else if(c.Damage != 0)
				target.AddComponent<ElementalEffect>().init(target, c.Damage, c.Interval, c.Duration, c.Effect);
		}
	}
	public void Frost(float dur, string fx)
	{
		//The reason Frost is split from Stun is that I need to work out a way to slow things
		EleFx.Add(new ElementalFxStorage(dur, fx));
	}
	public void Stun(float dur, string fx)
	{
		//Stun, ez
		EleFx.Add(new ElementalFxStorage(dur, fx));
	}
	public void Burn(float dmg, float interval, float dur, string fx)
	{
		//Damage w/ fire effect eventually
		EleFx.Add(new ElementalFxStorage(dmg, interval, dur, fx));
	}
	public void Lightning(float dmg, float interval, float dur, string fx)
	{
		//Same as above but zappy! Might add stun funcionality as well
		EleFx.Add(new ElementalFxStorage(dmg, interval, dur, fx));
	}
	public void HOT(float dmg, float interval, float dur, string fx)
	{
		//Heal over time, dmg should be negative
		EleFx.Add(new ElementalFxStorage(dmg, interval, dur, fx));
	}
	public void Heal(float dmg, float interval, float dur, string fx)
	{
		//Big heal hit, damage should be negative
		EleFx.Add(new ElementalFxStorage(dmg, interval, dur, fx));
	}
}
