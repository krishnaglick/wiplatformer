using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public GameObject self;

	public float maxHP = 100;

	public bool runAI = true;

	private float _curHP = 100;
	//So this bullshit is a properly
	//It's really cool
	//I get to return _curHP which is private when you call it
	//But if I change the fucker it'll alter the value PLUS update the hp bar
	public float curHP
	{
		get
		{
			return _curHP;
		}
		set
		{
			_curHP = value;
			//Dank shit right here
			self.GetComponent<HPBars>().updateHP(curHP, maxHP, getHPColor());
		}
	}
	
	void Start()
	{
		//Add an hp bar to this, 100 hp
		self.AddComponent<HPBars>().Init(self, curHP, maxHP, Color.green);

		//Enemy HP Regen
		InvokeRepeating("regen", 0, 1f);
	}
	
	void Update()
	{
		//Kill yo self
		if(curHP <= 0)
			Destroy(self);
		//Don't allow overheals, might allow for hp bar wrapping but NOT YET CAUSE MATH?
		if(_curHP > maxHP)
			_curHP = maxHP;
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
		curHP += 2.5f;
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