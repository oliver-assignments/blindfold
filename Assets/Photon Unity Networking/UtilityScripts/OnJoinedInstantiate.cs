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
				PhotonNetwork.Instantiate(o.name, new Vector3(Random.Range(-7.5f,7.5f),Random.Range(-3,3),-1), Quaternion.identity, 0);
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