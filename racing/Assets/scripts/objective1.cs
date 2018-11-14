using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objective1 : MonoBehaviour {

	void OnTriggerEnter(Collider target){
		if(target.tag == "Player"){
			bool objective1 = true;
			if(objective1 == true){
		print("objective 1 triggered");
			}
		}
	}
}

