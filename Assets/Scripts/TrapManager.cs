using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System;

public class TrapManager : MonoBehaviour {

	public string TrapDatabasePath;
	UInt32 numberOfTraps;
	byte[] TrapDB;

	public GameObject CircleTrap;
	public GameObject SquareTrap;

	// Use this for initialization
	void Start () {
		TrapDB = File.ReadAllBytes (TrapDatabasePath);

		ParseTrapDB ();
	}

	public void ParseTrapDB () {
		getNumberOfTraps ();

		for (int i=0; i<numberOfTraps; i++) {
			// CALCULATE OFFSET //
			int trapOffset = 4 + (80 * i);

			// BASIC INFO PARSING //
			UInt32 TrapType = (UInt32) TrapDB[trapOffset];
			string TrapID = BitConverter.ToUInt32 (TrapDB, trapOffset + 1).ToString ();
			Int32 TrapSize = (Int32)BitConverter.ToInt32 (TrapDB, trapOffset + 76);
			Vector2 CenterCoords = new Vector3(BitConverter.ToInt32 (TrapDB, trapOffset + 6), BitConverter.ToInt32 (TrapDB, trapOffset + 10), 0);

			// TRAP NAME //
			byte[] TrapName = new byte[62];
			for (int c = 14; c < 75; c++) {
				TrapName[c-14] = TrapDB[trapOffset + c];
			}
;
			string TrapNameString = System.Text.Encoding.ASCII.GetString(TrapName);

			// EFFECTS //
			bool Alignment = false;
			int[] Effects = new int[8];

			string EffectsFlags = Convert.ToString(TrapDB[trapOffset + 5], 2);
			for (int c = 0; c < EffectsFlags.Length; c++) {
				if (c == 0) {
					if (EffectsFlags[0] == '1') {
						Alignment = true;
					}
				} else {
					if (EffectsFlags[c] == '1') {
						Effects[c] = 1;
					} else {
						Effects[c] = -1;
					}
				}
			}


			// INSTANTIATION //
			GameObject trap = new GameObject();
			if (TrapType == 0) {
				trap = (GameObject) Instantiate(CircleTrap, CenterCoords, Quaternion.identity);
			} else if (TrapType == 1) {
				trap = (GameObject) Instantiate(SquareTrap, CenterCoords, Quaternion.identity);
			}

			// TRAP SETUP //
			trap.GetComponent<Trap>().TrapID = TrapID;
			trap.GetComponent<Trap>().TrapName = TrapNameString;

			trap.GetComponent<Trap>().EffectAlignment = Alignment;
			trap.GetComponent<Trap>().Effects = Effects;

			trap.transform.localScale = new Vector3(TrapSize, TrapSize, TrapSize);
		}
	}

	void getNumberOfTraps() {
		numberOfTraps = (UInt32) BitConverter.ToUInt32 (TrapDB, 0);
		print (numberOfTraps.ToString ());
	}
}
