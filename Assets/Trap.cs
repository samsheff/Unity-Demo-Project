using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public string TrapID;
	public string TrapName;

	public bool EffectAlignment;
	public int[] Effects;

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col) {
		if (hasConflictingEffects(col.gameObject.GetComponent<Trap>().Effects)) {
			GetComponent<SpriteRenderer> ().color = Color.red;

			print ("Conflicting trap with name: " + TrapName);
		}
	}

	bool hasConflictingEffects(int[] trapEffects) {
		for (int i=0; i<trapEffects.Length; i++) {
			if (trapEffects[i] == -1 && Effects[i] == 1) {
				return true;
			} else if (trapEffects[i] == 1 && Effects[i] == -1){
				return true;
			}
		}

		return false;
	}
}
