using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Action
{
	NONE,
	ATTACK,
	TALK
}

public class PlayerManager : MonoBehaviour
{
	PlayerController player;
	bool isPaused = false;

    GameObject actionsPanel;
    GameObject magicPanel;

    CastBook book;
    List<SpellButton> buttons = new List<SpellButton>();

    public GameObject spellButton;

	void Start ()
	{
        GameObject player_object = GameObject.FindGameObjectWithTag("Player");

        player = player_object.GetComponent<PlayerController> ();
        player.OnDialog += OpenDialog;

        book = player_object.GetComponent<CastBook>();

        actionsPanel = transform.Find("Canvas").Find("ActionsPanel").gameObject;
        magicPanel = transform.Find("Canvas").Find("MagicPanel").gameObject;
        dialogPanel = transform.Find("Canvas").Find("DialogPanel").gameObject;
        dialogText = dialogPanel.transform.Find("Dialog").GetComponent<Text>();
        dialogVariants = dialogPanel.transform.Find("Answers").Find("Viewport").Find("Variants").gameObject;
    }

	public void Attack ()
	{
		player.SetAction(PlayerController.Action.ATTACK);
	}

	public void Talk ()
	{
		player.SetAction(PlayerController.Action.TALK);
	}

    public void OpenMagic()
    {
        actionsPanel.SetActive(false);
        magicPanel.SetActive(true);

        Vector3 pos = new Vector3(75.0f, 25.0f, 0.0f);

        foreach (string spell_name in book.spells)
        {
            Spell spell = null;
            foreach(Spell i in SpellManager.instance.spells)
            {
                if (i != null && i.name == spell_name)
                {
                    spell = i;
                    break;
                }
            }

            if (spell == null)
            {
                continue;
            }

            GameObject spell_button = Instantiate(spellButton, magicPanel.transform);

            SpellButton spell_button_script = spell_button.GetComponent<SpellButton>();
            spell_button_script.spell = spell;
            buttons.Add(spell_button_script);

            spell_button.transform.position = pos;
            spell_button.GetComponent<Button>().onClick.AddListener(() => { CastMagic(spell_button); });

            GameObject image = spell_button.transform.Find("Image").gameObject;
            image.GetComponent<Image>().sprite = spell.icon;

            pos += new Vector3(50.0f, 0.0f, 0.0f);
        }
    }

    public void CloseMagic()
    {
        actionsPanel.SetActive(true);
        magicPanel.SetActive(false);

        foreach (SpellButton spell_button in buttons)
        {
            Destroy(spell_button.gameObject);
        }
    }

    public void CastMagic(GameObject btn)
    {
        SpellButton spell_button = btn.GetComponent<SpellButton>();

        if (spell_button == null)
            return;

        player.SetAction(CommonCharacterController.Action.CAST);
        player.CurrentSpell = spell_button.spell;
    }



    GameObject dialogPanel;
    Text dialogText;
    GameObject dialogVariants;

    public GameObject variantButton;




    class DialogState
    {
        public PlayerManager manager;
        public CommonCharacterController character;

        public void NewAnswer(Frase scene)
        {
            Debug.Log("answer: " + scene.words);
            if (scene.brunches.Count != 0)
            {
                foreach (Frase next_frase in scene.brunches)
                {
                    if (!next_frase.condition.Check(manager.player.plotVariables))
                        continue;

                    SetVariable(next_frase.result);
                    NewTalk(next_frase);
                }
            }
            else
            {
                if (manager.player.plotVariables["AGGRESSIVE"] == "true")
                {
                    Debug.Log(character.characterType + " is aggressive");

                    manager.player.plotVariables["AGGRESSIVE"] = "false";
                    character.SetAction(CommonCharacterController.Action.ATTACK);
                    character.Target = manager.player;
                }

                if (manager.player.plotVariables.ContainsKey("STATE"))
                {
                    Debug.Log(character.characterType + " changes state:" + manager.player.plotVariables["STATE"]);

                    NPCManager.instance.SetClassState(character.characterType, manager.player.plotVariables["STATE"]);
                    manager.player.plotVariables.Remove("STATE");
                }

                manager.StopDialog();
            }
        }

        public void NewTalk(Frase scene)
        {
            manager.dialogText.text = TextManager.instance.dictionary[scene.words];
            Debug.Log("new frase: " + scene.words);

            foreach (Transform child in manager.dialogVariants.transform)
                Destroy(child.gameObject);

            manager.dialogVariants.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical, 30.0f * scene.brunches.Count + 10.0f
            );

            CreateButtons(scene.brunches);
        }

        void CreateButtons(List<Frase> brunches, int i = 0)
        {
            Vector2 pos = new Vector2(5.0f, -5.0f - 30.0f * i);
            foreach (Frase next_frase in brunches)
            {
                if (!next_frase.condition.Check(manager.player.plotVariables))
                    continue;

                if (next_frase.go_to_scene != null)
                {
                    Frase target_scene = NPCManager.instance.GetFrase(next_frase.go_to_scene);

                    if (target_scene.brunches.Count > 0)
                    {
                        CreateButtons(target_scene.brunches, i);
                        i += target_scene.brunches.Count;
                    }
                    else
                    {
                        AddButton(target_scene, i, pos);
                        ++i;
                    }
                    continue;
                }

                AddButton(next_frase, i, pos);

                pos.y -= 30.0f;
                ++i;
            }
        }

        void AddButton(Frase next_frase, int i, Vector2 pos)
        {
            GameObject button = Instantiate(manager.variantButton, manager.dialogVariants.transform);
            button.GetComponent<Button>().onClick.AddListener(() => {
                foreach (Transform child in manager.dialogVariants.transform)
                {
                    Destroy(child.gameObject);
                }
                
                SetVariable(next_frase.result);

                NewAnswer(next_frase);
            });

            button.transform.GetChild(0).gameObject.GetComponent<Text>().text =
                (i + 1).ToString() + ". " + TextManager.instance.dictionary[next_frase.words];

            button.GetComponent<RectTransform>().anchoredPosition = pos;
        }

        public void SetVariable(Frase.Result result)
        {
            if (result.variable == null || result.variable == "")
                return;

            try
            {
                string value = manager.player.plotVariables[result.variable];
                if (result.value[0] == '|')
                {
                    value =  value + result.value;
                }
                else
                {
                    value = result.value;
                }

                manager.player.plotVariables[result.variable] = value;
            }
            catch (KeyNotFoundException)
            {
                manager.player.plotVariables.Add(result.variable, result.value);
            }
        }
    }

    public void OpenDialog(CommonCharacterController character)
    {
        Pause();

        dialogPanel.SetActive(true);
        Frase scene = NPCManager.instance.StartDialog(character);

        DialogState state = new DialogState
        {
            manager = this,
            character = character
        };

        state.NewTalk(scene);
    }

    public void StopDialog()
    {
        dialogPanel.SetActive(false);
        dialogText.text = "";
        foreach (Transform child in dialogVariants.transform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Dialog stopped");

        Pause();
    }

    public void OpenMenu ()
	{
		Debug.Log ("menu");
	}

	public void Pause ()
	{
		isPaused = !isPaused;
		PauseManager.instance.ChangeState (isPaused);
	}
}
