using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino3D : MonoBehaviour
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
        if(FindObjectOfType<Game3D>().isGameOver){
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        else{
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
            
    }
    void CheckUserInput() {
        // WASD: 人物移动 不在这里控制
        // 方向键：方块在水平面上的平移
        // F1F2F3: 旋转
        // 空格: 下降

        // -------------------- 水平方向上的平移 -------------------------
        if(Input.GetKeyDown(KeyCode.RightArrow) || FindObjectOfType<MenuSystem3D>().isright){
            transform.position += new Vector3(1,0,0);
            if(!CheckIsValidPosition()) {
                transform.position += new Vector3(-1,0,0);
            }
            else{
                FindObjectOfType<Game3D>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem3D>().isright = false;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || FindObjectOfType<MenuSystem3D>().isleft) {
            transform.position += new Vector3(-1,0,0);
            if(!CheckIsValidPosition()){
                transform.position += new Vector3(1,0,0);
            }
            else{
                FindObjectOfType<Game3D>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem3D>().isleft = false;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || FindObjectOfType<MenuSystem3D>().isbackward){
            transform.position += new Vector3(0,0,-1);
            if(!CheckIsValidPosition()){
                transform.position += new Vector3(0, 0, 1);
            }
            else{
                FindObjectOfType<Game3D>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem3D>().isbackward = false;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || FindObjectOfType<MenuSystem3D>().isforward){
            transform.position += new Vector3(0,0,1);
            if(!CheckIsValidPosition()){
                transform.position += new Vector3(0,0,-1);
            }
            else{
                FindObjectOfType<Game3D>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem3D>().isforward = false;
        }

        // -------------- 旋转 ------------------
        else if(Input.GetKeyDown(KeyCode.F1) || FindObjectOfType<MenuSystem3D>().isroll) {
            transform.Rotate(0,0,90);
            if(!CheckIsValidPosition()){
                transform.Rotate(0,0,-90);
            }
            else{
                FindObjectOfType<Game3D>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem3D>().isroll = false;
        }

        else if(Input.GetKeyDown(KeyCode.F2) || FindObjectOfType<MenuSystem3D>().ispitch) {
            transform.Rotate(0,90,0);
            if(!CheckIsValidPosition()){
                transform.Rotate(0,-90,0);
            }
            else{
                FindObjectOfType<Game3D>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem3D>().ispitch = false;
        }

        else if(Input.GetKeyDown(KeyCode.F3) || FindObjectOfType<MenuSystem3D>().isyaw) {
            transform.Rotate(90,0,0);
            if(!CheckIsValidPosition()){
                transform.Rotate(-90,0,0);
            }
            else{
                FindObjectOfType<Game3D>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem3D>().isyaw = false;
        }

        // -------------- 下降 ------------------
        else if(Input.GetKeyDown(KeyCode.B) || Time.time-fall>=fallSpeed || FindObjectOfType<MenuSystem3D>().isdown){
            transform.position += new Vector3(0,-1,0);
            fall = Time.time;
            if(!CheckIsValidPosition()){
                transform.position += new Vector3(0, 1, 0);
                //消行判断
                FindObjectOfType<Game3D>().CheckandDeleteRow();

                enable = false; //已经到底部 无法移动

                //判断是否gameover
                if(FindObjectOfType<Game3D>().CheckIsAboveGrid(this))
                    FindObjectOfType<Game3D>().GameOver();
                else
                    FindObjectOfType<Game3D>().SpawnNextTetromino(); //产生新的方块

                //加分
                FindObjectOfType<Game3D>().score += 1;

            }
            else{
                FindObjectOfType<Game3D>().UpdateGrid(this);
            }
            FindObjectOfType<MenuSystem3D>().isdown = false;
        }
    }

    bool CheckIsValidPosition(){
        foreach( Transform mino in transform){
            Vector3 pos = FindObjectOfType<Game3D>().Round(mino.position);

            //边界判断
            if(FindObjectOfType<Game3D>().CheckIsInsideGrid(pos)==false) {
                return false;
            }

            //是否有其他方块判断
            if(FindObjectOfType<Game3D>().GetTransformAtGridPosition(pos)!=null && 
            FindObjectOfType<Game3D>().GetTransformAtGridPosition(pos).parent!=transform){
                return false;
            }
        }
        return true;
    }
}
