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

    [Header("EqualPieces")]
    [SerializeField] List<GameObject> equalPieces = new List<GameObject>();

    [Header("Piece Detectors")]
    [SerializeField] CheckCollisionRight _checkCollisionRight;
    [SerializeField] CheckCollisionLeft _checkCollisionLeft;

    [SerializeField] GameObject[] PieceDetector = new GameObject[7];
    private GameObject currentARGameObj = null;

    [SerializeField] GameObject meshDebug;

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

            // if (currentARGameObj == null) Debug.Log("[CONTROLSCENE]: current ar game obj is null");

            if (!waiting) templateFound = currentARGameObj.GetComponent<CheckPosition>().TemplateFound();

            if (templateFound)
            {
                Debug.Log("[CONTROLSCENE]: Entrou if templatefound");
                found++;

                Debug.Log("[CONTROLSCENE]: Found: " + found);
                
                // game over 
                if (found >= 7)
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
        string col_r = _checkCollisionRight.GetCollision();
        string col_l = _checkCollisionLeft.GetCollision();

        string col = (col_r != "") ? col_r : col_l;

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

            // Verifica��o para n�o repetir o mesmo objeto
            if (selectedObj == currentARGameObj || selectedObj == null)
            {
                //Debug.Log("[CONTROLSCENE]: ignoring selected obj.");
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
        for (int i = 1; i < templates.transform.childCount; i++)
        {
            templates.transform.GetChild(i).gameObject.SetActive(i == index);
        }

        // index = -1 � para tirar a figura da cena
        if (index == -1) return; 

        // coloca os templates no lugar da figura
        GameObject templateRef = templates.transform.GetChild(0).gameObject;
        GameObject templateChoose = templates.transform.GetChild(index).gameObject;

        templateRef.transform.position = templateChoose.transform.position;
        templateRef.transform.rotation = templateChoose.transform.rotation;
        templateRef.transform.localScale = templateChoose.transform.localScale;

        int count = templateChoose.transform.childCount;

        for (int i = 1; i < count; i++)
        {
            templateRef.transform.GetChild(i).transform.position = templateChoose.transform.GetChild(i).transform.position;
            templateRef.transform.GetChild(i).transform.rotation = templateChoose.transform.GetChild(i).transform.rotation;
            templateRef.transform.GetChild(i).transform.localScale = templateChoose.transform.GetChild(i).transform.localScale;
        }

        // coloca mesh_2 no lugar 
        int gap = equalPieces.Count / 2;
        
        for (int i = 0; i < gap; i++)
        {

            GameObject mesh_1 = equalPieces[i].GetComponent<FindRightTemplate>().GetTemplateMesh();

            GameObject mesh_2 = equalPieces[i + gap].GetComponent<FindRightTemplate>().GetTemplateMesh();

            equalPieces[i + gap].GetComponent<FindRightTemplate>().SetTemplateMeshTransform(mesh_1.transform, 2);
            equalPieces[i].GetComponent<FindRightTemplate>().SetTemplateMeshTransform(mesh_2.transform, 2);
        }

    }

    public void CalibrationScreen()
    {
        // Desativa o mesh renderer das pe�as virtuais e dos placeholders
        ChangePieceMesh();
        ChangePiecesPlaceholder();

        // Desativa o script de calibra��o
        this.GetComponent<Calibration>().enabled = false;

        menus[0].SetActive(false);
        menus[1].SetActive(true);
    } 

    public void StartScreen()
    {
        menus[1].SetActive(false);
        menus[3].SetActive(true);
    }

    public void SelectModeScreen(int i)
    {
        mode = i;
        menus[3].SetActive(false);
        menus[2].SetActive(true);
    }

    public void SelectFigureScreen(int i)
    {
        figure = i;
        menus[2].SetActive(false);
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
            if(mode == 1) menus[4].SetActive(true);

            // cameraCanvas.SetActive(true);
            arucoTracking.SetActive(true);
            applicationCoordinator.SetActive(true);

            waiting = true;
            gameloop = true;
            // found++;
        }

        menus[5].SetActive(true);
    }

    public void ResetGame()
    {
        ActivateFigure(-1);

        int pieces = tangram.transform.childCount;

        if (mode == 0)
        {
            // Reset individual pieces 
            for (int i = 0; i < pieces; i++)
            {
                tangram.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = false;
                tangram.transform.GetChild(i).gameObject.SetActive(false);
            }

            tangram.SetActive(false);

            ResetTangramPos();
        }
        else
        {
            // Reset AR tangram
            for (int i = 0; i < ARGameObjects.Length; i++)
            {
                ARGameObjects[i].GetComponent<FindRightTemplate>().ChangeTemplateMaterial(null);
                ARGameObjects[i].GetComponent<FindRightTemplate>().DeactivateTemplateMesh();
                ARGameObjects[i].GetComponent<CheckPosition>().SetTemplateFound(false);
            }

            // Reativate piece placeholders
            for (int i = 0; i < ARGameObjects.Length; i++)
            {
                PieceDetector[i].SetActive(false);
            }
        }

        // Reset variables
        found = 0;
        waiting = false;
        gameloop = false;
        currentARGameObj = null;
        figure = -1;
        mode = -1;

        // Reset menus
        menus[3].SetActive(true);
        menus[2].SetActive(false);
        menus[4].SetActive(false);
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

        meshDebug.SetActive(meshState);
    }


    public void ChangePieceMesh()
    {
        meshState = !meshState;

        for (int i = 0; i < ARGameObjects.Length; i++)
        {
            ARGameObjects[i].transform.Find("Mesh").gameObject.SetActive(meshState);
        }
    }

    public GameObject GetCorrespondentPiece(GameObject obj)
    {
        int i;

        int gap = equalPieces.Count / 2;

        // encontra a pe�a na lista
        for(i=0; i<equalPieces.Count; i++)
        {
            if (equalPieces[i] == obj) break;
        }

        // retorna correspondente
        if(i<gap)
        {
            return equalPieces[i + gap];
        } 
        else
        {
            return equalPieces[i - gap];
        }
    }

    public void ShowHint()
    {
        Debug.Log((currentARGameObj == null) ? "currentARGameObj is NULL" : $"currentARGameObj: {currentARGameObj.name}");

        if (currentARGameObj == null)
        {
            Debug.LogError("[CONTROL SCENE]: SHOW HINT: ERRO: currentARGameObj n�o foi atribu�do!");
            return;
        }

        GameObject correspondentPiece = GetCorrespondentPiece(currentARGameObj);
        Debug.Log((correspondentPiece == null) ? "correspondentPiece is NULL" : $"correspondentPiece: {correspondentPiece.name}");

        if (correspondentPiece == null)
        {
            Debug.LogError("[CONTROL SCENE]: SHOW HINT:ERRO: correspondentPiece retornou null.");
            return;
        }

        FindRightTemplate correspondentTemplate = correspondentPiece.GetComponent<FindRightTemplate>();
        if (correspondentTemplate == null)
        {
            Debug.LogError("[CONTROL SCENE]: SHOW HINT: correspondentPiece n�o tem FindRightTemplate.");
            return;
        }

        bool isMesh_1active = correspondentTemplate.GetTemplateMeshActive(2);
        Debug.Log($"isMesh_1active: {isMesh_1active}");

        FindRightTemplate currentTemplate = currentARGameObj.GetComponent<FindRightTemplate>();
        if (currentTemplate == null)
        {
            Debug.LogError("[CONTROL SCENE]: SHOW HINT: ERRO: currentARGameObj n�o tem FindRightTemplate.");
            return;
        }

        if (isMesh_1active)
            currentTemplate.ActivateTemplateMesh(2);
        else
            currentTemplate.ActivateTemplateMesh(1);

        if (soundHint == null)
        {
            Debug.LogError(" ERRO: soundHint n�o foi atribu�do.");
            return;
        }

        soundHint.Play();
    }


    public void GameOver()
    {
        Instantiate(confetti, confettiRef.position, Quaternion.identity);
        soundGameOver.Play();
    }

    public bool GetGameLoop()
    {
        return gameloop;
    }
}
