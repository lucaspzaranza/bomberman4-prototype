using UnityEngine;
using System.Collections;

public class Explosion2Script : MonoBehaviour
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
    public char eixo;
    GameObject item_descoberto;

    // Use this for initialization
    void Start()
    {
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

        if (indice == 2)
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mapeamento")
        {
            mapeamento_script = other.gameObject.GetComponent<MapeamentoTerreno>();
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
            Explosion1Script.explosoes_que_acertaram_inimigo++;
            if (Explosion1Script.explosoes_que_acertaram_inimigo < 2)
            {
                Destroy(other.gameObject);
                PlayerScript.pontuacao += 800;
            }
        }

        if (other.tag == "item")
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