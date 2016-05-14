using UnityEngine;
using System.Collections;

public class MenuBehavior : MonoBehaviour
{
    public GameObject[] objectsToToggle;

    public void switchLevel(string levelName)
    {
        Application.LoadLevel(levelName);
    }

    public void switchVisible(int newVisibleIndex)
    {
        for (int i = 0; i < objectsToToggle.Length; i++)
        {
            objectsToToggle[i].SetActive(i == newVisibleIndex);
        }
    }
}
