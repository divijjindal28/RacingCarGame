using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaunchManager : MonoBehaviour
{

    public TMP_InputField playerName;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
            playerName.text = PlayerPrefs.GetString("PlayerName");


    }

    public void SetName(string name) {
        PlayerPrefs.SetString("PlayerName", name);
    }

    public void ConnectSingle() {
        SceneManager.LoadScene("CartRacing");
    }
}
