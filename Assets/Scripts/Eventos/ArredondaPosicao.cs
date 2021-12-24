using UnityEngine;
using System.Collections;

public class ArredondaPosicao : MonoBehaviour 
{

    public static float pos_x, pos_y;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

   public static float arredonda_x(float coordenada)
    {
        float parte_decimal;

        if (pos_x < 0.0f)
        {
            parte_decimal = (int)coordenada - pos_x;
        }
        else
        {
            parte_decimal = pos_x - (int)coordenada;
        }

        if (parte_decimal < 0.5f)
        {
            return (int)coordenada;
        }
        else
        {
            if (pos_x < 0.0f)
            {
                return (int)coordenada - 1.0f;
            }
            else
            {
                return (int)coordenada + 1.0f;
            }
        }
      
    }

   public static float arredonda_y(float coordenada)
    {
        float parte_decimal;

        if (pos_y < 0.0f)
        {
            parte_decimal = (int)coordenada - pos_y;
        }
        else
        {
            parte_decimal = pos_y - (int)coordenada;
        }

        if (parte_decimal < 0.5f)
        {
            return (int)coordenada;
        }
        else
        {
            if (pos_y < 0.0f)
            {
                return (int)coordenada - 1.0f;
            }
            else
            {
                return (int)coordenada + 1.0f;
            }

        }
    }
}
