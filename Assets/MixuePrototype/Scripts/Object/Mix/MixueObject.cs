using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixueObject : MonoBehaviour
{
    public float targetHeight = .35f;
    public float gravity = 10;
    ConstantForce objectForce;
    public bool isPulled = false;
    Grabbable grabbableComponent;

    public MixueVersionOne.mixueNumber mixNumber = MixueVersionOne.mixueNumber.Red;
    public Renderer gfxRenderer;
    public bool gotMixed = false;
    void Start()
    {
        objectForce = GetComponent<ConstantForce>();
        grabbableComponent = GetComponent<Grabbable>();
        MixueVersionOne.Instance.mixueInit(this);
    }

    void Update()
    {
        if (grabbableComponent.BeingHeld) return;

        if (isPulled)
        {
            objectForce.force = new Vector3(0, 0, 0);
        }
        else
        {
            if (transform.position.y > targetHeight)
            {
                objectForce.force = new Vector3(0, -gravity, 0);
            }
            else
            {
                objectForce.force = new Vector3(0, gravity, 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gotMixed) return;
        if(collision.gameObject.GetComponent<MixueObject>())
        {
            MixueObject otherMixue = collision.gameObject.GetComponent<MixueObject>();
            otherMixue.gotMixed = true;
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
