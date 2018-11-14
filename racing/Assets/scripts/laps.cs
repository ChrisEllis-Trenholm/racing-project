using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laps : MonoBehaviour {


public bool objective1;
public bool objective2;
int Laps = 0;

void OnTriggerEnter(Collider target){
	if(target.tag == "Objective1"){
		objective1 = true;
		print("objective 1 triggered");
	}
	else if(target.tag == "Objective2"){
		objective2 = true;
		print("objective 2");
	}
	else if(target.tag == "Lap" && objective1 == true && objective2 == true){
		Laps ++;
		objective1 = false;
		objective2 = false;
		print("lap");

	}

}

void Update(){
if(Laps >= 3){
	Application.LoadLevel("scene");
}
}
}
