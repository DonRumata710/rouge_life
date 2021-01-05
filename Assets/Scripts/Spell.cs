using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public float readingTime = 1.0f;
    public float castTime = 1.0f;
    public Effect effect;
    public bool castToTarget = true;
    public bool moveForCast = true;

    public Sprite icon;
    public GameObject castObject;
    public GameObject resultVisualEffect;
}
