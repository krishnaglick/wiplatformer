using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour
{
	//Holds the player
	public GameObject player;

	//Holds the player's sword
	public GameObject sword;

	//JumpHeight is how high you go per button press
	private float JumpHeight = 4;
	//GroundSpeed is the max speed the player can reach vertically
	private float GroundSpeed = 6;
	//This is how long it takes for bullets to die
	private float projectileDeathTime = 3;

	//This is how much you are knocked back when you hit an enemy
	private float knockbackX = 4;
	private float knockbackY = 3;

	//Players max hp
	private float maxHP = 100;
	//Players current hp
	private float curHP;
	//Players hp regen
	private float hpRegenRate = 5;

	//How much a melee attack swing does
	private float meleeDamage = 20;
	//How much damage a ranged attack does
	private float rangedDamage = 20;
	//Players limit on ranged attacks, max energy
	private float maxEnergy = 100;
	//Players limit on ranged attacks, max energy
	private float curEnergy;
	//Energy regen rate
	private float energyRegenRate = 10;

	//Bottom of the swing
	private float swingHeightMin = 0;
	//Top of the swing
	private float swingHeightMax = 20;
	//How far the swing extends
	private float swingDistance = 2;
	//How fast the swing is
	private float swingSpeed = 0.65f;
	//Controls the swing timer
	private bool canSwing = true;

	//Energy cost to use a ranged attack
	private float rangedCost = 50;
	//Energy cost to use a melee attack
	private float meleeCost = 0;

	//This controls a few things, but tracks if they player is facing left or right
	private float Facing = 10;

	//This controls how many jumps the player gets
	private int JumpMax = 2;
	//This tracks how many times the player has jumped.
	private int JumpCount = 0;

	//Switches between melee and ranged attacks
	private bool isMelee = false;
	//Keeps track of the controls
	private Dictionary<string, KeyCode> Controls = new Dictionary<string, KeyCode>();

	void Start()
	{
		//Set player to the player on the field. Will need changing for 2+ players
		player = GameObject.FindGameObjectWithTag("Player");

		//Set controls to buttons
		Controls.Add("Jump", KeyCode.UpArrow);
		Controls.Add("Left", KeyCode.LeftArrow);
		Controls.Add("Right", KeyCode.RightArrow);
		Controls.Add("Down", KeyCode.DownArrow);
		Controls.Add("Attack", KeyCode.Space);

		//Sets current hp and energy to max values
		curHP = maxHP;
		curEnergy = maxEnergy;

		//Starts the function that controls regen
		InvokeRepeating("regen", 0, 1f);

		//Sword spawning
		//sword = (GameObject)Instantiate(Resources.Load("Sword", typeof(GameObject)), player.transform.position, Quaternion.identity);
		//sword.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("BaseBlade", typeof(Sprite));
		//sword.GetComponent<MeleeWeapon>().Init(player, sword);
	}
	
	void Update()
	{
		//If jump is pressed and a jump is available
		if(Input.GetKeyDown(Controls["Jump"]) && JumpCount < JumpMax)
		{
			Jump();
		}

		//Move player left
		if(Input.GetKey(Controls["Left"]))
		{
			player.collider2D.rigidbody2D.AddForce(new Vector2(-10, 0), ForceMode2D.Force);
			Facing = -10;
		}

		//Move player right
		if(Input.GetKey(Controls["Right"]))
		{
			player.collider2D.rigidbody2D.AddForce(new Vector2(10, 0), ForceMode2D.Force);
			Facing = 10;
		}

		//Cycle weapons
		if(Input.GetKeyDown(Controls["Down"]))
			isMelee = !isMelee;

		//Attack!
		if(Input.GetKeyDown(Controls["Attack"]))
			Attack();

		//This makes sure the players ground velocity isn't above what it should be
		LimitMoveSpeed();
	}

	void OnGUI()
	{
		//Draw HP Bar
		GUI.DrawTexture(new Rect(0, 0, maxHP, 12), GetColor(Color.black));
		if(curHP > maxHP)
			GUI.DrawTexture(new Rect(0, 1, maxHP, 10), GetColor(Color.green));
		else
			GUI.DrawTexture(new Rect(0, 1, curHP/maxHP * 100, 10), GetColor(Color.green));
		//Draw Energy Bar
		GUI.DrawTexture(new Rect(120, 0, maxEnergy, 12), GetColor(Color.black));
		if(curEnergy > maxEnergy)
			GUI.DrawTexture(new Rect(120, 1, maxEnergy, 10), GetColor(Color.yellow));
		else
			GUI.DrawTexture(new Rect(120, 1, curEnergy/maxEnergy * 100, 10), GetColor(Color.yellow));
	}

	Texture2D GetColor(Color color)
	{
		//Returns the color you're looking for
		Texture2D simpleTexture = new Texture2D(1, 1, TextureFormat.ARGB32, true);
		simpleTexture .SetPixel(0, 1, color);
		simpleTexture.Apply();
		return simpleTexture;
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		//Reset jumps when you hit the ground
		if(col.gameObject.tag == "Ground")
			JumpCount = 0;
		//Player damage will probably be added to this, right now it just knocks you back
		if(col.gameObject.tag == "Enemy")
		{
			//If the enemy is to the right of the player knock left, otherwise knock right
			if(col.gameObject.transform.position.x > player.transform.position.x)
				player.collider2D.rigidbody2D.AddForce(new Vector2(-knockbackX, knockbackY), ForceMode2D.Impulse);
			else
				player.collider2D.rigidbody2D.AddForce(new Vector2(knockbackX, knockbackY), ForceMode2D.Impulse);
			//Will need to pull damage on an enemy by enemy basis
			//Store damage taken in enemy's AI component
			curHP -= 5;
		}
	}

	void Jump()
	{
		//Increment jump counter
		JumpCount++;
		//Set y-velocity to 0 so double jumps don't get weird
		player.collider2D.rigidbody2D.velocity = new Vector2(player.collider2D.rigidbody2D.velocity.x, 0);
		//Do the jump!
		player.collider2D.rigidbody2D.AddForce(new Vector2(0, JumpHeight), ForceMode2D.Impulse);
	}

	void LimitMoveSpeed()
	{
		//If the players ground speed is too high in left (negative) or right (positive) directions, limit it!
		if(player.collider2D.rigidbody2D.velocity.x > GroundSpeed)
			player.collider2D.rigidbody2D.velocity = new Vector2(GroundSpeed, player.collider2D.rigidbody2D.velocity.y);
		
		if(player.collider2D.rigidbody2D.velocity.x < (-GroundSpeed))
			player.collider2D.rigidbody2D.velocity = new Vector2((-GroundSpeed), player.collider2D.rigidbody2D.velocity.y);
	}

	void regen()
	{
		//If energy isn't maxed, add to it
		if(curEnergy < maxEnergy)
			curEnergy += energyRegenRate;
		//If hp isn't maxed, add to it
		if(curHP < maxHP)
			curHP += hpRegenRate;
	}

	void allowSwing()
	{
		//Allows for another melee swing. Maybe I should use IEnum crap here
		canSwing = true;
	}

	void Attack()
	{
		if(isMelee && canSwing)
		{
			//sword.GetComponent<MeleeWeapon>().Swing();
			//Make sure there's energy to do the attack
			if(curEnergy >= meleeCost)
			{
				canSwing = false;
				List<GameObject> hits = new List<GameObject>();
				//Decrease current energy by necessary amount
				for(float i = swingHeightMax; i >= swingHeightMin; i--)
				{
					//Spam raycasts till you find something
					RaycastHit2D hit = Physics2D.Raycast(player.transform.position, new Vector2(Facing, i), swingDistance, 1 << LayerMask.NameToLayer("Enemy"));
					if(hit.collider != null && hit.collider.gameObject.tag == "Enemy")
					{
						if(!hits.Contains(hit.collider.gameObject))
					    {
							//If it's an enemy and hasn't been hit before and exists
							//Do damage and add it to the list
							//Don't want to hit 1 enemy 20 times!!
							hits.Add(hit.collider.gameObject);
							hit.collider.gameObject.GetComponent<EnemyAI>().curHP -= meleeDamage;
							curEnergy -= meleeCost;
							//All this crap will be replaced with bullshit animation hit box shit later
						}
					}
				}
				//After a swing allow another swing!
				Invoke("allowSwing", swingSpeed);
			}
		}
		else if(!isMelee)
		{
			//Make sure there's energy to do the attack
			if(curEnergy >= rangedCost)
			{
				//Decrease current energy by necessary amount
				curEnergy -= rangedCost;
				//Create a new projectile
				GameObject shot = (GameObject)Instantiate(Resources.Load("Shot", typeof(GameObject)), player.transform.position, Quaternion.identity);
				//Shoot it in the correct direction using impulse which is instant
				shot.rigidbody2D.AddForce(new Vector2(Facing, 0), ForceMode2D.Impulse);
				//Set a destroy timer, don't want it running off into space forever and taking up memory
				Destroy(shot, projectileDeathTime);
				//Set the damage
				shot.GetComponent<ProjectileDamage>().dmg = rangedDamage;
				//Apply a stun
				shot.GetComponent<ProjectileDamage>().fx.Stun(3, "");
				//Apply a burn
				shot.GetComponent<ProjectileDamage>().fx.Burn(5, 2, 4, "");
			}
		}
	}
}