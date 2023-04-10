using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AsyncBlackJack
{
    class BlackJack
    {
        public string? cards { get; set; } = "";

        static IEnumerable<string> Suits()
        {
            yield return "clubs";
            yield return "diamons";
            yield return "hearts";
            yield return "spades";
        }
        static IEnumerable<string> Ranks()
        {
            yield return "two";
            yield return "three";
            yield return "four";
            yield return "five";
            yield return "six";
            yield return "seven";
            yield return "eight";
            yield return "nine";
            yield return "ten";
            yield return "jack";
            yield return "queen";
            yield return "king";
            yield return "ace";
        }

        static Dictionary<string, int> numbermap = new Dictionary<string, int>()
        {
            {"two",2},
            {"three",3},
            {"four",4},
            {"five",5},
            {"six",6},
            {"seven",7},
            {"eight",8},
            {"nine",9},
            {"ten",10},
            {"jack",10},
            {"queen",10},
            {"king",10},
            {"ace",1},
        };

        public static ArrayList players { get; set; } = new ArrayList { };
        public static int[] playersScore { get; set; } = new int[] { };
        public static int[] playersAce { get; set; } = new int[99];
        public static List<BlackJack> cardList { get; set; } = new List<BlackJack>();
        public static List<BlackJack> userCardList { get; set; } = new List<BlackJack>();
        public static List<BlackJack> sysCardList { get; set; } = new List<BlackJack>();

        public static Random random { get; set; } = new Random();
        public static int userCardValue { get; set; } = 0;
        public static int sysCardValue { get; set; } = 0;
        public static int randomNumber { get; set; } = 0;
        public static string[] separators = { "Suit =", ", Rank =", "}" };
        public static int countAce { get; set; } = 0;
        public static int sysCountAce { get; set; } = 0;
        public static string howManyPlayers;



        private static void Main(string[] args)
        {
            var allcards = from s in Suits()
                           from r in Ranks()
                           select new { Suit = s, Rank = r };

            foreach (var card in allcards)
            {
                cardList.Add(new BlackJack
                {
                    cards = card.ToString()
                });

            }
            // cardList.ForEach(p => Console.WriteLine(p.cards));
            Console.WriteLine("How many player in here?? enter 1-5");
            howManyPlayers = Console.ReadLine();
            Console.WriteLine("_______");
            startingGame(howManyPlayers);
            addsysCard();
        }

        static void startingGame(string howManyPlayers)
        {
            for (var z = 0; z < int.Parse(howManyPlayers); z++)
            {
                userCardList.Clear();
                for (var i = 1; i < 3; i++)
                {
                    randomNumber = random.Next(0, cardList.Count - 1);
                    userCardList.Add(new BlackJack
                    {
                        cards = cardList.ElementAt(randomNumber).cards
                    });
                    cardList.RemoveAt(randomNumber);
                }

                players.Add(new List<BlackJack>(userCardList));
                List<BlackJack> player = (List<BlackJack>)players[z];

                string[] cardsYouGot = new string[userCardList.Count];

                for (var i = 0; i < 2; i++)
                {
                    cardsYouGot[i] = player[i].cards.Split(separators, StringSplitOptions.None)[2].ToString().Trim();
                }

                Console.WriteLine("_________");

                if (cardsYouGot[0].ToString().Trim() == "ace" && cardsYouGot[1].ToString().Trim() == "ace")
                {
                    userCardValue += 12;
                    countAce += 2;
                    playersScore = playersScore.Append(userCardValue).ToArray();
                    playersAce[z] = countAce;
                }
                else if (cardsYouGot[0].ToString().Trim() == "ace")
                {
                    userCardValue = 11 + numbermap[cardsYouGot[1].ToString().Trim()];
                    playersScore = playersScore.Append(userCardValue).ToArray();
                    countAce++;
                    playersAce[z] = countAce;
                }
                else if (cardsYouGot[1].ToString().Trim() == "ace")
                {
                    userCardValue = numbermap[cardsYouGot[0].ToString().Trim()] + 11;
                    playersScore = playersScore.Append(userCardValue).ToArray();
                    countAce++;
                    playersAce[z] = countAce;
                }
                else
                {
                    userCardValue = numbermap[cardsYouGot[0].ToString().Trim()] + numbermap[cardsYouGot[1].ToString().Trim()];
                    playersScore = playersScore.Append(userCardValue).ToArray();
                    playersAce[z] = 0;
                }
                userCardList.ForEach(card => Console.WriteLine($"Player {z + 1} Card" + card.cards));
                Console.WriteLine("You got:" + userCardValue);
                Console.WriteLine("_________");
            }
            randomNumber = random.Next(0, cardList.Count - 1);

            sysCardList.Add(new BlackJack
            {
                cards = cardList.ElementAt(randomNumber).cards
            });
            cardList.RemoveAt(randomNumber);

            sysCardList.ForEach(card => Console.WriteLine("System Card" + card.cards));
            string[] sysFirstCard = sysCardList[0].cards.Split(separators, StringSplitOptions.None);
            if (sysFirstCard[2].ToString().Trim() == "ace")
            {
                sysCardValue += 11;
                sysCountAce++;
            }
            else
            {
                sysCardValue = numbermap[sysFirstCard[2].ToString().Trim()];
            }
            Console.WriteLine("system got: " + sysCardValue);

            Console.WriteLine("Cards remain: " + cardList.Count);
            Console.WriteLine("_________");

            for (var x = 1; x <= int.Parse(howManyPlayers); x++)
            {
                if (userCardValue < 21)
                {
                    Console.WriteLine($"Player {x} turn: Add card or not please enter 1 for add 2 for skip");
                    string? input = Console.ReadLine();
                    addCards(input, x - 1);
                }
            }


        }

        private static void addCards(string userInput, int playerIndex)
        {
            switch (userInput)
            {
                case "1":
                    userCardList.Clear();

                    randomNumber = random.Next(0, cardList.Count - 1);
                    List<BlackJack> player = (List<BlackJack>)players[playerIndex];
                    userCardList.Add(new BlackJack
                    {
                        cards = cardList.ElementAt(randomNumber).cards
                    });

                    player.Add(userCardList[0]);
                    cardList.RemoveAt(randomNumber);

                    string[] addCard = player[(player.Count - 1)].cards.Split(separators, StringSplitOptions.None);

                    if (playersAce[playerIndex] == 0)
                    {
                        if (addCard[2].ToString().Trim() == "ace")
                        {
                            if (playersScore[playerIndex] < 11)
                            {
                                playersScore[playerIndex] += 11;
                            }
                            else
                            {
                                playersScore[playerIndex]++;
                            }
                        }
                        else
                        {
                            playersScore[playerIndex] += numbermap[addCard[2].ToString().Trim()];
                        }
                    }
                    else if (countAce > 1 && (playersScore[playerIndex] + numbermap[addCard[2].ToString().Trim()]) <= 21)
                    {
                        playersScore[playerIndex] += numbermap[addCard[2].ToString().Trim()];
                    }
                    else
                    {
                        playersScore[playerIndex] = (playersScore[playerIndex] - 10) + numbermap[addCard[2].ToString().Trim()];
                        playersAce[playerIndex] = 0;
                    }

                    player.ForEach(card => Console.WriteLine("you card:" + card.cards));
                    Console.WriteLine("You got: " + playersScore[playerIndex]);

                    if (playersScore[playerIndex] > 21)
                    {
                        Console.WriteLine($"Player {playerIndex + 1} go over 21 you loosse");
                        playersScore[playerIndex] = 0;
                        Console.WriteLine("Cards remain:" + cardList.Count);
                    }
                    else
                    {
                        Console.WriteLine("Cards remain:" + cardList.Count);
                        Console.WriteLine("Add card or not !! please enter 1 for add. 2 for skip");
                        string userinput = Console.ReadLine();
                        addCards(userinput, playerIndex);
                    }
                    break;

                case "2":
                    break;
            }
        }
        private static void addsysCard()
        {
            while (sysCardValue < 17)
            {
                randomNumber = random.Next(0, cardList.Count - 1);
                sysCardList.Add(new BlackJack
                {
                    cards = cardList.ElementAt(randomNumber).cards
                });
                cardList.RemoveAt(randomNumber);

                string[] addsysCard = sysCardList[(sysCardList.Count - 1)].cards.Split(separators, StringSplitOptions.None);

                if (addsysCard[2].ToString().Trim() == "ace" && sysCardValue <= 21)
                {
                    if (sysCardValue < 11)
                    {
                        sysCardValue += 11;
                    }
                    else
                    {
                        sysCardValue++;
                    }
                }
                else
                {
                    sysCardValue += numbermap[addsysCard[2].ToString().Trim()];
                }

                sysCardList.ForEach(card => Console.WriteLine("system Card" + card.cards));
                Console.WriteLine("System got: " + sysCardValue);
            }

            if (sysCardValue > 21)
            {
                var indices = playersScore.Select((value, index) => new { value, index }).Where(x => x.value == 0).Select(x => x.index);
                int? gg = indices.Count();

                if (indices.Count() > 0)
                {
                    foreach (int index in indices)
                    {
                        Console.WriteLine($"Player {index + 1} looose the game");
                    }
                    Console.WriteLine("Other players win this game");
                }
                else
                {
                    Console.WriteLine("All players win this game");
                }
                cardList.Clear();
                sysCardList.Clear();
                players.Clear();
                Array.Clear(playersScore, 0, playersScore.Length);
                playersScore = new int[] { };
                Array.Clear(playersAce, 0, playersAce.Length);
                Main(null);

            }
            else
            {
                compareCards();
                cardList.Clear();
                sysCardList.Clear();
                players.Clear();
                Array.Clear(playersScore, 0, playersScore.Length);
                playersScore = new int[] { };
                Array.Clear(playersAce, 0, playersAce.Length);
                Main(null);
            }
        }
        private static void compareCards()
        {
            for (var i = 0; i < playersScore.Length; i++)
            {
                if (playersScore[i] == sysCardValue)
                {
                    Console.WriteLine("____________");
                    Console.WriteLine($"Player {i + 1} Tie this game!!");
                    Console.WriteLine("____________");
                }
                else if (playersScore[i] > sysCardValue)
                {
                    Console.WriteLine("____________");
                    Console.WriteLine($"Player {i + 1} Winer Winer Chicken dinner!!");
                    Console.WriteLine("____________");
                }
                else
                {
                    Console.WriteLine($"Player {i + 1} haha!!! Bad Luck try again");
                }
            }
        }


    }
}
