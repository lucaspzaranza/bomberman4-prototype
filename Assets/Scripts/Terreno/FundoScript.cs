using UnityEngine;
using System.Collections;

public class FundoScript : MonoBehaviour 
{
    public GameObject grama;
    public GameObject bloco_de_aco;
    public GameObject bloco_borda;
    public GameObject bloco_destrutivel;
    public GameObject mapeamento;
    public int x, y, i, j;
    float coord_x = -7.0f;
    float coord_y = 5.0f;
    GameObject[] gramado;
    GameObject[] blocos_de_aco;
    GameObject[] bordas;
    GameObject[] coordenadas;
    GameObject[,] blocos_destrutiveis = new GameObject[13, 11];
    GameObject[] itens;
    public static GameObject[,] coordenada = new GameObject[13, 11];
    public static MapeamentoTerreno[,] coordenadas_script = new MapeamentoTerreno[13,11];
    BlocoTerrenoScript bloco_script;
    int contador_frame = 0;
    int instancia_bloco_ou_nao = 0;

	// Use this for initialization
	void Start()
    {        
        instancia_grama();
        instancia_blocos_de_aco();
        instancia_blocos_borda();
        mapeia_terreno();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (contador_frame == 2)
        {
            instancia_blocos_destrutiveis();
            esconde_itens();
        }

        //if (contador_frame == 10)
        //{
        //    destroi_blocos_vazios();
        //}

        contador_frame++;                            
	}

    void destroi_blocos_vazios()
    {
        BlocoTerrenoScript [,] blocos_script = new BlocoTerrenoScript[13,11];

        for (int j = 0; j < 11; j++)
        {
            for (int i = 0; i < 13; i++)
            {
                if (blocos_destrutiveis[i, j] != null)
                {
                    blocos_script[i, j] = blocos_destrutiveis[i, j].GetComponent<BlocoTerrenoScript>();
                    if (!blocos_script[i, j].tem_item_no_bloco)
                    {
                        Destroy(blocos_destrutiveis[i, j]);
                    }
                }                
            }
        }
    }
    
    void mapeia_terreno()
    {
        int a = -6;
        int b = 5;

        for (j = 0; j < 11; j++)
        {
            for (i = 0; i < 13; i++)
            {
                coordenada[i, j] = Instantiate(mapeamento, transform.position , Quaternion.identity) as GameObject;
                coordenada[i, j].transform.position = new Vector3(a, b, 8);
                coordenadas_script[i, j] = coordenada[i, j].GetComponent<MapeamentoTerreno>();                
                a++;
            }
            b--;
            a = -6;
        }

        coordenadas = GameObject.FindGameObjectsWithTag("mapeamento");

        for (i = 1; i < coordenadas.Length; i++)
        {
            coordenadas[i].transform.parent = coordenadas[0].transform;
        }        
    }

    void instancia_blocos_destrutiveis()
    {
        int a = -6;
        int b = 5;
        for (j = 0; j < 11; j++)
        {
            for (i = 0; i < 13; i++)
            {
                if (coordenadas_script[i, j].isEmpty == true)
                {
                    instancia_bloco_ou_nao = Random.Range(-1, 16);   // y padrão = 16                                     
                    //instancia_bloco_ou_nao = 1;
                    if (instancia_bloco_ou_nao >= 1)
                    {                        
                        blocos_destrutiveis[i, j] = Instantiate(bloco_destrutivel, new Vector3(a, b, 7.5f), Quaternion.identity) as GameObject;
                    }
                }
                else
                {                    
                    if (coordenadas_script[i, j].playerNoBloco == true)
                    {
                        if (i == 0 && j == 0) //esquerda superior
                        {
                            coordenadas_script[1, 0].isEmpty = false; // direita
                            coordenadas_script[0, 1].isEmpty = false; // baixo                            
                        }
                        else if (i == 0 && j == 10) // esquerda inferior 
                        {                                                        
                            coordenadas_script[1, 10].isEmpty = false; // direita   
         
                            if (blocos_destrutiveis[0, 9] != null)
                            {
                                Destroy(blocos_destrutiveis[0, 9].gameObject); // cima
                            }                            
                        }
                        else if (i == 12 && j == 0) // direita superior
                        {
                            if (blocos_destrutiveis[11, 0] != null)
                            {
                                Destroy(blocos_destrutiveis[11, 0].gameObject); // esquerda
                            }

                            coordenadas_script[12, 1].isEmpty = false; // baixo
                        }
                        else if (i == 12 && j == 10) // direita inferior
                        {
                            if (blocos_destrutiveis[11, 10] != null)
                            {
                                Destroy(blocos_destrutiveis[11, 10].gameObject); // esquerda
                            }

                            if(blocos_destrutiveis[12, 9] != null)
                            {
                                Destroy(blocos_destrutiveis[12, 9].gameObject); // cima
                            }                            
                        }
                        else if (i == 6 && j == 5) // meio
                        {
                            if(blocos_destrutiveis[5, 4] != null)
                            {
                                Destroy(blocos_destrutiveis[5, 4].gameObject);
                            }

                            if (blocos_destrutiveis[6, 4] != null)
                            {
                                Destroy(blocos_destrutiveis[6, 4].gameObject); // blocos da linha acima
                            }

                            if (blocos_destrutiveis[7, 4] != null)
                            {
                                Destroy(blocos_destrutiveis[7, 4].gameObject);
                            }                            

                            coordenadas_script[5, 6].isEmpty = false;
                            coordenadas_script[6, 6].isEmpty = false; // blocos da linha abaixo
                            coordenadas_script[7, 6].isEmpty = false;
                        }
                    }
                }
                a++;
            }
            b--;
            a = -6;
        }
    }

    void instancia_grama()
    {
        for (y = 0; y <= 12; y++)
        {
            for (x = 0; x <= 14 ; x++)
            {
                Instantiate(grama, new Vector3(coord_x, coord_y, 8), Quaternion.identity);
                coord_x += 1.0f;
            }
            coord_x = -7.0f;
            coord_y -= 1.0f;
        }

        gramado = GameObject.FindGameObjectsWithTag("grass");
        for (int i = 1; i < gramado.Length; i++)
        {
            gramado[i].transform.parent = gramado[0].transform;
        }
    }

    void instancia_blocos_de_aco()
    {
        for (y = 4; y >= -4; y -= 2)
        {
            for (x = -5; x <= 5; x += 2)
            {
                Instantiate(bloco_de_aco, new Vector3(x, y, 8), Quaternion.identity);                
            }
        }
        
        blocos_de_aco = GameObject.FindGameObjectsWithTag("aco");
        for(int i = 1; i < blocos_de_aco.Length; i++)
        {
            blocos_de_aco[i].transform.parent = blocos_de_aco[0].transform;
        }
    }

    void instancia_blocos_borda()
    {        
        for (y = 7; y >= -7; y--)
        {
            if (y >= 6 || y <= -6)
            {
                for (x = -8; x <= 8; x++)
                {
                    Instantiate(bloco_borda, new Vector3(x, y, 8), Quaternion.identity);                    
                }
            }
            else
            {
                Instantiate(bloco_borda, new Vector3(-8, y, 8), Quaternion.identity);
                Instantiate(bloco_borda, new Vector3(-7, y, 8), Quaternion.identity);
                Instantiate(bloco_borda, new Vector3(7, y, 8), Quaternion.identity);
                Instantiate(bloco_borda, new Vector3(8, y, 8), Quaternion.identity);
            }              
        }

        bordas = GameObject.FindGameObjectsWithTag("concreto");
        for (i = 1; i < bordas.Length; i++)
        {
            bordas[i].transform.parent = bordas[0].transform;
        }
    }

    void esconde_itens()
    {
        itens = GameObject.FindGameObjectsWithTag("item");
        for (int a = 0; a < itens.Length; a++)
        {            
            i = Random.Range(0, 13);
            j = Random.Range(0, 11);
            while (blocos_destrutiveis[i, j] == null) // Verifica primeiro se o bloco existe
            {
                i = Random.Range(0, 13);
                j = Random.Range(0, 11);               
            }
            bloco_script = blocos_destrutiveis[i, j].GetComponent<BlocoTerrenoScript>();       
            while (blocos_destrutiveis[i, j] == null || bloco_script.tem_item_no_bloco)
            {
                i = Random.Range(0, 13);
                j = Random.Range(0, 11);                
                if(blocos_destrutiveis[i,j] != null)
                {
                    bloco_script = blocos_destrutiveis[i, j].GetComponent<BlocoTerrenoScript>();
                }
            }
            bloco_script = blocos_destrutiveis[i, j].GetComponent<BlocoTerrenoScript>();
            bloco_script.tem_item_no_bloco = true;  
            bloco_script.item = itens[a];            
            itens[a].transform.position = new Vector3(blocos_destrutiveis[i, j].transform.position.x, blocos_destrutiveis[i, j].transform.position.y, 9);            
        }
    }
}