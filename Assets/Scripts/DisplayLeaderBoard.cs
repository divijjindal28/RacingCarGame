using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLeaderBoard : MonoBehaviour
{
    public TextMeshProUGUI first;
    public TextMeshProUGUI second;
    public TextMeshProUGUI third;
    public TextMeshProUGUI fourth;

    
    void LateUpdate()
    {
        List<string> places = Leaderboard.GetPlaces();
        first.text = places[0];
        second.text = places[1];
        third.text = places[2];
        fourth.text = places[3];
    }
}
