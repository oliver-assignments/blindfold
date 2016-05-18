using UnityEngine;
using System.Collections;

public class KnifeThrow : MonoBehaviour
{
    [SerializeField]
    private float lifespan = 4;
	private float currentLifetime;
    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private Vector3 vel;
    private bool collided = false;
    public GameObject killMarker;

    public AudioClip fallenSound;
    public AudioClip playerHitSound;
    public AudioClip blockHitSound;
    public AudioSource audioSource;
    private int id;
    private float soundTimer = 0;
    private float soundCooldown = 1;
    private bool expired = false;

    // Use this for initialization
    void Start() {
		currentLifetime = lifespan;
	}

    void Kill(AudioClip clip)
    {
        expired = true;
        audioSource.Stop();

        audioSource.PlayOneShot(clip, 1);
        soundCooldown = clip.length;

        //  Hide and stop the object
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
    }
    public void Setup(GameObject shooter)
    {
        id = shooter.GetComponent<PlayerMult>().GetID();
        //Debug.Log(id);
        GetComponent<MeshRenderer>().enabled = true;
        transform.forward = shooter.transform.up;
        transform.position = shooter.transform.position + transform.forward;
        vel = transform.forward * speed;
        GetComponent<PhotonView>().RPC("SetKnife", PhotonTargets.Others, vel, transform.position, id);
        GetComponent<Rigidbody2D>().velocity = vel;
    }
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            if (!collided)
                GetComponent<Rigidbody2D>().velocity = vel;
			currentLifetime -= Time.deltaTime;
            if (currentLifetime <= 0)
            {
                Kill(fallenSound);
                collided = true;
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
            GetComponent<PhotonView>().RPC("SetKnife", PhotonTargets.Others, vel, transform.position, id);
        }
    }
    void OnCollisionEnter2D(Collision2D o)
    {
		//if (!collided) 
		{

			//Debug.Log(o.gameObject.tag);
			if (o.gameObject.tag == "Player") 
			{
				Debug.Log (currentLifetime);
				if (currentLifetime < lifespan-0.005) 
				{
					//Debug.Log (id);
					//Debug.Log (o.gameObject.GetComponent<PlayerMult> ().GetID ());
					if (o.gameObject.GetInstanceID () == id) {
						return;
					}
					collided = true;
					o.gameObject.GetComponent<PhotonView> ().RPC ("Respawn", PhotonTargets.All);
					PhotonNetwork.Instantiate (killMarker.name, transform.position, Quaternion.Euler (0, 0, Random.Range (0, 360)), 0);
					if (GetComponent<PhotonView> ().isMine)
						Kill (playerHitSound);
				} else {
					Debug.Log ("Too soon");
				}
			} else if (o.gameObject.tag == "Wind") {


			} else {
				Debug.Log (currentLifetime);
				collided = true;
				//Embed self in object, stop moving.
				transform.position += transform.forward * 0.1f;
				Kill (blockHitSound);
			}
		}
	}
    [PunRPC]
    void SetKnife(Vector3 vel, Vector3 pos, int owner)
    {
        id = owner;
        transform.position = pos;
        this.vel = vel;
    }
}
//=======
//public class KnifeThrow : MonoBehaviour
//{
//    [SerializeField]
//    private float lifespan = 4;
//    [SerializeField]
//    private float speed = 3;

//    [SerializeField]
//    private Vector3 vel;

//    public AudioClip fallenSound;
//    public AudioClip playerHitSound;
//    public AudioClip blockHitSound;
//    public AudioSource audioSource;
//    private float soundTimer = 0;
//    private float soundCooldown = 1;
//    private bool expired = false;

//    [SerializeField]
//    private bool collided = false;

//    // Use this for initialization
//    void Start()
//    {
//    }
//    public void Setup(GameObject shooter)
//    {
//        transform.forward = shooter.transform.up;
//        transform.position = shooter.transform.position + transform.forward;
//        vel = transform.forward * speed;
//        Debug.Log(transform.position);
//        GetComponent<Rigidbody2D>().velocity = vel;
//    }

//    void Kill(AudioClip clip)
//    {
//        expired = true;
//        audioSource.Stop();

//        audioSource.PlayOneShot(clip, 1);
//        soundCooldown = clip.length;

//        //  Hide and stop the object
//        GetComponent<MeshRenderer>().enabled = false;
//        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        lifespan -= Time.deltaTime;
//        if (lifespan <= 0 && GetComponent<PhotonView>().isMine && !expired)
//        {
//            //  Die to being in air too long
//            Kill(fallenSound);
//        }
//        if (expired)
//        {
//            //  We have either gone too far, hit a player, or hit a wall
//            soundTimer += Time.deltaTime;
//            if (soundTimer > soundCooldown)
//            {
//                PhotonNetwork.Destroy(gameObject);
//            }
//        }



//        GetComponent<PhotonView>().RPC("SetKnife", PhotonTargets.Others, vel, transform.position, transform.rotation);
//    }
//    void OnCollisionEnter2D(Collision2D o)
//    {
//        if (!expired) {
//            Debug.Log(o.gameObject.tag);
//            if (o.gameObject.tag == "Player")
//            {
//                o.gameObject.GetComponent<PhotonView>().RPC("Respawn", PhotonTargets.All);
//                if (GetComponent<PhotonView>().isMine)
//                {
//                    Kill(playerHitSound);
//                }
//            }
//            else if (o.gameObject.tag == "Wind")
//            {


//            }
//            else
//            {
//                collided = true;
//                //Embed self in object, stop moving.
//                transform.position += transform.forward * 0.1f;
//                vel = Vector3.zero;
//                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
//                Kill(blockHitSound);
//            }
//        }

//    }
//    [PunRPC]
//    void SetKnife(Vector3 vel, Vector3 pos, Quaternion rot)
//    {

//        transform.position = pos;
//        this.vel = vel;
//        transform.rotation = rot;
//    }
//}
//>>>>>>> origin/soundEffects
