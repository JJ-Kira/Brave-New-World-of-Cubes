using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private List<Material> variants;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private RectTransform portalImage;
    private Transform cameraTransform;
    private List<MeshRenderer> cubeRenderers;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        SpawnCube();
    }

    public void SpawnCube()
    {
        Tween cubeSpawnTween = portalImage.DOScale(Vector3.one, 2.0f);
            var forwardOffset = cameraTransform.forward;
            var position = forwardOffset * 0.5f;

            cubeSpawnTween.onComplete = () =>
            {
                var cube = Instantiate(cubePrefab, new Vector3(position.x, 2.0f,
                    position.z), Quaternion.identity);
                var meshRenderer = cube.GetComponent<MeshRenderer>();
                meshRenderer.material = variants[Random.Range(0, variants.Count - 1)];
                cubeRenderers.Add(meshRenderer);
            };

            DOTween.Sequence()
                .Insert(0.0f,
                    portalImage.DOMove(
                        new Vector3(position.x, 1.5f, position.z), 0.01f))
                .Insert(0.01f, cubeSpawnTween)
                .Insert(2.5f, portalImage.DOScale(Vector3.zero, 1.5f));
    }

    public void ShuffleVariants()
    {
        foreach (var cubeRenderer in cubeRenderers)
            cubeRenderer.material = variants[Random.Range(0, variants.Count - 1)];
    }
}
