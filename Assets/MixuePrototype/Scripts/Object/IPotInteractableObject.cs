using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPotInteractableObject : MonoBehaviour
{
    public enum InteractableType
    {
        Mixue,
        Bottle
    }
    public InteractableType Type { get; set;}
}
