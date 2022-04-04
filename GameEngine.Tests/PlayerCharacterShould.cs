using System;
using Xunit;

namespace GameEngine.Tests
{
    public class PlayerCharacterShould
    {
        // Asserting on boolean values
        [Fact]
        public void BeInexperiencedWhenNew()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.True(sut.IsNoob);
        }

        // Asserting on strings
        [Fact]
        public void CalculateFullName()
        {
            PlayerCharacter sut = new PlayerCharacter();

            sut.FirstName = "Sarah";
            sut.LastName = "Smith";

            Assert.Equal("Sarah Smith", sut.FullName);
        }

        [Fact]
        public void HaveFullNameStartingWithFirstName()
        {
            PlayerCharacter sut = new PlayerCharacter();

            sut.FirstName = "John";
            sut.LastName = "Jones";

            Assert.StartsWith("John", sut.FullName);
        }

        [Fact]
        public void HaveFullNameEndWithLastName()
        {
            PlayerCharacter sut = new PlayerCharacter();

            sut.LastName = "Marks";

            Assert.EndsWith("Marks", sut.FullName);
        }

        [Fact]
        public void CalculateFullName_IgnoreCaseAssertExample()
        {
            PlayerCharacter sut = new PlayerCharacter();

            sut.FirstName = "BOB";
            sut.LastName = "BALLARD";

            Assert.Equal("Bob Ballard", sut.FullName, ignoreCase: true);
        }

        [Fact]
        public void CalculateFullName_SubstringAssertExample()
        {
            PlayerCharacter sut = new PlayerCharacter();

            sut.FirstName = "Nathan";
            sut.LastName = "Rogers";

            Assert.Contains("an Ro", sut.FullName);
        }

        [Fact]
        public void CalculateFullNameWithTitleCase()
        {
            PlayerCharacter sut = new PlayerCharacter();

            sut.FirstName = "Henry";    // Test fails if the first character in each name is not capitalized.
            sut.LastName = "Brown";

            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", sut.FullName);
        }

        // Asserting on numeric values
        [Fact]
        public void StartWithDefaultHealth()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.Equal(100, sut.Health);
        }

        [Fact]
        public void StartWithDefaultHealth_NotEqualExample()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.NotEqual(0, sut.Health);
        }

        [Fact]
        public void IncreaseHealthAfterSleeping()
        /* The Sleep method calls another method to determine how much health to add. We can't use Assert.Equal
         * because we don't know exactly how much health is being added. */
        {
            PlayerCharacter sut = new PlayerCharacter();

            sut.Sleep();    // Expect increase between 1 to 100 inclusive

            // Assert.True(sut.Health >= 101 && sut.Health <= 200);    // There's a better way to do this. This can fail if the called method is changed.
            Assert.InRange<int>(sut.Health, 101, 200); // Expect health to be within the range 101 to 200 inclusive. Better error message if this fails.
        }

        // Asserting on Null values
        [Fact]
        public void NotHaveNickNameByDefault()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.Null(sut.Nickname);
            // Assert.NotNull(sut.Nickname);   //This test should fail. 
        }

        // Asserting on a collection
        [Fact]
        public void HaveALongBow()  // New characters are created with three weapons to start.
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.Contains("Long Bow", sut.Weapons);
        }

        [Fact]
        public void NotHaveAStaffOfWonder()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.DoesNotContain("Staff of Wonder", sut.Weapons);
        }

        [Fact]
        public void HasAtLeastOneKindOfSword()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.Contains(sut.Weapons, weapon => weapon.Contains("Sword"));   // Check to see if any of the items in the Weapons list contains the string "Sword"
        }

        [Fact]
        public void HaveAllExpectedWeapons()
        {
            PlayerCharacter sut = new PlayerCharacter();

            var expectedWeapons = new[]
            {
                "Long Bow",
                "Short Bow",
                "Short Sword"
            };

            Assert.Equal(expectedWeapons, sut.Weapons);
        }

        [Fact]
        public void HaveNoEmptyDefaultWeapons()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.All(sut.Weapons, weapon => Assert.False(string.IsNullOrWhiteSpace(weapon))); // Loops through all of the weapons and executes the Assert.False method on each item in collection
        }

        // Asserting that events are raised
        [Fact]
        public void RaiseSleptEvent()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.Raises<EventArgs>(   // This test passes if the Sleep method is called and the PlayerSlept event is raised
                handler => sut.PlayerSlept += handler,
                handler => sut.PlayerSlept -= handler,
                () => sut.Sleep());
        }

        [Fact]
        public void RaisePropertyChangedEvent() // If the class we're testing uses INotifyPropertyChanged you can use Assert.PropertyChanged
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.PropertyChanged(sut, "Health", () => sut.TakeDamage(10));
        }
    }
}
