using UnityEngine;
using System.Collections;


public class NetworkSync : Photon.MonoBehaviour {

	//syncing
	[HideInInspector] public float syncTime;
	[HideInInspector] public float syncDelay;
	[HideInInspector] public float lastSynchronizationTime;
	[HideInInspector] public Vector3 syncStartPosition;
	[HideInInspector] public Vector3 syncEndPosition;
    [HideInInspector] public Vector3 syncVelocity;
    // Use this for initialization
    void Start () {
		PhotonNetwork.sendRate = 30;
		PhotonNetwork.sendRateOnSerialize = 30;
	}
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			//We own this player: send the others our data
			stream.SendNext(transform.position);
			if(GetComponent<PlayerMult>())
				stream.SendNext(GetComponent<PlayerMult>().velocity);
//			else if(GetComponent<Wind>())
//				stream.SendNext(GetComponent<Wind>().velocity);
			else
				stream.SendNext(Vector3.zero);
		}
		else {
			//Network player, receive data
			Vector3 syncPosition = (Vector3)stream.ReceiveNext();
			syncVelocity = (Vector3)stream.ReceiveNext();
			//Debug.Log ("INCOMING");
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;

			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = transform.position;
		}
	}
}
