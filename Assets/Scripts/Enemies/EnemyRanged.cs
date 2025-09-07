using UnityEngine;

public class EnemyRanged : EnemyBase
{
    private SpellBase[] spells = new SpellBase[2];
    private int range;

    public EnemyRanged(int max_health, int damage, int healing, float move_speed, int range) : base(max_health, damage, healing, move_speed)
    {
        this.range = range;
    }

    public void AddSpell(SpellBase spell)
    {
        if(spell is ActiveSpell)
            spells[0] = spell;
            
        else if(spell is PassiveSpell)
        {
            spells[1] = spell;
            spell.Cast();
        }
    }

    public void CastSpell()
    {
        spells[0].Cast();
    }
}