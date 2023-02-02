using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Different Menus")]
    public GameObject refugeHostMenu;
    public GameObject refugeClientMenu;
    public GameObject boardHostMenu;
    public GameObject boardClientMenu;

    private void OnEnable()
    {
        refugeHostMenu.SetActive(false);
        refugeClientMenu.SetActive(false);
        boardHostMenu.SetActive(false);
        boardClientMenu.SetActive(false);
    }
}
