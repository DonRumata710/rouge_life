using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellManager : MonoBehaviour
{
    public GameObject castObject;
    public List<Spell> spells;

    #region Singleton

    public static SpellManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpellManager>();
            }
            return _instance;
        }
    }

    static SpellManager _instance;

    void Awake()
    {
        _instance = this;
    }

    #endregion
}
