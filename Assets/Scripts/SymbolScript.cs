using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles data for the symbols appearing on the screen.
/// </summary>
public class SymbolScript : MonoBehaviour
{
    [SerializeField]
    int id;
    [SerializeField]
    float speedVel;

    [SerializeField]
    bool CanMove = true;

    public int Id { get => id; set => id = value; }
    public bool CanMove1 { get => CanMove; set => CanMove = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && CanMove)
        {
            transform.position += Vector3.down * speedVel * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PaddDown"))
        {
            //Debug.Log("Hit");
            
            gameObject.SetActive(false);
            GameManager.instance.reelManager.AddNewCard(0);
        }
        else if (collision.CompareTag("PaddDown2"))
        {
            //Debug.Log("Hit");
            
            gameObject.SetActive(false);
            GameManager.instance.reelManager.AddNewCard(1);
        }
        else if (collision.CompareTag("PaddDown3"))
        {
            //Debug.Log("Hit");
            
            gameObject.SetActive(false);
            GameManager.instance.reelManager.AddNewCard(2);
        }
        else if (collision.CompareTag("winZone"))
        {
            GameManager.instance.reelManager.SetSelectedID(0,id);
        }
        else if (collision.CompareTag("winZone2"))
        {
            GameManager.instance.reelManager.SetSelectedID(1,id);
        }
        else if (collision.CompareTag("winZone3"))
        {
            GameManager.instance.reelManager.SetSelectedID(2,id);
        }
    }

    public void SwitchMovement()
    {
        CanMove = !CanMove;
    }

}
