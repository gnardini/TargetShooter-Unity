using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

    public Rigidbody bullet;
    public Rigidbody target;

    private int _framesSinceLastBullet;

	// Use this for initialization
	void Start () {
        _framesSinceLastBullet = 0;
	}
	
	// Update is called once per frame
	void Update () {
        _framesSinceLastBullet++;

        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Ended) {
                fireBullet();
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            fireBullet();
        }

        if (Random.value < 0.01) {
            createTarget();
        }
	}

    void fireBullet() {
        if (_framesSinceLastBullet < 5) {
            return;
        }
        _framesSinceLastBullet = 0;
        Rigidbody bulletObject = (Rigidbody) Instantiate(bullet, transform.position, transform.rotation);
        bulletObject.velocity = transform.forward * 200;
        bulletObject.gameObject.name = "Bullet";
    }

    void createTarget() {
        float xPosition = (float) ((Random.value - 0.5) * 50); 
        Vector3 targetPosition = new Vector3(xPosition, 3, 20);
        Rigidbody targetObject = (Rigidbody) Instantiate(target, targetPosition, transform.rotation);
        targetObject.velocity = transform.up * 20;
    }

}
