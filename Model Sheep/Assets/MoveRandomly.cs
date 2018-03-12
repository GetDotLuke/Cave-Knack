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

	public float jumpheight = 50000000;	// Variable used to give height to the model.

	public float jumpTime;	// Calculates how often the model should jump.

	// Use this for initialization
	void Start () {

		nav = gameObject.GetComponent<NavMeshAgent>();

	}

	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		jumpTime += Time.deltaTime;		// Time related variables are updated.

		if(timer <= newtarget)
		{
			newTarget();
			timer = 0;				// Picks the next target, resets the timer.

		}

		if(jumpTime <= newtarget)
		{
			Jump();

			jumpTime = 0;			// forces a jump, resets the timer.
		}
		nav.speed = speed;



	}

	void Jump ()				// Code used to force a jump. 
	{
		GetComponent<Rigidbody>().AddForce(0, jumpheight, 0);
	}

	void newTarget ()				// Code used to change target.
	{
		float myX = gameObject.transform.position.x;
		float myZ = gameObject.transform.position.z;

		float xPos = myX + Random.Range(myX - 100, myX + 100);
		float zPos = myZ + Random.Range(myZ - 100, myZ + 100);

		Target = new Vector3(xPos, gameObject.transform.position.y, zPos);

		nav.SetDestination(Target);

	}


}