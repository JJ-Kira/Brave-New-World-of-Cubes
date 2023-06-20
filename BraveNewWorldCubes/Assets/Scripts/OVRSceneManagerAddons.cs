using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OVRSceneManagerAddons : MonoBehaviour
{
    protected OVRSceneManager SceneManager { get; private set; }

    private void Awake()
    {
        SceneManager = GetComponent<OVRSceneManager>();
    }

    void Start()
    {
        SceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
    }

    private void OnSceneModelLoadedSuccessfully()
    {
        StartCoroutine(AddCollidersAndFixClassifications());
    }

    private IEnumerator AddCollidersAndFixClassifications()
    {
        yield return new WaitForEndOfFrame(); // to avoid race condition (so that all prefabs have been spawned)

        MeshRenderer[] allObjects = FindObjectsOfType<MeshRenderer>();

        foreach (var obj in allObjects)
        {
            if (obj.GetComponent<Collider>() == null)
            {
                obj.AddComponent<BoxCollider>();
            }
        }

        /*
        // fix upside down desks
        OVRSemanticClassification[] allClassifications = FindObjectsOfType<OVRSemanticClassification>().Where(c => c.Contains(OVRSceneManager.Classification.Desk)).ToArray();

        foreach (var classification in allClassifications)
        {
            transform.localPosition = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1);
        }
        */
    }
}
