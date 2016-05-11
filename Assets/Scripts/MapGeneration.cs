using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

    public GameObject blockPiece;

    private WindManager windManager;

	// Use this for initialization
	void Start () {
        windManager = GetComponent<WindManager>();
        //CreateMap();
	}

    public void CreateMap() {
        for (int b = 0; b < 20; b++) { 
			GameObject g = PhotonNetwork.Instantiate(blockPiece.name, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10),0), Quaternion.identity, 0);
			g.GetComponent<Rigidbody2D>().MovePosition(g.transform.position);
			windManager.windResistors.Add(g);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
