using UnityEngine;
using UnityEngine.InputSystem;

public class ScooterScript : MonoBehaviour
{
    InputToDeckScript deck;
    InputToBarScript bar;

    public Vector2 moveVal;

    void Start()
    {
        deck = GameObject.Find("DeckParent").GetComponent<InputToDeckScript>();
        bar = GameObject.Find("BarParent").GetComponent<InputToBarScript>();
    }

    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
        deck.SendMessage("Move", moveVal.y);
        bar.SendMessage("Rotate", moveVal.x);
    }
    void OnBunnyHop()
    {
        deck.SendMessage("BunnyHop");
    }
}
