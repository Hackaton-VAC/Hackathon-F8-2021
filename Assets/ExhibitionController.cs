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
    float timeCount = 0f;
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

    public void SignalShowRotation(HackathonUtils.Rotations direction)
    {
        switch (direction)
        {
            case HackathonUtils.Rotations.UP:
                rotatingV = true;
                targetVRotation = 90;
                deltaRotaion = deltaRotaion < 0 ? -deltaRotaion : deltaRotaion;
                break;
            case HackathonUtils.Rotations.DOWN:
                rotatingV = true;
                targetVRotation = 90;
                deltaRotaion = deltaRotaion < 0 ? deltaRotaion : -deltaRotaion;
                break;
            case HackathonUtils.Rotations.LEFT:
                rotatingH = true;
                targetHRotation = 90;
                deltaRotaion = deltaRotaion < 0 ? deltaRotaion : -deltaRotaion;
                break;
            case HackathonUtils.Rotations.RIGHT:
                rotatingH = true;
                targetHRotation = 90;
                deltaRotaion = deltaRotaion < 0 ? -deltaRotaion : deltaRotaion;
                break;
        }
    }

    void RotateMainShow()
    {
        if (rotatingH)
        {
            if (targetHRotation <= 0)
            {
                rotatingH = false;
                return;
            }
            mainShow.transform.Rotate(Vector3.forward, deltaRotaion);
            targetHRotation--;
        }
        if (rotatingV)
        {
            if (targetVRotation <= 0)
            {
                rotatingV = false;
                return;
            }
            mainShow.transform.Rotate(Vector3.right, deltaRotaion);
            targetVRotation--;
        }
    }

    public void SetAsMainSHow(string actorName)
    {
        // TODO PROTEGER CONTRA RELEVO NULO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        GameObject relevo;
        if (actorName == "Group")
        {
            relevo = backStage.Find((a) => a.transform.childCount > 0);
        }
        else
        {
            relevo = backStage.Find((a) => a.name == actorName);
        }
        if (relevo == null)
        {
            return;
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


    public void CollidePart(string toCollideName)
    {
        if (mainShow == null)
        {
            return;
        }
        GameObject grupo = backStage.Find((a) => a.transform.childCount > 0);
        GameObject toCollide = backStage.Find((a) => a.name == toCollideName);

        if (grupo != null)
        {
            if (grupo != mainShow)
            {
                if (toCollide != null)
                {
                    // Mainshow is not toCollide
                    backStage.Remove(toCollide);
                    toCollide.GetComponent<CollideController>().MergeTo(grupo);
                    SetAsMainSHow("Group");
                }
                else
                {
                    if (mainShow.name == toCollideName)
                    {
                        // ToCollide is main Show
                        mainShow.GetComponent<CollideController>().MergeTo(grupo);
                        grupo.transform.SetParent(stage.transform);
                        mainShow = grupo;
                    }
                    else
                    {
                        // toCollide does not exist or is inside group
                        return;
                    }
                }
            }else
            {
                if (toCollide != null)
                {
                    // bring it to the group
                    backStage.Remove(toCollide);
                    toCollide.GetComponent<CollideController>().MergeTo(mainShow);
                }else
                {
                    // toCollide is already in the group or doesn't exist
                    return;
                }
            }
        } else
        {
            if (toCollide != null)
            {
                // Group creation
                backStage.Remove(toCollide);
                toCollide.GetComponent<CollideController>().MergeTo(mainShow);
            }else
            {
                // Unexistent toCollide
                return;
            }
        }
    }

    public void RemovePartFromGroup(string toRemoveName)
    {
        if (mainShow == null)
        {
            return;
        }
        GameObject grupo = backStage.Find((a) => a.transform.childCount > 0);

        if (grupo != null)
        {
            Transform desheredado = grupo.transform.Find(toRemoveName);
            if (desheredado != null)
            {
                backStage.Add(desheredado.gameObject);
                desheredado.SetParent(dressingRoom.transform);
                dressingRoom.GetComponent<Stagehand>().ReShuffle();
            }
        }
        else
        {
            Transform desheredado = mainShow.transform.Find(toRemoveName);
            if ( desheredado != null)
            {
                backStage.Add(desheredado.gameObject);
                desheredado.SetParent(dressingRoom.transform);
                dressingRoom.GetComponent<Stagehand>().ReShuffle();
            }
        }
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
        if (Input.GetKey(KeyCode.Keypad6))
        {
            SetAsMainSHow("Group");
        }
    }

    void CollideManualControl()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            CollidePart("Frontal Lobe");
        }
        if (Input.GetKey(KeyCode.X))
        {
            CollidePart("Parietal Lobe");
        }
        if (Input.GetKey(KeyCode.C))
        {
            CollidePart("Temporal Lobe");
        }
        if (Input.GetKey(KeyCode.V))
        {
            CollidePart("Occipital Lobe");
        }
        if (Input.GetKey(KeyCode.B))
        {
            CollidePart("Stem");
        }
        if (Input.GetKey(KeyCode.N))
        {
            CollidePart("Cerebellum");
        }
    }

    void RemoveManualControl()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RemovePartFromGroup("Frontal Lobe");
        }
        if (Input.GetKey(KeyCode.S))
        {
            RemovePartFromGroup("Parietal Lobe");
        }
        if (Input.GetKey(KeyCode.D))
        {
            RemovePartFromGroup("Temporal Lobe");
        }
        if (Input.GetKey(KeyCode.F))
        {
            RemovePartFromGroup("Occipital Lobe");
        }
        if (Input.GetKey(KeyCode.G))
        {
            RemovePartFromGroup("Stem");
        }
        if (Input.GetKey(KeyCode.H))
        {
            RemovePartFromGroup("Cerebellum");
        }
    }

    void RotateManualControl()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            SignalShowRotation(HackathonUtils.Rotations.UP);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            SignalShowRotation(HackathonUtils.Rotations.DOWN);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            SignalShowRotation(HackathonUtils.Rotations.LEFT);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            SignalShowRotation(HackathonUtils.Rotations.RIGHT);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateMainShow();
        SelectManualControl();
        RotateManualControl();
        CollideManualControl();
        RemoveManualControl();
    }
}
