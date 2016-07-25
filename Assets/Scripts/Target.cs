using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

    private GameManager _gameManager;
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < 2.8f) {
            _gameManager.onTargetLost();
            Destroy(gameObject);
        }
	}

    // When a bullet hits this target, remove both of them from the scene and
    // notify the game manager.
    void OnCollisionEnter (Collision col) {
        if (col.gameObject.name == "Bullet") {
            Destroy(col.gameObject);
            Destroy(gameObject);
            _gameManager.onTargetHit();
        }
    }

    public void setGameManager(GameManager shooter) {
        _gameManager = shooter;
    }

}
