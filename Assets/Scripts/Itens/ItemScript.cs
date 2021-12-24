using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour 
{
    PlayerScript jogador;
    MapeamentoTerreno mapeamento;
    Explosion1Script explosion_1;
    Explosion2Script explosion_2;
    public Texture[] frames = new Texture[2];
    public Texture[] frames_explodindo = new Texture[5];
    float prox_frame = 0.06f;
    float prox_frame_explodindo = 0.055f;
    int i = 0;
    int indice = 0;
    public bool explodido = false;
    public GameObject caveira_item;
    GameObject caveira;
    static int random_efeito_caveira;
    int contador = 0;
    public bool esta_escondido = true;

	// Use this for initialization
	void Start ()
    {
        //collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (explodido == true)
        {
            destroi_item();
        }
        else
        { 
            anima_item();
        }

        /*if (transform.position.z == 7 && contador == 0)
        {
            StartCoroutine(ativa_collider());
            contador++;
        }*/
	}

    IEnumerator ativa_collider()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<Collider>().enabled = true;
    }

    void anima_item()
    {
        if (Time.time > prox_frame)
        {
            GetComponent<Renderer>().material.mainTexture = frames[i];
            i++;
            prox_frame = Time.time + 0.06f;
        }
        if (i == 2)
        {
            i = 0;
        }
    }

    void efeito_item()
    {
        switch (name)
        {
            case "Add Bomba":
            {
                if (jogador.num_bombas < 8)
                {
                    jogador.num_bombas += 1;
                }
                break;
            }
            case "Bomba Espinho Item":
            {
                jogador.tipo_bomba = "bomba espinho";
                break;
            }
            case "Bomba Relogio Item":
            {
                jogador.tipo_bomba = "bomba relogio";
                break;
            }
            case "Chuta Bomba":
            {
                jogador.chuta_bomba = true;
                break;
            }
            case "Coracao":
            {
                jogador.HP++;
                break;
            }
            case "Fogo":
            {
                if (jogador.poder_bomba < 8)
                {
                    jogador.poder_bomba += 1;
                }
                break;
            }
            case "Super Fogo":
            {
                jogador.poder_bomba = 9;
                break;
            }
            case "Hadouken Item":
            {
                jogador.hadouken_on = true;
                break;
            }
            case "Maozinha":
            {
                jogador.maozinha = true;
                break;
            }
            case "Patins":
            {
                if (jogador.PlayerSpeed < 6f)
                {
                    jogador.PlayerSpeed += 0.35f;
                }
                break;
            }
            case "Soca Bomba":
            {
                jogador.soca_bomba = true;
                break;
            }
            case "Parede":
            {
                jogador.atravessa_paredes = true;
                break;
            }
            case "Caveira":
            {                            
                jogador.efeito_caveira = true;
                random_efeito_caveira = Random.Range(1, 5);                
                jogador.efeitos_caveira(random_efeito_caveira);
                break;
            }
        }
    }

    void anula_efeito_caveira()
    {
        switch (random_efeito_caveira)
        {
            case 1:
            {
                jogador.PlayerSpeed = jogador.velocidade_antiga;
                break;
            }
            case 2:
            {
                jogador.PlayerSpeed = jogador.velocidade_antiga;
                break;
            }
            case 3:
            {
                jogador.inverte_movimento = false;
                break;
            }
            case 4:
            {
                jogador.nao_para = false;
                break;
            }
            case 5:
            {
                jogador.instancia_bomba_loucamente = false;
                break;
            }
        }        
    }

    public void reinstancia_caveira()
    {        
        int x, y;
        x = Random.Range(0, 13);
        y = Random.Range(0, 11);        
        while (FundoScript.coordenadas_script[x, y].isEmpty != true)
        {
            x = Random.Range(0, 13);
            y = Random.Range(0, 11);
        }
        caveira = Instantiate(caveira_item, new Vector3(FundoScript.coordenadas_script[x, y].transform.position.x, FundoScript.coordenadas_script[x, y].transform.position.y, 7.5f), Quaternion.AngleAxis(90, new Vector3(0,1,0))) as GameObject;
        caveira.name = "Caveira";
    }

    void destroi_item()
    {
        if (Time.time > prox_frame_explodindo)
        {
            GetComponent<Renderer>().material.mainTexture = frames_explodindo[indice];
            indice++;
            if (indice == 5)
            {
                indice = 0;
                GetComponent<Renderer>().enabled = false;
                Destroy(this.gameObject);
            }
            prox_frame_explodindo = Time.time + 0.055f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mapeamento")
        {
            mapeamento = other.gameObject.GetComponent<MapeamentoTerreno>();
        }        
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            jogador = other.gameObject.GetComponent<PlayerScript>();
            if (transform.position.x - other.transform.position.x < 0.7f && transform.position.x - other.transform.position.x > -0.7f && transform.position.y - other.transform.position.y < 0.7f && transform.position.y - other.transform.position.y > -0.7f)
            {                                
                if (name != "Caveira")
                {
                    if (jogador.efeito_caveira == true)
                    {
                        reinstancia_caveira();
                        anula_efeito_caveira();
                    }
                    jogador.efeito_caveira = false;
                }
                else
                {
                    if (jogador.efeito_caveira == true)
                    {
                        reinstancia_caveira();
                        anula_efeito_caveira();
                    }
                }
                efeito_item();
                mapeamento.itemNoBloco = false;
                mapeamento.isEmpty = true;
                Destroy(this.gameObject);
            }
        }
    }  
}
