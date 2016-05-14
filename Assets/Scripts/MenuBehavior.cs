using UnityEngine;
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour
{
    public GameObject[] objectsToToggle;

    public void SwitchLevel(string levelName)
    {
        Application.LoadLevel(levelName);
    }

    public void StartGame()
    {
        string inputRoomName = GameObject.Find("Room Input Field/Text").GetComponent<Text>().text;
        NetworkManager.roomName = (inputRoomName == "" ? "BlindRoom" : inputRoomName);
        SwitchLevel("ingame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void switchVisible(int newVisibleIndex)
    {
        for (int i = 0; i < objectsToToggle.Length; i++)
        {
            objectsToToggle[i].SetActive(i == newVisibleIndex);
        }
    }
}
