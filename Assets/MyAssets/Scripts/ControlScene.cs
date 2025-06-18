using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScene : MonoBehaviour
{
    [Header("Tangram References")]
    [SerializeField] GameObject tangramRef;
    [SerializeField] GameObject tangram;

    [SerializeField] GameObject templates;

    [Header("Menu Screens")]
    [SerializeField] GameObject menu0;
    [SerializeField] GameObject menu1;
    [SerializeField] GameObject menu2;
    [SerializeField] GameObject menu3;
    [SerializeField] GameObject menu4;

    private int figure = -1;
    private int mode = -1;
    private int pieces = 0;

    [Header("Aruco Objects")]
    [SerializeField] GameObject applicationCoordinator;
    [SerializeField] GameObject arucoTracking;
    [SerializeField] GameObject cameraCanvas;

    [Header("AR Pieces")]
    [SerializeField] GameObject[] ARGameObjects = new GameObject[7];

    private bool gameloop = false;
    private bool templateFound = false;
    private int found = 0;

    // Update is called once per frame
    void Update()
    {

        if (gameloop && mode == 1)
        {
            GameObject currentARGameObj = ARGameObjects[found];
            templateFound = currentARGameObj.GetComponent<CheckPosition>().TemplateFound();

            if (templateFound)
            {
                // ARGameObjects[found].transform.Find("Mesh").gameObject.SetActive(false);
                found++;

                // verifica se era a ultima peça fim de jogo
                if (found >= 4)
                {
                    Debug.Log("GameOver");
                    gameloop = false;
                    return;
                }

                // ARGameObjects[found].transform.Find("Mesh").gameObject.SetActive(true);
            }

        }

    }

    public void ResetTangramPos()
    {
        tangram.SetActive(false);

        tangram.transform.position = tangramRef.transform.position;
        tangram.transform.rotation = tangramRef.transform.rotation;

        pieces = tangram.transform.childCount;

        for (int i = 0; i < pieces; i++)
        {
            tangram.transform.GetChild(i).gameObject.transform.position = tangramRef.transform.GetChild(i).gameObject.transform.position;
            tangram.transform.GetChild(i).gameObject.transform.rotation = tangramRef.transform.GetChild(i).gameObject.transform.rotation;
        }

        tangram.SetActive(true);
    }

    public void ActivateFigure(int index)
    {
        if (index == 0)
        {
            templates.transform.GetChild(0).gameObject.SetActive(true);
            templates.transform.GetChild(1).gameObject.SetActive(false);
            templates.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (index == 1)
        {
            templates.transform.GetChild(1).gameObject.SetActive(true);
            templates.transform.GetChild(0).gameObject.SetActive(false);
            templates.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (index == 2)
        {
            templates.transform.GetChild(2).gameObject.SetActive(true);
            templates.transform.GetChild(0).gameObject.SetActive(false);
            templates.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            // Deactivate all
            templates.transform.GetChild(2).gameObject.SetActive(false);
            templates.transform.GetChild(0).gameObject.SetActive(false);
            templates.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void StartScreen()
    {
        menu0.SetActive(false);
        menu1.SetActive(true);
    }

    public void SelectFigureScreen(int i)
    {
        figure = i;
        menu1.SetActive(false);
        menu2.SetActive(true);
    }

    public void SelectModeScreen(int i)
    {
        mode = i;
        menu2.SetActive(false);
        StartGame();
    }

    public void StartGame()
    {

        ResetTangramPos();

        ActivateFigure(figure);

        // Activate all tangram pieces in case of virtual assembly 
        if (mode == 0)
        {
            tangram.SetActive(true);

            pieces = tangram.transform.childCount;

            for (int i = 0; i < pieces; i++)
            {
                tangram.transform.GetChild(i).gameObject.SetActive(true);
            }

        } else
        {
            menu4.SetActive(true);

            // cameraCanvas.SetActive(true);
            arucoTracking.SetActive(true);
            applicationCoordinator.SetActive(true);

            // ARGameObjects[1].transform.Find("Mesh").gameObject.SetActive(true);

            gameloop = true;
            found++;
        }

        menu3.SetActive(true);
    }

    public void ResetGame()
    {
        ActivateFigure(-1);

        ResetTangramPos();

        int pieces = tangram.transform.childCount;

        // Reset individual pieces 
        for (int i = 0; i < pieces; i++)
        {
            tangram.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = false;
            tangram.transform.GetChild(i).gameObject.SetActive(false);
        }

        tangram.SetActive(false);

        // Reset AR tangram
 /*       for (int i = 0; i < ARGameObjects.Length; i++)
        {
            ARGameObjects[i].GetComponent<FindRightTemplate>().ChangeTemplateMaterial(null);
            ARGameObjects[i].GetComponent<FindRightTemplate>().DeactivateTemplateMesh();
        }*/

        // Reset variables
        figure = -1;
        mode = -1;

        // Reset menus
        menu1.SetActive(true);
        menu2.SetActive(false);
        menu4.SetActive(false);
    }

    public void ShowHint()
    {
        ARGameObjects[found].GetComponent<FindRightTemplate>().ActivateTemplateMesh();
    }

}
