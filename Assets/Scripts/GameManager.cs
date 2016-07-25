using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static string KEY_HIGHSCORE = "stored_highscore";
    private static float MAX_TIME_MULTIPLIER = 0.99f;

    // Prefabs used to create game elements.
    public Rigidbody bullet;
    public Rigidbody target;

    // Texts that are shown on the screen.
    public TextMesh livesText;
    public TextMesh pointsText;
    public TextMesh highscoreText;

    // Container with play highscore, etc elements.
    private GameObject _uiContainer;

    // Weather a game is being played or not.
    private bool _inGame;

    // How many frames have passed since the last fired bullet. So that many bullets aren't fired on each press.
    // TODO(gnardini): Find a better way to do this.
    private int _framesSinceLastBullet;

    // Game vars.
    private int _livesLeft;
    private int _points;

    // Vars to determine when hte next target will be generated.
    private float _timeBetweenTargets;
    private float _timeUntilNextTarget;
    private float _timeDifferenceMultiplier;

	// Use this for initialization
	void Start () {
        updateHighscore(PlayerPrefs.GetInt(KEY_HIGHSCORE));
        _inGame = false;
	}
	
	// Update is called once per frame
	void Update () {
        _framesSinceLastBullet++;

        // If there's a click, fire a bullet.
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Ended) {
                fireBullet();
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            fireBullet();
        }

        // If playing, generate a target more often as the game progresses.
        if (_inGame) {
            _timeUntilNextTarget -= Time.deltaTime;
            if (_timeUntilNextTarget <= 0) {
                createTarget();
                _timeBetweenTargets *= _timeDifferenceMultiplier;
                _timeDifferenceMultiplier += 0.005f;
                if (_timeDifferenceMultiplier > MAX_TIME_MULTIPLIER) {
                    _timeDifferenceMultiplier = MAX_TIME_MULTIPLIER;
                }
                _timeUntilNextTarget = _timeBetweenTargets;
            }
        }
	}

    // Sets the conditions necessary to start playing.
    public void startGame() {
        restartGame();
        livesText.gameObject.SetActive(_inGame);
        pointsText.gameObject.SetActive(_inGame);
    }

    // Called when a target touches the ground.
    public void onTargetLost() {
        _livesLeft--;   
        updateUi();
        if (_livesLeft <= 0) {
            gameOver();
        }
    }

    // Called when a bullet hits a target.
    public void onTargetHit() {
        _points++;
        updateUi();
    }

    // Fire a bullet in the direction of the camera.
    private void fireBullet() {
        if (_framesSinceLastBullet < 5) {
            return;
        }
        _framesSinceLastBullet = 0;
        Rigidbody bulletObject = (Rigidbody) Instantiate(
            bullet, 
            transform.position + transform.forward * 4,
            transform.rotation);
        bulletObject.velocity = transform.forward * 200;
        bulletObject.gameObject.name = "Bullet";
    }

    // Create a target at a random position and shoot it upwards.
    private void createTarget() {
        float xPosition = (float) ((Random.value - 0.5) * 50); 
        Vector3 targetPosition = new Vector3(xPosition, 3, 20);
        Rigidbody targetObject = (Rigidbody) Instantiate(target, targetPosition, transform.rotation);
        targetObject.GetComponent<Target>().setGameManager(this);
        targetObject.velocity = transform.up * 20;
    }

    // Set the start conditions of the game.
    private void restartGame() {
        _framesSinceLastBullet = 0;
        _points = 0;
        _livesLeft = 5;
        _inGame = true;
        _timeBetweenTargets = 4;
        _timeUntilNextTarget = _timeBetweenTargets;  
        _timeDifferenceMultiplier = 0.9f;
        updateUi();
    }

    // Updates game-related vars on the UI.
    private void updateUi() {
        livesText.text = _livesLeft + " lives";
        pointsText.text = _points + " points";
    }

    // Sets the UI container that holds the play button, highscore, etc. 
    public void setUiContainer(GameObject uiContainer) {
        _uiContainer = uiContainer;
    }

    // When the game ends, update highscore, remove all residual objects and show the menu UI.
    private void gameOver() {
        _inGame = false;
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Bullet")) {
            Destroy(gameObject);
        }
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Target")) {
            Destroy(gameObject);
        }
        updateHighscore(_points);
        _uiContainer.SetActive(true);
        livesText.gameObject.SetActive(false);
        pointsText.gameObject.SetActive(false);
    }

    // Updates the highscore on the screen and saves it in |PlayerPrefs|.
    private void updateHighscore(int score) {
        int oldHighscore = PlayerPrefs.GetInt(KEY_HIGHSCORE);
        if (score > oldHighscore) {
            PlayerPrefs.SetInt(KEY_HIGHSCORE, score);
        }
        highscoreText.text = "Highscore: " + score;
    }

}
