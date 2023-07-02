using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughManager : MonoBehaviour
{
    private PassthroughStylist _passthroughStylist;
    private OVRPassthroughLayer _passthroughLayer;
    void Awake()
    {
        _passthroughLayer.colorMapEditorType = OVRPassthroughLayer.ColorMapEditorType.None;
        _passthroughLayer.textureOpacity = 0;
        _passthroughStylist = gameObject.AddComponent<PassthroughStylist>();
        _passthroughStylist.Init(_passthroughLayer);
        
        PassthroughStylist.PassthroughStyle darkPassthroughStyle = new PassthroughStylist.PassthroughStyle(
            new Color(0, 0, 0, 0),
            1.0f,
            0.0f,
            0.0f,
            0.0f,
            true,
            Color.black,
            Color.black,
            Color.black);
        _passthroughStylist.ForcePassthroughStyle(darkPassthroughStyle);
    }

    private void Start()
    {
        VirtualRoom.Instance.ShowAllWalls(false);
        VirtualRoom.Instance.HideEffectMesh();
        StartCoroutine(PlayIntroPassthrough());
    }

    IEnumerator PlayIntroPassthrough()
    {
        yield return new WaitForSeconds(2);
        
        VirtualRoom.Instance.ShowDarkRoom(true);
        VirtualRoom.Instance.AnimateEffectMesh();
        
        PassthroughStylist.PassthroughStyle darkPassthroughStyle = new PassthroughStylist.PassthroughStyle(
            new Color(0, 0, 0, 0),
            1.0f,
            0.0f,
            0.0f,
            0.0f,
            true,
            Color.black,
            Color.black,
            Color.black);
        _passthroughStylist.ForcePassthroughStyle(darkPassthroughStyle);

        // fade in edges
        float timer = 0.0f;
        float lerpTime = 4.0f;
        while (timer <= lerpTime)
        {
            Debug.Log("Intro playing");
            timer += Time.deltaTime;

            Color edgeColor = Color.white;
            edgeColor.a = Mathf.Clamp01(timer / 3.0f); // fade from transparent
            _passthroughLayer.edgeColor = edgeColor;

            float normTime = Mathf.Clamp01(timer / lerpTime);

            VirtualRoom.Instance.SetEdgeEffectIntensity(normTime);

            // once lerpTime is over, fade in normal passthrough
            if (timer >= lerpTime)
            {
                PassthroughStylist.PassthroughStyle normalPassthrough = new PassthroughStylist.PassthroughStyle(
                    new Color(0, 0, 0, 0),
                    1.0f,
                    0.0f,
                    0.0f,
                    0.0f,
                    false,
                    Color.white,
                    Color.black,
                    Color.white);
                _passthroughStylist.ShowStylizedPassthrough(normalPassthrough, 5.0f);
            }
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);
    }
}
