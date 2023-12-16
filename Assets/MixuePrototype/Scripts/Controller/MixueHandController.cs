using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(HandController))]
public class MixueHandController : MonoBehaviour
{
    HandController handController;
    public LineRenderer grabPointer;
    GameObject rayTarget;
    public float pullVelocity = 2;
    void Start()
    {
        handController = GetComponent<HandController>();
    }

    void Update()
    {
        if (handController.HoldingObject())
        {

        }
        else
        {
            if (handController.GripAmount == 1 || Input.GetKey(KeyCode.X))
            {
                grabPointer.gameObject.SetActive(true);
                //raycast
                Ray ray = new Ray(grabPointer.transform.position, grabPointer.transform.forward);
                RaycastHit hit;
                int layerMask = 1 << 10;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    Debug.Log("Did Hit");
                    grabPointer.SetPosition(1, new Vector3(0, 0, hit.distance));
                    if (hit.transform.gameObject != rayTarget) 
                        stopPulling();
                    rayTarget = hit.transform.gameObject;
                }
                else
                {
                    Debug.Log("Did not Hit");
                    grabPointer.SetPosition(1, new Vector3(0, 0, 0.3f));
                    if (rayTarget)
                        stopPulling();
                    rayTarget = null;
                }

                if (handController.PointAmount == 0 || Input.GetKey(KeyCode.C))
                {
                    startPulling();
                }
                else
                {
                    stopPulling();
                }
            }
            else
            {
                grabPointer.gameObject.SetActive(false);
                grabPointer.SetPosition(1, new Vector3(0, 0, 0.3f));
                stopPulling();
            }
        }
    }

    public void startPulling()
    {
        Rigidbody target = rayTarget.GetComponent<Rigidbody>();
        if (rayTarget.GetComponent<MixueObject>())
        {
            rayTarget.GetComponent<MixueObject>().isPulled = true;
            target.velocity = (transform.position - target.transform.position).normalized * pullVelocity;
        }
        else
        {
            target.useGravity = false;
            target.velocity = (transform.position - target.transform.position).normalized * pullVelocity;

        }
    }
    public void stopPulling()
    {
        if (rayTarget)
        {
            if (rayTarget.GetComponent<MixueObject>())
            {
                rayTarget.GetComponent<MixueObject>().isPulled = false;
            }
            else
            {
                rayTarget.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}
