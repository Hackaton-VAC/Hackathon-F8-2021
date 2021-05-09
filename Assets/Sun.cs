using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Sun : MonoBehaviour
{

    // Start is called before the first frame update
#if UNITY_EDITOR
    private SerializedObject halo;
    void Start()
    {
        halo = new SerializedObject(gameObject.GetComponent("Halo"));

    }

    // Update is called once per frame
    public void Update()
    {
        halo.FindProperty("m_Size").floatValue = transform.localScale.x*1.4f;
        halo.ApplyModifiedProperties();
    }
#endif
}
