using UnityEngine;
using System.Collections.Generic;

public class SpellGenerator : GoogleAIManager
{
    public SpellBase GenerateSpell(List<string> parameters)
    {
        SpellBase spell;
        bool isActive = parameters[0].ToLower() == "active";
        if(isActive) 
            spell = new ActiveSpell();
        else
            spell = new PassiveSpell();

        spell.name = parameters[1];
        spell.description = parameters[2];
        spell.spellType = (SpellType) System.Enum.Parse(typeof(SpellType), parameters[3]);
        if(isActive) 
        {
            (spell as ActiveSpell).cooldown = int.Parse(parameters[5]);
            (spell as ActiveSpell).damage = int.Parse(parameters[6]);
        }

        GenerateEffect(spell, parameters[4]);

        return spell;
    }

    private void GenerateEffect(SpellBase spell, string effect)
    {
        foreach (string eff in effect.Split(','))
        {
            string trimmedEffect = eff.Trim();
            var methodInfo = typeof(SpellEffects).GetMethod(trimmedEffect);
            if (methodInfo != null)
            {
                System.Action action = (System.Action) System.Delegate.CreateDelegate(typeof(System.Action), methodInfo);
                spell.Effect += action;
            }
            else
            {
                Debug.LogWarning($"Effect method '{trimmedEffect}' not found in SpellEffects.");
            }
        }
    }

    protected override string ProcessInput(string input)
    {
        string prompt = "Generate a spell from this description: " + input;
        prompt += "\n\nFormat the response as follows:\n";
        prompt += "Is Active: <Active or Passive>\n";
        prompt += "Name: <Spell Name>\n";
        prompt += "Description: <Brief description of the spell>\n";
        prompt += "Spell Type: <One of the following types - Fire, Water, Earth, Air, None>\n";
        prompt += "Effect: <Zero or more of the following effects - " + string.Join(", ", SpellEffects.effect_names) + ", separated with commas>\n";
        prompt += "Cooldown: <Cooldown time in seconds, only for Active spells>\n";
        prompt += "Damage: <Damage amount, only for Active spells>\n";
        prompt += "\nEnsure the response is concise and strictly follows the format. The damage should be between 1 and 10.";

        return prompt;
    }

    protected override void OnAIResponse(string response)
    {
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

        GenerateSpell(parameters);
    }
}