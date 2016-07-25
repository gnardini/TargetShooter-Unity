using UnityEngine;
using System.Collections;

// This script is used on the box that's behind the play button.
public class PlayBox : MonoBehaviour {

    private GameObject _uiContainer;
    private GameManager _gameManager;

	// Use this for initialization
	void Start () {
        _uiContainer = GameObject.Find("UIContainer");
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gameManager.setUiContainer(_uiContainer);
	}
	
    // When the box is hit by a bullet, start the game.
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Bullet") {
            Destroy(col.gameObject);
            _gameManager.startGame();
            _uiContainer.SetActive(false);
        }
    }

}
