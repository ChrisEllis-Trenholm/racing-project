using UnityEngine;
using System.Collections;

public class RaceManager : MonoBehaviour 
{
	public static RaceManager Instance { get { return instance;} }
	private static RaceManager instance = null;
	private int[] laps;
	public Rigidbody[] cars;
	public float respawnDelay = 5f;
	public float distanceToCover = 1f;
	private CarController[] scripts;
	private float[] respawnTimes;
	private float[] distanceLeftToTravel;
	private Transform[] waypoint;
	public Texture2D startRaceImage;
	public Texture2D Digit1Image;
	public Texture2D Digit2Image;
	public Texture2D Digit3Image;
	private int countdownTimerDelay;
	private float countdownTimerStartTime;
	
	// Use this for initialization

	void Awake()
	{
		CountdownTimerReset(3);
		if (instance != null && instance != this){
			Destroy(this.gameObject);
			return;
		}
		else {
			instance = this;
		}
		
	}
	void Start () 
	{
		respawnTimes = new float[cars.Length];
		distanceLeftToTravel = new float[cars.Length];
		scripts = new CarController[cars.Length];
		waypoint = new Transform[cars.Length];
		
		laps = new int[cars.Length];
		//intialize the arrays with starting values
		for(int i=0; i < respawnTimes.Length; ++i)
		{
			scripts[i] = cars[i].gameObject.GetComponent<CarController>();
			respawnTimes[i] = respawnDelay;
			distanceLeftToTravel[i] = float.MaxValue;
			laps[i] = 0;
		}

		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//check if any of the cars need a respawn.
	 	for(int i = 0; i < cars.Length; ++i)
		{
			Transform nextWaypoint = scripts[i].GetCurrentWaypoint();
			float distanceCovered = (nextWaypoint.position - cars[i].position).magnitude;
			
			//if the car has moved far enough or is now moving to a new waypoint reset its values.
			if(distanceLeftToTravel[i] - distanceToCover > distanceCovered || waypoint[i] != nextWaypoint)
			{
				waypoint[i] = nextWaypoint;
				respawnTimes[i] = respawnDelay;
				distanceLeftToTravel[i] = distanceCovered;
			}
			//otherwise tick down time before we respawn it.
			else
			{
				respawnTimes[i] -= Time.deltaTime;
			}
			
			//if it's respawn timer has elapsed.
			if(respawnTimes[i] <= 0)
			{
				//reset its respawn tracking variables
				respawnTimes[i] = respawnDelay;
				distanceLeftToTravel[i] = float.MaxValue;
				cars[i].velocity = Vector3.zero;
				//And spaw it at its last waypoint facing the next waypoint.
				Transform lastWaypoint = scripts[i].GetLastWaypoint();	
				cars[i].position = lastWaypoint.position;
				cars[i].rotation = Quaternion.LookRotation(nextWaypoint.position - lastWaypoint.position);
			}

			if (laps[0] >= 3 && laps[1] >= 3)
			{
				Application.LoadLevel("scene");
			}
		}
		CountdownTimerImage();
	
	}

	public void LapFinishedByAi(CarController script){
		for(int i=0; i< respawnTimes.Length; ++i)
		{
			if (scripts[0] == script)
			{
				laps[0]++;
				print(laps[0]);
				break;
			}
			if (scripts[1] == script){
				laps[1]++;
				print(laps[1]);
				break;
			}
		}
	}
	void OnGUI(){
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label(CountdownTimerImage());
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}

	Texture2D CountdownTimerImage()
	{
		switch(CountdownTimerSecondsRemaining())
		{
			case 3:
			return Digit3Image;
			case 2:
			return Digit2Image;
			case 1:
			return Digit1Image;
			case 0:
			return startRaceImage;
			default:
			return null;
		}
	}
	int CountdownTimerSecondsRemaining(){
		int elapsedSeconds =  (int) (Time.time - countdownTimerStartTime);
		int secondsLeft = (countdownTimerDelay - elapsedSeconds);
		
		return secondsLeft;
		

	}

	void CountdownTimerReset(int delayInSeconds){
		countdownTimerDelay = delayInSeconds;
		countdownTimerStartTime = Time.time;
		
	}

}
