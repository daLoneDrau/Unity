using UnityEngine;

public class SymbolicLivesCounterScript : MonoBehaviour
{
    /// <summary>
    /// The list of heart objects. This array will be filled in the Unity editor.
    /// </summary>
    [SerializeField]
    private GameObject[] hearts;
    /// <summary>
    /// the number of lives.
    /// </summary>
    private int lives;
    /// <summary>
    /// Tries to add a life.
    /// </summary>
    /// <returns><tt>true</tt> if a life could be added; <tt>false</tt> otherwise</returns>
    public bool AddLife()
    {
        bool added = false;
        if (lives < hearts.Length)
        {
            lives++;
            UpdateSymbolicLivesCounter();
            added = true;
        }
        return added;
    }
    /// <summary>
    /// Causes the player to lose a life.
    /// </summary>
    /// <returns><tt>true</tt> if the player has no more lives left; <tt>false</tt> otherwise</returns>
    public bool LoseLife()
    {
        bool isDead = true;
        lives--;
        if (lives > 0)
        {
            isDead = false;
        }
        else
        {
            lives = 0;
        }
        UpdateSymbolicLivesCounter();
        return isDead;
    }
    // Use this for initialization
    void Start()
    {
        lives = hearts.Length;
    }
    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Updates the life symbols on screen, deactivating some when the # of lives goes down.
    /// </summary>
    private void UpdateSymbolicLivesCounter()
    {
        for (int i = hearts.Length - 1; i >= 0; i--)
        {
            if (i < lives)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }
}
