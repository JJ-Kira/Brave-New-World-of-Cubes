using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    [SerializeField] private Transform eyeAnchor;
    void Start()
    {
        transform.position = eyeAnchor.position;
    }
}
