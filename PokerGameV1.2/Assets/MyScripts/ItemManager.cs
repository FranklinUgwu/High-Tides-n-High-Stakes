using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public int price = 2000;
    public int index;
    public TextMeshProUGUI title;
    public Material wallpaper;
    public TextMeshProUGUI priceTag;
    public GameObject confirmScreen;
    public GameObject equipScreen;
    public GameObject invalidScreen;

    public GameObject corner1;
    public GameObject corner2;
    public GameObject corner3;
    public GameObject corner4;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3LeftWin;
    public GameObject wall3RightWin;
    public GameObject wall3Centre;
    public GameObject wall4LeftWin;
    public GameObject wall4RightWin;
    public GameObject wall4Centre;

    private int shells;
    private GameObject[] room;
    private int status;

    // Start is called before the first frame update
    void Start()
    {
      room = new GameObject[]{ corner1, corner2, corner3, corner4,
                                            wall1, wall2, wall3LeftWin, wall3RightWin, wall3Centre,
                                            wall4LeftWin, wall4RightWin, wall4Centre};
      priceTag.text = price.ToString() + " Shells";
      PlayerPrefs.SetInt("Wallpaper 5", 0);
      PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
      shells = PlayerPrefs.GetInt("Shells", 99999);
      soldItem();
    }

    private void soldItem() {
      status = PlayerPrefs.GetInt(title.text, 1);
      if (status == 0) {
        priceTag.text = "Owned";
      }
    }

    public void confirmPurchase() {
      if (status == 1) {
        if (shells < price) {
          invalidScreen.SetActive(true);
        } else {
          confirmScreen.SetActive(true);
        }
      } else if (status == 0) {
        equipScreen.SetActive(true);
      }
    }

    public void purchaseConfirmed() {
      PlayerPrefs.SetInt("Shells", shells - price);
      PlayerPrefs.SetInt(title.text, 0);
      PlayerPrefs.Save();
      confirmScreen.SetActive(false);
    }

    public void confirmEquip() {
      foreach(GameObject wall in room) {
        MeshRenderer mesh = wall.GetComponent<MeshRenderer>();
        mesh.material = wallpaper;
      }
      PlayerPrefs.SetInt("WallpaperIndex", index);
      PlayerPrefs.Save();
      equipScreen.SetActive(false);
    }
}
