using DG.Tweening;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private RectTransform portalImage;
    [SerializeField] private GameObject cubePrefab;
    private Transform cameraTransform;
    
    void Start()
    {
        cameraTransform = Camera.main.transform;
        SpawnCube();
    }

    public void SpawnCube()
    {
        Tween cubeSpawnTween = portalImage.DOScale(Vector3.one, 2.0f);
        cubeSpawnTween.onComplete = () => Instantiate(cubePrefab, new Vector3(cameraTransform.position.x, cameraTransform.position.y + 2.0f, cameraTransform.position.z + 1.0f), Quaternion.identity);
        
        DOTween.Sequence()
            .Insert(0.0f, portalImage.DOLocalMove(new Vector3(cameraTransform.position.x + 1.0f, cameraTransform.position.y, 0.0f), 0.01f))
            .Insert(0.01f, cubeSpawnTween)
            .Insert(2.5f, portalImage.DOScale(Vector3.zero, 1.5f));

        
    }
}
