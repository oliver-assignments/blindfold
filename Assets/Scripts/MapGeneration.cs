using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapGeneration : MonoBehaviour {

    public GameObject blockPiece;
    public float roundSpashDuration;
    private float roundSplashTimer = 0;
    public Text objectiveSplash;


	// Use this for initialization
	void Start () {
        CreateMap();
	}

    public void CreateMap() {
        
    }

	// Update is called once per frame
	void Update () {
        if (objectiveSplash.enabled)
        {
            roundSplashTimer += Time.deltaTime;

            if (roundSplashTimer > roundSpashDuration)
            {
                objectiveSplash.enabled = false;
            }
        }
	}
}
