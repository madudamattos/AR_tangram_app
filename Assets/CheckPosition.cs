using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class CheckPosition : MonoBehaviour
{
    // Infos about template piece 
    [SerializeField] FindRightTemplate script;
    private GameObject templatePiece = null;
    private Material originalMaterial;

    // Infos about current piece
    private string pieceName;
    private GameObject proximity_sensor;
    private Collider ps_collider;

    // Suporting variables
    [SerializeField] Material highlightMaterial;
    Vector3 cp;
    private bool flag = false;
    private bool sameRot = false;
    private bool templateFound = false;
    private bool isChecking = false;
    float media;
    int queueCount = 0;

    Queue<bool> queue = new Queue<bool>();
    
    void OnEnable()
    {
        pieceName = this.gameObject.name;
        templatePiece = script.GetTemplate(pieceName);
        cp = templatePiece.transform.position;
        proximity_sensor = this.transform.Find("ProximitySensor").gameObject;
        ps_collider = proximity_sensor.GetComponent<Collider>();
    }
    
    void Update()
    {
        //if (templateFound) return;

        // verifing variables
        if (pieceName == null) {
            pieceName = this.gameObject.name;
            return;
        }

        if (templatePiece == null)
        {
            templatePiece = script.GetTemplate(pieceName);
            cp = templatePiece.transform.position;
            originalMaterial = templatePiece.GetComponent<Renderer>().material;
            return;
        }

        if (proximity_sensor == null)
        {
            proximity_sensor = this.transform.Find("ProximitySensor").gameObject;
            ps_collider = proximity_sensor.GetComponent<Collider>();
            return;
        }

        // game loop
        flag = checkPosition();

        if (flag && !isChecking)
        {
            Debug.Log("Started coroutine");
            StartCoroutine(IsSamePosition());
        }

        if (isChecking)
        {
            Debug.Log("Enqueing");
            queue.Enqueue(flag);
        }

    }

    public bool checkPosition()
    {
        if(!templatePiece) return false;
        return ps_collider.bounds.Contains(cp);
    }

    IEnumerator IsSamePosition()
    {
        isChecking = true;

        yield return new WaitForSeconds(.5f);

        isChecking = false;

        int total = 0;
        int totalTrue = 0;

        Debug.Log("Tamanho da fila: " + queue.Count);
        queueCount = queue.Count;

        while (queue.Count > 0)
        {
            bool item = queue.Dequeue();
            total++;
            if (item) totalTrue++;
        }

        if (total > 0)
        {
            media = (float) totalTrue / total;

            Debug.Log("Media: " + media);

            if (media > 0.75f)
            {
                script.ChangeTemplateMaterial(highlightMaterial);
                Debug.Log("Template found");
                templateFound = true;
            }
            else
            {
                script.ChangeTemplateMaterial(originalMaterial);
                templateFound = false;
            }
        }
    }



}
