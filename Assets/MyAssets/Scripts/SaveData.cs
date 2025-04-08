using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour
{
    [SerializeField] Transform OVRCenterEye;
    [SerializeField] Transform tableAnchor;
    [SerializeField] Transform tangramFigure;
    [SerializeField] Transform tangramID1;
    [SerializeField] Transform tangramID2;
    [SerializeField] Transform tangramID4;

    [System.Serializable]
    public class TransformData
    {
        public string name;
        public float[] position;
        public float[] rotation;

        public TransformData(string name, Transform transform)
        {
            this.name = name;
            this.position = new float[] { transform.position.x, transform.position.y, transform.position.z };
            this.rotation = new float[] { transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z };
        }
    }

    [System.Serializable]
    public class TransformCollection
    {
        public List<TransformData> Transforms = new List<TransformData>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            exportData();
        }
    }

    void exportData()
    {
        TransformCollection data = new TransformCollection();

        data.Transforms.Add(new TransformData("OVRCenterEye", OVRCenterEye));
        data.Transforms.Add(new TransformData("tableAnchor", tableAnchor));
        data.Transforms.Add(new TransformData("tangramFigure", tangramFigure));
        data.Transforms.Add(new TransformData("tangramID1", tangramID1));
        data.Transforms.Add(new TransformData("tangramID2", tangramID2));
        data.Transforms.Add(new TransformData("tangramID4", tangramID4));

        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/transforms.json";

        File.WriteAllText(path, json);
        Debug.Log("Dados exportados para " + path);
    }
}
