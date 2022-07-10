using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleImageChanger : MonoBehaviour
{
    public Texture[] images;
        public float changeTime;
    public RawImage myImage;
    public bool randomizeImages;
    public GameObject picture;

    // Start is called before the first frame update
    void Start()
    {
      
        myImage = gameObject.GetComponent<RawImage>();
     StartCoroutine("CylceImage");
    } 

   
    public IEnumerator CylceImage()
    {
        while (true)
        {
            for (int i = 0; i < images.Length; i++) {
                myImage.texture = images[i];
                myImage.color = new Color(Random.Range (0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0.5f, 1f));
                picture.GetComponent<Transform>().Rotate(new Vector3(0, 0, 1), 50);
                
                if (randomizeImages) { i = Random.Range(0, images.Length); }

                yield return new WaitForSeconds(changeTime); }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.transform.localScale =new Vector3(10,100,1);
            UIManager.dialogTextTyper.TypeMessage("CONNECTION ATTEMPT FAILED... REQUIRE TRANSFER CODE>>>", false, null, 2);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.transform.localScale =new Vector3(0.2f,0.2f,1);
        }
    }



}
