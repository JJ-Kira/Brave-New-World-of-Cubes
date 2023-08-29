using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;



public class PassthroughVFX : MonoBehaviour
{
    public OVRPassthroughLayer OVRPassthrough;

    public TMP_Dropdown dropdown;
    private int _activePassthrough;


    [Header("Gradient")]
    public Gradient gradient;
    public List<GradientColorKey> colorKeys;
    public List<GradientAlphaKey> alphaKeys;

    public RawImage sliderParent;
    public GameObject sliderObject;

    public Texture2D gradientTexture;

    public Slider[] gradientColorSliders;
    private List<Slider> _gradientSliders;

    private int lastSelectedSlider = 0;

    [SerializeField]
    public PassthroughData passthroughData = new();

    void Start()
    {
        gradient = new Gradient();

        /*
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        this.colorKeys = new List<GradientColorKey>();

        for (int i = 0; i < 2; i++)
        {
            GradientColorKey colorKey = new GradientColorKey()
            {
                color = new Color(i ,i ,i , 1),
                time = i
            };
            this.colorKeys.Add(colorKey);

            this.CreateSliderObject(i, colorKey);

            //spawn slider Object
        }

        this.UpdateColorSliders(this.colorKeys[this.lastSelectedSlider].color);
        this.UpdateGradient();*/

        this.LoadData();
        this.UpdateDropdown(0);
    }

    private void OnEnable()
    {
        if (this.passthroughData != null)
            UpdateDropdown(0);
    }

    public void CreateNewPreset()
    {
        if (this.passthroughData != null)
        {
            Passthrough newPassthrough = new Passthrough
            {
                name = "Passthrough " + this.passthroughData.passthroughs.Count,
                opacity = 1f,
                edgeRendering = false,
                edgeColor = new ColorData().ConvertVector4(Color.white),
                contrast = 0,
                brightness = 0,
                posterize = 0,
                gradientMode = (int)GradientMode.Blend,
                gradient = new List<GradientPoint>
                {
                    new GradientPoint() { position = 0, color = new ColorData().ConvertVector4(Color.black) },
                    new GradientPoint() { position = 1, color = new ColorData().ConvertVector4(Color.white) }
                }
            };

            this.passthroughData.passthroughs.Add(newPassthrough);
            this.UpdateDropdown(this.passthroughData.passthroughs.Count - 1);
        }
    }

    private void UpdateDropdown(int active)
    {
        if(this.dropdown != null)
        {
            this.dropdown.options.Clear();
            for (int i = 0; i < this.passthroughData.passthroughs.Count; i++)
                this.dropdown.options.Add(new TMP_Dropdown.OptionData() { text = this.passthroughData.passthroughs[i].name });

            this.dropdown.value = active;
            this.dropdown.RefreshShownValue();
            this._activePassthrough = active;
        }
        this.ShowNewData();
    }

    void ShowNewData()
    {
        Passthrough passthrough = this.passthroughData.passthroughs[this._activePassthrough];

        //delete all gradient stuff
        if (this._gradientSliders != null)
        {
            for (int i = 0; i < this._gradientSliders.Count; i++)
            {
                Destroy(this._gradientSliders[i].gameObject);
            }
        }

        //create new
        this._gradientSliders = new List<Slider>();
        this.colorKeys = new List<GradientColorKey>();
        this.alphaKeys = new List<GradientAlphaKey>();

        for (int i = 0; i < passthrough.gradient.Count; i++)
        {
            GradientColorKey colorKey = new GradientColorKey()
            {
                color = passthrough.gradient[i].color.GetColor(),
                time = passthrough.gradient[i].position
            };
            this.colorKeys.Add(colorKey);

            GradientAlphaKey alphaKey = new GradientAlphaKey()
            {
                alpha = (float)passthrough.gradient[i].color.a,
                time = passthrough.gradient[i].position
            };
            this.alphaKeys.Add(alphaKey);

            this.CreateSliderObject(i, colorKey);

            //spawn slider Object
        }
        this.lastSelectedSlider = 0;

        this.gradient.mode = (GradientMode)passthrough.gradientMode;
        this.UpdateColorSliders(this.colorKeys[this.lastSelectedSlider].color, this.alphaKeys[this.lastSelectedSlider].alpha);
        this.UpdateGradient();

        if (this.OVRPassthrough != null)
        {
            this.OVRPassthrough.textureOpacity = passthrough.opacity;
            this.OVRPassthrough.edgeRenderingEnabled = passthrough.edgeRendering;
            this.OVRPassthrough.edgeColor = passthrough.edgeColor.GetColor();

            this.OVRPassthrough.colorMapEditorContrast = passthrough.contrast;
            this.OVRPassthrough.colorMapEditorBrightness = passthrough.brightness;
            this.OVRPassthrough.colorMapEditorPosterize = passthrough.posterize;
        }
    }

    public void ChangeDropdown(int value)
    {
        this._activePassthrough = value;
        this.ShowNewData();
    }


    public void SetOpacity(float value)
    {
        if (this.OVRPassthrough != null)
            this.OVRPassthrough.textureOpacity = value;

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].opacity = value;
    }

    public void SetContrast(float value)
    {
        if (this.OVRPassthrough != null)
            this.OVRPassthrough.colorMapEditorContrast = value;

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].contrast = value;
    }

    public void SetPosterize(float value)
    {
        if (this.OVRPassthrough != null)
            this.OVRPassthrough.colorMapEditorPosterize = value;

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].posterize = value;
    }

    public void SetBrightness(float value)
    {
        if (this.OVRPassthrough != null)
            this.OVRPassthrough.colorMapEditorBrightness = value;

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].brightness = value;
    }


    public void ToggleEdgeRendering(bool value)
    {
        if (this.OVRPassthrough != null)
            this.OVRPassthrough.edgeRenderingEnabled = value;

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].edgeRendering = value;
    }

    public void SetRedEdgeColor(float value)
    {
        if (this.OVRPassthrough != null)
        {
            Color col = new Color(value, this.OVRPassthrough.edgeColor.g, this.OVRPassthrough.edgeColor.b, this.OVRPassthrough.edgeColor.a);
            this.OVRPassthrough.edgeColor = col;

            if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
                this.passthroughData.passthroughs[this._activePassthrough].edgeColor.ConvertVector4(col);
        }
    }
    public void SetGreenEdgeColor(float value)
    {
        if (this.OVRPassthrough != null)
        {
            Color col = new Color(this.OVRPassthrough.edgeColor.r, value, this.OVRPassthrough.edgeColor.b, this.OVRPassthrough.edgeColor.a);
            this.OVRPassthrough.edgeColor = col;

            if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
                this.passthroughData.passthroughs[this._activePassthrough].edgeColor.ConvertVector4(col);
        }
    }
    public void SetBlueEdgeColor(float value)
    {
        if (this.OVRPassthrough != null)
        {
            Color col = new Color(this.OVRPassthrough.edgeColor.r, this.OVRPassthrough.edgeColor.g, value, this.OVRPassthrough.edgeColor.a);
            this.OVRPassthrough.edgeColor = col;

            if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
                this.passthroughData.passthroughs[this._activePassthrough].edgeColor.ConvertVector4(col);
        }
    }
    public void SetAlphaEdgeColor(float value)
    {
        if (this.OVRPassthrough != null)
        {
            Color col = new Color(this.OVRPassthrough.edgeColor.r, this.OVRPassthrough.edgeColor.g, this.OVRPassthrough.edgeColor.b, value);
            this.OVRPassthrough.edgeColor = col;

            if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
                this.passthroughData.passthroughs[this._activePassthrough].edgeColor.ConvertVector4(col);
        }
    }


    #region gradient
    public void ChangeGradientMode(int value)
    {
        this.gradient.mode = (GradientMode)value;
        this.UpdateGradient();

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].gradientMode = value;
    }

    void UpdateGradient()
    {
        this.gradient.SetKeys(this.colorKeys.ToArray(), this.alphaKeys.ToArray());
        if (this.OVRPassthrough != null)
            this.OVRPassthrough.colorMapEditorGradient = this.gradient;

        if (this.gradientTexture == null)
        {
            this.gradientTexture = new Texture2D(64, 1);
            this.gradientTexture.wrapMode = TextureWrapMode.Clamp;
            this.sliderParent.texture = this.gradientTexture;
        }
        if (this.gradientTexture != null)
        {
            for (int i = 0; i < this.gradientTexture.width; i++)
                this.gradientTexture.SetPixel(i, 0, this.gradient.Evaluate((float)i / (this.gradientTexture.width - 1)));
            this.gradientTexture.Apply();
        }
    }

    public void CreateSliderObject(int index, GradientColorKey colorKey)
    {
        GameObject sliderObj = Instantiate(this.sliderObject, this.sliderParent.transform);

        if (sliderObj.TryGetComponent(out Slider slider))
        {
            slider.value = colorKey.time;
            ColorBlock colorBlock = slider.colors;
            Color col = colorKey.color;
            col.a = 1f;
            colorBlock.normalColor = col;
            slider.colors = colorBlock;
            slider.onValueChanged.AddListener(delegate { this.OnGradientSliderChange(index); });

            if (this._gradientSliders == null)
                this._gradientSliders = new List<Slider>();

            this._gradientSliders.Add(slider);
        }
    }

    public void OnGradientSliderChange(int index)
    {
        GradientColorKey colorKey = this.colorKeys[index];
        colorKey.time = this._gradientSliders[index].value;
        this.colorKeys[index] = colorKey;

        GradientAlphaKey alphaKey = this.alphaKeys[index];
        alphaKey.time = this._gradientSliders[index].value;
        this.alphaKeys[index] = alphaKey;

        this.UpdateGradient();

        this.lastSelectedSlider = index;
        this.UpdateColorSliders(this.colorKeys[this.lastSelectedSlider].color, this.alphaKeys[this.lastSelectedSlider].alpha);

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].gradient[index].position = this._gradientSliders[index].value;
    }

    void UpdateColorSliders(Color color, float a)
    {
        if (this.gradientColorSliders.Length >= 3)
        {
            this.gradientColorSliders[0].value = color.r;
            this.gradientColorSliders[1].value = color.g;
            this.gradientColorSliders[2].value = color.b;
            this.gradientColorSliders[3].value = a;
        }
    }

    public void AddGradientSlider()
    {
        GradientColorKey colorKey = new GradientColorKey()
        {
            color = Color.white,
            time = 0.5f
        };
        this.colorKeys.Add(colorKey);

        GradientAlphaKey alphaKey = new GradientAlphaKey()
        {
            alpha = 1,
            time = 0.5f
        };
        this.alphaKeys.Add(alphaKey);

        this.CreateSliderObject(this.colorKeys.Count - 1, colorKey);

        this.UpdateGradient();

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
        {
            GradientPoint gradientPoint = new GradientPoint()
            {
                position = colorKey.time,
                color = new ColorData().ConvertVector4(colorKey.color)
            };
            this.passthroughData.passthroughs[this._activePassthrough].gradient.Add(gradientPoint);
        }
    }


    public void SetRedGradientColor(float value)
    {
        ColorBlock colorBlock = this._gradientSliders[this.lastSelectedSlider].colors;
        colorBlock.normalColor = new Color(value, colorBlock.normalColor.g, colorBlock.normalColor.b, colorBlock.normalColor.a);
        this._gradientSliders[this.lastSelectedSlider].colors = colorBlock;

        GradientColorKey colorKey = this.colorKeys[this.lastSelectedSlider];
        colorKey.color = colorBlock.normalColor;
        this.colorKeys[this.lastSelectedSlider] = colorKey;

        this.UpdateGradient();

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].gradient[this.lastSelectedSlider].color.ConvertVector4(colorBlock.normalColor);
    }

    public void SetGreenGradientColor(float value)
    {
        ColorBlock colorBlock = this._gradientSliders[this.lastSelectedSlider].colors;
        colorBlock.normalColor = new Color(colorBlock.normalColor.r, value, colorBlock.normalColor.b, colorBlock.normalColor.a);
        this._gradientSliders[this.lastSelectedSlider].colors = colorBlock;

        GradientColorKey colorKey = this.colorKeys[this.lastSelectedSlider];
        colorKey.color = colorBlock.normalColor;
        this.colorKeys[this.lastSelectedSlider] = colorKey;

        this.UpdateGradient();

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].gradient[this.lastSelectedSlider].color.ConvertVector4(colorBlock.normalColor);
    }
    public void SetBlueGradientColor(float value)
    {
        ColorBlock colorBlock = this._gradientSliders[this.lastSelectedSlider].colors;
        colorBlock.normalColor = new Color(colorBlock.normalColor.r, colorBlock.normalColor.g, value, colorBlock.normalColor.a);
        this._gradientSliders[this.lastSelectedSlider].colors = colorBlock;

        GradientColorKey colorKey = this.colorKeys[this.lastSelectedSlider];
        colorKey.color = colorBlock.normalColor;
        this.colorKeys[this.lastSelectedSlider] = colorKey;

        this.UpdateGradient();

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].gradient[this.lastSelectedSlider].color.ConvertVector4(colorBlock.normalColor);
    }

    public void SetAlphaGradientColor(float value)
    {
        GradientAlphaKey alphaKey = this.alphaKeys[this.lastSelectedSlider];
        alphaKey.alpha = value;
        this.alphaKeys[this.lastSelectedSlider] = alphaKey;

        this.UpdateGradient();

        if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
            this.passthroughData.passthroughs[this._activePassthrough].gradient[this.lastSelectedSlider].color.ChangeAlpha(alphaKey.alpha);
    }

    public void RemoveSelectedGradientSlider()
    {
        if (this._gradientSliders.Count > 1)
        {
            Destroy(this._gradientSliders[this.lastSelectedSlider].gameObject);
            this._gradientSliders.RemoveAt(this.lastSelectedSlider);

            this.colorKeys.RemoveAt(this.lastSelectedSlider);
            this.alphaKeys.RemoveAt(this.lastSelectedSlider);

            this.lastSelectedSlider = this._gradientSliders.Count - 1;

            this.UpdateGradient();

            if (this.passthroughData != null && this.passthroughData.passthroughs.Count > this._activePassthrough)
                this.passthroughData.passthroughs[this._activePassthrough].gradient.RemoveAt(this.lastSelectedSlider);
        }
    }
    #endregion



    public void LoadData()
    {
        /*if (PlayerPrefs.HasKey("Ap_Count"))
        {
            int appartmentCounter = PlayerPrefs.GetInt("Ap_Count");
            for (int i = 0; i < appartmentCounter; i++)
            {
                this.apartmentNames.Add(PlayerPrefs.GetString("Ap_" + i + "Name"));
            }
            this._activeApartment = PlayerPrefs.GetInt("Active_Ap");

        }*/
        if (System.IO.File.Exists(Application.persistentDataPath + "/PassthroughData.json"))
        {
            using (StreamReader r = new StreamReader(Application.persistentDataPath + "/PassthroughData.json"))
            {
                string json = r.ReadToEnd();

                Debug.Log(json);
                JsonUtility.FromJsonOverwrite(json, this.passthroughData);
            }
        }
    }

    public void SaveIntoJson()
    {
        string passtrough = JsonUtility.ToJson(this.passthroughData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PassthroughData.json", passtrough);
        Debug.Log(Application.persistentDataPath + "/PassthroughData.json - " + "saved");
    }
}