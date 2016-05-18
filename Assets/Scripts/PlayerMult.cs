using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMult : MonoBehaviour
{
    private Transform cameraTransform;
    public Vector3 velocity;

	private float scale = 3f;
	private Vector2 movement;
	[SerializeField]
	private PhotonView playerPV;
    [SerializeField]
    private GameObject knife;
    private Image knifeFillImage;
    public float knifeCooldown;
    private float knifeTimer = 0;

    [SerializeField]
    private float secondsBeforeRespawn = 3;
    private float respawnTimer = 0;
    private Text respawnText;

    private int id;
    private bool canThrow = true;
	public float percentageOfMaxSpeedToStep;
	
    public AudioSource knifeRegain;
	public AudioSource grassWalking;

	//Sync
	private NetworkSync ns;

    // Use this for initialization
    void Start ()
    {

        velocity = Vector3.zero;
		if (playerPV.isMine) {
            respawnText = GameObject.Find("Canvas/Respawn").GetComponent<Text>();
			cameraTransform = Camera.main.transform;
			knifeFillImage = GameObject.FindGameObjectWithTag ("Fill").GetComponent<Image> ();
			knifeFillImage.color = new Color (1, 0, 0, 1);
			knifeFillImage.fillAmount = 0;
			canThrow = false;
			knifeTimer = 0;
            GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
		}
		ns = GetComponent<NetworkSync> ();
    }
	// Update is called once per frame
	void Update ()
    {

		if (playerPV.isMine) {
            if(respawnTimer > 0)
            {
                respawnTimer -= Time.deltaTime;
                respawnText.text = "RESPAWN IN: " + respawnTimer.ToString("F2");
                if(respawnTimer <= 0)
                {
                    respawnText.enabled = false;
                    transform.position = new Vector3(Random.Range(-14f, 14f), Random.Range(-8, 8), -1);
                    GetComponent<Rigidbody2D>().position = transform.position;

                    knifeRegain.Play(); //Or other respawn sound
                    knifeTimer = 0;
                    canThrow = true;
                    knifeFillImage.fillAmount = 1;
                    knifeFillImage.color = new Color(0, 1, 0, 1);
                }
                else
                {
                    return;
                }
            }
            Remaining();
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			Vector2 mouseDir = new Vector2 (Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f);
			transform.forward = mouseDir;
	        
			InputMovement ();
	
			if (canThrow) {
				if (Input.GetMouseButton (0)) {//Left
					canThrow = false;
	
					//  Throw knife
					GameObject g = (GameObject)PhotonNetwork.Instantiate (knife.name, transform.position + transform.forward, transform.rotation, 0);
					g.GetComponent<KnifeThrow> ().Setup (this.gameObject);
					knifeFillImage.color = new Color (1, 0, 0, 1);
					knifeFillImage.fillAmount = 0;
				}
			} else {
				if (knifeTimer > knifeCooldown) {

                    if (!knifeRegain.isPlaying)
                    {
                        knifeRegain.Play();
                    }
                    knifeTimer = 0;
                    canThrow = true;
                    knifeFillImage.fillAmount = 1;
                    knifeFillImage.color = new Color(0, 1, 0, 1);
                } else {
					knifeTimer += Time.deltaTime;
					knifeFillImage.fillAmount = knifeTimer / knifeCooldown;
				}
			}

            //Keeping player at a z-pos of 0

            //Moving player and then camera
            if(!GetComponent<PhotonView>().isMine)
			    Debug.Log (velocity.magnitude);
			if(velocity.magnitude > percentageOfMaxSpeedToStep*scale)
			{
                //Debug.Log("step step");
				if(!grassWalking.isPlaying)
				{
					grassWalking.Play();
				}
			}
			else
			{
				grassWalking.Stop ();
			}
            GetComponent<Rigidbody2D>().velocity = velocity;
            transform.position = GetComponent<Rigidbody2D> ().position;
			cameraTransform.position = transform.position + new Vector3 (0, 0, -1);
		} 
		else 
		{
			SyncedMovement ();
            if (!GetComponent<PhotonView>().isMine)
                ///Debug.Log(velocity.magnitude);
            if (velocity.magnitude > percentageOfMaxSpeedToStep * scale)
            {
                //Debug.Log("step step");
                if (!grassWalking.isPlaying)
                {
                    grassWalking.Play();
                }
            }
            else
            {
                grassWalking.Stop();
            }
        }
	}

	private void InputMovement()
	{

		//used to get input for direction
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		v *= scale;
		h *= scale;
 
		//store Movement
		movement = new Vector2 (h, v);

		// Distance from camera to object. In case it becomes !0.
		float camDis = cameraTransform.position.y - this.transform.position.y;

		// Get the mouse position in world space. Using camDis for the Z axis.
		Vector2 mouse = Camera.main.ScreenToWorldPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y));

		float AngleRad = Mathf.Atan2 (mouse.y - transform.position.y, mouse.x - transform.position.x);
		float angle = (180 / Mathf.PI) * AngleRad - 90;

		//if mouse is on the left side of our object
		if (Input.mousePosition.x < 0)
			angle = 360 - angle;
		
		//Uncomment this block to make the player move based on the mouse cursor
		/*float mouseDist = Mathf.Sqrt((relmousepos.x * relmousepos.x) + (relmousepos.y * relmousepos.y)) * 10
		if (mouseDist > 0.7f || v >= 0) 
		{
			movement = angle * movement;
		}
		*/

		transform.rotation = Quaternion.Euler(0, 0, angle);
		velocity = new Vector2(movement.x, movement.y);
		//GetComponent<Rigidbody2D>().rotation = angle;

	}

	private void SyncedMovement()
	{
		ns.syncTime += Time.deltaTime;
		GetComponent<Rigidbody2D>().position = Vector2.Lerp(ns.syncStartPosition, ns.syncEndPosition, ns.syncTime / ns.syncDelay);
		transform.position = GetComponent<Rigidbody2D> ().position;
        velocity = ns.syncVelocity;
	}
	void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag)
        {
            case "Solid Object":
                
                velocity *= -0.8f;
                
                break;
            
            case "Collectable":
                //objectsCollected++;
                Destroy(other.gameObject);
                break;
        }
    }
    void Remaining()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject.Find("Canvas/Players Remaining Label").GetComponent<Text>().text = "Players Remaining: " + players.Length;
    }
	[PunRPC]
	public void Respawn()
	{
		transform.position = new Vector3(Random.Range(-7.5f,7.5f),Random.Range(-3,3),-1);
		GetComponent<Rigidbody2D> ().position = transform.position;
        if (playerPV.isMine)
        {
            //Move player below field (read: Invisible and intangible)
            transform.position += new Vector3(0, 0, -100);
            GetComponent<Rigidbody2D>().position = transform.position;


            //Disable knives
            knifeFillImage.color = new Color(1, 0, 0, 1);
            knifeFillImage.fillAmount = 0;
            canThrow = false;
            knifeTimer = 0;

            //Set up respawn timer
            respawnTimer = secondsBeforeRespawn;
            respawnText.enabled = true;
            respawnText.text = "RESPAWN IN: " + secondsBeforeRespawn;
        }
	}
    [PunRPC]
    public void SetID(int i)
    {
        id = i;
        Debug.Log(id);
    }
    public int GetID() { return id; }
}
