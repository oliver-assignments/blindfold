using UnityEngine;
using System.Collections.Generic;

public class WindManager : MonoBehaviour
{
    public List<GameObject> wind = new List<GameObject>();

    public float maxWindDistance;
    public float maxWindForce;

    // Update is called once per frame
    void Update()
    {
        for (int w = 0; w < wind.Count; w++)
        {
            for (int q = 0; q < wind.Count; q++)
            {
                if (w == q) continue;

                Transform wT = wind[w].GetComponent<Transform>();
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
                }
            }
        }
    }
}
