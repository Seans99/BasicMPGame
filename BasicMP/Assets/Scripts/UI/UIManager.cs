using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Menu's")]
    [SerializeField] GameObject _MainMenu;
    [SerializeField] GameObject _HUD;

    [Header("Network")]
    [SerializeField] NetworkManager _NetworkManager;

    public void HostGame()
    {
        _MainMenu.SetActive(false);
        _HUD.SetActive(true);
        _NetworkManager.StartHost();
    }

    public void JoinGame()
    {
        _MainMenu.SetActive(false);
        _NetworkManager.StartClient();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
