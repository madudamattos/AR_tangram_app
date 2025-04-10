using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class CheckPosition : MonoBehaviour
{
    // Infos about template piece 
    [SerializeField] FindRightTemplate script;
    private GameObject templatePiece = null;
    private GameObject mesh;
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

    Queue<bool> queue = new Queue<bool>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        pieceName = this.gameObject.name;
        templatePiece = script.GetTemplate(pieceName);
        cp = templatePiece.transform.position;
        proximity_sensor = this.gameObject.transform.Find("Proximity sensor").gameObject;
        ps_collider = proximity_sensor.GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (templateFound) return;

        if (templatePiece == null)
        {
            templatePiece = script.GetTemplate(pieceName);
            cp = templatePiece.transform.position;
        }

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
        //Debug.Log("Collider bounds:" + ps_collider.bounds.Contains(cp));
        return ps_collider.bounds.Contains(cp);
    }

    IEnumerator IsSamePosition()
    {
        isChecking = true;

        yield return new WaitForSeconds(1f);

        isChecking = false;

        int total = 0;
        int totalTrue = 0;

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
        }
    }



}
