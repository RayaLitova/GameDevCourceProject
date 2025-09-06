using UnityEngine;
using System.Collections.Generic;

public class PlayerSpellsHandler : MonoBehaviour
{
    public List<SpellBase> passive_spells = new List<SpellBase>();
    public List<SpellBase> active_spells = new List<SpellBase>();

    public void AddSpell(SpellBase spell)
    {
        if(spell is PassiveSpell)
        {
            passive_spells.Add(spell);
            spell.Cast();
            return;
        }
        active_spells.Add(spell);
    }

    public void CastSpell(int index)
    {
        active_spells[index].Cast();
    }
}