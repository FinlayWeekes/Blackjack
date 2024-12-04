using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Text;



public class LogicJack
{

    public static int total = 0;
    public static bool bust = false;
    public static bool stand = false;
    public static int ace = 0;
    public static int ace1 = 0;
    public static int opponentTotal = 0;
    public static int opponentAce = 0;
    public static int opponentAce1 = 0;
    public static bool opponentStand = false;
    public static bool opponentBust = false;
    public static int score = 0;
    public static int opponentScore = 0;
    public static int dignity = 100;
    public static bool opponentForcedStand = false;
    public static bool opponentForcedStand2 = false;
    public static int opponentForcedHit = 0;
    public static int stake = 0;
    public static int bustLimit = 21;
    public static int money = 0;
    public static int removePrice = 10;
    public static int incPrice = 10;
    public static int buyPrice = 10;
    public static int boostPrice = 20;
    public static int round = 1;

    public static void Main(string[] args)
    {
        Random random = new Random();

        List<List<string>> deck = new List<List<string>> { new List<string>(), new List<string>() };
        List<string> hand = new List<string>();
        List<string> opponentHand = new List<string>();
        List<List<string>> opponentDeck = new List<List<string>> { new List<string>(), new List<string>() };
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 13; i++)
            {
                opponentDeck[j].Add(cards[j, i]);
            }
        }

        Console.WriteLine("Type s for stand and h for hit");
        Console.WriteLine("Shop appears every 3 games.");
        Console.WriteLine("You start with 100 dignity");
        Console.WriteLine("Gain 1 dignity for each round won");
        Console.WriteLine("Lose 2 dignity for each round lost");
        Console.WriteLine("Lose 5 dignity for each game lost");
        Console.WriteLine("Earn £5 for each game won");
        Console.WriteLine("Dignity lost and money scales with the stake of each game.");
        

        starterDeck(ref deck, ref cards);
        
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                stake++;
                
                Console.WriteLine();
                
                opponent(ref total, ref deck, ref bust, ref stand, ref ace, ref cards, ref hand, ref opponentHand, ref opponentTotal, ref opponentAce, ref opponentStand, ref opponentBust, ref opponentDeck, ref score, ref opponentScore, ref dignity, ref opponentForcedStand, ref opponentForcedHit, ref opponentForcedStand2, ref stake, ref bustLimit, ref money, ref ace1, ref opponentAce1, round);
                
                pickCard(ref total, ref deck, ref bust, ref stand, ref ace, ref cards, ref hand, ref opponentHand, ref opponentTotal, ref opponentAce, ref opponentStand, ref opponentBust, ref opponentDeck, ref score, ref opponentScore, ref dignity, ref opponentForcedStand, ref opponentForcedHit, ref opponentForcedStand2, ref stake, ref bustLimit, ref money, ref ace1, ref opponentAce1);

                round ++;
            }
            shop(ref deck, ref cards, ref stake, ref bustLimit, ref money, ref removePrice, ref incPrice, ref buyPrice);
        }
    }

    public static string[,] cards =
    {
        //odd numbers dont mess it up
        {"ace", "king", "queen", "jack", "10", "9", "8", "7", "6", "5"/*10*/, "4", "3", "2", "-10", "-9", "-8", "-7", "-6", "-5", "-4"/*20*/, "-3", "-2", "21", "1", "wild card(?)- can pick any rank between 1-5", "red card(5)- forces your opponent to stand", "gift card(7)- adds 5 to the opponents total", "scratch card(14)- 1/5 chance to set your total to the bust limit", "UNO +2 card(2)- forces the opponent to draw two cards", "purple hermit tarot card(1)- reveals the opponenets total to the nearest 5"
        /*30*/, "platinum star tarot card(6)- find the world tarot card to unlock ability", "the world tarot card(10)- find plainum star tarot card to unlock ability", "star platinum, the world(16)- permenantly increase your bust limit by 1", "green hierophant tarot card(6)- add a random negative card to your deck", "red magician tarot card(2)- remove 5 from opponents total if they have stood", "white chariot tarot card(?)- worth a random number between 1-5", "debit card(?)- +1 rank per £5 you have", 
        "credit card(10)- enter credit card number to widthdraw £10", "red joker(7)- sets total to bust limit if total is 7", "black joker(6)- sets total to bust limit if total is 16"/*40*/},
        
        {"1", "10", "10", "10", "10", "9", "8", "7", "6", "5"/*10*/, "4", "3", "2", "-10", "-9", "-8", "-7", "-6", "-5", "-4"/*20*/, "-3", "-2", "21", "1", "0", "5", "7", "14", "2", "1"
        /*30*/, "6", "10", "16", "6", "2", "0", "0", "10", "7", "6"/*40*/}
    };

    public static void youDraw(ref int total, ref List<List<string>> deck, ref bool bust, ref bool stand, ref int ace, ref string[,] cards, ref List<string> hand, ref List<string> opponentHand, ref int opponentTotal, ref int opponentAce, ref bool opponentStand, ref bool opponentBust, ref List<List<string>> opponentDeck, ref int score, ref int opponentScore, ref int dignity, ref bool opponentForcedStand, ref int opponentForcedHit, ref bool opponentForcedStand2, ref int stake, ref int bustLimit, ref int money, ref int ace1, ref int opponentAce1)
    {
        Random random = new Random();
        int cardNum = random.Next(0, deck[0].Count);

        //prints which card you drew
        Console.Write("\nYou drew the "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(deck[0][cardNum]); Console.ResetColor(); Console.WriteLine();

        //checks for special effect cards
        bool temp5 = false;
        if (deck[0][cardNum] == "ace")
        {
            ace1++;
            ace++;
            hand.Add(deck[0][cardNum]);
        }
        else if (deck[0][cardNum] == "wild card(?)- can pick any rank between 1-5")
        {
            bool temp2 = false;
            do
            {
                temp2 = false;
                ConsoleKeyInfo temp1 = Console.ReadKey();
                if (temp1.Key == ConsoleKey.D1)
                {
                    hand.Add("wild card(1)");
                    total += 1;
                }
                else if (temp1.Key == ConsoleKey.D2)
                {
                    hand.Add("wild card(2)");
                    total += 2;
                }
                else if (temp1.Key == ConsoleKey.D3)
                {
                    hand.Add("wild card(3)");
                    total += 3;
                }
                else if (temp1.Key == ConsoleKey.D4)
                {
                    hand.Add("wild card(4)");
                    total += 4;
                }
                else if (temp1.Key == ConsoleKey.D5)
                {
                    hand.Add("wild card(5)");
                    total += 5;
                }
                else
                {
                    temp2 = true;
                }
            }
            while (temp2 == true);
        }
        else if (deck[0][cardNum] == "red card(5)- forces your opponent to stand")
        {
            opponentForcedStand = true;
            hand.Add("red card(5)");
        }
        else if (deck[0][cardNum] == "gift card(7)- adds 5 to the opponents total")
        {
            opponentTotal += 5;
            hand.Add("gift card(7)");
        }
        else if (deck[0][cardNum] == "scratch card(14)- 1/5 chance to set your total to the bust limit")
        {
            hand.Add("scratch card(14)");
            int temp4 = random.Next(1,6);
            if (temp4 == 5)
            {
                total = bustLimit;
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("LUCKY!"); Console.ResetColor();
                temp5 = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Too bad..."); Console.ResetColor();
            }
        }
        else if (deck[0][cardNum] == "UNO +2 card(2)- forces the opponent to draw two cards")
        {
            opponentForcedHit = 2;
            hand.Add("+2 card(2)");
        }
        else if (deck[0][cardNum] == "purple hermit tarot card(1)- reveals the opponenets total to the nearest 5")
        {
            hand.Add("purple hermit card(1)");
            Console.Write("the opponents total is approximatly "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(Math.Round(opponentTotal / 5.0) * 5); Console.ResetColor();
            Console.WriteLine();
        }
        else if (deck[0][cardNum] == "platinum star tarot card(6)- find the world tarot card to unlock ability")
        {
            hand.Add("platinum star card(6)");
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] == "the world card(10)")
                {
                    hand.Remove("the world card(10)");
                    hand.Remove("platinum star card(6)");
                    hand.Add("star platinum the world(16)");
                    int temp6 = deck[0].IndexOf("the world tarot card(10)- find plainum star tarot card to unlock ability");
                    deck[0].RemoveAt(temp6);
                    deck[1].RemoveAt(temp6);
                    int temp7 = deck[0].IndexOf("platinum star tarot card(6)- find the world tarot card to unlock ability");
                    deck[0].RemoveAt(temp7);
                    deck[1].RemoveAt(temp7);
                    total += 10;
                    Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("`star platinum, the world(16)` has been added to your deck"); Console.ResetColor();
                }
            }
        }
        else if (deck[0][cardNum] == "the world tarot card(10)- find plainum star tarot card to unlock ability")
        {
            hand.Add("the world card(10)");
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] == "platinum star card(6)")
                {
                    //removes from deck and hand
                    hand.Remove("the world card(10)");
                    hand.Remove("platinum star card(6)");
                    hand.Add("star platinum the world(16)");
                    int temp6 = deck[0].IndexOf("the world tarot card(10)- find plainum star tarot card to unlock ability");
                    deck[0].RemoveAt(temp6);
                    deck[1].RemoveAt(temp6);
                    int temp7 = deck[0].IndexOf("platinum star tarot card(6)- find the world tarot card to unlock ability");
                    deck[0].RemoveAt(temp7);
                    deck[1].RemoveAt(temp7);
                    total += 6;
                    Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("`star platinum, the world(16)` has been added to your deck"); Console.ResetColor();
                    deck[0].Add("star platinum, the world(16)- permenantly increase your bust limit by 1");
                    deck[1].Add("16");
                }
            }
        }
        else if (deck[0][cardNum] == "star platinum, the world(16)- permenantly increase your bust limit by 1")
        {
            hand.Add("star platinum, the world(16)");
            bustLimit++;
        }
        else if (deck[0][cardNum] == "green hierophant tarot card(6)- add a random negative card to your deck")
        {
            int temp8 = random.Next(-10, -1);
            Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("{0} has been added to your deck", temp8); Console.ResetColor();
            deck[0].Add(Convert.ToString(temp8));
            deck[1].Add(Convert.ToString(temp8));
            hand.Add("green hierophant tarot card(6)");
        }
        else if (deck[0][cardNum] == "red magician tarot card(2)- remove 5 from opponents total if they have stood")
        {
            if (opponentStand == true || opponentBust == true)
            {
                opponentTotal -= 5;
            }
            hand.Add("red magician tarot card(2)");
        }
        else if (deck[0][cardNum] == "white chariot tarot card(?)- worth a random number between 1-5")
        {
            int temp9 = random.Next(1,6);
            total += temp9;
            hand.Add($"white chariot tarot card({temp9})");
        }
        else if (deck[0][cardNum] == "debit card(?)- +1 rank per £5 you have")
        {
            total += money / 5;
            hand.Add($"debit card({money / 5})");
        }
        else if (deck[0][cardNum] == "credit card(10)- enter credit card number to widthdraw £10")
        {
            Console.Write("Enter credit card number: ");
            Console.ReadLine();
            hand.Add("credit card(10)");
            money += 10;
        }
        else if (deck[0][cardNum] == "red joker(7)- sets total to bust limit if total is 7")
        {
            total += int.Parse(deck[1][cardNum]);
            hand.Add("red joker(7)");
            if (total == 7)
            {
                total = bustLimit;
            }
            temp5 = true;
        }
        else if (deck[0][cardNum] == "black joker(6)- sets total to bust limit if total is 16")
        {
            total += int.Parse(deck[1][cardNum]);
            hand.Add("black joker(6)");
            if (total == 16)
            {
                total = bustLimit;
            }
            temp5 = true;
        }
        else
        {
            hand.Add(deck[0][cardNum]);
        }

        //updates the total
        if (temp5 == false)
        {
            total += int.Parse(deck[1][cardNum]);
        }

        //ace shinanigans
        {
        int temp10 = 0;
        for (int i = 0; i < ace1; i++)
        {
            temp10++;
            total += 10;
        }
        ace1 -= temp10;
        if (total > bustLimit && ace > 0)
        {
            while (ace1 != ace && total > bustLimit)
            {
                total -= 10;
                ace1++;
            }
        }
        }
    }
    public static void opponentDraw(ref int total, ref List<List<string>> deck, ref bool bust, ref bool stand, ref int ace, ref string[,] cards, ref List<string> hand, ref List<string> opponentHand, ref int opponentTotal, ref int opponentAce, ref bool opponentStand, ref bool opponentBust, ref List<List<string>> opponentDeck, ref int score, ref int opponentScore, ref int dignity, ref bool opponentForcedStand, ref int opponentForcedHit, ref bool opponentForcedStand2, ref int stake, ref int bustLimit, ref int money, ref int ace1, ref int opponentAce1)
    {
        Random random = new Random();


        while (opponentForcedHit > 0)
        {
            int cardNum = random.Next(0, opponentDeck[0].Count);
            opponentHand.Add(opponentDeck[0][cardNum]);
            opponentTotal += int.Parse(opponentDeck[1][cardNum]);
            opponentForcedHit--;
        }

        //checks for card effects
        if (opponentForcedStand == true)
        {
            if (opponentForcedStand2 == false)
            {
                if (opponentHand.Count == 1)
                {
                    Console.Write("With "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(opponentHand.Count); Console.ResetColor(); Console.Write(" card, the opponent is forced to "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("stand"); Console.ResetColor(); Console.WriteLine();
                }
                else
                {
                    Console.Write("With "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(opponentHand.Count); Console.ResetColor(); Console.Write(" cards, the opponent if forced to "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("stand"); Console.ResetColor(); Console.WriteLine();
                }
                opponentForcedStand2 = true;
            }
        }
        else
        {
            //adds card to hand
            int cardNum = random.Next(0, opponentDeck[0].Count);
            opponentHand.Add(opponentDeck[0][cardNum]);

            //updates the total
            if (opponentStand == false && opponentBust == false)
            {
                opponentTotal += int.Parse(opponentDeck[1][cardNum]);
            }

            //card effects happen
            if (opponentDeck[0][cardNum] == "ace")
            {
                opponentAce++;
                opponentAce1++;
            }

            //ace shinanigans- kill me
            {
            int temp11 = 0;
            for (int i = 0; i < opponentAce1; i++)
            {
                temp11++;
                opponentTotal += 10;
            }
            opponentAce1 -= temp11;
            if (total > bustLimit && ace > 0)
            {
                while (opponentAce1 != opponentAce && total > bustLimit)
                {
                    total -= 10;
                    opponentAce1++;
                }
            }
            }

            //checks if they have gone bust
            while (opponentTotal > 21 && opponentBust == false)
            {
                if (opponentTotal > 21)
                {
                    opponentBust = true;
                }
            }

            //chooses wheather they hit or stand
            if (opponentTotal > 21 - 5)
            {
                if (opponentStand == false)
                {
                    //prints they have stood
                    //removes plural for only one card
                    if (opponentHand.Count == 1)
                    {
                        Console.Write("With "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(opponentHand.Count); Console.ResetColor(); Console.Write(" card, the opponent "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("stands"); Console.ResetColor(); Console.WriteLine();
                    }
                    else
                    {
                        Console.Write("With "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(opponentHand.Count); Console.ResetColor(); Console.Write(" cards, the opponent "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("stands"); Console.ResetColor(); Console.WriteLine();
                    }
                    opponentStand = true;
                }
            }
            else
            {
                //prints they have hit
                //removes plural for only one card
                if (opponentHand.Count == 1)
                {
                    Console.Write("With "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(opponentHand.Count); Console.ResetColor(); Console.Write(" card, the opponent "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("hits"); Console.ResetColor(); Console.WriteLine();
                }
                else
                {
                    Console.Write("With "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(opponentHand.Count); Console.ResetColor(); Console.Write(" cards, the opponent "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("hits"); Console.ResetColor(); Console.WriteLine();
                }
            }

        }
    }
    public static void opponent(ref int total, ref List<List<string>> deck, ref bool bust, ref bool stand, ref int ace, ref string[,] cards, ref List<string> hand, ref List<string> opponentHand, ref int opponentTotal, ref int opponentAce, ref bool opponentStand, ref bool opponentBust, ref List<List<string>> opponentDeck, ref int score, ref int opponentScore, ref int dignity, ref bool opponentForcedStand, ref int opponentForcedHit, ref bool opponentForcedStand2, ref int stake, ref int bustLimit, ref int money, ref int ace1, ref int opponentAce1, int round)
    {
        Console.WriteLine("Game {0} stake {0}", stake);

        //displays bust limit in cyan if changed
        if (bustLimit == 21)
        {
            Console.WriteLine("Bust limit: {0}", bustLimit);
        }
        else
        {
            Console.Write("Bust limit: "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(bustLimit); Console.ResetColor();    
        }

        //repeats rounds until someone wins
        while (score < 2 && opponentScore < 2)
        {
            bool none = false;

            youDraw(ref total, ref deck, ref bust, ref stand, ref ace, ref cards, ref hand, ref opponentHand, ref opponentTotal, ref opponentAce, ref opponentStand, ref opponentBust, ref opponentDeck, ref score, ref opponentScore, ref dignity, ref opponentForcedStand, ref opponentForcedHit, ref opponentForcedStand2, ref stake, ref bustLimit, ref money, ref ace1, ref opponentAce1);

            if (total > bustLimit && bust == false)
            {
                Console.WriteLine("Bust");
                bust = true;
            }

            if (bust == false)
            {
                Console.WriteLine("");
                //repeats while below bustLimit and hasnt stood
                do
                {
                    //asks to hit or stand
                    opponentDraw(ref total, ref deck, ref bust, ref stand, ref ace, ref cards, ref hand, ref opponentHand, ref opponentTotal, ref opponentAce, ref opponentStand, ref opponentBust, ref opponentDeck, ref score, ref opponentScore, ref dignity, ref opponentForcedStand, ref opponentForcedHit, ref opponentForcedStand2, ref stake, ref bustLimit, ref money, ref ace1, ref opponentAce1);
                    do
                    {
                        none = false;

                        Console.Write("Hit or stand: ");
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.H)
                        {
                            youDraw(ref total, ref deck, ref bust, ref stand, ref ace, ref cards, ref hand, ref opponentHand, ref opponentTotal, ref opponentAce, ref opponentStand, ref opponentBust, ref opponentDeck, ref score, ref opponentScore, ref dignity, ref opponentForcedStand, ref opponentForcedHit, ref opponentForcedStand2, ref stake, ref bustLimit, ref money, ref ace1, ref opponentAce1);
                        }
                        else if (key.Key == ConsoleKey.S)
                        {
                            Console.WriteLine();
                            stand = true;
                        }
                        else
                        {
                            none = true;
                            Console.WriteLine("");
                        }
                    }
                    while (none == true);

                    //prints out hand
                    Console.Write("You are holding: "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(hand[0]); Console.ResetColor();
                    for (int i = 1; i < hand.Count; i++)
                    {
                        Console.ResetColor(); Console.Write(", "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(hand[i]); Console.ResetColor();
                    }
                    
                    //prints the total before busting if you have busted
                    if (total > bustLimit)
                    {
                        Console.ResetColor(); Console.Write("\nYou bust with "); Console.ForegroundColor = ConsoleColor.Red; Console.Write(total); Console.ResetColor(); Console.WriteLine(""); Console.WriteLine("");
                        bust = true;
                    }

                    //prints the total after checking if you have busted
                    if (total <= bustLimit)
                    {
                        if (stand == false)
                        {
                            Console.WriteLine(""); Console.ResetColor(); Console.Write("Your total score is "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(total); Console.ResetColor(); Console.WriteLine(""); Console.WriteLine("");
                        }
                        else
                        {
                            Console.Write("\nYou stand at "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(total); Console.ResetColor();
                            if (opponentStand == true)
                            {
                                Console.ReadKey();
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                    }
                }
                while (bust == false && stand == false);

                //the opponent continues if you have finished
                while (opponentBust == false && opponentStand == false && opponentForcedStand == false)
                {
                    opponentDraw(ref total, ref deck, ref bust, ref stand, ref ace, ref cards, ref hand, ref opponentHand, ref opponentTotal, ref opponentAce, ref opponentStand, ref opponentBust, ref opponentDeck, ref score, ref opponentScore, ref dignity, ref opponentForcedStand, ref opponentForcedHit, ref opponentForcedStand2, ref stake, ref bustLimit, ref money, ref ace1, ref opponentAce1);
                    Console.ReadKey();
                    if (opponentStand == true || opponentBust == true)
                    {
                        Console.WriteLine("");
                    }
                }

                //scores are revealed
                {
                    if (bust == true)
                    {
                        Console.Write("You reveal to have busted with "); Console.ForegroundColor = ConsoleColor.Red; Console.Write(total); Console.ResetColor(); Console.ReadKey();
                    }
                    else
                    {
                        Console.Write("You reveal to have stood with "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(total); Console.ResetColor(); Console.ReadKey();
                    }
                    if (opponentBust == true)
                    {
                        Console.Write("\nThe opponent reveals to have busted with "); Console.ForegroundColor = ConsoleColor.Red; Console.Write(opponentTotal); Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("\nThe opponent reveals to have stood with "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(opponentTotal); Console.ResetColor();
                    }
                }

                //scores are compared
                {
                    if ((bust == true && opponentBust == true) || total == opponentTotal)
                    {
                        Console.Write("\nYou both drew"); Console.Write(" this round"); Console.WriteLine();
                    }
                    else if ((opponentBust == true && bust == false) || (bust == false && total > opponentTotal))
                    {
                        Console.Write("\nYou "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("won!"); Console.ResetColor(); Console.Write(" this round"); Console.WriteLine();
                        score++;
                        dignity += stake * 1;
                    }
                    else if ((opponentBust == false && bust == true) || total < opponentTotal)
                    {
                        Console.Write("\nYou "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("lost..."); Console.ResetColor(); Console.Write(" this round"); Console.WriteLine();
                        opponentScore++;
                        dignity += stake * -2;
                    }
                }

                //resets all variables and lists used in games exept scores and decks
                {
                    total = 0;
                    bust = false;
                    stand = false;
                    opponentTotal = 0;
                    opponentStand = false;
                    opponentBust = false;
                    opponentForcedHit = 0;
                    opponentForcedStand = false;
                    hand.Clear();
                    opponentHand.Clear();
                    ace = 0;
                    ace1 = 0;
                }

                //displays the scoreboard
                {
                    Console.WriteLine("{0}-{1}", score, opponentScore);
                    Console.ReadKey();

                    if (score == 2)
                    {
                        Console.Write("\nYou "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("won!"); Console.ResetColor(); Console.Write(" this game"); Console.WriteLine();
                    }
                    else if (opponentScore == 2)
                    {
                        Console.Write("\nYou "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("lost..."); Console.ResetColor(); Console.Write(" this game"); Console.WriteLine();
                    }
                }

                //removes dignity if lost
                if (opponentScore == 2)
                {
                    dignity += stake * -5;
                }

                //checks to see if you died
                if (dignity < 0)
                {
                    dignity = 0;
                }
                if (dignity < 51 && dignity > 25)
                {
                    Console.Write("Dignity: "); Console.ForegroundColor = ConsoleColor.DarkMagenta; Console.WriteLine(dignity); Console.ResetColor();
                }
                else if (dignity < 26 && dignity > 10)
                {
                    Console.Write("Dignity: "); Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(dignity); Console.ResetColor();
                }
                else if (dignity < 11)
                {
                    Console.Write("Dignity: "); Console.ForegroundColor = ConsoleColor.DarkRed; Console.WriteLine(dignity); Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Dignity: {0}", dignity);
                }
                if (dignity <= 0)
                {
                    Console.WriteLine();
                    Console.ReadKey();
                    Console.WriteLine("You died on round " + round);
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                
                //adds money
                if (score >= 2)
                {
                    money += 5 * stake;
                }
                
                //diplays stake
                if (score != 2 && opponentScore != 2)
                {
                    if (dignity <= stake * 5)
                    {
                        Console.Write("losing this game will cost "); Console.ForegroundColor = ConsoleColor.DarkRed; Console.Write(stake*5); Console.ResetColor(); Console.WriteLine(" dignity");
                    }
                    else
                {
                    Console.WriteLine("losing this game will cost {0} dignity", stake*5);
                }
                }

                //displays money
                Console.Write("money: "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("£{0}", money); Console.ResetColor();

                //displays another round
                if (score < 2 && opponentScore < 2)
            {
                Console.WriteLine("You go for another round");
            } 
            }
        }
        opponentScore = 0;
        score = 0;
        ace = 0;
        ace1 = 0;
        opponentAce = 0;
        opponentAce = 0;
    }
    public static void pickCard(ref int total, ref List<List<string>> deck, ref bool bust, ref bool stand, ref int ace, ref string[,] cards, ref List<string> hand, ref List<string> opponentHand, ref int opponentTotal, ref int opponentAce, ref bool opponentStand, ref bool opponentBust, ref List<List<string>> opponentDeck, ref int score, ref int opponentScore, ref int dignity, ref bool opponentForcedStand, ref int opponentForcedHit, ref bool opponentForcedStand2, ref int stake, ref int bustLimit, ref int money, ref int ace1, ref int opponentAce1)
    {
        Random random = new Random();

        int card1;
        int card2;
        int card3;
        bool none = true;

        //card variables are given unique random numbers without secret cards
        {
            do
            {
                card1 = random.Next(0, cards.Length / 2);
            }
            while (card1 == 32);
            do
            {
                card2 = random.Next(0, cards.Length / 2);
            }
            while (card1 == card2 ||card2 == 32);
            do
            {
                card3 = random.Next(0, cards.Length / 2);
            }
            while (card1 == card3 || card2 == card3 || card3 == 32);
        }

        //options are shown
        Console.WriteLine();
        Console.WriteLine("Pick a card or skip (type the number or s)");
        Console.WriteLine("1. {0}", cards[0, card1]);
        Console.WriteLine("2. {0}", cards[0, card2]);
        Console.WriteLine("3. {0}", cards[0, card3]);

        //adds the cards to deck
        do
        {
            none = true;
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.D1)
            {
                none = false;
                deck[0].Add(cards[0, card1]);
                deck[1].Add(cards[1, card1]);
            }
            else if (key.Key == ConsoleKey.D2)
            {
                none = false;
                deck[0].Add(cards[0, card2]);
                deck[1].Add(cards[1, card2]);
            }
            else if (key.Key == ConsoleKey.D3)
            {
                none = false;
                deck[0].Add(cards[0, card3]);
                deck[1].Add(cards[1, card3]);
            }
            else if (key.Key == ConsoleKey.S)
            {
                none = false;
            }
        }
        while (none == true);
    }
    public static void starterDeck(ref List<List<string>> deck, ref string[,] cards)
    {
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 13; i++)
            {
                deck[j].Add(cards[j, i]);
            }
        }
        
        /*
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 13; i++)
            {
                deck[j].Add(cards[j, 0]);
            }
        }
        */
        
    }
    public static void shop(ref List<List<string>> deck, ref string[,] cards, ref int stake, ref int bustLimit, ref int money, ref int removePrice, ref int incPrice, ref int buyPrice)
    {   
        Random random = new Random();
        
        int card1;
        int card2;
        int card3;
        int card4;
        int card5;
        int cardB1;
        int cardB2;
        int cardB3;
        bool none = true;
        bool exit = false;

        Console.WriteLine();

         //card variables are given unique random numbers without secret cards
        {
            do
            {
                card1 = random.Next(0, cards.Length / 2);
            }
            while (card1 == 32);
            do
            {
                card2 = random.Next(0, cards.Length / 2);
            }
            while (card1 == card2 || card2 == 32);
            do
            {
                card3 = random.Next(0, cards.Length / 2);
            }
            while (card1 == card3 || card2 == card3 || card3 == 32);
            do
            {
                card4 = random.Next(0, cards.Length / 2);
            }
            while (card1 == card4 || card2 == card4 || card3 == card4 || card4 == 32);
            do
            {
                card5 = random.Next(0, cards.Length / 2);
            }
            while (card1 == card5 || card2 == card5 || card3 == card5 || card4 == card5 || card5 == 32);
        }

        Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("£"); Console.WriteLine(money); Console.ResetColor();
        Console.WriteLine("You enter a shop");
        Console.ReadKey();
        
        //options are displayed
        do
        {    
            if (none == false)
            {
                Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("£" + money); Console.ResetColor();
            }
            Console.WriteLine("Here are your options (type the number or s to exit):");
            Console.WriteLine("1. View your deck - Free");
            Console.WriteLine("2. Remove a card - £" + removePrice);
            Console.WriteLine("3. Increase bust limit by 1 - £" + incPrice);
            Console.WriteLine("4. Booster pack (3 random cards) - £" + boostPrice);
            Console.WriteLine();
            Console.WriteLine("Any card £" + buyPrice);
            Console.WriteLine("5. {0}", cards[0, card1]);
            Console.WriteLine("6. {0}", cards[0, card2]);
            Console.WriteLine("7. {0}", cards[0, card3]);
            Console.WriteLine("8. {0}", cards[0, card4]);
            Console.WriteLine("9. {0}", cards[0, card5]);
            
            
            //checks the options
            do
            {
                none = true;
                bool temp14 = false;
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.D1)
                {
                    none = false;
                    Console.WriteLine();Console.WriteLine();
                    displayDeck(deck);
                }
                else if (key.Key == ConsoleKey.D2)
                {
                    if (money < removePrice)
                    {
                        Console.WriteLine("Not enough money!");
                    }
                    else
                    {
                        temp14 = true;
                        none = false;
                        Console.WriteLine("\n");

                        //deck is displayed with numbers
                        for (int i = 0; i < deck[0].Count; i++)
                        {
                            Console.WriteLine("{0}. {1}", i+1, deck[0][i]);
                        }
                        Console.WriteLine("Enter the number of the card you want to remove");
                        bool temp13 = false;
                        do
                        {
                            temp13 = false;
                            try
                            {
                                #pragma warning disable CS8604 
                                int temp12 = int.Parse(Console.ReadLine());
                                #pragma warning restore CS8604 
                                string temp15 = deck[0][temp12 - 1];
                                deck[0].RemoveAt(temp12 - 1);
                                deck[1].RemoveAt(temp12 - 1);
                                Console.ForegroundColor = ConsoleColor.Blue;Console.Write(temp15);  Console.ResetColor(); Console.Write(" was removed to your deck\n");
                            }
                            catch
                            {
                                Console.Write("Enter a valid number");
                                temp13 = true;
                            }
                        }
                        while (temp13 == true);
                        money-=removePrice;
                        removePrice+=5;
                    }
                }
                else if (key.Key == ConsoleKey.D3)
                {
                    if (money < incPrice)
                    {
                        Console.WriteLine("Not enough money!");
                    }
                    else
                    {
                        none = false;
                        bustLimit++;
                        Console.Write("\nBust limit is now "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(bustLimit); Console.ResetColor();
                        money -= incPrice;
                        incPrice += 5;
                    }
                }
                else if (key.Key == ConsoleKey.D4)
                {
                    if (money < boostPrice)
                    {
                        Console.WriteLine("Not enough money!");
                    }
                    else
                    {
                        none = false;
                        //random numbers
                        {
                        do
                        {
                            cardB1 = random.Next(0, cards.Length / 2);
                        }
                        while (cardB1 == 32);
                        do
                        {
                            cardB2 = random.Next(0, cards.Length / 2);
                        }
                        while (cardB1 == cardB2 || cardB2 == 32);
                        do
                        {
                            cardB3 = random.Next(0, cards.Length / 2);
                        }
                        while (cardB1 == cardB3 || cardB2 == cardB3 || cardB3 == 32);
                        }

                        //card added
                        deck[0].Add(cards[0, cardB1]);
                        deck[1].Add(cards[1, cardB1]);
                        deck[0].Add(cards[0, cardB2]);
                        deck[1].Add(cards[1, cardB2]);
                        deck[0].Add(cards[0, cardB3]);
                        deck[1].Add(cards[1, cardB3]);
                        
                        //cards displayed
                        Console.ForegroundColor = ConsoleColor.Blue; Console.Write("\n" + cards[0, cardB1]); Console.ResetColor(); Console.WriteLine(" was added to your deck");
                        Console.ReadKey();
                        Console.ForegroundColor = ConsoleColor.Blue; Console.Write(cards[0, cardB2]); Console.ResetColor(); Console.WriteLine(" was added to your deck");
                        Console.ReadKey();
                        Console.ForegroundColor = ConsoleColor.Blue; Console.Write(cards[0, cardB3]); Console.ResetColor(); Console.Write(" was added to your deck");

                        money -= boostPrice;
                        boostPrice += 10;
                    }
                }
                else if (key.Key == ConsoleKey.D5)
                {
                    if (money < buyPrice)
                    {
                        Console.WriteLine("Not enough money!");
                    }
                    else
                    {
                        none = false;
                        deck[0].Add(cards[0, card1]);
                        deck[1].Add(cards[1, card1]);
                        money -= buyPrice;
                        buyPrice += 5;
                        Console.ForegroundColor = ConsoleColor.Blue;Console.Write(cards[0, card1]);  Console.ResetColor(); Console.Write(" was added to your deck\n");
                    }
                }
                else if (key.Key == ConsoleKey.D6)
                {
                    if (money < buyPrice)
                    {
                        Console.WriteLine("Not enough money!");
                    }
                    else
                    {
                        none = false;
                        deck[0].Add(cards[0, card2]);
                        deck[1].Add(cards[1, card2]);
                        money -= buyPrice;
                        buyPrice += 5;
                        Console.ForegroundColor = ConsoleColor.Blue;Console.Write(cards[0, card2]);  Console.ResetColor(); Console.Write(" was added to your deck\n");
                    }
                }
                else if (key.Key == ConsoleKey.D7)
                {
                    if (money < buyPrice)
                    {
                        Console.WriteLine("Not enough money!");
                    }
                    else
                    {
                        none = false;
                        deck[0].Add(cards[0, card3]);
                        deck[1].Add(cards[1, card3]);
                        money -= buyPrice;
                        buyPrice += 5;
                        Console.ForegroundColor = ConsoleColor.Blue;Console.Write(cards[0, card3]);  Console.ResetColor(); Console.Write(" was added to your deck\n");
                    }
                }
                else if (key.Key == ConsoleKey.D8)
                {
                    if (money < buyPrice)
                    {
                        Console.WriteLine("Not enough money!");
                    }
                    else
                    {
                        none = false;
                        deck[0].Add(cards[0, card4]);
                        deck[1].Add(cards[1, card4]);
                        money -= buyPrice;
                        buyPrice += 5;
                        Console.ForegroundColor = ConsoleColor.Blue;Console.Write(cards[0, card4]);  Console.ResetColor(); Console.Write(" was added to your deck\n");
                    }
                }
                else if (key.Key == ConsoleKey.D9)
                {
                    if (money < buyPrice)
                    {
                        Console.WriteLine("Not enough money!");
                    }
                    else
                    {
                        none = false;
                        deck[0].Add(cards[0, card5]);
                        deck[1].Add(cards[1, card5]);
                        money -= buyPrice;
                        buyPrice += 5;
                        Console.ForegroundColor = ConsoleColor.Blue;Console.Write(cards[0, card5]);  Console.ResetColor(); Console.Write(" was added to your deck\n");
                    }
                }
                else if (key.Key == ConsoleKey.S)
                {
                    none = false;
                    exit = true;
                }
                if (none == false && exit == false && temp14 == false)
                {
                    Console.ReadKey();
                    Console.WriteLine();
                }
                temp14 = false;
            }
            while (none == true && exit == false);
        }
        while (exit == false);
        exit = false;
    }
    public static void displayDeck(List<List<string>> deck)
    {
        for (int i = 0; i < deck[0].Count; i++)
        {
            Console.WriteLine(deck[0][i]);
        }
        Console.WriteLine("{0} cards in total", deck[0].Count);
    }
}
