using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveRandomly : MonoBehaviour {

    public float timer;

    public int newtarget;

    public float speed;

    public NavMeshAgent nav;

    public Vector3 Target;

    public float jumpheight = 50000000;

    public float jumpTime;
    
	// Use this for initialization
	void Start () {

        nav = gameObject.GetComponent<NavMeshAgent>();
		
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        jumpTime += Time.deltaTime;

        if(timer <= newtarget)
        {
            newTarget();
            timer = 0;
        }

        if(jumpTime <= newtarget)
        {
            Jump();

            jumpTime = 0;
        }
        nav.speed = speed;

       

	}

    void Jump ()
    {
        GetComponent<Rigidbody>().AddForce(0, jumpheight, 0);
    }

    void newTarget ()
    {
        float myX = gameObject.transform.position.x;
        float myZ = gameObject.transform.position.z;

        float xPos = myX + Random.Range(myX - 100, myX + 100);
        float zPos = myZ + Random.Range(myZ - 100, myZ + 100);

        Target = new Vector3(xPos, gameObject.transform.position.y, zPos);

        nav.SetDestination(Target);
       
    }


}
