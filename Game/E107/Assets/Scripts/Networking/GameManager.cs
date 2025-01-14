using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        // 로비로 이동
        PhotonNetwork.JoinLobby();
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom : {0}", other.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("IsMasterClient : {0}", PhotonNetwork.IsMasterClient);
            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom : {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("IsMasterClient : {0}", PhotonNetwork.IsMasterClient);
            LoadArena();
        }   
    }

    public override void OnJoinedRoom()
    {
        Transform spawnpt = GameObject.Find("CampSpawn").transform;

        PhotonNetwork.Instantiate("", spawnpt.position, spawnpt.rotation, 0);
    }
    #endregion

    #region public method
    public void LeaveRoom()
    {
        Debug.Log("나가기");
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region private methods
    void LoadArena()
    {
        // 마스터 클라이언트인 경우에만 호출
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("PhotonNetwork: 레벨을 로드하려고 하지만 마스터 클라이언트가 아닙니다.");
            return;
        }

        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

        // 원하는 레벨 호출
        //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    #endregion

    private void Start()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        if (!PhotonNetwork.IsMasterClient) PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.CurrentRoom.SetCustomProperties(PhotonNetwork.CurrentRoom.CustomProperties);

        Debug.Log(PhotonNetwork.CurrentRoom.Players.Keys);

        List<Player> player = new List<Player>();
        foreach(Player playerId in PhotonNetwork.CurrentRoom.Players.Values)
        {
            player.Add(playerId);
            Debug.Log(playerId.ToString());
        }

        for(int i = 0; i<PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            string name = player[i].NickName;
            if (player[i].IsLocal) name += " its me!";
            Debug.Log(name);
        }
    }
}
