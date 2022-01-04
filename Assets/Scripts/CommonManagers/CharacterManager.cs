using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : MonoBehaviour
{
	#region Singleton

	public static CharacterManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<CharacterManager> ();
			}
			return _instance;
		}
	}

    private void Awake()
    {
		_instance = this;
		foreach (CharacterParametersSet cp in parameters)
		{
			_parameters [cp.character_name] = cp;
		}
    }

    static CharacterManager _instance;

	#endregion

	public CharacterParametersSet GetParameters (string name)
	{
		try
		{
			return _parameters [name];
		}
		catch(KeyNotFoundException)
		{
			Debug.Log ("failed getting " + name);
			return null;
		}
	}

	public CharacterParametersSet[] parameters;
	SortedDictionary<string, CharacterParametersSet> _parameters = new SortedDictionary<string, CharacterParametersSet> ();
}
