using UnityEngine;
using System.Collections.Generic;

public class WindManager : MonoBehaviour
{
    public List<GameObject> wind = new List<GameObject>();

    public float maxWindDistance;
    public float maxWindForce;

    
    private float windSpeed = 0;
    private float windPhase = 0;
    private float windDirection = 0;

    public bool breeze = false;
    public float breezeForce = 1;
    public float breezeRotationSpeed = 1;
    public float breezeBackAndForthSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        if (breeze) { Breeze(); }  //  Breeze

        for (int w = 0; w < wind.Count; w++)
        {
            Transform wT = wind[w].GetComponent<Transform>();
            Wind wW = wind[w].GetComponent<Wind>();

            for (int q = w+1; q < wind.Count; q++)
            {
                Transform qT = wind[q].GetComponent<Transform>();
                Wind qW = wind[q].GetComponent<Wind>();

                float distance = Vector3.Distance(wT.position, qT.position);

                if (distance < maxWindDistance)
                {
                    float ratio = 1 - (distance / maxWindDistance);

                    //  At a distance of 0, we apply the max force, at a distance of maxWindDistance, we apply 0 force
                    //  This would mean at a ratio of 0.5 we have 0.5 max force, its linear.
                    float force = (maxWindForce * ratio) / qW.mass;

                    //  Since each object looks at all others, we only worry about apply the force to OTHER object nto ourselves
                    Vector3 directionToOther = Vector3.Normalize(qT.position - wT.position);

                    Vector3 acceleration = directionToOther * force;

                    //  Apply the force to the other
                    qW.velocity += acceleration;
                    wW.velocity += -1*acceleration;
                }
            }
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
        };
    }
}
