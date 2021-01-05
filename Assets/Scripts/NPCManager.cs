using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class NPCManager : MonoBehaviour
{
    #region Singleton

    public static NPCManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NPCManager>();
            }
            return _instance;
        }
    }

    static NPCManager _instance;

    void Awake()
    {
        _instance = this;

        NPCScriptParser parser = new NPCScriptParser("DialogSystem/CharacterStates");

        characters = parser.characters;
        states = parser.states;
        scenes = parser.scenes;
    }

    #endregion


    public struct GameEvent
    {
        public enum Type
        {
            CHARACTER_DETECTED,
            TARGET_ACHIEVED,
            TARGET_LOST
        }

        public Type type;
        public GameObject target;

        public GameEvent(Type _type, GameObject _target)
        {
            type = _type;
            target = _target;
        }
    }


    public void GetNewTarget(NPCController npc, GameEvent game_event)
    {
        if (game_event.type == GameEvent.Type.TARGET_ACHIEVED)
        {
            List<Area> areas = states[characters[npc.characterType]].location;
            if (areas.Count == 1)
            {
                Vector3 point = areas[0].point.transform.position;
                point += new Vector3(
                    UnityEngine.Random.Range(-areas[0].distance, areas[0].distance),
                    0.0f,
                    UnityEngine.Random.Range(-areas[0].distance, areas[0].distance)
                );
                npc.Move(point);
                npc.SetTarget(areas[0].point);

                Debug.Log(npc.characterType + " target: " + point);
            }
            else
            {
                for(int i = 0; i < areas.Count; ++i)
                {
                    if (areas[i].point == game_event.target)
                    {
                        i = ++i % areas.Count;
                        Vector3 point = areas[0].point.transform.position;
                        point += new Vector3(
                            UnityEngine.Random.Range(-areas[i].distance, areas[i].distance),
                            0.0f,
                            UnityEngine.Random.Range(-areas[i].distance, areas[i].distance)
                        );
                        npc.Move(point);
                        npc.SetTarget(areas[i].point);

                        Debug.Log(npc.characterType + " target: " + point);
                    }
                }
            }
        }
    }


    public void SetClassState(string character, string state)
    {
        if (character == null || state == null || character == "" || state == "")
            return;

        characters[character] = state;
    }


    public Frase StartDialog(CommonCharacterController target)
    {
        return scenes[states[characters[target.characterType]].scene_name];
    }

    public string GetSceneName(CommonCharacterController target)
    {
        return states[characters[target.characterType]].scene_name;
    }

    public Frase GetFrase(string name)
    {
        return scenes[name];
    }


    Dictionary<string, string> characters = new Dictionary<string, string>();   // class -> state name
    Dictionary<string, State> states = new Dictionary<string, State>();         // state name -> state
    public Dictionary<string, Frase> scenes = new Dictionary<string, Frase>();  // scene name -> scene
}





class Area
{
    public GameObject point;
    public float distance;
}


public class Frase
{
    public struct Condition
    {
        internal enum Operation
        {
            EQUAL,
            NOEQUAL,
            CONTAIN,
            NOCONTAIN
        }

        internal Operation operation;
        internal string variable;
        internal string value;

        public bool Check(Dictionary<string, string> variables)
        {
            if (value == null)
                return true;

            try
            {
                switch (operation)
                {
                    case Operation.EQUAL:
                        return variables[variable] == value;
                    case Operation.NOEQUAL:
                        return variables[variable] != value;
                    case Operation.CONTAIN:
                        return variables[variable].Contains(value);
                    case Operation.NOCONTAIN:
                        return !variables[variable].Contains(value);
                    default:
                        return false;
                }
            }
            catch(KeyNotFoundException)
            {
                return false;
            }
        }
    }

    public struct Result
    {
        public string variable;
        public string value;
    }


    public int words = -1;
    public Condition condition;
    public Result result;
    public string go_to_scene;

    public bool character_is_actor = true;

    public List<Frase> brunches = new List<Frase>();
}


public class State
{
    public string scene_name;

    internal List<Area> location = new List<Area>();
    
    internal bool is_aggressive;
}


public class NPCScriptParser
{
    public NPCScriptParser(string file)
    {
        TextAsset script = Resources.Load(file) as TextAsset;

        if (script == null)
        {
            Debug.Log("Resource was not loaded");
            return;
        }

        XmlDocument doc = new XmlDocument();
        try
        {
            doc.LoadXml(script.text);
        }
        catch(XmlException ex)
        {
            Debug.Log(ex.Message);
            return;
        }

        Parse(doc.DocumentElement);
    }

    struct Expression
    {
        public string name;
        public int value;

        public Expression(string n, int v)
        {
            name = n;
            value = v;
        }
    }


    Dictionary<string, int> expressions = new Dictionary<string, int>();


    void Parse(XmlElement script)
    {
        XmlNode expression_list = script.SelectSingleNode("expressions");
        foreach (XmlNode node in expression_list)
            expressions.Add(node.Attributes.GetNamedItem("name").Value, Int32.Parse(node.Attributes.GetNamedItem("value").Value));

        XmlNode characters_desc = script.SelectSingleNode("classes");
        foreach (XmlNode character in characters_desc)
            characters.Add(character.Name, character.Attributes.GetNamedItem("state").Value);


        XmlNode scenes_desc = script.SelectSingleNode("scenes");
        foreach (XmlNode scene_desc in scenes_desc)
        {
            Frase scene = ParseDialog(scene_desc);
            scene.character_is_actor = (scene.words != -1);
            scenes.Add(scene_desc.Name, scene);
        }


        Frase attack_frase = new Frase();
        attack_frase.words = expressions["attack"];
        attack_frase.result.variable = "AGGRESSIVE";
        attack_frase.result.value = "true";
        attack_frase.character_is_actor = false;
        scenes.Add("ATTACK", attack_frase);


        XmlNode states_desc = script.SelectSingleNode("states");
        foreach (XmlNode state_desc in states_desc)
        {
            State state = new State();

            foreach (XmlAttribute attr in state_desc.Attributes)
            {
                if (attr.Name == "aggressive")
                    state.is_aggressive = attr.Value == "true";
                else if (attr.Name == "scene")
                    state.scene_name = attr.Value;
            }
            
            Area area = new Area();
            XmlNode location_desc = state_desc.SelectSingleNode("location");
            if (location_desc != null)
            {
                XmlNode area_desc = location_desc.FirstChild;

                foreach (XmlAttribute attr in area_desc.Attributes)
                {
                    if (attr.Name == "distance")
                        area.distance = float.Parse(attr.Value);
                    else if (attr.Name == "point")
                        area.point = GameObject.Find(attr.Value);
                }

                state.location.Add(area);
            }

            states.Add(state_desc.Name, state);
        }
    }


    int GetWordsNum(string value)
    {
        int words = 0;
        if (int.TryParse(value, out words))
            return words;
        else
            return expressions[value.Substring(1)];
    }


    Frase ParseDialog(XmlNode scene_node)
    {
        Frase scene = new Frase();

        scene.character_is_actor = (scene_node.Name == "talk" || scene_node.Name == "attack");

        foreach(XmlAttribute attr in scene_node.Attributes)
        {
            if (attr.Name == "words")
            {
                if (attr.Value[0] == '$')
                    scene.words = expressions[attr.Value.Substring(1)];
                else
                    scene.words = GetWordsNum(attr.Value);
            }
            else if (attr.Name == "state")
            {
                scene.result.variable = "STATE";
                scene.result.value = attr.Value;
            }
        }

        foreach (XmlNode node in scene_node)
        {
            if (node.Name == "check")
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    if (attr.Name == "variable")
                    {
                        scene.condition.variable = node.Attributes.GetNamedItem("variable").Value;
                    }
                    else if (attr.Name == "value")
                    {
                        scene.condition.value = node.Attributes.GetNamedItem("value").Value;
                    }
                    else if (attr.Name == "operation")
                    {
                        if (attr.Value == "equal")
                            scene.condition.operation = Frase.Condition.Operation.EQUAL;
                        else if (attr.Value == "not-equal")
                            scene.condition.operation = Frase.Condition.Operation.NOEQUAL;
                        else if (attr.Value == "contain")
                            scene.condition.operation = Frase.Condition.Operation.CONTAIN;
                        else if (attr.Value== "not-contain")
                            scene.condition.operation = Frase.Condition.Operation.NOCONTAIN;
                    }
                }
            }
            else if (node.Name == "set")
            {
                scene.result.variable = node.Attributes.GetNamedItem("variable").Value;
                scene.result.value = node.Attributes.GetNamedItem("value").Value;
            }
            else if (node.Name == "talk" || node.Name == "answer")
            {
                Frase frase = ParseDialog(node);
                scene.brunches.Add(frase);
            }
            else if (node.Name == "goto")
            {
                Frase goto_scene = ParseDialog(node);
                goto_scene.go_to_scene = node.Attributes.GetNamedItem("scene").Value;
                scene.brunches.Add(goto_scene);
            }
            else if (node.Name == "attack")
            {
                Frase attack = ParseDialog(node);

                Frase react = new Frase
                {
                    go_to_scene = "ATTACK"
                };

                attack.brunches.Add(react);
                scene.brunches.Add(attack);
            }
        }

        return scene;
    }


    public Dictionary<string, string> characters = new Dictionary<string, string>();
    public Dictionary<string, State> states = new Dictionary<string, State>();
    public Dictionary<string, Frase> scenes = new Dictionary<string, Frase>();
}
