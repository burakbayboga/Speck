using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using Newtonsoft.Json;

public class ShowcaseController : MonoBehaviour
{
    public static ShowcaseController instance;

    public Camera Camera;

    public SpriteRenderer StraightLine;
    public ParticleSystem[] StraightLineEdge;

    public Renderer Chaser;

    public Renderer BlackHole;
    public ParticleSystem BlackHoleScatter;
    public ParticleSystem BlackHoleConverge;

    public List<ParticleSystem> ElectricFence;
    public ParticleSystem Electric;
    public ParticleSystem ElectricHeadsUp;

    public ParticleSystem LeaperInactive;
    public ParticleSystem LeaperActive;

    public GameObject GradientColorKeyPrefab;
    public RectTransform GradientBaseTransform;
    public InputField DeleteColorKeyInputField;
    public Slider AddColorKeyTimeSlider;

    public List<GameObject> GradientColorKeysUI;
    ParticleSystem GradientSystemBeingEdited;

    GameObject ColorKeyBeingEdited;
    RectTransform ColorKeyBeingEditedRectTransform;
    GradientColorKey[] ColorKeyBuffer;
    Gradient GradientBuffer;
    Image ColorKeyUIImageCache;


    #region UIGameObjects
    public GameObject EditButtonsParent;

    public GameObject BlackHoleEditParent;
    public GameObject ChaserEditParent;
    public GameObject ElectricFenceEditParent;
    public GameObject ElectricFenceBaseButton;
    public GameObject ElectricFenceColorOverLifetimeButton;
    public GameObject ElectricFenceElectricButton;
    public GameObject ElectricFenceElectricHeadsUpButton;
    public GameObject LeaperEditParent;
    public GameObject StraightLineEditParent;

    public GameObject GradientEditParent;

    public Text NowEditingText;
    #endregion

    public ColorPicker ColorPicker;

    Editable CurrentlyBeingEdited;

    private void Awake()
    {
        instance = this;

        ColorPicker.onHSVChanged.AddListener(OnHSVChanged);
        CurrentlyBeingEdited = Editable.none;
        GradientColorKeysUI = new List<GameObject>();
    }

    private void Start()
    {
        InitColors();
    }

    private void InitColors()
    {
        string dataSerialized = PlayerPrefs.GetString(Utility.PrefsColorDataKey, string.Empty);
        if (dataSerialized == string.Empty)
        {
            return;
        }

        ColorData[] colorData = JsonConvert.DeserializeObject<ColorData[]>(dataSerialized);
        for (int i = 0; i < colorData.Length; i++)
        {
            ApplySavedColor(colorData[i]);
        }
    }

    private void ApplySavedColor(ColorData colorData)
    {
        switch (colorData.Target)
        {
            case Editable.blackHoleScatter:
                ParticleSystem.MainModule scatterModule = BlackHoleScatter.main;
                scatterModule.startColor = GetUnityColor(colorData.Color);
                break;
            case Editable.blackHoleConverge:
                ParticleSystem.MainModule convergeModule = BlackHoleConverge.main;
                convergeModule.startColor = GetUnityColor(colorData.Color);
                break;
            case Editable.blackHoleSprite:
                BlackHole.material.SetColor("_Color", GetUnityColor(colorData.Color));
                break;
            case Editable.chaserBase:
                Chaser.material.SetColor("_BaseColor", GetUnityColor(colorData.Color));
                break;
            case Editable.chaserOutline:
                Chaser.material.SetColor("_OutlineColor", GetUnityColor(colorData.Color));
                break;
            case Editable.straightLineSprite:
                StraightLine.color = GetUnityColor(colorData.Color);
                break;
            case Editable.straightLineEdge:
                ParticleSystem.MainModule edgeModule;
                for (int i = 0; i < 3; i++)
                {
                    edgeModule = StraightLineEdge[i].main;
                    edgeModule.startColor = GetUnityColor(colorData.Color);
                }
                break;
            case Editable.electricFenceBase:
                ParticleSystem.MainModule baseModule;
                for (int i = 0; i < ElectricFence.Count; i++)
                {
                    baseModule = ElectricFence[i].main;
                    baseModule.startColor = GetUnityColor(colorData.Color);
                }
                break;
            case Editable.electricFenceGradient:
                ParticleSystem.ColorOverLifetimeModule colModuleElectricBase;
                for (int i = 0; i < ElectricFence.Count; i++)
                {
                    colModuleElectricBase = ElectricFence[i].colorOverLifetime;
                    colModuleElectricBase.color = GetUnityGradient(colorData.Gradient);
                }
                break;
            case Editable.electricFenceElectric:
                ParticleSystem.ColorOverLifetimeModule colModuleElectric = Electric.colorOverLifetime;
                colModuleElectric.color = GetUnityGradient(colorData.Gradient);
                break;
            case Editable.electricFenceHeadsup:
                ParticleSystem.MainModule headsupModule = ElectricHeadsUp.main;
                headsupModule.startColor = GetUnityColor(colorData.Color);
                break;
            case Editable.leaperInactiveGradient:
                ParticleSystem.ColorOverLifetimeModule colModuleLeaperInactive = LeaperInactive.colorOverLifetime;
                colModuleLeaperInactive.color = GetUnityGradient(colorData.Gradient);
                break;
            case Editable.leaperActiveGradient:
                ParticleSystem.ColorOverLifetimeModule colModuleLeaperActive = LeaperActive.colorOverLifetime;
                colModuleLeaperActive.color = GetUnityGradient(colorData.Gradient);
                break;
            case Editable.background:
                Camera.backgroundColor = GetUnityColor(colorData.Color);
                break;
            default:
                break;
        }
    }

    public void OnHSVChanged(float h, float s, float v)
    {
        switch (CurrentlyBeingEdited)
        {
            case Editable.none:
                break;
            case Editable.blackHoleScatter:
                ParticleSystem.MainModule scatterModule = BlackHoleScatter.main;
                scatterModule.startColor = Color.HSVToRGB(h, s, v);
                break;
            case Editable.blackHoleConverge:
                ParticleSystem.MainModule convergeModule = BlackHoleConverge.main;
                convergeModule.startColor = Color.HSVToRGB(h, s, v);
                break;
            case Editable.blackHoleSprite:
                BlackHole.material.SetColor("_Color", Color.HSVToRGB(h, s, v));
                break;
            case Editable.blackHoleComposite:
                Color color = Color.HSVToRGB(h, s, v);
                ParticleSystem.MainModule scatterModuleC = BlackHoleScatter.main;
                scatterModuleC.startColor = color;
                ParticleSystem.MainModule convergeModuleC = BlackHoleConverge.main;
                convergeModuleC.startColor = color;
                BlackHole.material.SetColor("_Color", color);
                break;
            case Editable.chaserBase:
                Chaser.material.SetColor("_BaseColor", Color.HSVToRGB(h, s, v));
                break;
            case Editable.chaserOutline:
                Chaser.material.SetColor("_OutlineColor", Color.HSVToRGB(h, s, v));
                break;
            case Editable.straightLineSprite:
                StraightLine.color = Color.HSVToRGB(h, s, v);
                break;
            case Editable.straightLineEdge:
                ParticleSystem.MainModule edgeModule;
                for (int i = 0; i < 3; i++)
                {
                    edgeModule = StraightLineEdge[i].main;
                    edgeModule.startColor = Color.HSVToRGB(h, s, v);
                }
                break;
            case Editable.electricFenceBase:
                ParticleSystem.MainModule baseModule;
                for (int i = 0; i < ElectricFence.Count; i++)
                {
                    baseModule = ElectricFence[i].main;
                    baseModule.startColor = Color.HSVToRGB(h, s, v);
                }
                break;
            case Editable.electricFenceGradient:
                break;
            case Editable.electricFenceElectric:
                break;
            case Editable.electricFenceHeadsup:
                ParticleSystem.MainModule headsupModule = ElectricHeadsUp.main;
                headsupModule.startColor = Color.HSVToRGB(h, s, v);
                break;
            case Editable.leaperInactiveGradient:
                break;
            case Editable.leaperActiveGradient:
                break;
            case Editable.colorKey:
                int index = GradientColorKeysUI.IndexOf(ColorKeyBeingEdited);
                Color rgbColor = Color.HSVToRGB(h, s, v);
                ColorKeyBuffer[index].color = rgbColor;
                GradientBuffer.SetKeys(ColorKeyBuffer, GradientBuffer.alphaKeys);
                ParticleSystem.ColorOverLifetimeModule colModuleGeneric = GradientSystemBeingEdited.colorOverLifetime;
                colModuleGeneric.color = GradientBuffer;
                ColorKeyUIImageCache.color = rgbColor;
                break;
            case Editable.background:
                Camera.backgroundColor = Color.HSVToRGB(h, s, v);
                break;
            default:
                break;
        }
    }

    #region EditSelections

    public void OnEditBlackHoleClicked()
    {
        EditButtonsParent.SetActive(false);
        BlackHoleEditParent.SetActive(true);
        NowEditingText.text = "Now editing: black hole";
    }

    public void OnEditChaserClicked()
    {
        EditButtonsParent.SetActive(false);
        ChaserEditParent.SetActive(true);
        NowEditingText.text = "Now editing: chaser";
    }

    public void OnEditElectricFenceClicked()
    {
        EditButtonsParent.SetActive(false);
        ElectricFenceEditParent.SetActive(true);
        NowEditingText.text = "Now editing: electric fence";
    }

    public void OnEditLeaperClicked()
    {
        EditButtonsParent.SetActive(false);
        LeaperEditParent.SetActive(true);
        NowEditingText.text = "Now editing: leaper";
    }

    public void OnEditStraightLineClicked()
    {
        EditButtonsParent.SetActive(false);
        StraightLineEditParent.SetActive(true);
        NowEditingText.text = "Now editing: straight line";
    }

    public void OnEditBackgroundClicked()
    {
        EditButtonsParent.SetActive(false);
        CurrentlyBeingEdited = Editable.background;
        NowEditingText.text = "Now editing: background";
    }

    #endregion

    #region GradientEdits

    private void GradientEditInit()
    {
        switch (CurrentlyBeingEdited)
        {
            case Editable.electricFenceGradient:
                GradientSystemBeingEdited = ElectricFence[0];
                break;
            case Editable.electricFenceElectric:
                GradientSystemBeingEdited = Electric;
                break;
            case Editable.leaperInactiveGradient:
                GradientSystemBeingEdited = LeaperInactive;
                break;
            case Editable.leaperActiveGradient:
                GradientSystemBeingEdited = LeaperActive;
                break;
            default:
                break;
        }

        for (int i = 0; i < GradientColorKeysUI.Count; i++)
        {
            Destroy(GradientColorKeysUI[i]);
        }
        GradientColorKeysUI.Clear();

        GradientColorKey[] keys = GradientSystemBeingEdited.colorOverLifetime.color.gradient.colorKeys;

        for (int i = 0; i < keys.Length; i++)
        {
            GenerateGradientColorKeyImage(keys[i]);
        }
    }

    private void GenerateGradientColorKeyImage(GradientColorKey colorKey)
    {
        GameObject newColorKey = Instantiate(GradientColorKeyPrefab, GradientBaseTransform);
        newColorKey.GetComponent<Image>().color = colorKey.color;

        float newColorKeyPosX = Mathf.Lerp(0f, GradientBaseTransform.rect.width, colorKey.time);
        newColorKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(newColorKeyPosX, 0f);

        EventTrigger trigger = newColorKey.GetComponent<EventTrigger>();
        EventTrigger.Entry entryDrag = new EventTrigger.Entry();
        entryDrag.eventID = EventTriggerType.Drag;
        entryDrag.callback.AddListener((ped) => DragColorKey((PointerEventData)ped));
        trigger.triggers.Add(entryDrag);

        EventTrigger.Entry entryBeginDrag = new EventTrigger.Entry();
        entryBeginDrag.eventID = EventTriggerType.PointerDown;
        entryBeginDrag.callback.AddListener((ped) => BeginEditColorKey((PointerEventData)ped));
        trigger.triggers.Add(entryBeginDrag);

        EventTrigger.Entry entryClick = new EventTrigger.Entry();
        entryClick.eventID = EventTriggerType.PointerClick;
        entryClick.callback.AddListener((ped) => ClickColorKey((PointerEventData)ped));
        trigger.triggers.Add(entryClick);

        GradientColorKeysUI.Add(newColorKey);
        GradientColorKeysUI = GradientColorKeysUI.OrderBy(key => key.GetComponent<RectTransform>().anchoredPosition.x).ToList();
    }

    public void BeginEditColorKey(PointerEventData ped)
    {
        ColorKeyBeingEdited = ped.pointerEnter;
        ColorKeyBeingEditedRectTransform = ColorKeyBeingEdited.GetComponent<RectTransform>();
        GradientBuffer = GradientSystemBeingEdited.colorOverLifetime.color.gradient;
        ColorKeyBuffer = GradientBuffer.colorKeys;
        ColorKeyUIImageCache = ColorKeyBeingEdited.GetComponent<Image>();
    }

    public void DragColorKey(PointerEventData ped)
    {
        Vector2 currentPos = ColorKeyBeingEditedRectTransform.anchoredPosition;
        currentPos.x = Mathf.Clamp(currentPos.x + ped.delta.x, 0f, GradientBaseTransform.rect.width);
        ColorKeyBeingEditedRectTransform.anchoredPosition = currentPos;

        int colorKeyIndex = GradientColorKeysUI.IndexOf(ColorKeyBeingEdited);
        ColorKeyBuffer[colorKeyIndex].time = Mathf.InverseLerp(0f, GradientBaseTransform.rect.width, currentPos.x);
        GradientBuffer.SetKeys(ColorKeyBuffer, GradientSystemBeingEdited.colorOverLifetime.color.gradient.alphaKeys);

        ParticleSystem.ColorOverLifetimeModule colModule = GradientSystemBeingEdited.colorOverLifetime;
        colModule.color = GradientBuffer;

        GradientColorKeysUI = GradientColorKeysUI.OrderBy(key => key.GetComponent<RectTransform>().anchoredPosition.x).ToList();
    }

    public void ClickColorKey(PointerEventData ped)
    {
        CurrentlyBeingEdited = Editable.colorKey;
    }

    public void OnAddGradientKeyClicked()
    {
        Color newKeyColor = ColorPicker.CurrentColor;
        float newKeyNormalizedTime = AddColorKeyTimeSlider.value;
        GradientColorKey newColorKey = new GradientColorKey
        {
            color = newKeyColor,
            time = newKeyNormalizedTime
        };

        GradientColorKey[] oldKeys = GradientSystemBeingEdited.colorOverLifetime.color.gradient.colorKeys;
        GradientColorKey[] newKeys = new GradientColorKey[oldKeys.Length + 1];
        for (int i = 0; i < oldKeys.Length; i++)
        {
            newKeys[i] = oldKeys[i];
        }
        newKeys[newKeys.Length - 1] = newColorKey;

        Gradient newGradient = new Gradient();
        newGradient.SetKeys(newKeys, GradientSystemBeingEdited.colorOverLifetime.color.gradient.alphaKeys);
        ParticleSystem.ColorOverLifetimeModule colModule = GradientSystemBeingEdited.colorOverLifetime;
        colModule.color = newGradient;
        
        for (int i = 0; i < GradientColorKeysUI.Count; i++)
        {
            Destroy(GradientColorKeysUI[i]);
        }
        GradientColorKeysUI.Clear();

        for (int i = 0; i < newKeys.Length; i++)
        {
            GenerateGradientColorKeyImage(newKeys[i]);
        }

        GradientColorKeysUI = GradientColorKeysUI.OrderBy(key => key.GetComponent<RectTransform>().anchoredPosition.x).ToList();
    }

    public void OnDeleteGradientKeyClicked()
    {
        int toDeleteIndex;
        if (int.TryParse(DeleteColorKeyInputField.text, out toDeleteIndex) && toDeleteIndex < GradientColorKeysUI.Count)
        {
            GameObject toDeleteColorKey = GradientColorKeysUI[toDeleteIndex];
            GradientColorKeysUI.RemoveAt(toDeleteIndex);
            Destroy(toDeleteColorKey);

            GradientColorKey[] newKeys = new GradientColorKey[GradientColorKeysUI.Count];
            GradientColorKey[] currentKeys = GradientSystemBeingEdited.colorOverLifetime.color.gradient.colorKeys;
            for (int i = 0; i < currentKeys.Length; i++)
            {
                if (i < toDeleteIndex)
                {
                    newKeys[i] = currentKeys[i];
                }
                else if (i > toDeleteIndex)
                {
                    newKeys[i - 1] = currentKeys[i];
                }
            }

            Gradient newGradient = new Gradient();
            newGradient.SetKeys(newKeys, GradientSystemBeingEdited.colorOverLifetime.color.gradient.alphaKeys);

            ParticleSystem.ColorOverLifetimeModule colModule = GradientSystemBeingEdited.colorOverLifetime;
            colModule.color = newGradient;
        }
    }

    #endregion

    #region LeaperEdits

    public void OnLeaperInactiveClicked()
    {
        NowEditingText.text = "Now editing: leaper inactive";
        CurrentlyBeingEdited = Editable.leaperInactiveGradient;
        LeaperEditParent.SetActive(false);
        GradientEditParent.SetActive(true);
        GradientEditInit();
    }

    public void OnLeaperActiveClicked()
    {
        NowEditingText.text = "Now editing: leaper active";
        CurrentlyBeingEdited = Editable.leaperActiveGradient;
        LeaperEditParent.SetActive(false);
        GradientEditParent.SetActive(true);
        GradientEditInit();
    }

    #endregion

    #region ElectricFenceEdits

    public void OnElectricFenceBaseClicked()
    {
        CurrentlyBeingEdited = Editable.electricFenceBase;

        NowEditingText.text = "Now editing: electric fence base";
    }

    public void OnElectricFenceColorOverLifeTimeClicked()
    {
        ElectricFenceEditParent.SetActive(false);

        CurrentlyBeingEdited = Editable.electricFenceGradient;

        NowEditingText.text = "Now editing: electric fence coloroverlifetime";

        GradientEditParent.SetActive(true);
        GradientEditInit();
    }

    public void OnElectricFenceElectricClicked()
    {
        ElectricFenceEditParent.SetActive(false);

        CurrentlyBeingEdited = Editable.electricFenceElectric;

        NowEditingText.text = "Now editing: electric fence electric";

        GradientEditParent.SetActive(true);
        GradientEditInit();
    }

    public void OnElectricFenceElectricHeadsUpClicked()
    {
        CurrentlyBeingEdited = Editable.electricFenceHeadsup;

        NowEditingText.text = "Now editing: electric fence headsup";
    }

    #endregion

    #region StraightLineEdits

    public void OnStraightLineSpriteClicked()
    {
        NowEditingText.text = "Now editing: straight line sprite";
        CurrentlyBeingEdited = Editable.straightLineSprite;
    }

    public void OnStraightLineEdgeClicked()
    {
        NowEditingText.text = "Now editing: straight line edge";
        CurrentlyBeingEdited = Editable.straightLineEdge;
    }

    #endregion

    #region ChaserEdits

    public void OnChaserBaseClicked()
    {
        NowEditingText.text = "Now editing: chaser base";
        CurrentlyBeingEdited = Editable.chaserBase;
    }

    public void OnChaserOutlineClicked()
    {
        NowEditingText.text = "Now editing: chaser outline";
        CurrentlyBeingEdited = Editable.chaserOutline;
    }

    #endregion

    #region BlackHoleEdits

    public void OnBlackHoleScatterClicked()
    {
        NowEditingText.text = "Now editing: black hole scatter";
        CurrentlyBeingEdited = Editable.blackHoleScatter;
    }

    public void OnBlackHoleConvergeClicked()
    {
        NowEditingText.text = "Now editing: black hole converge";
        CurrentlyBeingEdited = Editable.blackHoleConverge;
    }

    public void OnBlackHoleSpriteClicked()
    {
        NowEditingText.text = "Now editing: black hole sprite";
        CurrentlyBeingEdited = Editable.blackHoleSprite;
    }

    public void OnBlackHoleCompositeClicked()
    {
        NowEditingText.text = "Now editing: black hole composite";
        CurrentlyBeingEdited = Editable.blackHoleComposite;
    }

    #endregion

    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("menu");
    }

    public void OnResetSelectionsButtonClicked()
    {
        EditButtonsParent.SetActive(true);
        BlackHoleEditParent.SetActive(false);
        ChaserEditParent.SetActive(false);
        ElectricFenceEditParent.SetActive(false);
        LeaperEditParent.SetActive(false);
        StraightLineEditParent.SetActive(false);
        GradientEditParent.SetActive(false);
        CurrentlyBeingEdited = Editable.none;

        NowEditingText.text = string.Empty;
    }

    public void OnResetColorDataClicked()
    {
        InitColors();
    }

    #region DataPersistence

    private void SaveColorData()
    {
        ColorData[] colorData = new ColorData[14];

        ColorData blackHoleScatterData = new ColorData
        {
            Color = GetSerializableColor(BlackHoleScatter.main.startColor.color),
            Target = Editable.blackHoleScatter
        };
        colorData[0] = blackHoleScatterData;

        ColorData blackHoleConvergeData = new ColorData
        {
            Color = GetSerializableColor(BlackHoleConverge.main.startColor.color),
            Target = Editable.blackHoleConverge
        };
        colorData[1] = blackHoleConvergeData;

        ColorData blackHoleSpriteData = new ColorData
        {
            Color = GetSerializableColor(BlackHole.material.GetColor("_Color")),
            Target = Editable.blackHoleSprite
        };
        colorData[2] = blackHoleSpriteData;

        ColorData chaserBaseData = new ColorData
        {
            Color = GetSerializableColor(Chaser.material.GetColor("_BaseColor")),
            Target = Editable.chaserBase
        };
        colorData[3] = chaserBaseData;

        ColorData chaserOutlineData = new ColorData
        {
            Color = GetSerializableColor(Chaser.material.GetColor("_OutlineColor")),
            Target = Editable.chaserOutline
        };
        colorData[4] = chaserOutlineData;

        ColorData straightLineSpriteData = new ColorData
        {
            Color = GetSerializableColor(StraightLine.color),
            Target = Editable.straightLineSprite
        };
        colorData[5] = straightLineSpriteData;

        ColorData straightLineEdgeData = new ColorData
        {
            Color = GetSerializableColor(StraightLineEdge[0].main.startColor.color),
            Target = Editable.straightLineEdge
        };
        colorData[6] = straightLineEdgeData;

        ColorData electricFenceBaseData = new ColorData
        {
            Color = GetSerializableColor(ElectricFence[0].main.startColor.color),
            Target = Editable.electricFenceBase
        };
        colorData[7] = electricFenceBaseData;

        ColorData electricFenceGradientData = new ColorData
        {
            Gradient = GetSerializableGradient(ElectricFence[0].colorOverLifetime.color.gradient),
            Target = Editable.electricFenceGradient
        };
        colorData[8] = electricFenceGradientData;

        ColorData electricFenceElectricData = new ColorData
        {
            Gradient = GetSerializableGradient(Electric.colorOverLifetime.color.gradient),
            Target = Editable.electricFenceElectric
        };
        colorData[9] = electricFenceElectricData;

        ColorData electricFenceHeadsupData = new ColorData
        {
            Color = GetSerializableColor(ElectricHeadsUp.main.startColor.color),
            Target = Editable.electricFenceHeadsup
        };
        colorData[10] = electricFenceHeadsupData;

        ColorData leaperInactiveData = new ColorData
        {
            Gradient = GetSerializableGradient(LeaperInactive.colorOverLifetime.color.gradient),
            Target = Editable.leaperInactiveGradient
        };
        colorData[11] = leaperInactiveData;

        ColorData leaperActiveData = new ColorData
        {
            Gradient = GetSerializableGradient(LeaperActive.colorOverLifetime.color.gradient),
            Target = Editable.leaperActiveGradient
        };
        colorData[12] = leaperActiveData;

        ColorData backgroundData = new ColorData
        {
            Color = GetSerializableColor(Camera.backgroundColor),
            Target = Editable.background
        };
        colorData[13] = backgroundData;

        string dataSerialized = JsonConvert.SerializeObject(colorData);
        PlayerPrefs.SetString(Utility.PrefsColorDataKey, dataSerialized);
    }

    private SerializableGradient GetSerializableGradient(Gradient gradient)
    {
        GradientColorKey[] unityKeys = gradient.colorKeys;
        SerializableColorKey[] keys = new SerializableColorKey[unityKeys.Length];
        for (int i = 0; i < unityKeys.Length; i++)
        {
            keys[i] = new SerializableColorKey
            {
                r = unityKeys[i].color.r,
                g = unityKeys[i].color.g,
                b = unityKeys[i].color.b,
                time = unityKeys[i].time
            };
        }

        SerializableGradient sg = new SerializableGradient
        {
            ColorKeys = keys
        };

        return sg;
    }

    private Gradient GetUnityGradient(SerializableGradient sg)
    {
        GradientColorKey[] colorKeys = new GradientColorKey[sg.ColorKeys.Length];

        for (int i = 0; i < colorKeys.Length; i++)
        {
            colorKeys[i] = new GradientColorKey
            {
                color = new Color(sg.ColorKeys[i].r, sg.ColorKeys[i].g, sg.ColorKeys[i].b),
                time = sg.ColorKeys[i].time
            };
        }

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1f, 0f);
        alphaKeys[1] = new GradientAlphaKey(1f, 1f);

        Gradient gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);

        return gradient;
    }

    private Color GetUnityColor(SerializableColor sc)
    {
        return new Color(sc.r, sc.g, sc.b);
    }

    private SerializableColor GetSerializableColor(Color color)
    {
        return new SerializableColor
        {
            r = color.r,
            g = color.g,
            b = color.b
        };
    }

    public void OnSaveDataClicked()
    {
        SaveColorData();
    }

    #endregion

}

public enum Editable
{
    none,
    blackHoleScatter,
    blackHoleConverge,
    blackHoleSprite,
    blackHoleComposite,
    chaserBase,
    chaserOutline,
    straightLineSprite,
    straightLineEdge,
    electricFenceBase,
    electricFenceGradient,
    electricFenceElectric,
    electricFenceHeadsup,
    leaperInactiveGradient,
    leaperActiveGradient,
    background,
    colorKey
}

public class ColorData
{
    public SerializableGradient Gradient;
    public SerializableColor Color;
    public Editable Target;
    
    public ColorData() { }
}

public class SerializableGradient
{
    public SerializableColorKey[] ColorKeys;

    public SerializableGradient() { }
}

public class SerializableColorKey
{
    public float r;
    public float g;
    public float b;
    public float time;

    public SerializableColorKey() { }
}

public class SerializableColor
{
    public float r;
    public float g;
    public float b;

    public SerializableColor() { }
}
