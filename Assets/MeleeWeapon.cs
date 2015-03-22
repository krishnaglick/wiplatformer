using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour
{
	//Who owns the blade
	public GameObject owner;
	//The Blade
	public GameObject Sword;
	//Weapon damage
	public float damage = 0;
	//Bottom of the swing
	public float swingHeightMin = 0;
	//Top of the swing
	public float swingHeightMax = 180;
	//How far the swing extends
	public float swingDistance = 2;
	//How fast the swing is
	public float swingSpeed = 0.65f;
	//Prevents double swinging, and reversion to default state
	public bool isSwinging = false;

	//Hit left or right
	public float Facing = 10;
	
	void Start()
	{
	
	}

	public void Init(GameObject attachee, GameObject self)
	{
		owner = attachee;
		Sword = self;
	}
	
	void Update()
	{
		if(owner != null && !isSwinging)
			Sword.transform.position = owner.transform.position;
	}

	public void Swing()
	{
		//Sword.transform.localRotation = Quaternion.FromToRotation(new Vector2(Sword.transform.position.x, swingHeightMax), new Vector2(Sword.transform.position.x, swingHeightMin));
		isSwinging = true;
		for(float i = swingHeightMax; i >= swingHeightMin; i--)
		{
			Sword.transform.position = new Vector2(owner.transform.position.x + 50, i);
		}
		Invoke ("blah", 3);
	}
	void blah()
	{
		isSwinging = false;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//if(col.gameObject.tag == "Enemy")
			//col.gameObject.GetComponent<HPBars>().updateHP(damage, false);
	}
}
