using UnityEngine;
using System.Collections.Generic;

public class WindManager : Singleton<WindManager>
{
    public List<GameObject> wind = new List<GameObject>();
    public List<GameObject> windResistors = new List<GameObject>();

    public float maxWindDistance = 1;
    public float windSpacingForce =1;
    
    public float windResistForce =1;
    public float wrapDistanceFromOrigin = 1;

    
    private float windSpeed = 0;
    private float windPhase = 0;
    private float windDirection = 0;

    public bool breeze = false;
    public float breezeForce = 1;
    public float breezeRotationSpeed = 1;
    public float breezeBackAndForthSpeed = 1;

    public bool intraWindCollisions = true;
    public bool resistorWindCollisions = true;

    // Update is called once per frame
    void Update()
    {
        if (breeze) { Breeze(); }  //  Breeze

        for (int w = 0; w < wind.Count; w++)
        {
            
            Transform wT = wind[w].GetComponent<Transform>();
            Wind wW = wind[w].GetComponent<Wind>();
            if (intraWindCollisions)
            {
                for (int q = w + 1; q < wind.Count; q++)
                {
                    Transform qT = wind[q].GetComponent<Transform>();
                    Wind qW = wind[q].GetComponent<Wind>();

                    float distance = Vector3.Distance(wT.position, qT.position);

                    if (distance < maxWindDistance)
                    {
                        float ratio = 1 - (distance / maxWindDistance);

                        //  At a distance of 0, we apply the max force, at a distance of maxWindDistance, we apply 0 force
                        //  This would mean at a ratio of 0.5 we have 0.5 max force, its linear.
                        float force = (windSpacingForce * ratio) / qW.mass;

                        //  Since each object looks at all others, we only worry about apply the force to OTHER object nto ourselves
                        Vector3 directionToOther = Vector3.Normalize(qT.position - wT.position);

                        Vector3 acceleration = directionToOther * force;

                        //  Apply the force to the other
                        qW.velocity += acceleration;
                        wW.velocity += -1 * acceleration;
                    }
                }
            }
            if (resistorWindCollisions)
            {
                //  The wind collisiosn with real objects
                for (int r = 0; r < windResistors.Count; r++)
                {
                    Transform rT = windResistors[r].GetComponent<Transform>();
                    WindResistor rWR = windResistors[r].GetComponent<WindResistor>();

                    if (rWR != null)
                    {
                        float distance = Vector3.Distance(wT.position, rT.position);

                        if (distance < rWR.windResistDistance)
                        {
                            // Wind wW = wind[w].GetComponent<Wind>();

                            float ratio = 1 - (distance / maxWindDistance) / wW.mass;

                            //  At a distance of 0, we apply the max force, at a distance of maxWindDistance, we apply 0 force
                            //  This would mean at a ratio of 0.5 we have 0.5 max force, its linear.
                            float force = (windResistForce * ratio);

                            //  Since each object looks at all others, we only worry about apply the force to OTHER object nto ourselves
                            Vector3 directionFromOtherToSelf = Vector3.Normalize(wT.position - rT.position);

                            Vector3 acceleration = directionFromOtherToSelf * force;

                            //  Apply the force to the other
                            wW.velocity += acceleration;
                        }
                    }
                    else
                    {
                        Debug.Log(windResistors[r].ToString());
                    }
                }
            }

            if (wT.position.x > wrapDistanceFromOrigin) wT.position = new Vector3(wT.position.x - (wrapDistanceFromOrigin * 2), wT.position.y, wT.position.z);
            if (wT.position.x < -wrapDistanceFromOrigin) wT.position = new Vector3(wT.position.x + (wrapDistanceFromOrigin * 2), wT.position.y, wT.position.z);
            if (wT.position.y > wrapDistanceFromOrigin) wT.position = new Vector3(wT.position.x, wT.position.y - (wrapDistanceFromOrigin * 2), wT.position.z);
            if (wT.position.y < -wrapDistanceFromOrigin) wT.position = new Vector3(wT.position.x, wT.position.y + (wrapDistanceFromOrigin * 2), wT.position.z);
        }
    }
    void Breeze()
    {
        windDirection += breezeRotationSpeed * Time.deltaTime;

        //  Wind speed oscillates from -max to +max
        windPhase += breezeBackAndForthSpeed * Time.deltaTime;
        float windSpeed = Mathf.Sin(windPhase) * breezeForce;

        for (int q = 0; q < wind.Count; q++)
        {
            Wind qW = wind[q].GetComponent<Wind>();

            qW.velocity = new Vector3(
                    qW.velocity.x + ((Mathf.Cos(windDirection) * windSpeed) / qW.mass),
                    qW.velocity.y + ((Mathf.Sin(windDirection) * windSpeed) / qW.mass),
                    qW.velocity.z);
        }
    }
}
