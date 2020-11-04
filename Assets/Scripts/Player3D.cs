using UnityEngine;
using System.Collections;
public class Player3D: MonoBehaviour{
    public Transform m_transform;
    //角色控制器组件
    CharacterController m_ch;
    //角色移动速度
    float m_movSpeed=3.0f;
    //重力
    float m_gravity=2.0f;

    //摄像机Transform
    Transform m_camTransform;
    //摄像机旋转角度
    Vector3 m_camRot;
    //摄像机高度
    float m_camHeight=1.4f;


    void Start(){
        m_transform=this.transform;
        //获取角色控制器组件
        m_ch=this.GetComponent<CharacterController>();
        //获取摄像机
        m_camTransform=Camera.main.transform;
        Vector3 pos=m_transform.position;
        pos.y+=m_camHeight;
        m_camTransform.position=pos;
        //设置摄像机的旋转方向与主角一致
        m_camTransform.rotation=m_transform.rotation;
        m_camRot=m_camTransform.eulerAngles;
        //锁定鼠标
        Screen.lockCursor=true;
    }
    void Update(){
        Control();
    }
    void Control(){
        //定义3个值控制移动
        float xm=0, ym=0, zm=0;
        //重力运动
        ym-=m_gravity*Time.deltaTime;
        //前后左右移动
        if(Input.GetKey(KeyCode.W)){
            zm+=m_movSpeed*Time.deltaTime;
        }else if(Input.GetKey(KeyCode.S)){
            zm-=m_movSpeed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A)){
            xm-=m_movSpeed*Time.deltaTime;
        }else if(Input.GetKey(KeyCode.D)){
            xm+=m_movSpeed*Time.deltaTime;
        }
        //使用角色控制器提供的Move函数进行移动
        m_ch.Move(m_transform.TransformDirection(new Vector3(xm, ym, zm)));

        //获取鼠标移动距离
        float rh=Input.GetAxis("Mouse X");
        float rv=Input.GetAxis("Mouse Y");
        //旋转摄像机
        m_camRot.x-=rv;
        m_camRot.y+=rh;
        m_camTransform.eulerAngles=m_camRot;
        //使角色的面向方向与摄像机一致
        Vector3 camrot=m_camTransform.eulerAngles;
        camrot.x=0;camrot.z=0;
        m_transform.eulerAngles=camrot;
        //操作角色移动代码
        //使摄像机位置与角色一致
        Vector3 pos=m_transform.position;
        pos.y+=m_camHeight;
        m_camTransform.position=pos;
    }
}

