# Theory
There are two ways of changing code
## 1. Edit and Pray
Typical game dev “testing” scenario:
- You write some code
- You run the game
- You play around to see what’s up
### Problems with your typical testing
- This takes too long - you run through this whole sequence just to test a single function. 
- You can never be sure you’ve tested sufficiently
- Your feature may be untestable without adding more features
- What’s the point of running the whole game to test just one method? That you don’t know better. 
## 2. Cover and Modify
### The dream of unit testing
Imagine a world where your code is fixed and you could instantly see all consequences of your actions

Games are too big to comprehend as a whole, so you’re either blind or write unit tests

Your test is a vise that clamps the code so you can know exactly what’s going on
### Unit Testing Definition:
> a piece of code that runs separately from the rest of your project that validates if a specific method functions as expected
###### Unit test example - Does taking away 10 HP actually do so?
```csharp
        [Test]
        public void TakeDamage_Remove10HP_HealthIs90()
        {
                    player.TakeDamage(10);
                    Assert.AreEqual(90, player.health);
        }
```
### Naming your Unit Tests
The name of your test should consist of three parts:
- The name of the method being tested.
- The scenario under which it's being tested.
- The expected behavior when the scenario is invoked.
# Practice
## 1. Set up the Unity Test Framework
1. Go to the Package Manager and make sure the Unity Test Framework is installed and up to date
2. Open the Test Runner
	- Window -> General -> Test Runner
3. Make a test assembly folder called “Editor” 
4. Create a new Assembly definition called "Scripts" for your gameplay behavior scripts
5. Add a reference to the new assembly in the “EditModeTests” assembly settings
## Making an Edit Mode test
### Set up a behavior to test
1. Add a Cube to the scene to represent a character we may have 
2. Write the Cube behavior class
```csharp
public class Cube : MonoBehaviour
{
    public int health;

    void Update()
    {
            DisableOnDeath();
    }

    public void DisableOnDeath()
    {
            if (health < 0)
            {
                    gameObject.SetActive(false);
            }
    }
}
```
As it is, we can't test the DisableOnDeath() method without writing additional functionality like attacking, UI, or whatever could reduce health. 
### Making our first Unit Test covering the death condition
1. Go to the Test Runner and make a new Edit Mode test called “CubeTests”
2. Open it and delete the default functions. 
3. Write the basic test method:
```csharp
        [Test]
        public void DisableOnDeath_EmptyHP_ObjectSetInactive()
        {
                GameObject testObject = new GameObject();
                Cube cubeScript = testObject.AddComponent<Cube>();

                cubeScript.health = 0;
                cubeScript.DisableOnDeath();

                Assert.IsFalse(testObject.activeSelf);
        }
```
>The Assert class comes from the NUnit framework and includes a lot of intuitive methods for your tests. It's what actually does the checking and determines if your test passes or fails. 

4. Run the test from the Test Runner in Unity
5. See it fails
6. Check the Test Runner for why
7. Realize that we’ve forgotten to include 0 in the if statement
8. Wow, tests are awesome
9. Fix the code and run the test again
10. Great success
### Making our second Unit Test covering the alive condition
```csharp
        [Test]
        public void DisableOnDeath_HasHP_ObjectRemainsActive()
        {
                GameObject testObject = new GameObject();
                Cube cubeScript = testObject.AddComponent<Cube>();

                cubeScript.health = 20;
                cubeScript.DisableOnDeath();

                Assert.IsTrue(testObject.activeSelf);
        }
```
Run the test and see that it works. 

We now have two tests which share mostly the exact same code. To address this, we can select the repetitive code and use Extract Method (Ctrl+R, M)

### TestCase
Our two tests curretly only cover two specific scenarios - 0 and 20. However, there are other cases which need to be tested to ensure that the function behaves properly. 

Instead of copy-pasting the same code, we can use the TestCase attribute provided by NUnit. It lets us feed the test certain values directly. Ultimately, our class with cube tests should look like this: 
```csharp
public class CubeEditTests
{
        [Test]
        [TestCase(0)]
        [TestCase(-29)]
        public void DisableOnDeath_EmptyHP_ObjectSetInactive(float hp)
        {
                GameObject testObject = MakeFakeCube(hp);

                Assert.IsFalse(testObject.activeSelf);
        }

        [Test]
        [TestCase(1)]
        [TestCase(0.6f)]
        [TestCase(100)]
        [TestCase(999999)]
        public void DisableOnDeath_HasHP_ObjectRemainsActive(float hp)
        {
                GameObject testObject = MakeFakeCube(hp);

                Assert.IsTrue(testObject.activeSelf);
        }

        private static GameObject MakeFakeCube(float hp)
        {
                GameObject testObject = new GameObject();
                Cube cubeScript = testObject.AddComponent<Cube>();

                cubeScript.health = hp;
                cubeScript.DisableOnDeath();
                return testObject;
        }
}
```
Before we can go and run our new tests, we can see that we have an error on the line:
```csharp
                cubeScript.health = hp;
```
While we are writing the test it already shows an error we made. The "health" variable in the Cube class is an int, when it should be a float. Go back to that class and fix that. 

Now we can go back to the Unity editor, run the tests, and see that everything passes. 

> Wow, tests are awesome

# Closing words
- There’s much more functionality to the Unity Test Framework you can explore
- But it’s really easy to start with simple single-unit tests
- Writing tests even helps when you’re stuck on writing algorithms
- If you don’t write unit tests you’re writing bad code, by definition
- You’ll end up with an unmanageable mess eventually if you don’t write tests
