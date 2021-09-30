using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDataTests : MonoBehaviour {
    #region Fields
    // Ideally the program is written in a way that key fields such as the maximum and current life of the player are ScriptableObjects and can be passed around easily to systems
    // that need them, but we're going to give wholesale references to the CharacterInputController, CharacterCollider, and the Life UI in this case.  
    [SerializeField]
    private CharacterInputController controller;
    [SerializeField]
    private CharacterCollider characterCollider;
    [SerializeField]
    private GameObject lifeUI;
    // Ideally this would be a ScriptableObject containing testing values, but since we're only testing one value here I've just used an int instead.
    [SerializeField]
    private int intendedStartingLife = 5;
    
    private List<TestStruct<int>> tests = new List<TestStruct<int>>();
    #endregion

    private void Awake() {
        EventSubscribe();
    }

    private void EventSubscribe() {
        // I added an event to the CharacterCollider class invoked in the OnTriggerEnter method, which is being subscribed here.
        characterCollider.characterHitEvent += OnCharacterHit;
    }

    private void AssertInitialLifeValues (int intendedStartingLife) {
        bool initialMaximumLifeTestResult = controller.maxLife == intendedStartingLife;
        string initialMaximumLifeTestNotes = "Initial maximum life is not set correctly on startup.";
        TestStruct<int> initialMaximumLifeTest = new TestStruct<int> ("Initial Maximum Life", controller.maxLife, intendedStartingLife, initialMaximumLifeTestResult, initialMaximumLifeTestNotes);
        tests.Add(initialMaximumLifeTest);

        bool initialCurrentLifeTestResult = controller.currentLife == intendedStartingLife;
        string initialCurrentLifeTestNotes = "Initial current life is not set correctly on startup.";
        TestStruct<int> initialCurrentLifeTest = new TestStruct<int> ("Initial Current Life", controller.currentLife, intendedStartingLife, initialCurrentLifeTestResult, initialCurrentLifeTestNotes);
        tests.Add(initialCurrentLifeTest);
    }

    private void AssertInitialLifeUI (int intendedStartingLife) {
        bool initialMaximumLifeUITestResult = lifeUI.GetComponentsInChildren<Image>().Length == intendedStartingLife;
        string initialMaximumLifeUITestNotes = "The number of life hearts in the UI is not set correctly on startup.";
        TestStruct<int> initialMaximumLifeUITest = new TestStruct<int> ("Initial Maximum Life UI", lifeUI.GetComponentsInChildren<Image>().Length, intendedStartingLife, initialMaximumLifeUITestResult, initialMaximumLifeUITestNotes);
        tests.Add(initialMaximumLifeUITest);
        
        // The way that the life heart UI is initialized and colored is poor, so without changing the code this is the "best" way to test.
        int numberOfRedUIHearts = CountNumberOfRedUIHearts();
        bool initialCurrentLifeUITestResult = numberOfRedUIHearts == intendedStartingLife;
        string initialCurrentLifeUITestNotes = "The number of current life hearts in the UI is not set correctly on startup.";
        TestStruct<int> initialCurrentLifeUITest = new TestStruct<int> ("Initial Current Life UI", numberOfRedUIHearts, intendedStartingLife, initialCurrentLifeUITestResult, initialCurrentLifeUITestNotes);
        tests.Add(initialCurrentLifeUITest);
    }

    private int CountNumberOfRedUIHearts() {
        int numberOfRedUIHearts = 0;

        foreach (Image lifeHeart in lifeUI.GetComponentsInChildren<Image>()) {
            if (lifeHeart.color == Color.white) {
                numberOfRedUIHearts++;
            }
        }

        return numberOfRedUIHearts;
    }

    private void OnCharacterHit() {
        AssertCurrentLifeUIUpdateOnHit();
    }

    private void AssertCurrentLifeUIUpdateOnHit() {
        int numberOfRedUIHearts = CountNumberOfRedUIHearts();
        bool currentLifeUIUpdateTestResult = numberOfRedUIHearts == controller.currentLife;
        string currentLifeUIUpdateTesetNotes = "The number of current life hearts in the UI is not updating correctly on being hit.";
        TestStruct<int> currentLifeUIUpdateTest = new TestStruct<int> ("Current Life UI Update", numberOfRedUIHearts, controller.currentLife, currentLifeUIUpdateTestResult, currentLifeUIUpdateTesetNotes);
        tests.Add(currentLifeUIUpdateTest);
    }

    // Currently the method to trigger collisions is OnTriggerEnter, which is not passed to a different function to be handled.  The trigger method is also protected, so I can't call
    // it from this unit test and feed it dummy collision data to test if the current life is decremented properly.  If it weren't protected, I would set the character's current life
    // to a known value, then call the function feeding it collision data that is on the obstacle layer, then check to see if the current life was updated to one less than what I set
    // it to.
}
