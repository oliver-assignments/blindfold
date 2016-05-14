using UnityEngine;
using System.Collections;

public class MenuBehavior : MonoBehaviour
{
    public GameObject[] objectsToToggle;

    public void switchLevel(string levelName)
    {
        Application.LoadLevel(levelName);
    }

<<<<<<< HEAD
=======
    public void ExitGame()
    {
        Application.Quit();
    }

>>>>>>> 507970207f23810147fe176e4ddab01913971b66
    public void switchVisible(int newVisibleIndex)
    {
        for (int i = 0; i < objectsToToggle.Length; i++)
        {
            objectsToToggle[i].SetActive(i == newVisibleIndex);
        }
    }
}
