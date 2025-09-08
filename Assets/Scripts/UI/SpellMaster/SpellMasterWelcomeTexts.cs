using UnityEngine;
using UnityEngine.UI;

public class SpellMasterWelcomeTexts : MonoBehaviour
{
    public string[] tutorial_texts = new string[]
    {
        "Welcome, traveler! I am the Spell Master, here to guide you on your journey to mastering the arcane arts.",
        "In this realm, magic is not just a tool, but a way of life. Whether you seek to heal, protect, or unleash destruction, the power of spells is at your fingertips.",
        "To begin your training, simply describe the type of spell you wish to create. Be as detailed as possible, and I will help you bring your vision to life.",
        "Remember, the more specific you are about the spell's effects, duration, and any special conditions, the better I can assist you in crafting a spell that suits your needs.",
        "Are you ready to embark on this magical journey? Let's create some powerful spells together!"
    };

    public string[] welcome_texts = new string[]
    {
        "Welcome back, traveler! The arcane arts await your command."
    };

    public Text text;
    public GameObject bubble;
    public GameObject input_field;

    private static bool tutorial_passed = false;
    private int current_text_index = 0;

    void Start()
    {   
        if(SpellMasterWelcomeTexts.tutorial_passed) 
            text.text = welcome_texts[0];
        else
            text.text = tutorial_texts[current_text_index];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SpellMasterWelcomeTexts.tutorial_passed)
            {
                Destroy(this);
                return;
            }

            current_text_index++;

            if(current_text_index >= tutorial_texts.Length)
            {
                Destroy(this);
                return;
            }
            text.text = tutorial_texts[current_text_index];

        }
        else if (Input.GetKeyDown(KeyCode.Escape))
            Destroy(this);
    }

    void OnDestroy()
    {
        input_field.SetActive(true);
        SpellMasterWelcomeTexts.tutorial_passed = true;
        bubble.SetActive(false);
    }
}
