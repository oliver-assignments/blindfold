using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

    public GameObject blockPiece;

    private WindManager windManager;

	// Use this for initialization
	void Start () {
        windManager = GetComponent<WindManager>();
	}

    public void CreateMap() {

        windManager.windResistors.Add((GameObject)GameObject.Instantiate(blockPiece, Vector3.zero, Quaternion.identity));

    }

	// Update is called once per frame
	void Update () {
	
	}
}
