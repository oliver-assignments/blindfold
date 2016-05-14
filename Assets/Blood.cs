using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {

	public float duration =1;
	private float timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<PhotonView> ().isMine) {
			timer += Time.deltaTime;
			if (timer > duration) {

				PhotonNetwork.Destroy (gameObject);		
			}
		}
	}
}
