using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

    private Transform transform;
    public float mass;
    public Vector3 velocity;

	// Use this for initialization
	void Start () {
        transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        velocity *= 0.99f;
        Vector3 newPosition = (transform.position + (velocity * Time.deltaTime));
        transform.position = newPosition ;
	}
}
