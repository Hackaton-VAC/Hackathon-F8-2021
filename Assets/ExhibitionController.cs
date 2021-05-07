using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackathonUtils;
public class ExhibitionController : MonoBehaviour
{

    GameObject mainShow;
    public List<GameObject> backStage;
    public float floatRadius = 2;
    public float floatHeight = 2;
    // Start is called before the first frame update
    void Start()
    {
        int angleStep = 180 / (backStage.Count - 1);
        int currentAngle = 0;
        foreach(GameObject actor in backStage)
        {
            Renderer actorRenderer = actor.GetComponent<Renderer>();
            Vector2 position2D = Utils.GetPointOnCircle(new Vector2(transform.position.x, transform.position.z), floatRadius, currentAngle);
            Vector3 resultingTransform = new Vector3(position2D.x, transform.position.y, position2D.y);
            actor.transform.position = resultingTransform;
            Vector3 fromPlatFormToActor = transform.position - resultingTransform;
            Vector3 fromPlatFormToActorsRenderer = transform.position - actorRenderer.bounds.center;
            Vector3 dispalcementAdjustment = fromPlatFormToActor.magnitude > fromPlatFormToActorsRenderer.magnitude ? fromPlatFormToActor - fromPlatFormToActorsRenderer : fromPlatFormToActorsRenderer - fromPlatFormToActor;
            Vector3 finalTransform = resultingTransform + dispalcementAdjustment;
            actor.transform.position = finalTransform + new Vector3(0, floatHeight, 0);
            currentAngle += angleStep;

            Vector3 platformCenterDirection = (transform.position - actorRenderer.bounds.center).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(platformCenterDirection);

            //actor.transform.rotation.SetFromToRotation(finalTransform, transform.position);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}