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

public abstract class SpellBase : SpellEffects
{
    public string name;
    public string description;
    public SpellType spellType;
    public Action Effect;
    
    public abstract void Cast();

    public SpellBase() { }
    
    protected SpellBase(string name, string description, Action Effect, SpellType spell_type = SpellType.None)
    {
        this.name = name;
        this.description = description;
        this.Effect = Effect;
    }

    public override string ToString()
    {
        return $"Spell: {name}\nDescription: {description}\nType: {spellType}";
    }
}

public class SpellEffects
{
    public static List<string> effect_names = typeof(SpellEffects).GetMethods().Select(m => m.Name).ToList();

    public static void Heal()
    {
        Debug.Log("Casting Heal!");
    }
}      

public class PassiveSpell : SpellBase
{
    public PassiveSpell(string name, string description, Action Effect, SpellType spell_type) : base(name, description, Effect, spell_type) { }
    public PassiveSpell() : base(){ }
    public override void Cast()
    {
        Effect();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}

public class ActiveSpell : SpellBase
{
    public int cooldown;
    public int damage;

    private float next_cast_time;

    private void ApplyCooldown()
    {
        next_cast_time = Time.time + cooldown;
    }

    private bool IsOffCooldown()
    {
        return Time.time >= next_cast_time;
    }

    public ActiveSpell() : base(){ }   

    public ActiveSpell(string name, string description, SpellType spellType, int cooldown, Action Effect, int damage)
        : base(name, description, Effect, spellType)
    {
        this.spellType = spellType;
        this.cooldown = cooldown;
        this.damage = damage;
    }

    public override void Cast()
    {
        if (!IsOffCooldown()) return;

        Effect();
        ApplyCooldown();
    }

    public override string ToString()
    {
        return base.ToString() + $"\nCooldown: {cooldown}\nDamage: {damage}";
    }
}