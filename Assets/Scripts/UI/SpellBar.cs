using UnityEngine;
using UnityEngine.UI;

public class SpellBarHandler : MonoBehaviour
{
    private PlayerSpells player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerSpells>();
        Debug.Log(PlayerStats.active_spells.Count);
        for (int i = 0; i < PlayerStats.active_spells.Count; i++)
            gameObject.transform.GetChild(i).GetComponent<Image>().sprite = PlayerStats.active_spells[i].icon;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            player.CastSpell(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0);
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            player.CastSpell(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
        else if(Input.GetKeyDown(KeyCode.Alpha3))
            player.CastSpell(Camera.main.ScreenToWorldPoint(Input.mousePosition), 2);
        else if(Input.GetKeyDown(KeyCode.Alpha4))
            player.CastSpell(Camera.main.ScreenToWorldPoint(Input.mousePosition), 3);
        else if(Input.GetKeyDown(KeyCode.Alpha5))
            player.CastSpell(Camera.main.ScreenToWorldPoint(Input.mousePosition), 4);
        else if(Input.GetKeyDown(KeyCode.Alpha6))
            player.CastSpell(Camera.main.ScreenToWorldPoint(Input.mousePosition), 5);
    }
}