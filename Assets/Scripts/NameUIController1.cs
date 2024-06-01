using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameUIController1 : MonoBehaviour
{
    
    public Transform target;
    public TextMeshProUGUI playerNameText;
    CanvasGroup canvasGroup;
    public Renderer carRend;
    public TextMeshProUGUI lapDisplay;
    CheckPointManager cpManager;

    int carRego;
    // Start is called before the first frame update
    void Start()
    {

        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        playerNameText = this.GetComponent<TextMeshProUGUI>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        carRego = Leaderboard.RegisterCar(playerNameText.text);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!RaceMonitor.racing) { canvasGroup.alpha = 0; return; }
        if (carRend == null) return;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool carInView = GeometryUtility.TestPlanesAABB(planes, carRend.bounds);
        canvasGroup.alpha = carInView ? 1 : 0;
        this.transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 1.2f);

        if (cpManager == null)
            cpManager = target.gameObject.GetComponent<CheckPointManager>();

        Leaderboard.SetPosition(carRego, cpManager.lap, cpManager.checkPoint,cpManager.timeEntered);
        string position = Leaderboard.getPosition(carRego);
        lapDisplay.text = position;// + " " + cpManager.lap + " (" + cpManager.checkPoint + ")";
        //lapDisplay.text = "Lap: ";
    }
}
