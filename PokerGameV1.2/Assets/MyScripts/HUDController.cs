using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{

    public TextMeshProUGUI moneyOutput;

    private int shells;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Shells")) {
          PlayerPrefs.SetInt("Shells", 1000);
        }
        shells = PlayerPrefs.GetInt("Shells");
        if (shells <= 0)
        {
          PlayerPrefs.SetInt("Shells", 100);
        }
        moneyOutput.text = shells.ToString() + " Shells";
    }

    // Update is called once per frame
    void Update()
    {
      if (!PlayerPrefs.HasKey("Shells")) {
        PlayerPrefs.SetInt("Shells", 1000);
      }
      shells = PlayerPrefs.GetInt("Shells");
      moneyOutput.text = shells.ToString() + " Shells";
    }
}
