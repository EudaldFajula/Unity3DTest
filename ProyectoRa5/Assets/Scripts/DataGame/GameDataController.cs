using UnityEngine;
using System.IO;

public class GameDataController : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject flareGunFloor;
    [SerializeField]
    public GameObject flareGunHand;
    public string saveFile;
    public GameData gameData = new GameData();

    private void Awake()
    {
        saveFile = Application.dataPath + "/gameData.json";
        LoadData();
    }

    public void LoadData()
    {
        if (File.Exists(saveFile))
        {
            string content = File.ReadAllText(saveFile);
            gameData = JsonUtility.FromJson<GameData>(content);

            player.transform.position = gameData.position;
            flareGunFloor.SetActive(gameData.flareGunAdquired);
            flareGunHand.SetActive(!gameData.flareGunAdquired);
        }
        else
        {
            Debug.Log("File doesn't exists");
        }
    }
    public void SaveData()
    {
        GameData newData = new GameData()
        {
            position = player.transform.position,
            flareGunAdquired = flareGunFloor.active
        };

        string Json = JsonUtility.ToJson(newData);
        File.WriteAllText(saveFile, Json);

        Debug.Log("File Saved");
    }
}
