using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objective1 : MonoBehaviour {

	void OnTriggerEnter(Collider target){
		if(target.tag == "Player"){
		print("objective 1 triggered");
		}
	}
}

