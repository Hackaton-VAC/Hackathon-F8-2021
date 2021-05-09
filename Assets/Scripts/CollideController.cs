using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideController : MonoBehaviour
{
    //GameObject wrapper;
    bool touched = false;
    public float snap, V0, time, angularVelocity;
    float Acceleration, dw, alpha, total;
    // Use this for initialization
    void Start()
    {
        Acceleration = -V0 / time;
        alpha = angularVelocity / time;
        if (time == 0)
        {
            time = 1.7F;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff;
        GameObject attachedToMe;
        float dt, holis;
        if (touched)
        {
            diff = this.transform.parent.position - this.transform.position;
            diff.Normalize();
            holis = Vector3.Distance(this.transform.position, this.transform.parent.position);
            if (holis < snap)
            {
                touched = false;
                this.transform.position = this.transform.parent.position;
                this.transform.rotation = this.transform.parent.rotation;
                
            }
            else
            {
                dt = Time.deltaTime / time;
                dw = 1 - holis / total;
                this.transform.position = Vector3.MoveTowards(transform.position,
                                                              transform.parent.position,
                                                              Acceleration * dt * dt +
                                                              V0 * dt);
                this.transform.rotation = Quaternion.Lerp(transform.rotation,
                                                          transform.parent.rotation,
                                                           dw);
            }
        }
    }

    public void MergeTo(GameObject destiny)
    {
        transform.SetParent(destiny.transform);
        touched = true;
        total = Vector3.Distance(transform.position, destiny.transform.position);
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log(other.collider.name + " Sale");
        touched = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        //collisin
    }

}
