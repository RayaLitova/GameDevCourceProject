using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum SpellType
{
    Fire,
    Water,
    Earth,
    Air,
    None
}

public abstract class SpellBase
{
    public string name;
    public string description;
    public SpellType spellType;
    public Action Effect;
    
    public abstract void Cast();

    public SpellBase() { }

    public override string ToString()
    {
        return $"Spell: {name}\nDescription: {description}\nType: {spellType}";
    }

    public SpellBase(SpellBase other)
    {
        name = other.name;
        description = other.description;
        spellType = other.spellType;
        Effect = other.Effect;
    }

    public void AddEffect(string effect_name)
    {
        Effect += () => typeof(SpellEffects).GetMethod(effect_name).Invoke(null, new object[] { this });
    }
} 

public class PassiveSpell : SpellBase
{
    public PassiveSpell() : base(){ }
    public override void Cast()
    {
        Effect();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public PassiveSpell(PassiveSpell other) : base(other) { }
}

public class ActiveSpell : SpellBase
{
    public int cooldown;
    public int base_damage;
    public int base_healing;

    private float next_cast_time;

    public ActiveSpell() : base(){ }

    public ActiveSpell(ActiveSpell other) : base(other) 
    { 
        cooldown = other.cooldown;
        base_damage = other.base_damage;
        base_healing = other.base_healing;
    }

    private void ApplyCooldown()
    {
        next_cast_time = Time.time + cooldown;
    }

    private bool IsOffCooldown()
    {
        return Time.time >= next_cast_time;
    }

    public override void Cast()
    {
        if (!IsOffCooldown()) return;

        Globals.player.Heal(base_healing);
        DealDamage();
        Effect();
        ApplyCooldown();
    }

    public void DealDamage()
    {
        int total_damage = (int)(base_damage * (Globals.player.damage_modifier / 100.0f));
        //TODO
        Debug.Log($"Dealt {total_damage} damage.");
    }

    public override string ToString()
    {
        return base.ToString() + $"\nCooldown: {cooldown}\nDamage: {base_damage}";
    }
}