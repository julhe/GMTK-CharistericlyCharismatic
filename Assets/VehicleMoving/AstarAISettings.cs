using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AISettings.asset", menuName = "AISettings")]
public class AstarAISettings : ScriptableObject
{
    public float Speed = 30f;
    public float VisualTurningSpeed = 120f;
}
