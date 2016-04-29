using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomNetworkManager : NetworkManager
{

	//get input from text input fields
	void SetIPAddress ()
	{
		string ipAddress = GameObject.Find ("Address Input Field").transform.FindChild ("Text").GetComponent<Text> ().text;
		NetworkManager.singleton.networkAddress = (ipAddress == "" ? "127.0.0.1" : ipAddress);
	}
	
	void SetPort ()
	{
		NetworkManager.singleton.networkPort = 10777;
	}
	
	// button event handlers
	public void StartupHost ()
	{
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}
	
	public void JoinGame ()
	{
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();
	}
	
	public void Disconnect ()
	{
		NetworkManager.singleton.StopHost ();
	}

	//make sure correct buttons appear on different scenes 
	void OnLevelWasLoaded (int level)
	{
		if (level == 0)
		{
			GameObject.Find ("Host Button").GetComponent<Button> ().onClick.RemoveAllListeners ();
			GameObject.Find ("Host Button").GetComponent<Button> ().onClick.AddListener (StartupHost);
			
			GameObject.Find ("Join Button").GetComponent<Button> ().onClick.RemoveAllListeners ();
			GameObject.Find ("Join Button").GetComponent<Button> ().onClick.AddListener (JoinGame);
		}
		else
		{
//			GameObject.FindGameObjectWithTag("Disconnect").GetComponent<Button>().onClick.RemoveAllListeners();
//			GameObject.FindGameObjectWithTag("Disconnect").GetComponent<Button>().onClick.AddListener (NetworkManager.singleton.StopHost);
		}
	}
}
