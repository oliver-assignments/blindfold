using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnJoinedInstantiate : MonoBehaviour
{
    public float PositionOffset = 2.0f;
    public GameObject[] PrefabsToInstantiate;   // set in inspector
	public GameObject MasterPrefab;

    public void OnJoinedRoom()
    {
        Debug.Log("Present.");
        if (this.PrefabsToInstantiate != null)
        {
            foreach (GameObject o in this.PrefabsToInstantiate)
            {
                Debug.Log("Instantiating: " + o.name);
				

                Debug.Log("Instantiating: " + o.name);
				

                int iDNum = GameObject.FindGameObjectsWithTag("Player").Length;
				GameObject g = (GameObject)PhotonNetwork.Instantiate(o.name, new Vector3(Random.Range(-14, 14), Random.Range(-8, 8), -1), Quaternion.identity, 0);
                g.GetComponent<PhotonView>().RPC("SetID", PhotonTargets.OthersBuffered, iDNum);
                Debug.Log(iDNum);
            }
        }
		if( PhotonNetwork.isMasterClient == false )
		{
			return;
		}
		//GetComponent<MapGeneration> ().CreateMap ();
		//PhotonNetwork.Instantiate(MasterPrefab.name, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10),0), Quaternion.identity, 0);
		//PhotonNetwork.Instantiate(MasterPrefab.name, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10),0), Quaternion.identity, 0);
		//PhotonNetwork.Instantiate(MasterPrefab.name, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10),0), Quaternion.identity, 0);
		//PhotonNetwork.Instantiate(MasterPrefab.name, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10),0), Quaternion.identity, 0);
    }
}
