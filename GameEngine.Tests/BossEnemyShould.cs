using Xunit;
using Xunit.Abstractions;

namespace GameEngine.Tests
{
    public class BossEnemyShould
    {
        private readonly ITestOutputHelper _output; // Needed to generate logging messages during test execution

        public BossEnemyShould(ITestOutputHelper output)    // Needed to generate logging messages during test execution
        {
            _output = output;
        }

        // Asserting on a floating point value. 
        [Fact]
        [Trait("Category", "Boss")]
        public void HaveCorrectPower()
        {
            _output.WriteLine("Creating Boss Enemy");   // Needed to generate logging messages during test execution
            BossEnemy sut = new BossEnemy();

            Assert.Equal(166.667, sut.TotalSpecialAttackPower, 3);
        }
    }
}
