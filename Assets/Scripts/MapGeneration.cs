using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

    public GameObject blockPiece;

    private WindManager windManager;

	// Use this for initialization
	void Start () {
        windManager = GetComponent<WindManager>();
        CreateMap();
	}

    public void CreateMap() {
        for (int b = 0; b < 20; b++) { 

            windManager.windResistors.Add((GameObject)GameObject.Instantiate(blockPiece, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10),0), Quaternion.identity));
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
