using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
	#region Singleton

	public static PauseManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<PauseManager> ();
			}
			return _instance;
		}
	}

	static PauseManager _instance;

	void Awake ()
	{
		_instance = this;
	}

	#endregion

	int counter = 0;

	public void Pause ()
	{
		counter++;
		Time.timeScale = 0.0f;
	}

	public void Play ()
	{
		counter--;
		if (counter <= 0)
			Time.timeScale = 1.0f;
	}

	public void ChangeState (bool pause)
	{
		if (pause)
			Pause ();
		else
			Play ();
	}
}
