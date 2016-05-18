using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour {
    public static string roomName = "ThisRoom";
	public bool AutoConnect = true;
	public byte Version = 0;

	private bool ConnectInUpdate = true;
	// Use this for initialization
	void Start () {
		PhotonNetwork.autoJoinLobby = false;
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
		}
	}


	// below, we implement some callbacks of PUN
	// you can find PUN's callbacks in the class PunBehaviour or in enum PhotonNetworkingMessage


	public virtual void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinOrCreateRoom();");
		RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 8 };
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        GameObject.Find("Canvas/Room Label").GetComponent<Text>().text = "Room: " + roomName;
    }

	public virtual void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinOrCreateRoom();");
		RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 8 };
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
	}

	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 8 };
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
	}

	// the following methods are implemented to give you some context. re-implement them as needed.

	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}

	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
	}
}
