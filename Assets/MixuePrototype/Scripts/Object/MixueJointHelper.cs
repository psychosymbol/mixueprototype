using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixueJointHelper : JointHelper
{
    public float minRotY = -180f, maxRotY = 180f;
    private void Start()
    {
        
    }
    protected override void lockPosition()
    {
        //base.lockPosition();
        if (LockXPosition || LockYPosition || LockZPosition)
        {
            currentPosition = transform.localPosition;
            transform.localPosition = new Vector3(LockXPosition ? initialPosition.x : currentPosition.x, LockYPosition ? initialPosition.y : currentPosition.y, LockZPosition ? initialPosition.z : currentPosition.z);
        }

        if (LockXScale || LockYScale || LockZScale)
        {
            currentScale = transform.localScale;
            transform.localScale = new Vector3(LockXScale ? initialScale.x : currentScale.x, LockYScale ? initialScale.y : currentScale.y, LockZScale ? initialScale.z : currentScale.z);
        }



        if (LockXRotation || LockYRotation || LockZRotation)
        {
            var clampY = Mathf.Clamp(currentRotation.y, minRotY, maxRotY);

            currentRotation = transform.localEulerAngles;
            transform.localRotation = Quaternion.Euler(new Vector3(
                LockXRotation ? initialRotation.x : currentRotation.x,
                LockYRotation ? initialRotation.y : clampY  ,
                LockZRotation ? initialRotation.z : currentRotation.z
                ));
        }
    }


}
