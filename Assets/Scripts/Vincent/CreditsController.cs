using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour {
    private enum Section { Root, Abdul, Brandon, Elygh, Vincent}

    private int section = 0;
    private bool isInDeveloperCreditSection;
    [Header("Text")] 
    public GameObject abdulText;
    public GameObject brandonText;
    public GameObject elyghText;
    public GameObject vincentText;
    public GameObject thirdPartyText;
    public GameObject honorableMentionsText;
    
    [Header("Buttons")]
    public GameObject rootButtons;
    public GameObject developersButtons;

    private void Awake() {
        disableAllDeveloperText();
        developersButtons.SetActive(false);
        thirdPartyText.SetActive(false);
        honorableMentionsText.SetActive(false);
        rootButtons.SetActive(true);
    }

    public void BackButton() {
        if (rootButtons.activeSelf) {
            SceneManager.LoadScene("Main_Menu");
            return;
        }
        if (isInDeveloperCreditSection) {
            disableAllDeveloperText();
            developersButtons.SetActive(true);
            isInDeveloperCreditSection = false;
            return;
        }
        thirdPartyText.SetActive(false);
        honorableMentionsText.SetActive(false);
        developersButtons.SetActive(false);
        rootButtons.SetActive(true);
    }

    public void DevelopersButton() {
        rootButtons.SetActive(false);
        developersButtons.SetActive(true);
    }

    public void ThirdPartyButton() {
        thirdPartyText.SetActive(true);
        rootButtons.SetActive(false);
    }

    public void HonorableMentionsButton() {
        honorableMentionsText.SetActive(true);
        rootButtons.SetActive(false);
    }

    public void AbdulButton() {
        developersButtons.SetActive(false);
        abdulText.SetActive(true);
        isInDeveloperCreditSection = true;
    }

    public void BrandonButton() {
        developersButtons.SetActive(false);
        brandonText.SetActive(true);
        isInDeveloperCreditSection = true;
    }

    public void ElyghButton() {
        developersButtons.SetActive(false);
        elyghText.SetActive(true);
        isInDeveloperCreditSection = true;
    }

    public void VincentButton() {
        developersButtons.SetActive(false);
        vincentText.SetActive(true);
        isInDeveloperCreditSection = true;
    }

    private void disableAllDeveloperText() {
        abdulText.SetActive(false);
        brandonText.SetActive(false);
        elyghText.SetActive(false);
        vincentText.SetActive(false);
    }
}
