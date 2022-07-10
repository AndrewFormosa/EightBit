using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationScript : MonoBehaviour
{
 
    

   public Sprite[] restSprites;
    public Sprite[] upSprites;
    public Sprite[] downSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;
    public Sprite[] upRightSprites;
    public Sprite[] upLeftSprites;
    public Sprite[] downRightSprites;
    public Sprite[] downLeftSprites;

    public Sprite[] restSpritesRest;
    public Sprite[] upSpritesRest;
    public Sprite[] downSpritesRest;
    public Sprite[] leftSpritesRest;
    public Sprite[] rightSpritesRest;
    public Sprite[] upRightSpritesRest;
    public Sprite[] upLeftSpritesRest;
    public Sprite[] downRightSpritesRest;
    public Sprite[] downLeftSpritesRest;


    private Sprite[][] sprites;

    public float animationSpeed;

    private Vector3 directionVector;
    private bool isMoving;

    private ObjVar objVar;

    private int directionIndex;
    private int animationIndex;
    private int animationLength;
    private float animationDelayMultplier;
    private float frameDelay;
    private float nextFrameTime;

    private float inV;
    private float inH;
    private bool inMoving;

    private SpriteRenderer spriteRenderer;
    private Transform objectTransform;
    private Vector3 currentPosition;
    private Vector3 previousPosition;

    // Start is called before the first frame update
    void Start()
    {
      
        objVar = this.GetComponent<ObjVar>();

        //set up sprite array;
        sprites = new Sprite[18][];
        sprites[0] = downLeftSprites;
        sprites[1] = downSprites;
        sprites[2] = downRightSprites;
        sprites[3] = leftSprites;
        sprites[4] = restSprites;
        sprites[5] = rightSprites;
        sprites[6] = upLeftSprites;
        sprites[7] = upSprites;
        sprites[8] = upRightSprites;
        sprites[9] = downLeftSpritesRest;
        sprites[10] = downSpritesRest;
        sprites[11] = downRightSpritesRest;
        sprites[12] = leftSpritesRest;
        sprites[13] = restSpritesRest;
        sprites[14] = rightSpritesRest;
        sprites[15] = upLeftSpritesRest;
        sprites[16] = upSpritesRest;
        sprites[17] = upRightSpritesRest;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        inV = 1;
        inH = 1;
        inMoving=true;
        animationDelayMultplier = 10;
        frameDelay = animationDelayMultplier * Time.deltaTime / animationSpeed;

    }
    


    // Update is called once per frame
    void Update()
    {
        //check direction change
        directionVector =objVar.directionVector;
        isMoving = objVar.isMoving;
        float v = directionVector.y;
        float h =directionVector.x;
        if ((inV != v || inH != h || isMoving!=inMoving))
        {
            inV = v;
            inH = h;
            inMoving = isMoving;
            directionIndex = (int)((h + 1) + ((v + 1) * 3));
            if (!isMoving) directionIndex += 9;
           
            animationIndex = 0;
            animationLength = sprites[directionIndex].Length;
            //changeSprite();
        }
      
    
        //anmiamtePlayer
        if (Time.time > nextFrameTime && animationLength > 1)
        {
            animationIndex++;
            if (animationIndex == sprites[directionIndex].Length)
            {
                animationIndex = 0;
            }
            changeSprite();
        }
    }


    public void changeSprite()
    {
        spriteRenderer.sprite = sprites[directionIndex][animationIndex];
        nextFrameTime = Time.time + frameDelay;
    }

    

}
