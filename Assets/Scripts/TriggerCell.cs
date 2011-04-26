using UnityEngine;
using System.Collections;

public class TriggerCell : MonoBehaviour {
	public int noteNumber = 74;
	
	private OSCTestSender oscTestSender;
	private bool wasHit = false;
	
	void Start () {
		oscTestSender = GameObject.FindGameObjectWithTag("OSC").GetComponent(typeof(OSCTestSender)) as OSCTestSender;
	}
	
	public void Hit (bool isHit) {
		if (isHit && !wasHit) {
			oscTestSender.SendNoteOn (noteNumber);
		} else if (!isHit && wasHit) {
			oscTestSender.SendNoteOff (noteNumber);
		}
		wasHit = isHit;
	}
	
}
