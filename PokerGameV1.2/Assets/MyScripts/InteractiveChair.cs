using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveChair : MonoBehaviour
{

    public GameObject interactPrompt;

    // Start is called before the first frame update
    void Start()
    {
        interactPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactPrompt.activeSelf && Input.GetButtonDown("Interact")) {
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
