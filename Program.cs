// See https://aka.ms/new-console-template for more information
Console.WriteLine("Game starts");
Console.WriteLine("Please enter the number of Players");
int noOfPlayer =Convert.ToInt16( Console.ReadLine());
List<string> playerList = new List<string>();
Dictionary<string,List<double>> playerScoreList = new Dictionary<string,List<double>>();
List<double> roundList=new List<double>();
for (int round = 0; round < 10; round++)
{
    roundList.Add(round+1);
}
playerScoreList.Add("No. of Round", roundList);
    
while (Convert.ToInt16(noOfPlayer) > 0)
{
    StorePlayerName(noOfPlayer, playerList);
    noOfPlayer--;
}

double totalScore = 0;
string ?previousScore = null;
double score1 = 0;
double score2 = 0;
double score3 = 0;
for (int round = 1; round < 11; round++)
{
    Console.WriteLine($"Round {round}");
    foreach (string playerName in playerList)
    {
        Console.WriteLine($"{playerName},please enter your score");
        for (int turn = 1; turn < 4; turn++)
        {
            previousScore = null;
            Console.WriteLine($"Turn {turn}:");

            totalScore = CalculateTotalScore(ref score1, ref score2, ref score3, turn,round);
            AddingBonus(ref totalScore, ref previousScore, score1, score2, score3, turn,round);

            //Creating new score list for new player
            if (round == 1 && turn == 1)
            {
                playerScoreList.Add(playerName, new List<double> { totalScore });
            }

            //Add 3 turns for round 10
            if (round == 10)
            {
                switch (turn)
                {
                    case 2:
                        playerScoreList[playerName].RemoveAt(playerScoreList[playerName].Count() - 1);
                        playerScoreList[playerName].Add(totalScore);
                        break;
                    case 1:
                        playerScoreList[playerName].Add(totalScore);
                        break;
                    default:
                        playerScoreList[playerName].RemoveAt(playerScoreList[playerName].Count() - 1);
                        playerScoreList[playerName].Add(totalScore);
                        score1 = 0;
                        score2 = 0;
                        score3 = 0;
                        break;
                }
            }
            //Add 2 turns for round 1-9
            else if(round!=1 &&round!=10)
            {
                switch (turn)
                {
                    case 1:
                        playerScoreList[playerName].Add(totalScore);
                        break;
                    default:
                        playerScoreList[playerName].RemoveAt(playerScoreList[playerName].Count() - 1);
                        playerScoreList[playerName].Add(totalScore);
                        score1 = 0;
                        score2 = 0;
                        break;
                }
            }

            if (previousScore == "X" && round!=10)
            {
                break;
            }
        }
    }
    //Populating the score for each round
    var DisplayScore = playerScoreList.Select(x => new { x.Key, x.Value }).ToList();
    foreach (var Result in DisplayScore)
    {
        Console.WriteLine($"{Result.Key} : {string.Join(",",Result.Value)}");
    }
    if (round==10)
    {
        Console.WriteLine("The total score for each player is");
        var TotalScore = playerScoreList.Select(x => new { x.Key, sum = x.Value.Sum() });
        foreach (var Result in TotalScore.Skip(1))
        {
            Console.WriteLine($"{Result.Key} : {Result.sum}");
        }
    }
}


static void StorePlayerName(int noOfPlayer, List<string> playerList)
{
    Console.WriteLine($"PLayer[{noOfPlayer}],Please enter your name");
    string playerName = Console.ReadLine()??"Player A";
    playerList.Add(playerName);
}

static double CalculateTotalScore(ref double score1, ref double score2, ref double score3, int turn,int round)
{
    double totalScore;
    switch (turn)
    {
        case 1:
            score1 = Convert.ToDouble(Console.ReadLine());
            break;

        case 2:
            score2 = Convert.ToDouble(Console.ReadLine());
            break;

        case 3:
            if (round== 10)
            {
                score3 = Convert.ToDouble(Console.ReadLine());
            }
            break;
    }

    totalScore = score1 + score2 + score3;
    return totalScore;
}

static void AddingBonus(ref double totalScore, ref string? previousScore, double score1, double score2, double score3, int turn,int round)
{
    if (score1 == 10 && turn == 1)
    {
        totalScore = 10;
        previousScore = "X";
    }
    if (score2 == 10 && turn == 2 && round==10)
    {
        score2 = 10;
        previousScore = "X";
    }
    if (score3 == 10 && turn == 3 && round == 10)
    {
        score3 = 10;
        previousScore = "X";
    }
    if (totalScore == 10 && turn != 1)
    {
        previousScore = "/";
    }
    // 
    switch (previousScore)
    {
        case "X":
            totalScore = totalScore * 1.2;
            break;

        case "/":
            totalScore = totalScore * 1.1;
            break;

        default:
            break;
    }
}
