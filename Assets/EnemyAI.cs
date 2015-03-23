using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public GameObject self;
    public HPBar creatureHPBar;
    public bool runAI = true;
	
    public float maxHP = 100;
    private float _curHP = 100;
	public float curHP
	{
		get
		{
			return _curHP;
		}
		set
		{
			_curHP = value;
            creatureHPBar.updateCreatureHP(curHP / maxHP, getHPColor());
		}
	}
	
	void Start()
	{
        creatureHPBar = new HPBar(self, curHP/maxHP, Color.green);

		//Enemy HP Regen
		InvokeRepeating("regen", 0, 1f);
	}
	
	void Update()
	{
		//Kill yo self
        if (curHP <= 0) {
            creatureHPBar.Destroy();
            Destroy(self);
        }
		//Don't do move things if computer has the dumb
		if(!runAI)
			return;
		//My "AI", basically spaz back and forth based on randomness.
		if(new System.Random().Next(2) == 0)
			self.GetComponent<Collider2D>().GetComponent<Rigidbody2D>().AddForce(new Vector2(-20, 0), ForceMode2D.Force);
		else
			self.GetComponent<Collider2D>().GetComponent<Rigidbody2D>().AddForce(new Vector2(20, 0), ForceMode2D.Force);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//Damage thing is now in the projectile class
	}
	

	void regen()
	{
		//Heal some!
        if (curHP < maxHP) {
            curHP += 2.5f;
        }
        else if(curHP >= maxHP) {
            curHP = maxHP;
        }
	}

	Color getHPColor()
	{
		//Pick what color the hp bar should be! <3
		//Handled here, not in hp bars class!
		float percent = curHP / maxHP;
		Color c = Color.green;
		if(percent >= .5)
			c = Color.green;
		if(percent < .5)
			c = Color.yellow;
		if(percent < .3)
			c = new Color(1f, 0.6f, 0);
		if(percent < .15)
			c = Color.red;
		return c;
	}
}