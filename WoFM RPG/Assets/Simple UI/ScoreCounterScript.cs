using UnityEngine;
using UnityEngine.UI; // for Text class

public class ScoreCounterScript : MonoBehaviour
{
    /// <summary>
    /// The Text instance.
    /// </summary>
    private Text uiText;
    /// <summary>
    /// The score.
    /// </summary>
    private long score = 0;
    /// <summary>
    /// Public method to add points to the counter.
    /// </summary>
    /// <param name="points">the number of points being added</param>
    public void AddPoints(int points)
    {
        score += points;
        updateScoreCounter();
    }
    public long GetScore()
    {
        return score;
    }
    /// <summary>
    /// Used for initialization.
    /// </summary>
    void Start()
    {
        uiText = this.GetComponent<Text>(); // get the parent Text component
        updateScoreCounter();
    }
    /// <summary>
    /// Public method to subtract points from the counter.
    /// </summary>
    /// <param name="points">the number of points being subtracted</param>
    public void SubtractPoints(int points)
    {
        score -= points;
        if (score < 0)
        {
            score = 0;
        }
        updateScoreCounter();
    }
    /// <summary>
    /// Updates the score.
    /// </summary>
    private void updateScoreCounter()
    {
        // you can also use <b> for bold
        uiText.text = string.Concat("<color=blue>Score</color>: ", score);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
