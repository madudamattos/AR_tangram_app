using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(FindRightTemplate))]
public class CheckPosition : MonoBehaviour
{
    // Infos about template piece 
    private FindRightTemplate script;
    public GameObject templatePiece = null;
    private Material colorMaterial;

    // Infos about current piece
    private string pieceName;
    private GameObject proximity_sensor;
    private Collider ps_collider;
    private GameObject mesh;

    // Suporting variables
    [SerializeField] Material highlightMaterial;
    private Vector3 cp;
    private bool flag = false;
    public bool templateFound = false;
    private bool isChecking = false;
    private float media = 0;
    private int queueCount = 0;
    private Queue<bool> queue = new Queue<bool>();
    
    void OnEnable()
    {
        script = this.GetComponent<FindRightTemplate>();
        pieceName = this.gameObject.name;
        templatePiece = script.GetTemplate();
        cp = templatePiece.transform.position;
        proximity_sensor = this.transform.Find("ProximitySensor").gameObject;
        ps_collider = proximity_sensor.GetComponent<Collider>();
    }
    
    void Update()
    {
        if (templateFound) return;

        // verifing variables
        if (pieceName == null) {
            pieceName = this.gameObject.name;
            return;
        }

        if (templatePiece == null)
        {
            templatePiece = script.GetTemplate();
            cp = templatePiece.transform.position;
            return;
        }

        if (proximity_sensor == null)
        {
            proximity_sensor = this.transform.Find("ProximitySensor").gameObject;
            ps_collider = proximity_sensor.GetComponent<Collider>();
            return;
        }

        if (mesh == null) {
            mesh = this.transform.Find("Mesh").gameObject;
            colorMaterial = mesh.GetComponent<Renderer>().material;
            return;
        }

        Debug.Log("Check Position loop");

        // game loop
        flag = CheckTemplatePosition();

        if (flag && !isChecking)
        {
            Debug.Log("Started coroutine");
            StartCoroutine(IsSamePosition());
        }

        if (isChecking)
        {
            Debug.Log("Enqueing + " + templatePiece);
            queue.Enqueue(flag);
        }

    }

    public bool CheckTemplatePosition()
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

            if (media > 0.75f)
            {
                script.ChangeTemplateMaterial(colorMaterial);
                templateFound = true;
            }
        }
    }

    public bool TemplateFound()
    {
        if(templateFound) return true;
        return false;
    }

    

}
