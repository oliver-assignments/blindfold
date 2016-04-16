using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [System.NonSerialized]
    public static string currentLevel, nextLevel;

    [SerializeField]
    private string curLvl, nxtLvl;
    [SerializeField]
    private Transform arrowTransform, playerTransform;
    [SerializeField]
    private GameObject finishGO;

	// Use this for initialization
	void Start ()
    {
        currentLevel = curLvl;
        nextLevel = nxtLvl;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 delta = finishGO.transform.position - playerTransform.position;

        arrowTransform.position = playerTransform.position + delta.normalized * 2;
        arrowTransform.forward = delta;
	}
}
