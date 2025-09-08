using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpellGenerator : GoogleAIManager
{    
    public UnityEngine.UI.Text input_field;
    public GameObject input;

	public GameObject spell_effect_prefab;
    private bool should_close_after_adding = false;

	public Sprite[] spell_icons;

    public void GenerateSpellFromDescription()
    {
        Debug.Log("Generating spell from description: " + input_field.text);
        SendRequest(input_field.text);
        input_field.text = "";
        PlayerStats.available_generations--;
        should_close_after_adding = PlayerStats.available_generations == 0;
    }

    public void OnMouseOver(){
        if(PlayerStats.available_generations > 0 && Input.GetMouseButtonDown(0)){
            input.SetActive(true);
        }
    }

    public SpellBase GenerateSpell(List<string> parameters)
    {
        try 
        {
            SpellBase spell;
            bool isActive = parameters[0] == "Active";
            if(isActive) 
                spell = new ActiveSpell();
            else
                spell = new PassiveSpell();

            spell.name = parameters[1];
            spell.description = parameters[2];
            spell.spellType = (SpellType) System.Enum.Parse(typeof(SpellType), parameters[3]);
			spell.icon = spell_icons.Length > 0 ? spell_icons[(int)spell.spellType] : null;
            if(isActive) 
            {
                (spell as ActiveSpell).cooldown = int.Parse(parameters[5]);
                (spell as ActiveSpell).base_damage = int.Parse(parameters[6]);
                (spell as ActiveSpell).base_healing = int.Parse(parameters[7]);
				(spell as ActiveSpell).effectPrefab = spell_effect_prefab;
            }

            GenerateEffect(spell, parameters[4]);
            return spell;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error generating spell: " + e.Message);
            return null;
        }
    }

    private void GenerateEffect(SpellBase spell, string effect)
    {
        foreach (string eff in effect.Split(','))
            spell.AddEffect(eff.Trim());
    }

    protected override string ProcessInput(string input)
    {
        string prompt = "Generate a spell from this description: " + input;
        prompt += "\n\nFormat the response as follows:\n";
        prompt += "Is Active: <Active or Passive>\n";
        prompt += "Name: <Spell Name>\n";
        prompt += "Description: <Brief description of the spell>\n";
        prompt += "Spell Type: <One of the following types - Fire, Water, Earth, Air, None>\n";
        prompt += "Effect: <Zero or more of the following effects - " + string.Join(", ", typeof(SpellEffects).GetMethods().Select(m => m.Name).ToArray()) + ", separated with commas>\n";
        prompt += "Cooldown: <Cooldown time in seconds, only for Active spells>\n";
        prompt += "Damage: <Damage amount, only for Active spells>\n";
        prompt += "Healing: <Healing amount, only for Active spells>\n";
        prompt += "\nEnsure the response is concise and strictly follows the format.";
        prompt += "The base_damage should be between 1 and 10.";
        prompt += "There cannot be passive spells that deal damage or heal.";
        prompt += "If the spell deals damage to the player set the healing to minus the damage amount.";
        prompt += "If an effect has Active in its name it can only be used in active spells (the same applies for Passive).";

        return prompt;
    }

    protected override void OnAIResponse(string response)
    {
        Debug.Log("AI Response:\n" + response);
        List<string> parameters = new List<string>();
        string[] lines = response.Split('\n');

        foreach (string line in lines)
        {
            string[] split = line.Split(':');
            if (split.Length > 1)
                parameters.Add(split[1].Trim());
            else
                parameters.Add("");
        }

        SpellBase spell = GenerateSpell(parameters);
        if( spell == null ) return; // return some message
        PlayerStats.AddSpell(spell);
        if(should_close_after_adding)
            gameObject.GetComponent<SceneLoader>().LoadScene();
    }
}
