using UnityEngine;
using System.Collections;


public class NetworkSync : Photon.MonoBehaviour {

	//syncing
	[HideInInspector] public float syncTime;
	[HideInInspector] public float syncDelay;
	[HideInInspector] public float lastSynchronizationTime;
	[HideInInspector] public Vector3 syncStartPosition;
	[HideInInspector] public Vector3 syncEndPosition;
	// Use this for initialization
	void Start () {
		PhotonNetwork.sendRate = 30;
		PhotonNetwork.sendRateOnSerialize = 15;
	}
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			//We own this player: send the others our data
			stream.SendNext(transform.position);
			if(GetComponent<Player>())
				stream.SendNext(GetComponent<PlayerMult>().velocity);
			else if(GetComponent<Wind>())
				stream.SendNext(GetComponent<Wind>().velocity);
			else
				stream.SendNext(Vector3.zero);
		}
		else {
			//Network player, receive data
			Vector3 syncPosition = (Vector3)stream.ReceiveNext();
			Vector3 syncVelocity = (Vector3)stream.ReceiveNext();

			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;

			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = transform.position;
		}
	}
}
