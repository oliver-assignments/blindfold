using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform cameraTransform;
    public CharacterController charControl;
    private Vector3 velocity;

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

        if (canThrow)
        {
            if (Input.GetMouseButton(0))//Left
            {
                canThrow = false;

                //  Throw knife
            }
        }
        else
        {
            if (knifeTimer > knifeCooldown)
            {
                knifeTimer = 0;
                canThrow = true;
            }
            else
            {
                knifeTimer += Time.deltaTime;
            }
        }
            
        

        //Keeping player at a z-pos of 0
        velocity.z = -1 * transform.position.z;

        //Moving player and then camera
        charControl.Move(velocity * Time.deltaTime);
        cameraTransform.position = transform.position+new Vector3(0, 0, -10);
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
