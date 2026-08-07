using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemAnimator : MonoBehaviour
{
    public Sprite[] mySprites;      
    public float frameRate = 0.08f; 

    private SpriteRenderer mySpriteRenderer;
    private int index = 0;

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(AnimateCoroutine());
    }

    IEnumerator AnimateCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(frameRate);

            if (mySprites != null && mySprites.Length > 0)
            {
                mySpriteRenderer.sprite = mySprites[index];
                index++;

                if (index >= mySprites.Length)
                {
                    index = 0;
                }
            }
        }
    }
}
