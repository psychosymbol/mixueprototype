using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class MixueObject : IPotInteractableObject
{
    public InteractableType Type;
    public float targetHeight = 3f;
    public float targetHeightThreshold = .1f;
    public float gravity = 10;
    ConstantForce objectForce;
    public bool isPulled = false;
    Grabbable grabbableComponent;

    public MixueVersionOne.mixueNumber mixNumber = MixueVersionOne.mixueNumber.Red;
    public Renderer gfxRenderer;
    public bool gotMixed = false;


    public GameObject targetHeightDebugSphere;
    void Start()
    {
        objectForce = GetComponent<ConstantForce>();
        grabbableComponent = GetComponent<Grabbable>();
        MixueVersionOne.Instance.mixueInit(this);
        if(targetHeightDebugSphere.activeInHierarchy)
        targetHeightDebugSphere.transform.parent = null;
    }

    public void StopPulling()
    {
        isPulled = false;
    }

    void Update()
    {
        if (grabbableComponent.BeingHeld)
        {
            return;
        }

        if (isPulled)
        {

        }
        else
        {
            floating();

        }

        if (targetHeightDebugSphere.activeInHierarchy)
        {
            targetHeightDebugSphere.transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
        }
    }
    void floating()
    {
        float currentHeight = transform.position.y;
        if (Mathf.Abs(currentHeight - targetHeight) > targetHeightThreshold)
        {
            objectForce.force = new Vector3(0, gravity * (currentHeight < targetHeight ? 1 : -1), 0);
        }
        else
        {
            objectForce.force = Vector3.zero;
        }
    }
        

    private void OnTriggerEnter(Collider other)
    {
        if (gotMixed) return;
        if (other.gameObject.GetComponent<MixueObject>())
        {
            MixueObject otherMixue = other.gameObject.GetComponent<MixueObject>();
            otherMixue.gotMixed = true;
            gotMixed = true;
            if ((int)mixNumber + (int)otherMixue.mixNumber > MixueVersionOne.Instance.lastMix)
            {
                otherMixue.gotMixed = false;
                return;
            }
            else
            {
                MixueVersionOne.Instance.mixItUp(this, otherMixue);
            }
        }
    }
}
