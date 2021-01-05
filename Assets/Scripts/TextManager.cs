using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager
{
    #region Singleton

    public static TextManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TextManager();
            }
            return _instance;
        }
    }

    static TextManager _instance;

    public TextManager()
    {
        _instance = this;
        ParseText();
    }

    #endregion


    public Dictionary<int, string> dictionary = new Dictionary<int, string>();


    void ParseText()
    {
        TextAsset text = Resources.Load("DialogSystem/Text") as TextAsset;

        if (text == null)
        {
            Debug.Log("Resource was not loaded");
            return;
        }
        
        string[] words_list = text.text.Split('\n');
        foreach(string words in words_list)
        {
            int separator = words.IndexOf(';');
            if (separator >= 0)
                dictionary.Add(Int32.Parse(words.Substring(0, separator)), words.Substring(separator + 1));
        }
    }
}
