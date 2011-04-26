using UnityEngine;
using System.Collections;
using MyLibMono;
using xn;
using xnv;
using System;

// Must be attached to a camera

public class DrawGL : MonoBehaviour
{
	public float distance = 1.0f;
	public Material onMaterial;
	public Material offMaterial;

	private MyLib myLib;
	private OpenNI2 openNI;
	private DepthGenerator depthGenerator;
	private Transform[] boxTriggers;
	private AudioSource[] audioSources;
	private TriggerCell[] triggerCells;
	
	void Start ()
	{
		myLib = new MyLib ();
		openNI = GameObject.FindGameObjectWithTag ("OpenNI").GetComponent (typeof(OpenNI2)) as OpenNI2;
		depthGenerator = openNI.depth;
		
		// Find all triggers
		GameObject triggers = GameObject.FindGameObjectWithTag("Triggers") as GameObject;
		boxTriggers = new Transform[triggers.transform.GetChildCount()];
		audioSources = new AudioSource[triggers.transform.GetChildCount()];
		triggerCells = new TriggerCell[triggers.transform.GetChildCount()];
		int i = 0;
		foreach (Transform t in triggers.transform) {
			boxTriggers[i] = t;
			audioSources[i] = t.GetComponent (typeof(AudioSource)) as AudioSource;
			triggerCells[i] = t.GetComponent (typeof(TriggerCell)) as TriggerCell;
			i++;
		}
	}

	void OnPostRender ()
	{
		if (depthGenerator != null) {
			IntPtr depthP = depthGenerator.GetDepthMapPtr ();
			int focalLength = (int)depthGenerator.GetIntProperty ("ZPD");
			double pixelSize = depthGenerator.GetRealProperty ("ZPPS");
			Matrix4x4 m = camera.worldToCameraMatrix;
			
			bool result;

			// Set up colliders
			result = myLib.SetNumColliders (boxTriggers.Length);
			for (int i = 0; i < boxTriggers.Length; i++) {
				Transform boxTrigger = boxTriggers[i];
				float minX = boxTrigger.localScale.x * -0.5f + boxTrigger.position.x;
				float maxX = boxTrigger.localScale.x * 0.5f + boxTrigger.position.x;
				float minY = boxTrigger.localScale.y * -0.5f + boxTrigger.position.y;
				float maxY = boxTrigger.localScale.y * 0.5f + boxTrigger.position.y;
				float minZ = boxTrigger.localScale.z * -0.5f + boxTrigger.position.z;
				float maxZ = boxTrigger.localScale.z * 0.5f + boxTrigger.position.z;
				result = myLib.SetCollider (i, minX, maxX, minY, maxY, minZ, maxZ);
			}
			
			// Draw the point cloud
			result = myLib.SetMatrix (m[0], m[1], m[2], m[3], m[4], m[5], m[6], m[7], m[8], m[9],
			m[10], m[11], m[12], m[13], m[14], m[15]);
			result = myLib.DrawPointCloud (focalLength, pixelSize, 640, 480, depthP);
			Debug.Log ("DrawPointCloud returned: " + result);
			
			// Show any collisions
			for (int i = 0; i < boxTriggers.Length; i++) {
				Transform boxTrigger = boxTriggers[i];
				AudioSource audio = audioSources[i];
				TriggerCell triggerCell = triggerCells[i];
				result = myLib.IsColliderHit (i);
				triggerCell.Hit (result);
				if (result) {
					boxTrigger.renderer.material = onMaterial;
					if (!audio.isPlaying) {
						audio.Play();
					}
				} else {
					boxTrigger.renderer.material = offMaterial;
				}
			}
		} else {
			// Try again
			OpenNI2 openNI = GameObject.FindGameObjectWithTag ("OpenNI").GetComponent (typeof(OpenNI2)) as OpenNI2;
			depthGenerator = openNI.depth;
		}
	}
}
