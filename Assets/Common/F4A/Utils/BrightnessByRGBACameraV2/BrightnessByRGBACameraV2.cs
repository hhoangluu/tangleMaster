using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* For use with BrightnessByRGBAProbe
 * 
 * Takes a screen shot of regions around the probes,
 * and assigns values to the probes
 * 
 * Feel free to use in any project
 */

// V2.1
public class BrightnessByRGBACameraV2 : MonoBehaviour {
	public int snapshotDelay = 10; // How many loops to skip before running the screenshot coroutine
	public int spaceSize = 5; // Pixel border to ensure the region screenshot is large enough when moving
	
	static List<BrightnessByRGBAProbeV2> pList = new List<BrightnessByRGBAProbeV2>(); // List of active probes
	static List<SnapshotRegion> SSList = new List<SnapshotRegion>(); // List of regions on the screen to track
	Camera mainCam; // Reduce calls for efficiency, if the camera ever changes you may need to update this script
	bool loading = false; // Is the coroutine running
	int loopCount = 0; // How many FixedUpdates have been skipped

	// Finds/creates a SnapshotRegion for each gameobject bring tracked
	// Adds the SnapshotRegion and the probe to a list
	public static void addProbe( BrightnessByRGBAProbeV2 aProbe) {
		if ( !pList.Contains( aProbe)) {
			pList.Add( aProbe); // Add this probe
			SnapshotRegion nRegion = null;
			foreach ( SnapshotRegion tRegion in SSList) { 
				if ( tRegion.myGameObject == aProbe.gameObject.transform.parent.gameObject) { // The parent game object
					nRegion = tRegion;
					tRegion.includePoint( aProbe, aProbe.transform.localPosition);
					break;
				}
			}
			if ( nRegion == null) { // No region was found in the list for this object
				SnapshotRegion tRegion = new SnapshotRegion( aProbe.gameObject.transform.parent.gameObject);
				SSList.Add( tRegion); // Add the new region
				tRegion.includePoint( aProbe, aProbe.transform.localPosition);
			}
		}
	}

	// Clear the current list
	public static void clearProbes() {
		pList.Clear();
		SSList.Clear();
	}

	// Find all loaded probes
	public static void findProbes() {
		BrightnessByRGBAProbeV2[] tProbes = GameObject.FindObjectsOfType<BrightnessByRGBAProbeV2>();
		foreach (BrightnessByRGBAProbeV2 aProbe in tProbes) {
			addProbe( aProbe);
		}
	}

	/*
	// Testing GUI
	void OnGUI() {
		GUI.Label(new Rect(10, 40, 50, 30), pList.Count + " / " + SSList.Count);
		if (GUI.Button(new Rect(10, 70, 50, 30), "Clear")) {
			clearProbes();
		}
		if (GUI.Button(new Rect(10, 100, 50, 30), "Find")) {
			findProbes();
		}
	}
	*/

	void Start() {
		if (mainCam == null) {
			mainCam = Camera.main; // reduce calls
		}
	}

	// Periodically run the coroutine to generate a screenshot
	void FixedUpdate() {
		if ( !loading) {
			if ( loopCount++ > snapshotDelay) {
				loopCount = 0;
				StartCoroutine( "snapshotCurrentScreen");
			}
		}
	}

	// Copy the current screen into a texture
	IEnumerator snapshotCurrentScreen() {
		loading = true; // don't run multiple instances
		yield return new WaitForEndOfFrame(); // wait for this frame to finish being drawn

		Texture2D tex = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false);
		//tex.filterMode = FilterMode.Point; // #TODO Changing filter mode might help
		for ( int i = 0; i < SSList.Count; i ++) { // For each Snapshot Region
			Vector3 tVect = mainCam.WorldToScreenPoint( SSList[ i].myGameObject.transform.position + SSList[ i].myOrigin); // Bottom Left
			Vector3 tVect2 = mainCam.WorldToScreenPoint( SSList[ i].myGameObject.transform.position + SSList[ i].myLength); // Top Right
			Vector3 finalOrigin = new Vector3( (int) tVect.x - spaceSize, (int) tVect.y - spaceSize, (int) tVect.z - spaceSize); // With Pixel Spacer
			float rwidth = (tVect2.x - finalOrigin.x) + spaceSize; // Pixel Width with Spacer
			float rheight = (tVect2.y - finalOrigin.y) + spaceSize; // Pixel Height with Spacer

			if ( finalOrigin.x > Screen.width) { continue;} // Too far off screen
			if ( finalOrigin.y > Screen.height) { continue;} // Too far off screen
			if ( finalOrigin.x + rwidth < 0) { continue;} // Too far off screen
			if ( finalOrigin.y + rheight < 0) { continue;} // Too far off screen

			if ( finalOrigin.x < 0) { finalOrigin.x = 0;} // Right Visible
			if ( finalOrigin.y < 0) { finalOrigin.y = 0;} // Top Visible
			if ( (int) finalOrigin.x + rwidth > Screen.width) { rwidth = Screen.width - (int) finalOrigin.x;} // Left Visible
			if ( (int) finalOrigin.y + rheight > Screen.height) { rheight = Screen.height - (int) finalOrigin.y;} // Bottom Visible

			if ( rwidth < 1 || rheight < 1) { continue;} // Can't see this region
			Color[] cols = null;

			tex.ReadPixels( new Rect( (int) finalOrigin.x, (int) finalOrigin.y, (int) rwidth, (int) rheight), (int) finalOrigin.x, (int) finalOrigin.y); // Read region contents into the texture
			cols = tex.GetPixels( (int) finalOrigin.x, (int) finalOrigin.y, (int) rwidth, (int) rheight); // Copy approximate pixel colors from texture

			foreach ( BrightnessByRGBAProbeV2 tProbe in SSList[ i].myProbeList) { // For each probe inside this region
				Vector3 bVect = mainCam.WorldToScreenPoint( tProbe.gameObject.transform.position) - finalOrigin; // find the probe's position on the texture
				if ( bVect.x > 0f && bVect.y > 0f && bVect.x < rwidth && bVect.y < rheight) { // if the probe is inside the texture
					tProbe.applyColor( cols[ ( (int) bVect.x) + ( ( (int) bVect.y) * ( (int) rwidth))]);
				} else { // the probe is off screen
					tProbe.removeColor();
				}
			}
			yield return new WaitForEndOfFrame(); // #TODO Comment this line for higher accuracy at the cost of speed
		}
		loading = false;
	}
}

// This class tracks a point in space relative to the main game object
public class SnapshotRegion {
	public GameObject myGameObject;
	public Vector3 myOrigin;
	public Vector3 myLength;
	public List<BrightnessByRGBAProbeV2> myProbeList = new List<BrightnessByRGBAProbeV2>();
	
	public SnapshotRegion( GameObject tParent) {
		myGameObject = tParent;
	}

	// Expand to include tPoint
	public void includePoint( BrightnessByRGBAProbeV2 aProbe, Vector3 tPoint) {
		if ( !myProbeList.Contains( aProbe)) {
			myProbeList.Add( aProbe);
			if ( tPoint.x < myOrigin.x) { myOrigin.x = tPoint.x;}
			if ( tPoint.y < myOrigin.y) { myOrigin.y = tPoint.y;}
			if ( tPoint.z < myOrigin.z) { myOrigin.z = tPoint.z;}
			if ( tPoint.x > myLength.x) { myLength.x = tPoint.x;}
			if ( tPoint.y > myLength.y) { myLength.y = tPoint.y;}
			if ( tPoint.z > myLength.z) { myLength.z = tPoint.z;}
		}
	}
	
	public override string ToString () {
		return string.Format ("[ {0}, {1}] [ {2}, {3}]", myOrigin.x, myOrigin.y, myLength.x, myLength.y);
	}
}