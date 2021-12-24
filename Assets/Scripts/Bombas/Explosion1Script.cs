using UnityEngine;
using System.Collections;

public class Explosion1Script : MonoBehaviour 
{    
    public Texture[] frame_explosao = new Texture[3];
    int indice = 0;
    int repeticoes = 0;
    BlocoTerrenoScript bloco_atingido_script;
    ItemScript item_script;
    MapeamentoTerreno mapeamento_script;
    PlayerScript jogador;
    BombScript bomba;
    float prox_frame = 0.05f;
    public bool bomba_espinho_on = false;
    public PlayerScript current_player;
    ExplosionScript explosao_central;
    int num_frames;
    public char eixo;
    public static int explosoes_que_acertaram;
    public static int explosoes_que_acertaram_inimigo;
    bool acerta_jogador;
    GameObject item_descoberto;

	// Use this for initialization
	void Start()
    {
        explosoes_que_acertaram = 0;
        prox_frame = 0.05f;        
    }
	
	// Update is called once per frame
	void Update()
    {
        if (Time.time > prox_frame)
        {
            GetComponent<Renderer>().material.mainTexture = frame_explosao[indice];
            indice++;
            prox_frame = Time.time + 0.05f;
        }

		if(indice == 2)
		{
			indice = 0;
			repeticoes++;            
		}

        if (repeticoes == 4)
        {
            indice = 0;
            repeticoes = 0;
            Destroy(this.gameObject);
        }

        num_frames++;
	}
  
    void OnTriggerEnter(Collider other)
    {        

        if (other.tag == "mapeamento")
        {
            mapeamento_script = other.gameObject.GetComponent<MapeamentoTerreno>();
        }

        if (other.tag == "Player")
        {
            if (explosoes_que_acertaram < 1)
            {
                explosoes_que_acertaram++;
                acerta_jogador = true;
                jogador = other.gameObject.GetComponent<PlayerScript>();
                if (jogador.pisca != true)
                {
                    jogador.atingido = true;
                }
            }                     
        }
       
        if (other.tag == "bloco")
        {
            bloco_atingido_script = other.GetComponent<BlocoTerrenoScript>();
            bloco_atingido_script.destruido = true;
            if (bloco_atingido_script.tem_item_no_bloco)
            {
                item_descoberto = bloco_atingido_script.item;
            }
        }
            
        if (other.tag == "inimigo")
        {
            explosoes_que_acertaram_inimigo++;
            if (explosoes_que_acertaram_inimigo < 2)
            {
                Destroy(other.gameObject);                
                PlayerScript.pontuacao += 800;              
            }
        }

        if(other.tag == "item")
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

        if (other.tag == "bomba")
        {
           bomba = other.gameObject.GetComponent<BombScript>();
           bomba.destroi_bomba = true;
        }
     }   
}