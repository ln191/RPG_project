using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance = null;

    [SerializeField]
    ///
    // empty heart sprite must be in place zero in list and the full hearth must be the last in the list
    ///
    private List<Sprite> hearthStates = new List<Sprite>();

    [SerializeField]
    private List<GameObject> hearths = new List<GameObject>();

    private void Awake()
    {
        //singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void UpdateLifeGUI(int currentHealth)
    {
        int hearthToChange = currentHealth / (hearthStates.Count - 1);
        int remainingLifeInHearth = currentHealth % (hearthStates.Count - 1);

        for (int hp = 0; hp < hearths.Count; hp++)
        {
            if (hp < hearthToChange)
            {
                //Full Hearths
                hearths[hp].GetComponent<Image>().sprite = hearthStates[hearthStates.Count - 1];
            }
            else if (hp == hearthToChange)
            {
                //change sprite to fit the remaining life in hearth
                hearths[hearthToChange].GetComponent<Image>().sprite = hearthStates[remainingLifeInHearth];
            }
            else
            {
                //empty hearths
                hearths[hp].GetComponent<Image>().sprite = hearthStates[0];
            }
        }
    }
}