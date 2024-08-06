using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Menu's")]
    [SerializeField] GameObject _MainMenu;
    [SerializeField] GameObject _PauseMenu;

    bool bInGame = false;
    bool bPaused = false;

    private void Update()
    {
        if (bInGame && Input.GetKeyDown(KeyCode.Escape) && !bPaused)
        {
            _PauseMenu.SetActive(true);
        }

        if (bInGame && Input.GetKeyDown(KeyCode.Escape) && bPaused)
        {
            _PauseMenu.SetActive(false);
        }
    }

    public void HostGame()
    {
        _MainMenu.SetActive(false);
        bInGame = true;
    }

    public void JoinGame()
    {
        _MainMenu.SetActive(false);
        bInGame = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
