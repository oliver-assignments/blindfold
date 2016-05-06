using UnityEngine;
using System.Collections;

public class KnifeThrow : MonoBehaviour {
    [SerializeField]
	private float lifespan = 4;
    [SerializeField]
	private float speed = 3;
    [SerializeField]
    public CharacterController charControl;
    private Vector2 vel;
	// Use this for initialization
	void Start () {}
	public void Setup(GameObject shooter)
    {
        transform.forward = shooter.transform.up;
        transform.position = shooter.transform.position + transform.forward;
		vel = transform.forward * speed;
	}
	// Update is called once per frame
	void Update () 
	{
		lifespan -= Time.deltaTime;
		if(lifespan <= 0)
		{
			Destroy(gameObject); 
		}
		else
		{
			charControl.Move(vel * Time.deltaTime);
            if(charControl.collisionFlags != CollisionFlags.None)
            {
                //Embed self in object, stop moving.
                transform.position += transform.forward*0.1f;
                vel = Vector2.zero;

                //TODO: If object is player, delete self immediately (alternatively: "fall to floor" by setting z-pos to something >=1)
            }
		}
	}
}
