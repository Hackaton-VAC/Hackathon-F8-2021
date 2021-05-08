using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagehand : MonoBehaviour
{
    public float timeToPlace = 1;
    public float switchVelocity = 1;
    public float backStageThreshold = 0.5f;
    public float floatRadius = 2;
    public float floatHeight = 2;
    bool reshuffle = false;
    Vector3 fixedHeight;

    // Start is called before the first frame update
    void Start()
    {
        fixedHeight = new Vector3(0, floatHeight, 0);
        ArrangeBackstage();
    }

    // Update is called once per frame
    void Update()
    {
        if (reshuffle)
        {
            SendToBack();
        }
    }

    public void ReShuffle()
    {
        reshuffle = true;
    }

    void ArrangeBackstage()
    {
        int angleStep = 180 / (transform.childCount - 1);
        float currentAngle = transform.rotation.eulerAngles.y;
        foreach (Transform child in transform)
        {
            GameObject actor = child.gameObject;
            Renderer actorRenderer = actor.GetComponent<Renderer>();
            Vector2 position2D = HackathonUtils.Utils.GetPointOnCircle(new Vector2(transform.position.x, transform.position.z), floatRadius, currentAngle);
            Vector3 resultingTransform = new Vector3(position2D.x, fixedHeight.y, position2D.y);
            actor.transform.position = resultingTransform;
            //
            Vector3 fromPlatFormToActor = transform.position - resultingTransform;
            Vector3 fromPlatFormToActorsRenderer = transform.position - actorRenderer.bounds.center;
            Vector3 dispalcementAdjustment = fromPlatFormToActor.magnitude > fromPlatFormToActorsRenderer.magnitude ? fromPlatFormToActor - fromPlatFormToActorsRenderer : fromPlatFormToActorsRenderer - fromPlatFormToActor;
            Vector3 finalTransform = resultingTransform + new Vector3(dispalcementAdjustment.x, 0, dispalcementAdjustment.z);
            actor.transform.position = finalTransform;
            currentAngle += angleStep;

            //Vector3 platformCenterDirection = (finalTransform - transform.position).normalized;

        }
    }
    // Centro geometrico al transform
    void SendToBack()
    {
        int angleStep = transform.childCount - 1 <= 0 ? 0 :  180 / (transform.childCount - 1);
        float currentAngle = transform.rotation.eulerAngles.y;
        float dt = Time.deltaTime / timeToPlace;
        int placed = 0;
        foreach (Transform child in transform)
        {
            GameObject actor = child.gameObject;
            Renderer actorRenderer = actor.GetComponent<Renderer>();
            Vector2 position2D = HackathonUtils.Utils.GetPointOnCircle(new Vector2(transform.position.x, transform.position.z), floatRadius, currentAngle);
            Vector3 resultingTransform = new Vector3(position2D.x, fixedHeight.y, position2D.y);
            var aux = new Vector3(actorRenderer.bounds.center.x, fixedHeight.y, actorRenderer.bounds.center.z);
            float distanceToTarget = Vector3.Distance(aux, resultingTransform);

            if (distanceToTarget > backStageThreshold)
            {
                //actor.transform.position = resultingTransform;
                Vector3 fromPlatFormToActor = transform.position - resultingTransform;
                //Vector3 fromPlatFormToActorsRenderer = transform.position - actorRenderer.bounds.center;
                //Vector3 dispalcementAdjustment = fromPlatFormToActor.magnitude > fromPlatFormToActorsRenderer.magnitude ? fromPlatFormToActor - fromPlatFormToActorsRenderer : fromPlatFormToActorsRenderer - fromPlatFormToActor;
                //Vector3 finalTransform = resultingTransform + new Vector3(dispalcementAdjustment.x, 0, dispalcementAdjustment.z);
                actor.transform.position = Vector3.MoveTowards(actor.transform.position, resultingTransform, switchVelocity * dt);
            }
            else
            {
                // TODO separate this and the Start() one into a 
                actor.transform.position = resultingTransform;
                Vector3 fromPlatFormToActor = transform.position - resultingTransform;
                Vector3 fromPlatFormToActorsRenderer = transform.position - actorRenderer.bounds.center;
                Vector3 dispalcementAdjustment = fromPlatFormToActor.magnitude > fromPlatFormToActorsRenderer.magnitude ? fromPlatFormToActor - fromPlatFormToActorsRenderer : fromPlatFormToActorsRenderer - fromPlatFormToActor;
                Vector3 finalTransform = resultingTransform + new Vector3(dispalcementAdjustment.x, 0, dispalcementAdjustment.z);
                actor.transform.position = finalTransform;
                placed++;
                print(child.gameObject.name);
            }

            currentAngle += angleStep;

        }
        if (placed == transform.childCount)
        {
            reshuffle = false;
        }
    }

}
