using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SetWallpaper : MonoBehaviour
{

    public Material wallpaper1;
    public Material wallpaper2;
    public Material wallpaper3;
    public Material wallpaper4;
    public Material wallpaper5;
    public Material wallpaper6;
    public Material wallpaper7;
    public Material wallpaper8;
    public Material wallpaper9;
    public Material wallpaper10;

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

    int index;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] room = new GameObject[]{ corner1, corner2, corner3, corner4,
                                              wall1, wall2, wall3LeftWin, wall3RightWin, wall3Centre,
                                              wall4LeftWin, wall4RightWin, wall4Centre };
        Material[] wallpapers = new Material[]{ wallpaper1, wallpaper2, wallpaper3, wallpaper4, wallpaper5,
                                              wallpaper6, wallpaper7, wallpaper8, wallpaper9, wallpaper10 };
        index = PlayerPrefs.GetInt("WallpaperIndex", 4);
        foreach(GameObject wall in room) {
          MeshRenderer mesh = wall.GetComponent<MeshRenderer>();
          mesh.material = wallpapers[index];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
