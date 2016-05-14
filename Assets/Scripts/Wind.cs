using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {


	private MeshRenderer renderer;
	private Rigidbody2D rigidBody;
	public float magnitudeForOpaque =10;
	public bool varyingOpacity = true;
	// Use this for initialization
	void Start () {
		renderer = GetComponent<MeshRenderer> ();
		rigidBody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (varyingOpacity) {
			if (rigidBody.velocity.magnitude > magnitudeForOpaque) {
				renderer.material.color = new Color (
				renderer.material.color.r,
				renderer.material.color.g,
				renderer.material.color.b,
				1);
			} else {
				renderer.material.color = new Color (
				renderer.material.color.r,
				renderer.material.color.g,
				renderer.material.color.b,
				rigidBody.velocity.magnitude / magnitudeForOpaque);
			}
		}

	}
}
