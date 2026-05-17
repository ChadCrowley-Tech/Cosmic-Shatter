[System.Serializable]
public class LeaderboardData
{
    // Holds the lists for scores and names
    public int[] scores = new int[10];
    public string[] names = new string[10];

    // Fills the lists with default empty values
    public LeaderboardData()
    {
        for (int i = 0; i < 10; i++)
        {
            scores[i] = 0;
            names[i] = "---";
        }
    }
}
