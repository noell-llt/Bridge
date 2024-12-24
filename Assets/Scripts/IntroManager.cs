using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroManager: MonoBehaviour
{
    public GameObject background;
    public GameObject title;
    public GameObject rulesButton;
    public GameObject startButton;
    public GameObject playButton;
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject rulesPage;
    public TextMeshProUGUI rules;

 public void startGame()
    {
        background.SetActive(false);
        title.SetActive(false);
        rulesButton.SetActive(false);
        startButton.SetActive(false);
    }

    public void showRules()
    {
        title.SetActive(false);
        rulesButton.SetActive(false);
        startButton.SetActive(false);
        rulesPage.SetActive(true);
        nextButton.SetActive(true);
        backButton.SetActive(false);
        playButton.SetActive(false);
        rules.text =
            "<size=150%>OVERVIEW</size>\n" +
            "Welcome to our card game, where strategy, bidding, and trick-taking are the heart of the play." +
            "This game uses a standard 52-card deck and is designed for multiple players, each competing to " +
            "win sets by playing higher-value cards. Unlike traditional contract bridge, this game has unique " +
            "features that make for a dynamic and unpredictable experience.\n\n" +
            "<size=150%>SETUP</size>\n" +
            "- standard 52-card deck is used.\n" +
            "- each player has 13 cards.\n\n" +
            "<size=150%>OBJECTIVE</size>\n" +
            "You and your partner, which is the player opposite have to win more sets than the other 2 players." +
            "The goal is to win sets. A set consists of one card played by each player. The highest-value card " +
            "in the leading suit sins the trick, unless trumped. \n\n" +
            "<size=150%>TRUMP SUIT</size>\n" +
            "- A trump suit will be randomly generated at the start of the game.\n" +
            "- Cards from the trump suit override cards from any other suit.\n\n";
    }

    public void RulesPage2()
    {
        backButton.SetActive(true);
        nextButton.SetActive(false);
        playButton.SetActive(true);
        rules.text =
            "<size=150%>PLAYING THE GAME</size>\n" +
            "- Play proceeds clockwise. \n" +
            "- Each player contributes one card to each set.\n" +
            "- The player who wins the set leads the next one. \n" +
            "- If a player cannot follow suit, they may play any card but unless it is the trump card, the card would be insignificant. \n" +
            "- If a trump card is played, it wins the set unless a higher trump card is played.\n\n" +
            "<size=150%>GAME DURATION</size>\n" +
            "The game ends after 13 sets have been collected from all 4 players.\n\n" +
            "<size=150%>WINNING</size>\n" +
            "- The partners with the most sets at the end of the play wins the game! \n\n\n\n" +
            "<size=125%><I>ENJOY the game and may the best strategist win!<I></size>";
    }

    public void GoBack()
    {
        showRules();
        playButton.SetActive(false);
    }

    public void PlayGame()
    {
        rulesPage.SetActive(false);
        title.SetActive(true);
        rulesButton.SetActive(true);
        startButton.SetActive(true);
    }
}