using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;
    
    public int damage_taken_modifier = 100; // percentage
    public int damage_modifier = 100; // percentage
    public int healing_modifier = 100; // percentage
    public int speed_modifier = 100; // percentage
    public int thorn_damage = 0; // flat damage reflected to attackers

    public List<PassiveSpell> passive_spells = new List<PassiveSpell>();
    public List<ActiveSpell> active_spells = new List<ActiveSpell>();

    void Awake()
    {
        Globals.player = this;
    }

    public void AddSpell(SpellBase spell)
    {
        if(spell is PassiveSpell)
        {
            passive_spells.Add(spell as PassiveSpell);
            spell.Cast();
            return;
        }
        active_spells.Add(spell as ActiveSpell);
    }

    public void CastSpell(int index)
    {
        active_spells[index].Cast();
    }

    public void Heal(int amount)
    {
        if(amount <= 0)
        {
            TakeDamage(-amount);
            return;
        }

        currentHealth += (int)(amount * (healing_modifier / 100.0f));
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= (int)(amount * (damage_taken_modifier / 100.0f));
        if (currentHealth < 0)
            currentHealth = 0;
        //if (thorn_damage > 0 && source != null)
        //    source.TakeDamage(thorn_damage);
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        while (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            experienceToNextLevel = (int)(experienceToNextLevel * 1.5f);
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        maxHealth += 20;
        currentHealth = maxHealth;
    }
}