using UnityEngine;
using System.Collections;

public class PlayerUIControl : MonoBehaviour {
    public GameObject blood_prefab;
    private GameObject blood_instance;
    private Vector3 blood_WorldPostion;
    // Use this for initialization
    void Start() {
        blood_WorldPostion = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        blood_instance = GameObject.Instantiate(blood_prefab, blood_WorldPostion, Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void Update() {
        if (blood_instance)
        {
            blood_WorldPostion = new Vector3(transform.position.x, transform.position.y , transform.position.z);
            blood_instance.transform.position = WorldToUI(blood_WorldPostion);
        }
       
    }
    public Vector3 WorldToUI(Vector3 _postion)
    {
        float distance = Vector3.Distance(blood_WorldPostion, Camera.main.transform.position);
        Vector3 temp = Camera.main.ViewportToScreenPoint(new Vector3(_postion.x, _postion.y,_postion.z));
        temp.z = 0;
        temp = NGUITools.FindCameraForLayer(blood_instance.layer).WorldToScreenPoint(temp);
        return temp;
    }
    
}
