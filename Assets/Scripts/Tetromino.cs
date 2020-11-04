using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;

    bool enable = true; //是否可以移动

    // public GameObject Parent = GameObject.Find("MyImage Target/Grid");

    // Start is called before the first frame update
    void Start()
    {
        // this.transform.parent = Parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //还没有下落到底部
        if (enable) {
            CheckUserInput();
        }
        //gameover 掉落
        if(FindObjectOfType<Game>().isGameOver){
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        else{
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
            
    }
    void CheckUserInput() {
        if(Input.GetKeyDown(KeyCode.RightArrow) || FindObjectOfType<MenuSystem>().isright){
            transform.position += new Vector3(1,0,0);
            if(!CheckIsValidPosition()) {
                transform.position += new Vector3(-1,0,0);
            }
            else{
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem>().isright = false;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || FindObjectOfType<MenuSystem>().isleft) {
            transform.position += new Vector3(-1,0,0);
            if(!CheckIsValidPosition()){
                transform.position += new Vector3(+1,0,0);
            }
            else{
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem>().isleft = false;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Time.time-fall>=fallSpeed ||FindObjectOfType<MenuSystem>().isdown){
            FindObjectOfType<MenuSystem>().isdown = false;

            transform.position += new Vector3(0,-1,0);
            fall = Time.time;
            if(!CheckIsValidPosition()){
                transform.position += new Vector3(0, 1, 0);
                //消行判断
                FindObjectOfType<Game>().CheckandDeleteRow();

                enable = false; //已经到底部 无法移动

                //判断是否gameover
                if(FindObjectOfType<Game>().CheckIsAboveGrid(this))
                    FindObjectOfType<Game>().GameOver();
                else
                    FindObjectOfType<Game>().SpawnNextTetromino(); //产生新的方块

                //加分
                FindObjectOfType<Game>().score += 1;

            }
            else{
                FindObjectOfType<Game>().UpdateGrid(this);
            }
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || FindObjectOfType<MenuSystem>().isroll){
            transform.Rotate(0,0,90);
            if(!CheckIsValidPosition()){
                transform.Rotate(0,0,-90);
            }
            else{
                FindObjectOfType<Game>().UpdateGrid(this);
            }

            FindObjectOfType<MenuSystem>().isroll = false;
        }
    }

    bool CheckIsValidPosition(){
        foreach( Transform mino in transform){
            Vector3 pos = FindObjectOfType<Game>().Round(mino.position);

            //边界判断
            if(FindObjectOfType<Game>().CheckIsInsideGrid(pos)==false) {
                return false;
            }

            //是否有其他方块判断
            if(FindObjectOfType<Game>().GetTransformAtGridPosition(pos)!=null && 
            FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent!=transform){
                return false;
            }
        }
        return true;
    }
}
