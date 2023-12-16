using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MixueVersionOne : MonoBehaviour
{
    #region mixueObject
    public enum mixueNumber
    {
        Red = 1,
        Green = 2,
        Yellow = 3,
        Blue = 4,
        Purple = 5,
        Cyan = 6

    }
    [HideInInspector]
    public int lastMix = 0;
    [Header("Mixue Properties")]
    public Material[] mixueMaterials;
    public GameObject mixuePrefab;
    public void mixItUp(MixueObject a, MixueObject b)
    {
        mixueNumber newMixue = (mixueNumber)((int)a.mixNumber + (int)b.mixNumber);
        Vector3 mixPosition = Vector3.Lerp(a.transform.position, b.transform.position, 0.5f);
        GameObject newMix = Instantiate(mixuePrefab, mixPosition, Quaternion.identity);
        Destroy(a.gameObject);
        Destroy(b.gameObject);
        MixueObject newMixueObj = newMix.GetComponent<MixueObject>();
        newMixueObj.mixNumber = newMixue;
        mixueInit(newMixueObj);
    }

    public void mixueInit(MixueObject mixue)
    {
        int actualMix = (int)mixue.mixNumber - 1;
        mixue.gfxRenderer.material = mixueMaterials[actualMix];
        mixue.transform.localScale = Vector3.one + (Vector3.one * actualMix * .25f);
    }
    #endregion

    #region game timer
    [Header("Time Properties")]
    public int timePerDay = 9;
    public int dayTimer;
    AudioSource timeAudioSource;
    [Tooltip("(Optional) Play this clip when starting to slow time")]
    public AudioClip SlowTimeClip;
    [Tooltip("(Optional) Play this clip when ending slow mo")]
    public AudioClip SpeedupTimeClip;
    [Tooltip("Timescale to slow down to if slow down key is pressed")]
    public float SlowTimeScale = 0.5f;
    [Tooltip("Timescale to speed up to if speed up key is pressed")]
    public float SpeedTimeScale = 2f;
    [Tooltip("If true, will set Time.fixedDeltaTime to the device refresh rate")]
    public bool SetFixedDelta = false;
    float originalFixedDelta;
    bool _slowingTime = false;
    public void resetDay()
    {
        dayTimer = timePerDay;
    }

    public void timePass()
    {
        dayTimer -= 1;
        foreach (var item in pots)
        {
            if (item.isStartMixing)
            {
                item.mixingTimer += 1;
                spawnByProduct(item);
            }
        }
    }
    public void SlowTime()
    {

        if (!_slowingTime)
        {
            // Play Slow time clip
            timeAudioSource.clip = SlowTimeClip;
            timeAudioSource.Play();

            // Haptics
            if (SpeedupTimeClip)
            {
                InputBridge.Instance.VibrateController(0.1f, 0.2f, SpeedupTimeClip.length, ControllerHand.Left);
            }

            Time.timeScale = SlowTimeScale;
            Time.fixedDeltaTime = originalFixedDelta * Time.timeScale;

            _slowingTime = true;
        }
    }

    public void NormalTime()
    {

        if (!_slowingTime)
        {
            // Play Slow time clip
            timeAudioSource.clip = SlowTimeClip;
            timeAudioSource.Play();

            // Haptics
            if (SpeedupTimeClip)
            {
                InputBridge.Instance.VibrateController(0.1f, 0.2f, SpeedupTimeClip.length, ControllerHand.Left);
            }

            Time.timeScale = 1;
            Time.fixedDeltaTime = originalFixedDelta;

            _slowingTime = true;
        }
    }

    public void SpeedTime()
    {

        if (!_slowingTime)
        {
            // Play Slow time clip
            timeAudioSource.clip = SlowTimeClip;
            timeAudioSource.Play();

            // Haptics
            if (SpeedupTimeClip)
            {
                InputBridge.Instance.VibrateController(0.1f, 0.2f, SpeedupTimeClip.length, ControllerHand.Left);
            }

            Time.timeScale = SlowTimeScale;
            Time.fixedDeltaTime = originalFixedDelta * Time.timeScale;

            _slowingTime = true;
        }
    }
    #endregion

    #region pots
    [Header("Pot Properties")]
    public List<Pot> pots = new List<Pot>();

    public void fillPot(Pot pot, MixueObject mixue)
    {

        pot.mixues.Add((int)mixue.mixNumber);
        pot.mixIngredientsNumber += 1;
        Destroy(mixue.gameObject);
        //Color newContent = Color.Lerp(mixueMaterials[(int)mixue.mixNumber - 1].color, pot.potContent.material.color, .5f);
        float h1, h2, s, v;
        Color.RGBToHSV(mixueMaterials[(int)mixue.mixNumber - 1].color, out h1, out s, out v);
        Color.RGBToHSV(pot.potContent.material.color, out h2, out s, out v);
        float n = Mathf.Lerp(h1, h2, .5f);
        Color newContent = Color.HSVToRGB(n, s, v);
        pot.potContent.material.color = newContent;
    }

    public void spawnByProduct(Pot potContent)
    {
        int byProductType = potContent.mixues[potContent.mixingTimer - 1];
        byProductType = byProductType - 1; // method will change with design

        if (byProductType <= 0) return;
        MixueObject byProduct = Instantiate(mixuePrefab, potContent.byProductSpawnPoint.position, Quaternion.identity).GetComponent<MixueObject>();
        byProduct.mixNumber = (mixueNumber)byProductType;
        mixueInit(byProduct);


    }
    #endregion

    #region initialization
    public static MixueVersionOne Instance;
    private void Awake()
    {
        if (!Instance) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }
    void Start()
    {
        resetDay();
        lastMix = (int)Enum.GetValues(typeof(mixueNumber)).Cast<mixueNumber>().Last();

        if (SetFixedDelta)
        {
            Time.fixedDeltaTime = (Time.timeScale / UnityEngine.XR.XRDevice.refreshRate);
        }

        originalFixedDelta = Time.fixedDeltaTime;

        timeAudioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region update
    void Update()
    {
        
    }
    #endregion
}
