using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.Utilities {
    /// <summary>
    /// Interface to implement for all classes that require drawing.
    /// </summary>
    public interface IDrawable {
        void draw();
    }

    public class DrawHandler : MonoBehaviour {
        public static List<object> itemsToDraw = new List<object>();

        void OnGUI() {
            foreach (IDrawable drawable in itemsToDraw) {
                drawable.draw();
            }
        }

        /// <summary>
        /// Returns a 2D Texture based off of a given color
        /// </summary>
        /// <param name="color">Desired HP Bar Color</param>
        /// <returns>Texture2D of desired color</returns>
        public static Texture2D getColor(Color color) {
            Texture2D simpleTexture = new Texture2D(1, 1, TextureFormat.ARGB32, true);
            simpleTexture.SetPixel(0, 1, color);
            simpleTexture.Apply();
            return simpleTexture;
        }
    }
}