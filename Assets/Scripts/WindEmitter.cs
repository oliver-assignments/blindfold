using UnityEngine;
using System.Collections;

public class WindEmitter : MonoBehaviour {

    public GameObject windParticle;
    public WindManager windManager;
    public float spread;

    public float cooldown;
    private float timer = 0;

    public bool hasLimit = false;
    public float limit = 10;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (limit > 0)
        {
            timer += Time.deltaTime;
            if (timer > cooldown)
            {
                timer -= cooldown;

                limit--;

                Vector3 newPosition = new Vector3(
                    transform.position.x + Random.Range(-spread / 2, spread / 2),
                    transform.position.y + Random.Range(-spread / 2, spread / 2),
                    transform.position.z);

                windManager.wind.Add((GameObject)GameObject.Instantiate(windParticle, newPosition, transform.rotation));
            }
        }
	}
}
