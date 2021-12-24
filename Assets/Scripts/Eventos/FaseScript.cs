using UnityEngine;
using System.Collections;

public class FaseScript : MonoBehaviour 
{
    int contador = 0;
    public GameObject[] inimigos;    
    public static int num_inimigos;

	// Use this for initialization
	void Start () 
    {
        num_inimigos = ajusta_numero_de_inimigos(Application.levelCount);              
        //num_inimigos = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (contador == 5)
        {
            instancia_inimigos();
        }
        contador++;
	}

    int ajusta_numero_de_inimigos(int numero_da_fase)
    {
        switch (numero_da_fase)
        {
            case 1:
                return 4;                
            default:
                break;
        }
        return 0;
    }

    void instancia_inimigos()
    {
        MapeamentoTerreno [] posicoes_vazias = new MapeamentoTerreno[FundoScript.coordenadas_script.Length];
        int[] posicao_recem_preenchida;
        int indice = 0;
        int posicao_aleatoria;
        Vector3 posicao_escolhida;

        for(int j = 0; j < 11; j++)
        {
            for(int i = 0; i < 13; i++)
            {
               if(FundoScript.coordenadas_script[i,j].isEmpty)
               {
                   posicoes_vazias[indice] = FundoScript.coordenadas_script[i,j]; // Vai deixar somente as coordenadas vazias nesse vetor.
                   indice++;
               }
            }
        }

        posicao_recem_preenchida = new int[indice];        
        for (int i = 0; i < num_inimigos; i++)
        {
            posicao_aleatoria = Random.Range(0, indice);
            while (numero_ja_foi_usado(posicao_aleatoria, ref posicao_recem_preenchida))
            {
                posicao_aleatoria = Random.Range(0, indice);                    
            }
            posicao_escolhida = posicoes_vazias[posicao_aleatoria].transform.position;
            posicao_escolhida.z = 7;           
            Instantiate(inimigos[Random.Range(0, inimigos.Length)], posicao_escolhida, Quaternion.AngleAxis(180, new Vector3(0,1,0)));
            posicao_recem_preenchida[i] = posicao_aleatoria;                        
        }
    }

    bool numero_ja_foi_usado(int numero, ref int[] vetor)
    {
        for (int i = 0; i < vetor.Length; i++)
        {
            if (numero == vetor[i])
            {
                return true;
            }
        }
        return false;
    }
}