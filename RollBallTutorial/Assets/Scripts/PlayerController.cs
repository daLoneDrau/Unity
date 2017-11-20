using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
    /// <summary>
    /// the number of pieces picked up
    /// </summary>
    private int Count;
    public Text countText;
    public Text winText;
    /// <summary>
    /// the amount of thrust provided by a move.
    /// </summary>
    public float thrust;
    /// <summary>
    /// the player ball's body.
    /// </summary>
    public Rigidbody rb;
    /// <summary>
    /// FixedUpdate is called before performing any physics calculations.
    /// </summary>
    void FixedUpdate()
    {
        // get horizontal/vertical movement from keyboard input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // apply force to the ball
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.AddForce(movement * thrust);
    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// 
    /// This message is sent to the trigger Collider and the Rigidbody(if any) that the trigger Collider
    /// belongs to, and to the Rigidbody(or the Collider if there is no Rigidbody) that touches the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        // kills the game object - not used
        // Destroy(other.gameObject);
        Debug.Log("boom");
        // efficient string comparison method - CompareTag
        if (other.gameObject.CompareTag("PickUp"))
        {
            Debug.Log("hit pickup");
            // de-activate the object, turning off all renderers, colliders, rigidbodies, and scripts
            other.gameObject.SetActive(false);
            Count++;
            UpdateScore();
        }
    }
    /// <summary>
    /// Called on the first frame the script is active.  Only called once.
    /// </summary>
    void Start()
    {
        // get the rigid body at start
        rb = this.GetComponent<Rigidbody>();
        Count = 0;
        UpdateScore();
    }
    // Update is called once per frame
    void Update()
    {

    }
    void UpdateScore()
    {
        this.countText.text = string.Format("Score: {0}", (Count * 10));
        if (Count < 12)
        {
            winText.text = "";
        } else
        {
            winText.text = "You WIN!!";
        }
    }
}
