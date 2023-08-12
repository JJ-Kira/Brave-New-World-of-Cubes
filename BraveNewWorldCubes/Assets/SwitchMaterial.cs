using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer))]
public class SwitchMaterial : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;

    private int currentMaterialIndex = 0;

    [SerializeField]
    private Button prevButton, nextButton;

    void Start()
    {
        prevButton.onClick.AddListener(PrevMaterial);
        nextButton.onClick.AddListener(NextMaterial);
    }

    private void NextMaterial()
    {
        currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;
        GetComponent<MeshRenderer>().material = materials[currentMaterialIndex];
    }

    private void PrevMaterial()
    {
        currentMaterialIndex = (currentMaterialIndex - 1 + materials.Length) % materials.Length;
        GetComponent<MeshRenderer>().material = materials[currentMaterialIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
