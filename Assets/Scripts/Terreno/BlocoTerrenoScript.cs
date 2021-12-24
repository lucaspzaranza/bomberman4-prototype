using UnityEngine;
using System.Collections;

public class BlocoTerrenoScript : MonoBehaviour 
{
    MapeamentoTerreno mapeamento;   

    public Texture[] texturas = new Texture[6];
    public GameObject item;   
    public bool destruido = false;
    public bool tem_item_no_bloco = false;
    float prox_frame = 0.05f;
    int indice = 0;

	// Use this for initialization
	void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (destruido == true)
        {
            animacao_destruido();
        }
	}

    void animacao_destruido()
    {
        if (Time.time > prox_frame)
        {
            GetComponent<Renderer>().material.mainTexture = texturas[indice];
            indice++;
            prox_frame = Time.time + 0.05f;
            if (indice == 6)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mapeamento")
        {
            mapeamento = other.gameObject.GetComponent<MapeamentoTerreno>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GetComponent<Collider>().isTrigger)
            {
                GetComponent<Collider>().isTrigger = false;
            }
        }
    }

    void OnDestroy()
    {
        if (mapeamento != null)
        {
            mapeamento.blocoNoBloco = false;
            mapeamento.isEmpty = true;
            mapeamento.bloco = null;
        }

        if (tem_item_no_bloco)
        {            
            item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 7);                                                     
        }
    }
}