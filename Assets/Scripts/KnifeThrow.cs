using UnityEngine;
using System.Collections;

public class KnifeThrow : MonoBehaviour {
    [SerializeField]
	private float lifespan = 4;
    [SerializeField]
	private float speed = 3;
    [SerializeField]
    private Vector3 vel;

    public AudioClip fallenSound;
    public AudioSource audioSource;
    private float soundTimer = 0;
    private float soundCooldown =1;
    private bool expired = false;
	// Use this for initialization
	void Start ()
    {
    }
	public void Setup(GameObject shooter)
    {
        transform.forward = shooter.transform.up;
        transform.position = shooter.transform.position + transform.forward;
		vel = transform.forward * speed;
		Debug.Log(transform.position);
        GetComponent<Rigidbody2D>().AddForce(vel*100000);
		GetComponent<PhotonView> ().RPC ("SetKnife", PhotonTargets.Others, vel, transform.position, transform.rotation);
	}
	// Update is called once per frame
	void Update () 
	{
		lifespan -= Time.deltaTime;
		if(lifespan <= 0 && GetComponent<PhotonView>().isMine && !expired)
		{
            //  Die to being in air too long
            expired = true;
            audioSource.Stop();

            audioSource.PlayOneShot(fallenSound,1);
            soundCooldown = fallenSound.length;

            //  Hide and stop the object
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        if (expired)
        {
            //  We have either gone too far, hit a player, or hit a wall
            soundTimer += Time.deltaTime;
            if (soundTimer > soundCooldown)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
	}
	[PunRPC]
	void SetKnife(Vector3 vel, Vector3 pos, Quaternion rot)
	{
        
		transform.position = pos;
		this.vel = vel;
        GetComponent<Rigidbody2D>().AddForce(vel*100000);
	}
}
