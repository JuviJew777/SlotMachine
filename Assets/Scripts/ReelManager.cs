using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Clase encargada del manejo y logica de los Reels dentro del juego.
/// </summary>
public class ReelManager : MonoBehaviour
{
    #region Variables

    [Header("Strips Values")]
    [SerializeField]
    int[] currentID = new int[3];
    [SerializeField]
    ReelData[] reelsObjects = new ReelData[3];
    GameObject[] winingSymbols = new GameObject[3];
    Spin currentWinSpin = new Spin();

    Transform[] lastCardCreated = new Transform[3];

    enum GameStates
    {
        Playing,
        FirstReel,
        SecondReel,
        ThirdReel,
        End
    }
    GameStates currentState = GameStates.Playing;

    #endregion



    private void Update()
    {
        if (currentState == GameStates.End)
        {
            if(currentWinSpin.ActiveReelCount > 0)
            {
                RewardModalManager.Instance.StartSuccessAnimation(currentWinSpin.ActiveReelCount,currentWinSpin.WinAmount);
                currentState = GameStates.Playing;
                if(currentWinSpin.ActiveReelCount < 3)
                {
                    winingSymbols[0].GetComponent<Animator>().Play("FadeInOut");
                    winingSymbols[1].GetComponent<Animator>().Play("FadeInOut");
                }
                else
                {
                    winingSymbols[0].GetComponent<Animator>().Play("FadeInOut");
                    winingSymbols[1].GetComponent<Animator>().Play("FadeInOut");
                    winingSymbols[2].GetComponent<Animator>().Play("FadeInOut");
                }
            }
        }
    }

    #region Functions

    /// <summary>
    /// Funcion para activar el movimiento de las bandas al momento de clickear el boton spin
    /// </summary>
    public void ActivateReels()
    {
        currentState = GameStates.Playing;
        GetWinData(GameManager.instance.GetRandomSpin());
        //GetWinData(GameManager.instance.GetRandomSpin(0));

        foreach (ReelData n in reelsObjects)
        {
            n.canSpawn = true;
        }

        for (int i = 0; i < 3; i++)
            foreach(Transform t in reelsObjects[i].cardContainer)
            {
                if (t.gameObject.activeSelf)
                {
                    t.GetComponent<SymbolScript>().SwitchMovement();
                }
            }
        StartCoroutine(ChangeState(1, GameStates.FirstReel));
    }

    /// <summary>
    /// Coorrutina para esperar a la siguiente banda
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="newSate"></param>
    /// <returns></returns>
    IEnumerator ChangeState(float seconds,GameStates newSate)
    {
        yield return new WaitForSeconds(seconds);
        currentState = newSate;
    }

    /// <summary>
    /// Funcion para detener el movimiento de todos los simbolos por banda seleccionada
    /// </summary>
    /// <param name="reelIndex"></param>
    void DeactivateReels(int reelIndex)
    {

        reelsObjects[reelIndex].canSpawn = false;
        foreach (Transform child in reelsObjects[reelIndex].cardContainer.transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.GetComponent<SymbolScript>().SwitchMovement();
            }
        }
    }

    /// <summary>
    /// Obtenemos la data del json ya parseada.
    /// </summary>
    /// <param name="winSpin"></param>
    public void GetWinData(Spin winSpin)
    {
        currentWinSpin = new Spin(winSpin.ReelIndex,winSpin.ActiveReelCount,winSpin.WinAmount);
    }

    /// <summary>
    /// Sumamos la cantidad de ids por banda
    /// </summary>
    /// <param name="reelIndex"></param>
    public void AddID(int reelIndex)
    {
        currentID[reelIndex]++;
    }

    /// <summary>
    /// Guardamos la posision de la ultima carta spawneada
    /// </summary>
    /// <param name="strip"></param>
    /// <param name="newCard"></param>
    public void SetLastCard(int strip,Transform newCard)
    {
        lastCardCreated[strip] = newCard;
    }

    /// <summary>
    /// Se activa la carta y se le posiciona justo en la posicion del pading para simular el movimiento
    /// </summary>
    /// <param name="strip"></param>
    public void AddNewCard(int strip)
    {
        if (!reelsObjects[strip].canSpawn) return;
        int nextCardIndez = currentID[strip] % reelsObjects[strip].cardContainer.childCount;
        GameObject newCard = reelsObjects[strip].cardContainer.transform.GetChild(nextCardIndez).gameObject;
        newCard.SetActive(true);
        if (!newCard.GetComponent<SymbolScript>().CanMove1)
        {
            newCard.GetComponent<SymbolScript>().SwitchMovement();
        }
        newCard.transform.position = new Vector3(lastCardCreated[strip].position.x, lastCardCreated[strip].position.y +GameManager.instance.InstancePadding,0);
        currentID[strip]++;
        SetLastCard(strip, newCard.transform);
    }

    /// <summary>
    /// Funcion para obtener los datos del symbolo mostrado en la zona de win
    /// </summary>
    /// <param name="strip"></param>
    /// <param name="_selectedID"></param>
    public void SetSelectedID(int strip,int _selectedID)
    {
        reelsObjects[strip].iD = _selectedID;
        if (currentState == GameStates.FirstReel && strip == 0)
        {
            if (reelsObjects[strip].iD == currentWinSpin.ReelIndex[strip] )
            {
                reelsObjects[strip].cardContainer.GetChild(_selectedID).localPosition = Vector3.zero;
                winingSymbols[0] = reelsObjects[strip].cardContainer.GetChild(_selectedID).gameObject;
                DeactivateReels(strip);
                StartCoroutine(ChangeState(.3f, GameStates.SecondReel));

            }
        }
        else if (currentState == GameStates.SecondReel && strip == 1)
        {
            if (reelsObjects[strip].iD == currentWinSpin.ReelIndex[strip] )
            {
                reelsObjects[strip].cardContainer.GetChild(_selectedID).localPosition = Vector3.zero;
                winingSymbols[1] = reelsObjects[strip].cardContainer.GetChild(_selectedID).gameObject;
                DeactivateReels(strip);
                StartCoroutine(ChangeState(0.3f, GameStates.ThirdReel));

            }
        }
        else if (currentState == GameStates.ThirdReel && strip == 2)
        {
            if (reelsObjects[strip].iD == currentWinSpin.ReelIndex[strip] )
            {
                reelsObjects[strip].cardContainer.GetChild(_selectedID).localPosition = Vector3.zero;
                winingSymbols[2] = reelsObjects[strip].cardContainer.GetChild(_selectedID).gameObject;
                DeactivateReels(strip);
                StartCoroutine(ChangeState(0.3f, GameStates.End));

            }
        }
    }

    #endregion
}

[System.Serializable]
public class ReelData
{
    public Transform cardContainer;
    public Transform startPosition;
    public int iD;
    public bool canSpawn = true;
}
