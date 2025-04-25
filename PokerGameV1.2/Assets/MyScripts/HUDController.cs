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
        shells = PlayerPrefs.GetInt("Shells");
        moneyOutput.text = shells.ToString() + " Shells";
    }

    // Update is called once per frame
    void Update()
    {
      shells = PlayerPrefs.GetInt("Shells");
      moneyOutput.text = shells.ToString() + " Shells";
    }
}
