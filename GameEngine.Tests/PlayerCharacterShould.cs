using System;
using Xunit;
using Xunit.Abstractions;

namespace GameEngine.Tests
{
    public class PlayerCharacterShould : IDisposable    // Use IDisposable for cleanup code when using Xunit.
    {
        private readonly PlayerCharacter _sut;  // Moving setup code to the class constructor
        private readonly ITestOutputHelper _output;

        public PlayerCharacterShould(ITestOutputHelper output)  // Moving setup code to the class constructor
        {
            _output = output;   // Adding logging to the constructor, so each test will generate a log entry

            _output.WriteLine("Creating new PlayerCharacter");
            _sut = new PlayerCharacter();
        }

        public void Dispose()
        {
            _output.WriteLine($"Disposing PlayerCharacter {_sut.FullName}");

            // _sut.Dispose();
        }

        // Asserting on boolean values
        [Fact]
        public void BeInexperiencedWhenNew()
        {
            Assert.True(_sut.IsNoob);
        }

        // Asserting on strings
        [Fact]
        public void CalculateFullName()
        {
            _sut.FirstName = "Sarah";
            _sut.LastName = "Smith";

            Assert.Equal("Sarah Smith", _sut.FullName);
        }

        [Fact]
        public void HaveFullNameStartingWithFirstName()
        {
            _sut.FirstName = "John";
            _sut.LastName = "Jones";

            Assert.StartsWith("John", _sut.FullName);
        }

        [Fact]
        public void HaveFullNameEndWithLastName()
        {
            _sut.LastName = "Marks";

            Assert.EndsWith("Marks", _sut.FullName);
        }

        [Fact]
        public void CalculateFullName_IgnoreCaseAssertExample()
        {
            _sut.FirstName = "BOB";
            _sut.LastName = "BALLARD";

            Assert.Equal("Bob Ballard", _sut.FullName, ignoreCase: true);
        }

        [Fact]
        public void CalculateFullName_SubstringAssertExample()
        {
            _sut.FirstName = "Nathan";
            _sut.LastName = "Rogers";

            Assert.Contains("an Ro", _sut.FullName);
        }

        [Fact]
        public void CalculateFullNameWithTitleCase()
        {
            _sut.FirstName = "Henry";    // Test fails if the first character in each name is not capitalized.
            _sut.LastName = "Brown";

            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", _sut.FullName);
        }

        // Asserting on numeric values
        [Fact]
        public void StartWithDefaultHealth()
        {
            Assert.Equal(100, _sut.Health);
        }

        [Fact]
        public void StartWithDefaultHealth_NotEqualExample()
        {
            Assert.NotEqual(0, _sut.Health);
        }

        [Fact]
        public void IncreaseHealthAfterSleeping()
        /* The Sleep method calls another method to determine how much health to add. We can't use Assert.Equal
         * because we don't know exactly how much health is being added. */
        {
            _sut.Sleep();    // Expect increase between 1 to 100 inclusive

            // Assert.True(_sut.Health >= 101 && sut.Health <= 200);    // There's a better way to do this. This can fail if the called method is changed.
            Assert.InRange<int>(_sut.Health, 101, 200); // Expect health to be within the range 101 to 200 inclusive. Better error message if this fails.
        }

        // Asserting on Null values
        [Fact]
        public void NotHaveNickNameByDefault()
        {
            Assert.Null(_sut.Nickname);
            // Assert.NotNull(_sut.Nickname);   //This test should fail. 
        }

        // Asserting on a collection
        [Fact]
        public void HaveALongBow()  // New characters are created with three weapons to start.
        {
            Assert.Contains("Long Bow", _sut.Weapons);
        }

        [Fact]
        public void NotHaveAStaffOfWonder()
        {
            Assert.DoesNotContain("Staff of Wonder", _sut.Weapons);
        }

        [Fact]
        public void HasAtLeastOneKindOfSword()
        {
            Assert.Contains(_sut.Weapons, weapon => weapon.Contains("Sword"));   // Check to see if any of the items in the Weapons list contains the string "Sword"
        }

        [Fact]
        public void HaveAllExpectedWeapons()
        {
            var expectedWeapons = new[]
            {
                "Long Bow",
                "Short Bow",
                "Short Sword"
            };

            Assert.Equal(expectedWeapons, _sut.Weapons);
        }

        [Fact]
        public void HaveNoEmptyDefaultWeapons()
        {
            Assert.All(_sut.Weapons, weapon => Assert.False(string.IsNullOrWhiteSpace(weapon))); // Loops through all of the weapons and executes the Assert.False method on each item in collection
        }

        // Asserting that events are raised
        [Fact]
        public void RaiseSleptEvent()
        {
            Assert.Raises<EventArgs>(   // This test passes if the Sleep method is called and the PlayerSlept event is raised
                handler => _sut.PlayerSlept += handler,
                handler => _sut.PlayerSlept -= handler,
                () => _sut.Sleep());
        }

        [Fact]
        public void RaisePropertyChangedEvent() // If the class we're testing uses INotifyPropertyChanged you can use Assert.PropertyChanged
        {
            Assert.PropertyChanged(_sut, "Health", () => _sut.TakeDamage(10));
        }

        // Data driven tests
        //[Fact]
        //public void TakeZeroDamage()
        //{
        //    _sut.TakeDamage(0);

        //    Assert.Equal(100, _sut.Health);
        //}

        //[Fact]
        //public void TakeSmallDamage()
        //{
        //    _sut.TakeDamage(1);

        //    Assert.Equal(99, _sut.Health);
        //}

        //[Fact]
        //public void TakeMediumDamage()
        //{
        //    _sut.TakeDamage(50);

        //    Assert.Equal(50, _sut.Health);
        //}

        //[Fact]
        //public void HaveMinimum1Health()
        //{
        //    _sut.TakeDamage(101);

        //    Assert.Equal(1, _sut.Health);
        //}

        [Theory]
        //[InlineData(0, 100)]
        //[InlineData(1, 99)]
        //[InlineData(50, 50)]
        //[InlineData(101, 1)]
        //[MemberData(nameof(ExternalHealthDamageTestData.TestData), MemberType = typeof(ExternalHealthDamageTestData))]
        [HealthDamageData]
        public void TakeDamage(int damage, int expectedHealth)
        {
            _sut.TakeDamage(damage);

            Assert.Equal(expectedHealth, _sut.Health);
        }
    }
}
