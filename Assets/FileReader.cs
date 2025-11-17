using UnityEngine;
using System.IO;

public class TextFileReader : MonoBehaviour
{
    public string[] paths;
    public string[] contenu;
    public int lastId = -1;

    private void Start()
    {

    }

    public void Read(int _id)
    {
        if (_id == lastId) return;
        lastId = _id;
        string[] content = File.ReadAllLines(Application.dataPath + "\\" + paths[_id]);
        contenu = content;
    }
}
