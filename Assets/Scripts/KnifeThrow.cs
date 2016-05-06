using UnityEngine;
using System.Collections;

public class KnifeThrow : MonoBehaviour {
    [SerializeField]
	private float lifespan = 4;
    [SerializeField]
	private float speed = 3;
    [SerializeField]
    public CharacterController charControl;
    private Vector3 vel;
	private bool collided = false;
	// Use this for initialization
	void Start () {}
	public void Setup(GameObject shooter)
    {
        transform.forward = shooter.transform.up;
        transform.position = shooter.transform.position + transform.forward;
		vel = transform.forward * speed;
		GetComponent<PhotonView> ().RPC ("SetKnife", PhotonTargets.Others, vel, transform.position, transform.rotation);
	}
	// Update is called once per frame
	void Update () 
	{
		lifespan -= Time.deltaTime;
		if(lifespan <= 0 && GetComponent<PhotonView>().isMine)
		{
			PhotonNetwork.Destroy(gameObject); 
		}
		else
		{
			if (!collided) 
			{
				charControl.Move (vel * Time.deltaTime);
				if (charControl.collisionFlags != CollisionFlags.None) {	
					collided = true;
					//Embed self in object, stop moving.
					transform.position += transform.forward * 0.1f;
					vel = Vector3.zero;

					//TODO: If object is player, delete self immediately (alternatively: "fall to floor" by setting z-pos to something >=1)
				}
			}
		}
	}
	void OnControllerColliderHit(ControllerColliderHit o)
	{
		Debug.Log (o.gameObject.tag);
		if (o.gameObject.tag == "Player") {
			o.gameObject.GetComponent<PhotonView>().RPC("Respawn", PhotonTargets.All);
			if(GetComponent<PhotonView>().isMine)
				PhotonNetwork.Destroy(gameObject); 
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
