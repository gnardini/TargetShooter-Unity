using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	void Update () {
        // If the bullet goes off-screen, remove it.  
        // TODO(gnardini): Add more validations to remove the bullet.
        if (transform.position.z > 200) {
            Destroy(gameObject);
        }
	}
}
