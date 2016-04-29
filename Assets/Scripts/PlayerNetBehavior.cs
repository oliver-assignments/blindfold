using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetBehavior : NetworkBehaviour
{
    [SyncVar]
    private Vector3 syncPos;

	// Use this for initialization
	void Start ()
    {
        syncPos = transform.position;

	    if(isLocalPlayer)
        {
            Player myBehavior = gameObject.GetComponent<Player>();
            myBehavior.cameraTransform = GameObject.Find("Main Camera").transform;
            myBehavior.knifeFillImage = GameObject.Find("Knife Bar Fill").GetComponent<Image>();
        }
        else
        {
            gameObject.GetComponent<Player>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
            Destroy(transform.FindChild("Cube").gameObject);
        }
	}
	
	void FixedUpdate ()
    {
        if(isLocalPlayer)
        {
            TransmitPosition();
        }
        else
        {
            transform.position = syncPos;
        }
	}

    [Client]
    void TransmitPosition()
    {
        if (transform.position != syncPos)
        {
            CmdSendPosToServer(transform.position);
        }
    }

    [Command]
    void CmdSendPosToServer(Vector3 newPos)
    {
        syncPos = newPos;
    }
}
