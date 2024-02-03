using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
/// <summary>
/// Clase que se encarga del manejo de datos como back para poder manejarlos en front utilizando otras clases
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public float InstancePadding { get => instancePadding; set => instancePadding = value; }

    public ReelManager reelManager;


    #region Variables
    [SerializeField]
    TextAsset[] myJsonFiles = null;
    
    [SerializeField]
    GameObject[] currentStrips;
    reelStrips mainReel = new reelStrips();
    Strip mainSpins = new Strip();

    [Header("Creation Data")]
    [SerializeField]
    float instancePadding;
    [SerializeField]
    GameObject[] symbolsPref;

    

    #endregion

    private void Awake()
    {
        reelManager = GetComponent<ReelManager>();


        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    private void Start()
    {
        //Leemos la data de los jsons Guardandola en variables para utilizarlas mas adelante
        mainReel = reelStrips.GetJsonData(myJsonFiles[0].text);
        mainSpins = Strip.GetJsonData(myJsonFiles[1].text);

        for (int i = 0; i < currentStrips.Length; i++)
        {
            StripInstanciation(currentStrips[i].transform.GetChild(1).transform.position, i);
        }


    }

    #region Strip Creation and Logic
    /// <summary>
    /// Funcion para spawnear todos los symbolos en pantalla al iniciar el juego.
    /// </summary>
    /// <param name="initialPosition"></param>
    /// <param name="strip"></param>
    void StripInstanciation(Vector3 initialPosition, int strip)
    {
        int id = 0;
        foreach(string item in mainReel.ReelStrips[strip]){
            switch (item)
            {
                case "CLUBS":
                    CardPrefabInstance(symbolsPref[0], strip, id);
                    break;
                case "DIAMOND":
                    CardPrefabInstance(symbolsPref[1], strip, id);

                    break;
                case "HEARTS":
                    CardPrefabInstance(symbolsPref[2], strip, id);

                    break;
                case "SPADES":
                    CardPrefabInstance(symbolsPref[3], strip, id);
                    break;

                default: Debug.Log(99); break;
            }
            id++;
        }
        Transform startPos = currentStrips[strip].transform.GetChild(1).transform;
        for (int i = 0; i < 5; i++)
        {
            GameObject currentCard = currentStrips[strip].transform.GetChild(2).transform.GetChild(i).gameObject;
            currentCard.GetComponent<SymbolScript>().SwitchMovement();
            if(i!= 0)currentCard.SetActive(true);
            Vector3 newPos = new Vector3(startPos.position.x,startPos.position.y + (InstancePadding*i),0);
            currentCard.transform.position = newPos;
            reelManager.SetLastCard(strip, currentCard.transform);
            reelManager.AddID(strip);
        }
    }
    /// <summary>
    /// Dependiendo el tipo de carta spawnearemos un prefab en el reel correspondiente
    /// </summary>
    /// <param name="_prefab"></param>
    /// <param name="_strip"></param>
    /// <param name="_currentID"></param>
    void CardPrefabInstance(GameObject _prefab,int _strip, int _currentID)
    {
        GameObject newCard = Instantiate(_prefab, parent: currentStrips[_strip].transform.GetChild(2).transform);
        newCard.GetComponent<SymbolScript>().Id = _currentID;
        newCard.SetActive(false);
    }

    /// <summary>
    /// Obtenemos una de las tiradas random del json ya parseado en el código
    /// </summary>
    /// <returns></returns>
    public Spin GetRandomSpin()
    {
        int randomNumber = Random.Range(0, mainSpins.spins.Count);
        return mainSpins.spins[randomNumber];
    }
    /// <summary>
    /// Obtenemos una tirada especifica del json
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Spin GetRandomSpin(int id)
    {
        return mainSpins.spins[id];
    }
    

    
    #endregion
}


#region Json clases
[System.Serializable]
public class reelStrips
{
    public List<List<string>> ReelStrips;

    public static reelStrips GetJsonData(string jsonString)
    {
        return JsonConvert.DeserializeObject<reelStrips>(jsonString);
    }
}

[System.Serializable]
public class Strip
{
    public List<Spin> spins;
    public static Strip GetJsonData(string jsonString)
    {
        return JsonConvert.DeserializeObject<Strip>(jsonString);
    }
}

[System.Serializable]
public class Spin
{
    public List<int> ReelIndex = null;
    public int ActiveReelCount = 0;
    public int WinAmount = 0;

    public Spin(List<int> reelIndex = null, int activeReelCount =0, int winAmount = 0)
    {
        ReelIndex = reelIndex;
        ActiveReelCount = activeReelCount;
        WinAmount = winAmount;
    }
}

#endregion