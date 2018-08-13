using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Rpg_2d_Unit_Controller>().MaxHealth > other.gameObject.GetComponent<Rpg_2d_Unit_Controller>().Health)
            {
                other.gameObject.GetComponent<Rpg_2d_Unit_Controller>().Health += 1;
                UI.instance.UpdateLifeGUI(other.gameObject.GetComponent<Rpg_2d_Unit_Controller>().Health);
                Debug.Log("life + 1 : " + other.gameObject.GetComponent<Rpg_2d_Unit_Controller>().Health);
            }

            Destroy(this.gameObject);
        }
    }
}