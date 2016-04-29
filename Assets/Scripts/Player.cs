using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform cameraTransform;
    [SerializeField]
    private CharacterController charControl;
    private Vector3 velocity;

	private float scale = 2.5f;
	private Vector2 movement;

	[SerializeField]
	private GameObject knife;
    public Image knifeFillImage;
    public float knifeCooldown;
    private float knifeTimer = 0;
    private bool canThrow = true;

    public AudioClip knifeThrowSound;
    private AudioSource audio;

    // Use this for initialization
    void Start ()
    {
        velocity = Vector3.zero;

        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 mouseDir = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f, 0);
        transform.forward = mouseDir;
        
        InputMovement();

        if (canThrow)
        {
            if (Input.GetMouseButton(0))//Left
            {
                canThrow = false;

                //  Throw knife
				GameObject g = GameObject.Instantiate(knife);
				g.GetComponent<KnifeThrow>().Setup(this.gameObject);
                knifeFillImage.color = new Color(1, 0, 0, 1);
                knifeFillImage.fillAmount = 0;
            }
        }
        else
        {
            if (knifeTimer > knifeCooldown)
            {
                knifeTimer = 0;
                canThrow = true;
                knifeFillImage.fillAmount = 1;
                knifeFillImage.color = new Color(0, 1, 0, 1);
            }
            else
            {
                knifeTimer += Time.deltaTime;
                knifeFillImage.fillAmount = knifeTimer / knifeCooldown;
            }
        }

        //Keeping player at a z-pos of 0
        velocity.z = -1 * transform.position.z;

        //Moving player and then camera
        charControl.Move(velocity * Time.deltaTime);
        cameraTransform.position = transform.position+new Vector3(0, 0, -10);
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
		
		//following code used to make player character face mouse
		Vector2 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);       //Mouse position
		Vector3 objpos = Camera.main.WorldToViewportPoint(transform.position);        //Object position on screen
		Vector2 relobjpos = new Vector2(objpos.x - 0.5f, objpos.y - 0.5f);            //Set coordinates relative to object's center
		Vector2 relmousepos = new Vector2(mouse.x - 0.5f, mouse.y - 0.5f) - relobjpos;//Mouse cursor relative to object's center
		float angle = Vector2.Angle(Vector2.up, relmousepos);                         //Angle calculation
		
		//if mouse is on the left side of our object
		if (relmousepos.x > 0)
			angle = 360 - angle;
		
		//Uncomment this block to make the player move based on the mouse cursor
		/*float mouseDist = Mathf.Sqrt((relmousepos.x * relmousepos.x) + (relmousepos.y * relmousepos.y)) * 10
		if (mouseDist > 0.7f || v >= 0) 
		{
			movement = angle * movement;
		}
		*/

		transform.rotation = Quaternion.Euler(0, 0, angle);
		velocity = new Vector3(movement.x, movement.y, 0);
		//GetComponent<Rigidbody2D>().rotation = angle;

	}

    void OnTriggerEnter(Collider other)
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
}
