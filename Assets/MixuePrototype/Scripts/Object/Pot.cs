using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : MonoBehaviour
{
    public List<int> mixues = new List<int>();
    public bool isDoneMixing = false;
    public bool isStartMixing = false;
    public int mixIngredientsNumber = 0;
    public int mixingTimer = 0;
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
        if (isDoneMixing)
        {
            if (mixIngredientsNumber == 0)
            {
                isDoneMixing = false;
                mixues.Clear();
                timerFiller.fillAmount = 0;
                potContent.material.color = normalContent;
            }
        }
        else
        {
            if (mixIngredientsNumber >= 1)
            {
                isStartMixing = true;
            }
        }

        if (isStartMixing)
        {
            timerFiller.fillAmount = (float)mixingTimer / (float)mixIngredientsNumber;
            if (mixingTimer / mixIngredientsNumber == 1)
            {
                mixingTimer = 0;
                isStartMixing = false;
                isDoneMixing = true;
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MixueObject>())
        {
            if (!isDoneMixing && mixues.Count < 3)
            {
                MixueObject mixue = other.gameObject.GetComponent<MixueObject>();
                MixueVersionOne.Instance.fillPot(this, mixue);
            }
        }
        else if (other.gameObject.GetComponent<Bottle>())
        {
            if(isDoneMixing)
            {
                Bottle bottle = other.gameObject.GetComponent<Bottle>();
                bottle.bottleContent.material.color = potContent.material.color;
                mixIngredientsNumber -= 1;
            }
        }
    }
}
