using UnityEngine;
using System.Collections;

public class OnWindTrigger : MonoBehaviour {

    // Use this for initialization
    void Start() { 

    }
    void OnTriggerEnter(Collider other)
    {
        Wind windParticle = other.gameObject.GetComponent<Wind>();
        Debug.Log(gameObject.ToString());
        //float distance = Vector3.Distance(transform.position, other.transform.position);



        //float ratio = 1 - (distance / maxWindDistance) / windParticle.mass;

        //  At a distance of 0, we apply the max force, at a distance of maxWindDistance, we apply 0 force
        //  This would mean at a ratio of 0.5 we have 0.5 max force, its linear.
        float force = 10;//(windResistForce * ratio);

            //  Since each object looks at all others, we only worry about apply the force to OTHER object nto ourselves
        Vector3 directionToOther = Vector3.Normalize(other.transform.position - transform.position);

        Vector3 acceleration = directionToOther * force;

            //  Apply the force to the other
        windParticle.velocity += acceleration;
        
    
    }
	// Update is called once per frame
	void Update () {
	
	}
}
