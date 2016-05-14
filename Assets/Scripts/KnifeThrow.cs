using UnityEngine;
using System.Collections;

public class KnifeThrow : MonoBehaviour {
	[SerializeField]
	private float lifespan = 4;
	[SerializeField]
	private float speed = 3;
	[SerializeField]
	private Vector3 vel;
	private bool collided = false;
	public GameObject killMarker;
	// Use this for initialization
	void Start () {}
	public void Setup(GameObject shooter)
	{
		transform.forward = shooter.transform.up;
		transform.position = shooter.transform.position + transform.forward;
		vel = transform.forward * speed;
		Debug.Log(transform.position);
		GetComponent<Rigidbody2D>().velocity = vel;
	}
	// Update is called once per frame
	void Update () 
	{
		lifespan -= Time.deltaTime;
		if(lifespan <= 0 && GetComponent<PhotonView>().isMine)
		{
			PhotonNetwork.Destroy(gameObject); 
		}
		GetComponent<PhotonView> ().RPC ("SetKnife", PhotonTargets.Others, vel, transform.position, transform.rotation);
	}
	void OnCollisionEnter2D(Collision2D o)
	{
		Debug.Log (o.gameObject.tag);
		if (o.gameObject.tag == "Player") {
			o.gameObject.GetComponent<PhotonView>().RPC("Respawn", PhotonTargets.All);
			PhotonNetwork.Instantiate(killMarker.name, transform.position, Quaternion.identity, 0);
			if(GetComponent<PhotonView>().isMine)
				PhotonNetwork.Destroy(gameObject); 
		}
		else if (o.gameObject.tag == "Wind"){


		}
		else{
			collided = true;
			//Embed self in object, stop moving.
			transform.position += transform.forward * 0.1f;
			vel = Vector3.zero;
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}
	[PunRPC]
	void SetKnife(Vector3 vel, Vector3 pos, Quaternion rot)
	{
		transform.position = pos;
		this.vel = vel;
		transform.rotation = rot;
	}
}