using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Pot : MonoBehaviour
{
    public enum MixingState
    {
        BeforeMixing,
        IsMixing,
        DoneMixing
    }
    public MixingState mixState = MixingState.BeforeMixing;
    public void setMixState(MixingState newState)
    {
        mixState = newState;
    }

    public List<int> mixues = new List<int>();
    public int mixIngredientsNumber = 0;
    public float mixingTimer = 0;
    public float ingredientMixDuration = 60;
    float totalMixDuration;
    float byProductElasped = 0;
    public int byProductType = 0;
    public GameObject timerCanvas;
    public Image timerFiller;
    public Transform byProductSpawnPoint;
    public Renderer potContent;
    Color normalContent;
    void Start()
    {
        normalContent = potContent.material.color;
    }

    void Update()
    {
        UpdateInterval();
        UpdatePotUI();
    }

    void UpdatePotUI()
    {
        switch (mixState)
        {
            default:
            case MixingState.BeforeMixing:
                break;
            case MixingState.IsMixing:
                timerFiller.fillAmount = mixingTimer / totalMixDuration;
                break;
            case MixingState.DoneMixing:
                break;
        }
    }

    public void StartMixing()
    {
        totalMixDuration = ingredientMixDuration * mixIngredientsNumber;
        setMixState(MixingState.IsMixing);
    }

    public void FinishMixing()
    {
        mixingTimer = 0;
        setMixState(MixingState.DoneMixing);
        byProductType = 0;
    }

    public void ResetPotState()
    {
        timerFiller.fillAmount = 0;
        mixues.Clear();
        potContent.material.color = normalContent;
        setMixState(MixingState.BeforeMixing);
    }

    public void UpdateInterval()
    {
        switch (mixState)
        {
            default:
            case MixingState.BeforeMixing:
                break;
            case MixingState.IsMixing:
                mixingTimer += Time.deltaTime;
                byProductElasped += Time.deltaTime;
                if (byProductElasped >= ingredientMixDuration)
                {
                    MixueVersionOne.Instance.spawnByProduct(this);
                    byProductElasped -= ingredientMixDuration;
                }
                if (mixingTimer >= totalMixDuration)
                {
                    FinishMixing();
                }
                break;
            case MixingState.DoneMixing:
                if(mixIngredientsNumber <= 0)
                {
                    ResetPotState();
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IPotInteractableObject>())
        {
            IPotInteractableObject currentObject = other.gameObject.GetComponent<IPotInteractableObject>();
            Debug.Log("interacting with " + currentObject.name);
            switch (mixState)
            {
                default:
                case MixingState.BeforeMixing:
                    if (currentObject.Type == IPotInteractableObject.InteractableType.Mixue)
                    {
                        if (mixues.Count < 3)
                        {
                            MixueObject mixue = other.gameObject.GetComponent<MixueObject>();
                            MixueVersionOne.Instance.fillPot(this, mixue);
                        }
                    }
                    break;
                case MixingState.IsMixing:
                    break;
                case MixingState.DoneMixing:
                    if (currentObject.Type == IPotInteractableObject.InteractableType.Bottle)
                    {
                        Bottle bottle = other.gameObject.GetComponent<Bottle>();
                        bottle.bottleContent.material.color = potContent.material.color;
                        mixIngredientsNumber -= 1;
                    }
                    break;
            }
        }
    }
}
