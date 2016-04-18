using UnityEngine;
using System.Collections;

public class KnifeThrow : MonoBehaviour {
	private float lifespan = 4;
	private float speed = 3;
	private Vector3 vel;
	// Use this for initialization
	void Start () {
		
	}
	public void Setup(GameObject shooter)
	{
		this.transform.position = shooter.transform.position + transform.forward;
		this.transform.rotation = shooter.transform.rotation;
		vel = this.transform.forward * speed;
		//Debug.Log (vel);
	}
	// Update is called once per frame
	void Update () 
	{
		lifespan -= Time.deltaTime;
		if(lifespan <= 0)
		{
			Destroy(gameObject); 
		}
		else
		{
			transform.position += vel * Time.deltaTime;
		}
	}
}
