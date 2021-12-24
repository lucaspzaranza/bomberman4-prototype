using UnityEngine;
using System.Collections;

public class ExplosionScript : BombScript 
{
	public Texture [] frame_explosao = new Texture[3];    
	int indice = 0;
	int repeticoes = 0;   
    BlocoTerrenoScript bloco_atingido_script;
    ItemScript item_script;    
	float prox_frame_animacao = 0.05f;
    public PlayerScript player;
    GameObject item_descoberto;

	// Use this for initialization
	void Start () 
	{
        
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (Time.time > prox_frame_animacao)
		{
			GetComponent<Renderer>().material.mainTexture = frame_explosao[indice];
			indice++;
            prox_frame_animacao = Time.time + 0.05f;
		}
		
		if(indice == 2)
		{
			indice = 0;
			repeticoes++;
		}
		if(repeticoes == 4)
		{
			indice = 0;
			repeticoes = 0;            
            player.num_bombas++;          
			Destroy(this.gameObject);
		}		
	}
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "inimigo")
        {
            Explosion1Script.explosoes_que_acertaram_inimigo++;
            if (Explosion1Script.explosoes_que_acertaram_inimigo < 2)
            {
                Destroy(other.gameObject);
                PlayerScript.pontuacao += 800;
            }
        }

        if (other.tag == "Player")
        {
            if (Explosion1Script.explosoes_que_acertaram < 1)
            {
                Explosion1Script.explosoes_que_acertaram++;
                jogador = other.gameObject.GetComponent<PlayerScript>();
                if (jogador.pisca != true)
                {
                    jogador.atingido = true;
                }     
            }                     
        }

        if (other.tag == "explosion 1" || other.tag == "explosion 2")
        {                        
            if(other.transform.IsChildOf(transform) == false)
            {
                if (other.transform.position == transform.position)
                {
                    Destroy(other.gameObject);
                }                
            }            
        }

        if (other.tag == "bloco" || other.tag == "item")
        {                        
            if (transform.position.x - other.transform.position.x == 1.0f || transform.position.x - other.transform.position.x == -1.0f)
            {
                if (transform.position.y - other.transform.position.y == 0)
                {
                    if (other.tag == "bloco")
                    {
                        bloco_atingido_script = other.GetComponent<BlocoTerrenoScript>();
                        bloco_atingido_script.destruido = true;
                        if (bloco_atingido_script.tem_item_no_bloco)
                        {
                            item_descoberto = bloco_atingido_script.item;
                        }
                    }
                    else
                    {
                        if (item_descoberto != other.gameObject)
                        {
                            item_script = other.GetComponent<ItemScript>();
                            if (other.name != "Caveira")
                            {
                                item_script.explodido = true;
                            }
                            else
                            {
                                item_script.reinstancia_caveira();
                                Destroy(other.gameObject);
                            }
                        }          
                    }
                }
            }
            else if (transform.position.y - other.transform.position.y == 1.0f || transform.position.y - other.transform.position.y == -1.0f)
            {
                if (transform.position.x - other.transform.position.x == 0.0f)
                {
                    if (other.tag == "bloco")
                    {
                        bloco_atingido_script = other.GetComponent<BlocoTerrenoScript>();
                        bloco_atingido_script.destruido = true;
                        if (bloco_atingido_script.tem_item_no_bloco)
                        {
                            item_descoberto = bloco_atingido_script.item;
                        }
                    }
                    else
                    {
                        if (item_descoberto != other.gameObject)
                        {
                            item_script = other.GetComponent<ItemScript>();
                            if (other.name != "Caveira")
                            {
                                item_script.explodido = true;
                            }
                            else
                            {
                                item_script.reinstancia_caveira();
                                Destroy(other.gameObject);
                            }
                        }         
                    }        
                }
            }           
        }        
    }    
}