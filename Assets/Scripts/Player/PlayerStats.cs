using UnityEngine;
using System.Collections.Generic;

public static class PlayerStats
{
    public static int maxHealth = 100;
    public static int currentHealth;
    public static int level = 1;
    public static int experience = 0;
    public static int experienceToNextLevel = 100;
 
    public static int damage_taken_modifier = 100; // percentage
    public static int damage_modifier = 100; // percentage
    public static int healing_modifier = 100; // percentage
    public static int speed_modifier = 100; // percentage
    public static int thorn_damage = 0; // flat damage reflected to attackers
 
    public static List<PassiveSpell> passive_spells = new List<PassiveSpell>();
    public static List<ActiveSpell> active_spells = new List<ActiveSpell>();

    public static void AddSpell(SpellBase spell)
    {
        if(spell is PassiveSpell)
        {
            passive_spells.Add(spell as PassiveSpell);
            spell.Cast();
            return;
        }
        active_spells.Add(spell as ActiveSpell);
    }

    public static void Heal(int amount)
    {
        if(amount <= 0)
        {
            PlayerStats.TakeDamage(-amount);
            return;
        }

        PlayerStats.currentHealth += (int)(amount * (PlayerStats.healing_modifier / 100.0f));
        if (PlayerStats.currentHealth > PlayerStats.maxHealth)
            PlayerStats.currentHealth = PlayerStats.maxHealth;
    }

    public static void TakeDamage(int amount, EnemyBase enemy = null)
    {
        PlayerStats.currentHealth -= (int)(amount * (PlayerStats.damage_taken_modifier / 100.0f));
        if (PlayerStats.currentHealth < 0)
            PlayerStats.currentHealth = 0;
        if (PlayerStats.thorn_damage > 0 && enemy != null)
            enemy.TakeDamage(PlayerStats.thorn_damage);
    }

    public static void GainExperience(int amount)
    {
        PlayerStats.experience += amount;
        while (PlayerStats.experience >= PlayerStats.experienceToNextLevel)
        {
            PlayerStats.experience -= PlayerStats.experienceToNextLevel;
            PlayerStats.experienceToNextLevel = (int)(PlayerStats.experienceToNextLevel * 1.5f);
            PlayerStats.LevelUp();
        }
    }

    public static void LevelUp()
    {
        PlayerStats.level++;
        PlayerStats.maxHealth += 20;
        PlayerStats.currentHealth = PlayerStats.maxHealth;
    }
}