using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomImageSwap : MonoBehaviour
{

    public List<Sprite> BillboardSprites;
    private int randomInt;

	void Start ()
	{
        randomInt = Random.Range(0, BillboardSprites.Count);
	    transform.GetComponent<UnityEngine.UI.Image>().sprite = BillboardSprites[randomInt];
	}

}
