using Xunit;

namespace GameEngine.Tests
{
    public class BossEnemyShould
    {
        // Asserting on a floating point value. 
        [Fact]
        public void HaveCorrectPower()
        {
            BossEnemy sut = new BossEnemy();

            Assert.Equal(166.667, sut.TotalSpecialAttackPower, 3);
        }
    }
}
