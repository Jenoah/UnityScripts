using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Save : MonoBehaviour
{
    [SerializeField]
    private ScriptedObject[] scores = null;
    [SerializeField]
    private ScriptedObject[] loadedScores = null;

    // Start is called before the first frame update
    void Start()
    {
        SaveGame();
        LoadGame();
    }

    void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Application.persistentDataPath + "savedata.dat";
        FileStream stream = new FileStream(savePath, FileMode.Create);

        formatter.Serialize(stream, scores);
        stream.Close();
    }

    void LoadGame()
    {
        string savePath = Application.persistentDataPath + "savedata.dat";
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            ScriptedObject[] retrievedInfo;
            retrievedInfo = formatter.Deserialize(stream) as ScriptedObject[];
            loadedScores = retrievedInfo;
            stream.Close();
        }
        else
        {
            print("No save found");
        }
    }
}
