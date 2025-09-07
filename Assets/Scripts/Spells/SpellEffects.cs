using UnityEngine;

public static class SpellEffects
{
    public static void ActiveHeal(SpellBase spell)
    {
        PlayerStats.Heal((spell as ActiveSpell).base_healing);
        Debug.Log("Healed for: " + (spell as ActiveSpell).base_healing);
    }

    public static void PassiveHeal(SpellBase spell)
    {
        System.Random rnd = new System.Random();
        PlayerStats.Heal(rnd.Next(-5, 5));
        Debug.Log("Healed for a small amount.");
    }

    public static void ModifyDamage(SpellBase spell)
    {
        System.Random rnd = new System.Random();
        PlayerStats.damage_modifier += rnd.Next(-10, 20) + (spell is ActiveSpell ? 10 : 0);
        Debug.Log("Damage modifier is now: " + PlayerStats.damage_modifier);
    }

    public static void ModifyHealing(SpellBase spell)
    {
        System.Random rnd = new System.Random();
        PlayerStats.healing_modifier += rnd.Next(-10, 20) + (spell is ActiveSpell ? 10 : 0);
        Debug.Log("Healing modifier is now: " + PlayerStats.healing_modifier);
    }

    public static void AddThornDamage(SpellBase spell)
    {
        System.Random rnd = new System.Random();
        PlayerStats.thorn_damage += rnd.Next(-5, 5) + (spell is ActiveSpell ? 10 : 0);
        Debug.Log("Thorn damage is now: " + PlayerStats.thorn_damage);
    }
}