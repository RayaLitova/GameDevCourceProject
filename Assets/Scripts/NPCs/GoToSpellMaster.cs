using UnityEngine;

public class GoToSpellMaster : MonoBehaviour
{
    public void OnMouseOver(){
        if(PlayerStats.available_generations > 0 && Input.GetMouseButtonDown(0)){
            gameObject.GetComponent<SceneLoader>().LoadScene();
        }
    }
}
