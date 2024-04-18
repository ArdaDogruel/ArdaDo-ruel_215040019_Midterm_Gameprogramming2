using System;
using System.Threading.Tasks;


//In this game, the knight and the goblin attempt to defeat each other through combat in every round.
//The first one whose health reaches zero loses.
//The game features mechanics such as attacking, blocking, dodging, and counterattacking.


namespace ArdaDoğruel_215040019_Midterm_Gameprogramming2
{
    // Base character class
    class Character
    {
        protected static Random rnd = new Random();

        public string Name { get; set; }
        public int Health { get; set; }

        // Constructor for initializing character name and health
        public Character(string name, int health)
        {
            Name = name;
            Health = health;
        }

        // Method for character attacking another character
        public virtual void Attack(Character target)
        {
            int damage = CalculateDamage();
            Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage!");
            if (!target.Dodge())
                target.Health -= damage;
        }

        // Method for character dodging an attack
        public bool Dodge()
        {
            // 20% chance to dodge an attack
            if (rnd.Next(1, 101) <= 20)
            {
                Console.WriteLine($"{Name} dodges the attack!");
                return true;
            }
            return false;
        }

        // Method for character speaking during combat
        public void Speak()
        {
            if (rnd.Next(1, 101) <= 30)
            {
                string[] dialogues = { "I'll crush you!", "Prepare to meet your death!", "You can't defeat me!", "Feel the power!!!"};
                int index = rnd.Next(dialogues.Length);
                Console.WriteLine($"{Name}: {dialogues[index]}");
            }
        }

        // Method for calculating damage during an attack
        protected int CalculateDamage()
        {
            int damage = 10;
            if (rnd.Next(1, 101) <= 20)
            {
                Console.WriteLine("Critical Hit!BOOM!!!");
                damage = 20;
            }
            return damage;
        }
    }

    // Subclass: Enemy character
    class Goblin : Character
    {
        public Goblin(string name, int health) : base(name, health)
        {
        }

        // Overriding the Attack method for goblin
        public override void Attack(Character target)
        {
            int damage = CalculateDamage();
            Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage!");
            if (!target.Dodge())
                target.Health -= damage;
        }
    }

    // Subclass: Player character
    class Knight : Character
    {
        public Knight(string name, int health) : base(name, health)
        {
        }

        // Method for blocking an enemy's attack
        public void Block(Character target)
        {
            Console.WriteLine($"{Name} blocks {target.Name}'s attack!");
            if (rnd.Next(1, 101) <= 20)
            {
                Console.WriteLine($"{Name} performs a counter attack!");
                target.Health -= 20;
                Console.WriteLine($"{Name} deals 20 damage to {target.Name}!");
            }
        }

        // Overriding the Attack method for knight
        public override void Attack(Character target)
        {
            int damage = CalculateDamage();
            Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage!");
            if (!target.Dodge())
                target.Health -= damage;
        }
    }

    // Game world class
    class GameWorld
    {
        public Knight Knight { get; set; }
        public Goblin Goblin { get; set; }

        // Constructor for initializing game world with knight and goblin
        public GameWorld(Knight knight, Goblin goblin)
        {
            Knight = knight;
            Goblin = goblin;
        }

        // Method to start the game
        public async Task StartGameAsync()
        {
            Console.WriteLine($"{Knight.Name} encounters {Goblin.Name}!");

            while (Knight.Health > 0 && Goblin.Health > 0)
            {
                Knight.Speak();
                Goblin.Speak();

                Console.WriteLine($"\n{Knight.Name}'s Health: {Knight.Health}");
                Console.WriteLine($"{Goblin.Name}'s Health: {Goblin.Health}");

                int choice = 0; // Define the choice variable and assign a default value
                bool validInput = false;
                while (!validInput)
                {
                    Console.WriteLine("\nWhat will you do?");
                    Console.WriteLine("1. Attack");
                    Console.WriteLine("2. Block");

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        if (choice == 1 || choice == 2)
                        {
                            validInput = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice! Please enter 1 for Attack or 2 for Block.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input! Please enter a number.");
                    }
                }

                switch (choice)
                {
                    case 1:
                        Knight.Attack(Goblin);
                        if (Goblin.Health <= 0)
                        {
                            Console.WriteLine($"{Goblin.Name} is defeated!");
                            break;
                        }
                        break;
                    case 2:
                        Knight.Block(Goblin);
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                if (choice != 2)
                {
                    Goblin.Attack(Knight);
                    if (Knight.Health <= 0)
                    {
                        Console.WriteLine($"{Knight.Name} is defeated!");
                        break;
                    }
                }

                await Task.Delay(1000); // Wait between turns
            }

            // Declare the winner at the end of the game
            if (Knight.Health <= 0)
            {
                Console.WriteLine($"Winner {Goblin.Name}!");
            }
            else if (Goblin.Health <= 0)
            {
                Console.WriteLine($"Winner {Knight.Name}!");
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // Create instances of Knight and Goblin characters
            Knight knight = new Knight("Knight", 100);
            Goblin goblin = new Goblin("Goblin", 100);

            // Create the game world and start the game
            GameWorld gameWorld = new GameWorld(knight, goblin);
            await gameWorld.StartGameAsync();
        }
    }
}

