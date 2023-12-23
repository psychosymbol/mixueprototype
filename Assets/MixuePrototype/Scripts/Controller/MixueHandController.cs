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
    public bool pullingObject = false;
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
            if(handController.PointAmount == 0 || Input.GetKey(KeyCode.C))
            {
                startPulling();
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
                        //Debug.Log("Did Hit");
                        grabPointer.SetPosition(1, new Vector3(0, 0, hit.distance));
                        if (hit.transform.gameObject != rayTarget)
                            stopPulling();
                        rayTarget = hit.transform.gameObject;
                    }
                    else
                    {
                        //Debug.Log("Did not Hit");
                        grabPointer.SetPosition(1, new Vector3(0, 0, 0.3f));
                        if (rayTarget)
                            stopPulling();
                        rayTarget = null;
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
    }

    public void startPulling()
    {
        if (!rayTarget) return;
        Rigidbody target = rayTarget.GetComponent<Rigidbody>();
        pullingObject = true;
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
        if (!rayTarget || !pullingObject) return;
        pullingObject = false;
        if (rayTarget.GetComponent<MixueObject>())
        {
            rayTarget.GetComponent<MixueObject>().StopPulling();
        }
        else
        {
            rayTarget.GetComponent<Rigidbody>().useGravity = true;
        }

    }
}
