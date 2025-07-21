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
    [SerializeField] List<GameObject> menus = new List<GameObject>();

    private int figure = -1;
    private int mode = -1;
    private int pieces = 0;

    [Header("Aruco Objects")]
    [SerializeField] GameObject applicationCoordinator;
    [SerializeField] GameObject arucoTracking;
    [SerializeField] GameObject cameraCanvas;

    [Header("AR Pieces")]
    [SerializeField] GameObject[] ARGameObjects = new GameObject[7];

    [Header("Piece Detectors")]
    [SerializeField] CheckCollision _checkCollision;
    [SerializeField] GameObject[] PieceDetector = new GameObject[7];
    private GameObject currentARGameObj = null;

    private bool gameloop = false;
    private bool templateFound = false;
    private int found = 0;
    private bool waiting = false;
    private bool meshState = true;
    private int index = -1;

    [Header("Extra Assets")]
    [SerializeField] private AudioSource soundHint;
    [SerializeField] private AudioSource soundGameOver;
    [SerializeField] private GameObject confetti;
    [SerializeField] private Transform confettiRef;

    // Update is called once per frame
    void Update()
    {
        if (gameloop && mode == 1)
        {
            selectPiece();

            if (currentARGameObj == null) Debug.Log("[CONTROLSCENE]: current ar game obj is null");

            if (!waiting) templateFound = currentARGameObj.GetComponent<CheckPosition>().TemplateFound();

            if (templateFound)
            {
                Debug.Log("[CONTROLSCENE]: Entered templatefound");
                found++;

                // game over 
                if (found > 7)
                {
                    Invoke(nameof(GameOver), 1.5f);
                    Debug.Log("[CONTROLSCENE]: GameOver");
                    gameloop = false;
                    return;
                }

                PieceDetector[index].SetActive(false);

                templateFound = false;
                waiting = true;
                    
                Debug.Log("[CONTROLSCENE]: Set waiting = true");
            }

        }
    }

    public void selectPiece()
    {
        string col = _checkCollision.GetCollision();

        if (col != "")
        {
            GameObject selectedObj = null;

            switch (col)
            {
                case "Piece.001": index = 0; break;
                case "Piece.002": index = 1; break;
                case "Piece.003": index = 2; break;
                case "Piece.004": index = 3; break;
                case "Piece.005": index = 4; break;
                case "Piece.006": index = 5; break;
                case "Piece.007": index = 6; break;
                default: return;
            }

            selectedObj = ARGameObjects[index];

            // Verificação para não repetir o mesmo objeto
            if (selectedObj == currentARGameObj || selectedObj == null)
            {
                Debug.Log("[CONTROLSCENE]: ignoring selected obj.");
                return;
            }
            
            currentARGameObj = selectedObj;
            waiting = false;
            Debug.Log("[CONTROLSCENE]: Set waiting = false");
        }

        return;
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

    public void CalibrationScreen()
    {
        // Desativa o mesh renderer das peças virtuais e dos placeholders
        ChangePieceMesh();
        ChangePiecesPlaceholder();

        // Desativa o script de calibração
        this.GetComponent<Calibration>().enabled = false;

        menus[0].SetActive(false);
        menus[1].SetActive(true);
    }

    public void StartScreen()
    {
        menus[1].SetActive(false);
        menus[2].SetActive(true);
    }

    public void SelectFigureScreen(int i)
    {
        figure = i;
        menus[2].SetActive(false);
        menus[3].SetActive(true);
    }

    public void SelectModeScreen(int i)
    {
        mode = i;
        menus[3].SetActive(false);
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
            menus[5].SetActive(true);

            // cameraCanvas.SetActive(true);
            arucoTracking.SetActive(true);
            applicationCoordinator.SetActive(true);

            waiting = true;
            gameloop = true;
            found++;
        }

        menus[4].SetActive(true);
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
        menus[2].SetActive(true);
        menus[3].SetActive(false);
        menus[5].SetActive(false);
    }

    public void ChangePiecesPlaceholder()
    {
        for (int i = 0; i < PieceDetector.Length; i++)
        {
            Transform meshTransform = PieceDetector[i].transform.Find("Mesh");

            MeshRenderer meshRenderer = meshTransform.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = meshState;
                Debug.Log($"[CONTROL SCENE]: MeshRenderer de {PieceDetector[i].name} desativado.");
            }
        }
    }


    public void ChangePieceMesh()
    {
        meshState = !meshState;

        for (int i = 0; i < ARGameObjects.Length; i++)
        {
            ARGameObjects[i].transform.Find("Mesh").gameObject.SetActive(meshState);
        }
    }

    public void ShowHint()
    {
        currentARGameObj.GetComponent<FindRightTemplate>().ActivateTemplateMesh();
        soundHint.Play();
    }

    public void GameOver()
    {
        Instantiate(confetti, confettiRef.position, Quaternion.identity);
        soundGameOver.Play();
    }
}
