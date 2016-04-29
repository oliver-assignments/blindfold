using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

    private Transform transform;
    public float mass;
    public Vector3 velocity;

	//sync
	private NetworkSync ns;
	// Use this for initialization
	void Start () {
        transform = GetComponent<Transform>();
		PhotonNetwork.sendRate = 30;
		PhotonNetwork.sendRateOnSerialize = 15;
		ns = GetComponent<NetworkSync> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<PhotonView> ().isMine) {
			velocity *= 0.99f;
			Vector3 newPosition = (transform.position + (velocity * Time.deltaTime));
			transform.position = newPosition;
		} else {
			GetServerWind ();
		}
	}
	void GetServerWind()
	{
		ns.syncTime += Time.deltaTime;
		transform.position = Vector3.Lerp(ns.syncStartPosition, ns.syncEndPosition, ns.syncTime / ns.syncDelay);
	}
}
