using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCeremony : MonoBehaviour
{
    public float timeToCenter = 1;
    public float actorAcceleration = 1;
    public float actorVelocity = 1;
    public float centerThreshold = 1;
    // This avoids collitions problems with the platform (advised to be the same as the Platform one)
    public float floatHeight = 1;
    float timeCount = 0;
    Vector3 fixedHeight;
    // Start is called before the first frame update
    void Start()
    {
        fixedHeight = new Vector3(0, floatHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        PresentShow();
    }


    void PresentShow()
    {
        if (transform.childCount > 0)
        {
            // We assume there will be only one child. But in case of improvement we could do a foreach
            GameObject mainShow = transform.GetChild(0).gameObject;
            float dt = Time.deltaTime / timeToCenter;
            float distance = Vector3.Distance(mainShow.transform.position, transform.position);
            if ( Vector3.Distance(mainShow.transform.position, transform.position) > centerThreshold)
            {
                mainShow.transform.position = Vector3.MoveTowards(mainShow.transform.position, transform.position + fixedHeight, actorAcceleration * dt * dt + actorVelocity * dt);
                mainShow.transform.rotation = Quaternion.Slerp(mainShow.transform.rotation, transform.rotation, centerThreshold/distance);
            }
            else
            {
                mainShow.transform.position = transform.position + fixedHeight;
                mainShow.transform.rotation = transform.rotation;
            }
        }
    }
}
