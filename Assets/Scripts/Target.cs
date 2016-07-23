using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < 2.8f) {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter (Collision col) {
        if(col.gameObject.name == "Bullet") {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
