using UnityEngine;

public class Enemy_Hurt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool hurt = false;
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player_Attack"){
            hurt=true;
        }
    }
}
