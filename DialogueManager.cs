using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; //The text that we change on dialogue

    public bool startDialogueOnStart; //checking if we need to start dialogue at start
    
    [SerializeField] public string[] phrases; //Sentences that we want to see in dialogue.
    
    public float timeBetweenLetter; //The time between letters in the dialogue, if you want your dialogue be instantly give it -1, if you want to wait 1 frame between letter than give it 0.
    
    public KeyCode dialogueSkipKey; //The key that we need press to skip dialogue

    private int _phraseQueue = 0; //The variable that makes sentences written in order.

    private void Start()
    {
        if(startDialogueOnStart)StartDialogue(); // im starting my dialogue here if you want you can start it from a method or another script
    }

    public void StartDialogue() //trigger this for start dialogue //If you getting phrases from another script or a method, get paramater as "string[] phrases_" and make phrases equal to it.
    {
        dialogueText.text = string.Empty; //Clearing text
        StartCoroutine(TypeSentence(phrases[_phraseQueue])); //we are starting our IEnumerator coroutine with _phrasequeue to write phrases in order
    }

    public void EndDialogue() //Ending our dialogue
    {
        Debug.Log("Dialogue is ended");
        dialogueText.text = string.Empty;
    }
    private IEnumerator TypeSentence(string phrase) //We are using a IEnumerator cause we can want a delay between letters.
    {
        foreach (char letter in phrase.ToCharArray()) //in here we are getting a char array of our phrase to get all letters of our phrase. we are using foreach cause it will repeat until phrase ends.
        {
            dialogueText.text += letter; //we are adding our letter to text
            yield return timeBetweenLetter switch  //Now if we selected to delay, this switch will look what kind of delay that we want and set it.
            {
                0 => new WaitForEndOfFrame(),
                -1 => null,
                _ => new WaitForSeconds(timeBetweenLetter)
            };
        }
        _phraseQueue++; //We need to increase phrase queue to write phrase in order
        yield return new WaitUntil(WriteNewLine); //Here we are looking for the situation to write other sentence.
        if (_phraseQueue >= phrases.Length)// we are checking if dialogue write all the phrases to end the dialogue
        {
            EndDialogue();
        }
        else StartDialogue(); //we are starting our dialogue again but now it will write the another sentence accordingly in order
    }

    bool WriteNewLine() => Input.GetKeyDown(dialogueSkipKey); //This method returns true if space is pressed
}
