using System;

// Аргументи події
class MoneyEventArgs : EventArgs
{
    public decimal Amount { get; }
    public decimal Balance { get; }

    public MoneyEventArgs(decimal amount, decimal balance)
    {
        Amount = amount;
        Balance = balance;
    }
}

// Клас банківського рахунку
class BankAccount
{
    public string AccountNumber { get; }
    public decimal Balance { get; private set; }

    // Події
    public event EventHandler<MoneyEventArgs> MoneyDeposited;
    public event EventHandler<MoneyEventArgs> MoneyWithdrawn;

    public BankAccount(string accountNumber, decimal initialBalance = 0)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    // Внесення коштів
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сума внесення повинна бути більшою за 0.");
            return;
        }

        Balance += amount;

        // Викликаємо подію
        MoneyDeposited?.Invoke(
            this,
            new MoneyEventArgs(amount, Balance)
        );
    }

    // Зняття коштів
    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сума зняття повинна бути більшою за 0.");
            return;
        }

        if (amount > Balance)
        {
            Console.WriteLine("Недостатньо коштів на рахунку.");
            return;
        }

        Balance -= amount;

        // Викликаємо подію
        MoneyWithdrawn?.Invoke(
            this,
            new MoneyEventArgs(amount, Balance)
        );
    }
}

// Клас клієнта банку
class BankClient
{
    public string Name { get; }

    public BankClient(string name)
    {
        Name = name;
    }

    // Підписка на події рахунку
    public void Subscribe(BankAccount account)
    {
        account.MoneyDeposited += OnMoneyDeposited;
        account.MoneyWithdrawn += OnMoneyWithdrawn;
    }

    // Обробник внесення коштів
    private void OnMoneyDeposited(
        object sender,
        MoneyEventArgs e)
    {
        Console.WriteLine(
            $"{Name}: внесено {e.Amount} грн. " +
            $"Поточний баланс: {e.Balance} грн."
        );
    }

    // Обробник зняття коштів
    private void OnMoneyWithdrawn(
        object sender,
        MoneyEventArgs e)
    {
        Console.WriteLine(
            $"{Name}: знято {e.Amount} грн. " +
            $"Поточний баланс: {e.Balance} грн."
        );
    }
}

// Головна програма
class Program
{
    static void Main()
    {
        // Створюємо банківський рахунок
        BankAccount account = new BankAccount("UA123456", 1000);

        // Створюємо клієнта
        BankClient client = new BankClient("Іван");

        // Підписуємо клієнта на події рахунку
        client.Subscribe(account);

        Console.WriteLine(
            $"Рахунок: {account.AccountNumber}"
        );
        Console.WriteLine(
            $"Початковий баланс: {account.Balance} грн.\n"
        );

        // Операції з рахунком
        account.Deposit(500);
        account.Withdraw(200);
        account.Withdraw(300);
        account.Deposit(1000);

        Console.WriteLine(
            $"\nКінцевий баланс: {account.Balance} грн."
        );
    }
}
