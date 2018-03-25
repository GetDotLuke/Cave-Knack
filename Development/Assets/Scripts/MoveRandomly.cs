using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveRandomly : MonoBehaviour {


	public float timer;         // Used to update time in the code.

	public int newtarget;       // Used for picking the direction of a model, which is updated according to the time.

	public float speed;		// Sets the speed of the Model.

	public NavMeshAgent nav;	// The NavMesh allows the model to travel.

	public Vector3 Target;	// Gives the Model a movement to reach newtarget.

	// Use this for initialization
	void Start () {

		nav = gameObject.GetComponent<NavMeshAgent>();

	}

	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if(timer <= newtarget)
		{
			newTarget();
			timer = 0;				// Picks the next target, resets the timer.

		}

	}


	void newTarget ()				// Code used to change target.
	{
		float myX = gameObject.transform.position.x;
		float myZ = gameObject.transform.position.z;

		float xPos = myX + Random.Range(myX - 200, myX + 200);
		float zPos = myZ + Random.Range(myZ - 200, myZ + 200);

		Target = new Vector3(xPos, gameObject.transform.position.y, zPos);

		nav.SetDestination(Target);

	}


}