using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPromptPoker : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject cardGameUI;
    public Camera cardGameCamera;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        interactPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactPrompt.activeSelf && Input.GetButtonDown("Interact")) {
            cardGameCamera.enabled = true;
            mainCamera.enabled = false;
            cardGameUI.SetActive(true);
            cardGameUI.GetComponent<CardGameUI>().from_start();
            Debug.Log("Game Started");
            interactPrompt.SetActive(false);
        }
    }

    void OnTriggerEnter() {
      interactPrompt.SetActive(true);
    }

    void OnTriggerExit() {
      interactPrompt.SetActive(false);
    }
}
