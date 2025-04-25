using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBlackjack : MonoBehaviour
{

    public GameObject interactPrompt;
    public DeckManager deckManager;
    public GameObject hitButton;
    public GameObject standButton;

    // Start is called before the first frame update
    void Start()
    {
        hitButton.SetActive(false);
        standButton.SetActive(false);
        interactPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactPrompt.activeSelf && Input.GetButtonDown("Interact"))
        {
            Debug.Log("Game Started");
            interactPrompt.SetActive(false);
            hitButton.SetActive(true);
            standButton.SetActive(true);
            deckManager.StartGame();
        }
    }

    void OnTriggerEnter()
    {
        interactPrompt.SetActive(true);
    }

    void OnTriggerExit()
    {
        interactPrompt.SetActive(false);
    }
}
