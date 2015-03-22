using UnityEngine;
using System.Collections;

public class HPBars : MonoBehaviour {

	//The Gameobject to appear above
	private GameObject target;

	//Foreground color, aka the health color
	private Texture2D Foreground;
	//Background color, aka what you see when you've lost hp
	private Texture2D Background;

	//Max HP
	private float MaxHP;
	//Current HP
	private float CurrentHP;

	public void updateHP(float hpCur, float hpMax, Color c)
	{
		//Sets the max hp, current hp, and foreground color.
		MaxHP = hpMax;
		CurrentHP = hpCur;
		Foreground = GetColor(c);
	}
	

	public void Init(GameObject go, float hpCur, float hpMax, Color c)
	{
		//Set max hp to passed in value
		MaxHP = hpMax;
		//Setting target to go to keep track of the attachee
		target = go;
		//Sets the current hp to max hp, at spawn the mob should be max hp
		CurrentHP = hpCur;
		//Sets the default foreground and background colors
		Foreground = GetColor(c);
		Background = GetColor(Color.black);
	}

	void OnGUI()
	{
		//This keeps track of the mob relative to the screen
		Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
		//Gets the x position of the mob and centers the hp bar
		float xPosition = screenPos.x - (MaxHP / 2);
		//Gets the mob's y position relative to the screen with an offset that won't screw up on multiple screen sizes
		float yPosition = Camera.main.pixelHeight - screenPos.y - (Camera.main.pixelHeight / 8);
		//Scales the bar's height based on the size of the enemy
		float barHeight = target.renderer.bounds.size.y * 6;
		//Draws the hp and background bars
		GUI.DrawTexture(new Rect(xPosition, yPosition, MaxHP, barHeight), Background);
		//If the current hp is above the max hp, don't fuck up the hp bar amg
		if(CurrentHP > MaxHP)
			GUI.DrawTexture(new Rect(xPosition, yPosition, MaxHP, barHeight), Foreground);
		else
			GUI.DrawTexture(new Rect(xPosition, yPosition, CurrentHP, barHeight), Foreground);
	}

	Texture2D GetColor(Color color)
	{
		//Returns the color you're looking for
		Texture2D simpleTexture = new Texture2D(1, 1, TextureFormat.ARGB32, true);
		simpleTexture .SetPixel(0, 1, color);
		simpleTexture.Apply();
		return simpleTexture;
	}
}