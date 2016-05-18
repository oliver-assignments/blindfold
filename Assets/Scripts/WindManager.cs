using UnityEngine;
using System.Collections.Generic;

public class WindManager : Singleton<WindManager>
{
    public List<Rigidbody2D> wind = new List<Rigidbody2D>();

    public float maxWindDistance = 1;
    public float windSpacingForce = 1;
    
    public float windResistForce = 1;
    public Vector2 wrapDistanceFromOrigin = new Vector2(15, 6);

    
    private float windSpeed = 0;
    private float windPhase = 0;
    private float windDirection = 0;

    public bool breeze = false;
    public float breezeForce = 1;
    public float breezeRotationSpeed = 1;
    public float breezeBackAndForthSpeed = 1;

    public bool intraWindCollisions = true;

    public AudioSource ambientWindSource;
    public float maxWindVolume = 1f;
    public float minWindVolume = 0.25f;

	private float currTime;

    // Update is called once per frame
    void Update()
    {
        if (breeze) { Breeze(); }  //  Breeze

        for (int q = 0; q < wind.Count; q++)
        {
            if (wind[q].position.x > wrapDistanceFromOrigin.x) wind[q].position = new Vector2(wind[q].position.x - (wrapDistanceFromOrigin.x * 2), wind[q].position.y);
            if (wind[q].position.x < -wrapDistanceFromOrigin.x) wind[q].position = new Vector2(wind[q].position.x + (wrapDistanceFromOrigin.x * 2), wind[q].position.y);
            if (wind[q].position.y > wrapDistanceFromOrigin.y) wind[q].position = new Vector2(wind[q].position.x, wind[q].position.y - (wrapDistanceFromOrigin.y * 2));
            if (wind[q].position.y < -wrapDistanceFromOrigin.y) wind[q].position = new Vector2(wind[q].position.x, wind[q].position.y + (wrapDistanceFromOrigin.y * 2));
        }
    }
    void Breeze()
    {
		currTime = (float)PhotonNetwork.time;
		windDirection = (float)(breezeRotationSpeed * currTime);

        //  Wind speed oscillates from -max to +max 
        ambientWindSource.volume = minWindVolume + (Mathf.Abs(Mathf.Sin(windPhase))*(maxWindVolume-minWindVolume));

		windPhase = breezeBackAndForthSpeed * currTime;
        float windSpeed = Mathf.Sin(windPhase) * breezeForce;

        for (int q = 0; q < wind.Count; q++)
        {
            wind[q].velocity = new Vector2(
                    wind[q].velocity.x + ((Mathf.Cos(windDirection) * windSpeed) / wind[q].mass),
                    wind[q].velocity.y + ((Mathf.Sin(windDirection) * windSpeed) / wind[q].mass));
        }
    }
}
