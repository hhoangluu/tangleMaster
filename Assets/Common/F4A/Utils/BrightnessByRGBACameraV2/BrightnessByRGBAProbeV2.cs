using UnityEngine;
using System.Collections;

/* For use with BrightnessByRGBACamera
 * 
 * Adds this script to the global probe list
 * 
 * Feel free to use in any project
 */

// V2
public class BrightnessByRGBAProbeV2 : MonoBehaviour {
	public Color surfaceColor;
	public float brightness1; // http://stackoverflow.com/questions/596216/formula-to-determine-brightness-of-rgb-color
	public float brightness2; // http://www.nbdtech.com/Blog/archive/2008/04/27/Calculating-the-Perceived-Brightness-of-a-Color.aspx

	public void applyColor( Color tColor) {
		surfaceColor = tColor;
		brightness1 = (tColor.r + tColor.r + tColor.b + tColor.g + tColor.g + tColor.g) / 6; // BRIGHTNESS APPROX
		brightness2 = Mathf.Sqrt((tColor.r * tColor.r * 0.2126f + tColor.g * tColor.g * 0.7152f + tColor.b * tColor.b * 0.0722f)); // BRIGHTNESS
	}

	public void removeColor() {
		surfaceColor = Color.black;
		brightness1 = 0f;
		brightness2 = 0f;
	}

	void Start() {
		BrightnessByRGBACameraV2.addProbe( this);
	}
}
