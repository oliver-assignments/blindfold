using UnityEngine;
using System.Collections;

public class WindEmitter : MonoBehaviour {

    public GameObject windParticle;
    
    public float spread =1;

    public float cooldown = 0.5f;
    private float timer = 0;

    public bool hasLimit = false;
    public float limit = 10;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        if ((hasLimit && limit > 0)||!hasLimit)
        {
            timer += Time.deltaTime;
            while (timer > cooldown)
            {
                timer -= cooldown;

                limit--;

                Vector3 newPosition = new Vector3(
                    transform.position.x + Random.Range(-spread / 2, spread / 2),
                    transform.position.y + Random.Range(-spread / 2, spread / 2),
                    transform.position.z);

				GameObject wind = (GameObject)Instantiate(windParticle, newPosition, transform.rotation);
				WindManager.Instance.wind.Add(wind.GetComponent<Rigidbody2D>());
			}
        }
	}
}
