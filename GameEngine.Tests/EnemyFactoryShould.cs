using System;
using Xunit;

namespace GameEngine.Tests
{
    public class EnemyFactoryShould
    {
        // Asserting against object type
        [Fact]
        public void CreateNormalEnemyByDefault()
        {
            EnemyFactory sut = new EnemyFactory();

            Enemy enemy = sut.Create("Zombie");

            Assert.IsType<NormalEnemy>(enemy);
        }

        [Fact]
        public void CreateNormalEnemyByDefault_NotTypeExample()
        {
            EnemyFactory sut = new EnemyFactory();

            Enemy enemy = sut.Create("Zombie");

            Assert.IsNotType<DateTime>(enemy);
        }

        [Fact]
        public void CreateBossEnemy()
        {
            EnemyFactory sut = new EnemyFactory();

            Enemy enemy = sut.Create("Zombie King", true);

            Assert.IsType<BossEnemy>(enemy);
        }

        [Fact]
        public void CreateBossEnemy_CastReturnedTypeExample()
        {
            EnemyFactory sut = new EnemyFactory();

            Enemy enemy = sut.Create("Zombie King", true);

            // Assert and get cast result
            BossEnemy boss = Assert.IsType<BossEnemy>(enemy);   // If the type returned is not a BossEnemy this assert will fail

            // Additional asserts on typed object
            Assert.Equal("Zombie King", boss.Name); // This only happens if the previous assert is successful
        }

        [Fact]
        public void CreateBossEnemy_AssertAssignableTypes()
        {
            EnemyFactory sut = new EnemyFactory();

            Enemy enemy = sut.Create("Zombie King", true);

            // Assert.IsType<Enemy>(enemy);    // BossEnemy inherits from Enemy, but this will fail because IsType operates in a strict fashion
            Assert.IsAssignableFrom<Enemy>(enemy);  // IsAssignableFrom considers inheritance and this should pass
        }

        // Asserting on object instances
        [Fact]
        public void CreateSeparateInstances()  // Testing to ensure that two objects point to separate instances (not the same object)
        {
            EnemyFactory sut = new EnemyFactory();

            Enemy enemy1 = sut.Create("Zombie");
            Enemy enemy2 = sut.Create("Zombie");

            Assert.NotSame(enemy1, enemy2);
            // Assert.Same(enemy1, enemy2);    // This test should fail.
        }

        // Asserting on exceptions
        [Fact]
        public void NotAllowNullName()
        {
            EnemyFactory sut = new EnemyFactory();

            Assert.Throws<ArgumentNullException>(() => sut.Create(null));
            // Assert.Throws<ArgumentNullException>("isBoss", () => sut.Create(null)); // Fails because 'name' is expected to be null, not 'isBoss'
        }

        [Fact]
        public void OnlyAllowKingOrQueenBossEnemies()
        {
            EnemyFactory sut = new EnemyFactory();

            EnemyCreationException ex = Assert.Throws<EnemyCreationException>(() => sut.Create("Zombie", true));

            Assert.Equal("Zombie", ex.RequestedEnemyName);
        }
    }
}
