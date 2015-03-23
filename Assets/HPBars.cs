using UnityEngine;
using System.Collections;
using Game.Utilities;

public class HPBar : IDrawable {
    private GameObject targetCreature;
    private float hpPercent;
    private Texture2D hpBarColor;
    private Texture2D hpBarBackgroundColor;

    /// <summary>
    /// Updates the current HP percent.
    /// </summary>
    /// <param name="newHPPercent"></param>
    public void updateCreatureHP(float newHPPercent) {
        hpPercent = newHPPercent;
    }

    /// <summary>
    /// Updates the current hp percent and color.
    /// </summary>
    /// <param name="newHPPercent"></param>
    /// <param name="hpBarColor"></param>
    public void updateCreatureHP(float newHPPercent, Color hpBarColor) {
        hpPercent = newHPPercent;
        this.hpBarColor = DrawHandler.getColor(hpBarColor);
    }

    /// <summary>
    /// Creates the HP Bar Object
    /// </summary>
    /// <param name="targetCreature"></param>
    /// <param name="hpPercent"></param>
    /// <param name="hpBarColor"></param>
    public HPBar(GameObject targetCreature, float hpPercent, Color hpBarColor, object hpBarBackgroundColor = null) {
        this.targetCreature = targetCreature;
        this.hpPercent = hpPercent;
        this.hpBarColor = DrawHandler.getColor(hpBarColor);
        //Look at this dumb shit.
        if (hpBarBackgroundColor == null) {
            this.hpBarBackgroundColor = DrawHandler.getColor(Color.black);
        }
        else {
            this.hpBarBackgroundColor = DrawHandler.getColor((Color)hpBarBackgroundColor);
        }

        DrawHandler.itemsToDraw.Add(this);
    }

    /// <summary>
    /// Draws the HP bars background.
    /// </summary>
    /// <param name="xPosition"></param>
    /// <param name="yPosition"></param>
    /// <param name="barHeight"></param>
    private void drawHPBarBackground(float xPosition, float yPosition, float barHeight) {
        GUI.DrawTexture(new Rect(xPosition, yPosition, 100, barHeight), hpBarBackgroundColor);
    }

    /// <summary>
    /// Draws the HP bars foreground.
    /// </summary>
    /// <param name="xPosition"></param>
    /// <param name="yPosition"></param>
    /// <param name="barHeight"></param>
    private void drawHPBarForeground(float xPosition, float yPosition, float barHeight) {
        GUI.DrawTexture(new Rect(xPosition, yPosition, hpPercent*100, barHeight), hpBarColor);
    }

    /// <summary>
    /// For the draw calls.
    /// </summary>
    public void draw() {
        Vector3 hpBarDrawPosition = Camera.main.WorldToScreenPoint(targetCreature.transform.position);
        float hpBarXPosition = hpBarDrawPosition.x - 50;
        float hpBarYPosition = Camera.main.pixelHeight - hpBarDrawPosition.y - (Camera.main.pixelHeight / 8);
        float barHeight = targetCreature.GetComponent<Renderer>().bounds.size.y * 6;

        drawHPBarBackground(hpBarXPosition, hpBarYPosition, barHeight);
        drawHPBarForeground(hpBarXPosition, hpBarYPosition, barHeight);
    }

    /// <summary>
    /// Stops this object from being drawn.
    /// </summary>
    public void Destroy() {
        DrawHandler.itemsToDraw.Remove(this);
    }
}