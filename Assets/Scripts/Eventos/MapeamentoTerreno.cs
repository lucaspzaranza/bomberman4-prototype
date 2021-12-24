using UnityEngine;
using System.Collections;

public class MapeamentoTerreno : FundoScript
{   
    public bool isEmpty = true;
    public bool playerNoBloco;
    public bool bombaNoBloco;
    public bool itemNoBloco;
    public bool blocoNoBloco;
    public bool inimigoNoBloco;
    public static int namba;
    public GameObject bomba;
    public GameObject bloco;

	// Use this for initialization
	void Start () 
    {
        GetComponent<Renderer>().enabled = false;        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (bomba == null)
        {
            bombaNoBloco = false;
        }

        if (bloco == null)
        {
            blocoNoBloco = false;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNoBloco = true;
            isEmpty = false;            
        }

        if (other.tag == "concreto" || other.tag == "aco" || other.tag == "bloco")
        {
            if (other.tag == "bloco")
            {
                bloco = other.gameObject;
            }
            blocoNoBloco = true;
            isEmpty = false;
        }

        if (other.tag == "bomba")
        {
            bomba = other.gameObject;
            bombaNoBloco = true;
            isEmpty = false;
        }

        if (other.tag == "item")
        {
            itemNoBloco = true;
            isEmpty = false; 
        }

        if (other.tag == "inimigo")
        {
            inimigoNoBloco = true;
            isEmpty = false; 
        }                     
    }   
   
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerNoBloco = false;
            if (bombaNoBloco == false && itemNoBloco == false)
            {
                isEmpty = true;
            }
        }
        else if (other.tag == "bomba")
        {
            bombaNoBloco = false;
            if (playerNoBloco == false && itemNoBloco == false)
            {
                isEmpty = true;
            }
        }

        if (other.tag == "inimigo")
        {
            inimigoNoBloco = false;
            if (playerNoBloco == false && itemNoBloco == false)
            {
                isEmpty = true;
            }
        }            
    }
}