using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ExhibitionController : MonoBehaviour
{

    GameObject mainShow;
    GameObject stage;
    GameObject dressingRoom;
    public List<GameObject> backStage;
    public float floatRadius = 2;
    public float floatHeight = 2;
    public float deltaRotaion = 1;
    public float rotationThreshold = 2;
    
    float targetHRotation = 0;
    float targetVRotation = 0;
    bool rotatingH = false;
    bool rotatingV = false;
    bool reshuffle = false;
    bool[] joining;
    bool[] removing;
    bool[] selecting;
    const string STAGE_NAME = "MainShowRotator";
    const string DRESSINGROOM_NAME = "BackstageRotator";
    // Start is called before the first frame update
    void Start()
    {
        joining = Enumerable.Repeat(false, backStage.Count).ToArray();
        removing = Enumerable.Repeat(false, backStage.Count).ToArray();
        selecting = Enumerable.Repeat(false, backStage.Count).ToArray();
        stage = transform.Find(STAGE_NAME).gameObject;
        dressingRoom = transform.Find(DRESSINGROOM_NAME).gameObject;
    }


    public void ArrangeBackstage()
    {
        int angleStep = 180 / (backStage.Count - 1);
        float currentAngle = transform.rotation.eulerAngles.y;
        foreach (GameObject actor in backStage)
        {
            Renderer actorRenderer = actor.GetComponent<Renderer>();
            Vector2 position2D = HackathonUtils.Utils.GetPointOnCircle(new Vector2(transform.position.x, transform.position.z), floatRadius, currentAngle);
            Vector3 resultingTransform = new Vector3(position2D.x, transform.position.y, position2D.y);
            actor.transform.position = resultingTransform;
            Vector3 fromPlatFormToActor = transform.position - resultingTransform;
            Vector3 fromPlatFormToActorsRenderer = transform.position - actorRenderer.bounds.center;
            Vector3 dispalcementAdjustment = fromPlatFormToActor.magnitude > fromPlatFormToActorsRenderer.magnitude ? fromPlatFormToActor - fromPlatFormToActorsRenderer : fromPlatFormToActorsRenderer - fromPlatFormToActor;
            Vector3 finalTransform = resultingTransform + dispalcementAdjustment;
            actor.transform.position = finalTransform + new Vector3(0, floatHeight, 0);
            currentAngle += angleStep;

            //Vector3 platformCenterDirection = (finalTransform - transform.position).normalized;

        }
    }

    public void SignalSHowRotation(bool vertical)
    {
        if (vertical) {
            rotatingV = true;
            targetHRotation = mainShow.transform.rotation.eulerAngles.z + 90;
        }
        else {
            rotatingH = true;
            targetVRotation = mainShow.transform.rotation.eulerAngles.y + 90;

        }
    }

    void RotateMainShow()
    {
        if (rotatingH)
        {
            if (Mathf.Abs(mainShow.transform.rotation.eulerAngles.z % 360) <= rotationThreshold)
            {
                Vector3 olValues = mainShow.transform.rotation.eulerAngles;
                mainShow.transform.rotation = Quaternion.Euler(new Vector3(olValues.x, olValues.y, targetHRotation));
                rotatingH = false;
                return;
            }
            mainShow.transform.Rotate(Vector3.forward, deltaRotaion);
        }
        if (rotatingV)
        {
            if (Mathf.Abs(mainShow.transform.rotation.eulerAngles.y % 360) <= rotationThreshold)
            {
                Vector3 olValues = mainShow.transform.rotation.eulerAngles;
                mainShow.transform.rotation = Quaternion.Euler(new Vector3(olValues.x, targetVRotation, olValues.z));
                rotatingV = false;
                return;
            }
            mainShow.transform.Rotate(Vector3.up, deltaRotaion);

        }
    }

    public void SetAsMainSHow(string actorName)
    {
        GameObject relevo;
        if (actorName == "Group")
        {
            relevo = backStage.Find((a) => a.transform.childCount > 0);
        }
        else
        {
            relevo = backStage.Find((a) => a.name == actorName);
        }
        backStage.Remove(relevo);
        relevo.transform.SetParent(stage.transform);

        if (mainShow != null)
        {
            backStage.Add(mainShow);
            mainShow.transform.SetParent(dressingRoom.transform);
        }

        mainShow = relevo;
        dressingRoom.GetComponent<Stagehand>().ReShuffle();
    }


    void SelectManualControl()
    {
        if (Input.GetKey(KeyCode.Keypad0)){
            SetAsMainSHow("Frontal Lobe");
        }
        if (Input.GetKey(KeyCode.Keypad1)){
            SetAsMainSHow("Parietal Lobe");
        }
        if (Input.GetKey(KeyCode.Keypad2)){
            SetAsMainSHow("Temporal Lobe");
        }
        if (Input.GetKey(KeyCode.Keypad3)){
            SetAsMainSHow("Occipital Lobe");
        }
        if (Input.GetKey(KeyCode.Keypad4)){
            SetAsMainSHow("Stem");
        }
        if (Input.GetKey(KeyCode.Keypad5)){
            SetAsMainSHow("Cerebellum");
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateMainShow();
        SelectManualControl();
    }
}
