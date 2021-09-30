using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStruct<T> {
    public string testName;
    public T testedValue;
    public T expectedValue;
    public bool testPassed;
    public string failNotes;

    public TestStruct(string thisTestName, T thisTestedValue, T thisExpectedValue, bool thisTestPassed = true, string thisFailNotes = "") {
        testName = thisTestName;
        testedValue = thisTestedValue;
        expectedValue = thisExpectedValue;
        testPassed = thisTestPassed;
        failNotes = thisFailNotes;
    }

    public string GetTestResult() {
        string passOrFail = testPassed ? "Passed" : "Failed";
        string testFailedNotes = testPassed ? "." : ", Notes: " + failNotes;

        return "Test " + passOrFail + ".  [" + testName + "] - Test Value: " + testedValue.ToString() + ", Expected Value: " + expectedValue.ToString() + testFailedNotes;
    }
}
