
using System;
using System.Collections.Generic;

// ===== Base Account =====
public class Account
{
    private string name;
    private double balance;

    public Account(string name = "Unnamed Account", double balance = 0.0)
    {
        this.name = name;
        this.balance = balance;
    }

    public virtual bool Deposit(double amount)
    {
        if (amount < 0) return false;
        balance += amount;
        return true;
    }

    public virtual bool Withdraw(double amount)
    {
        if (balance - amount < 0) return false;
        balance -= amount;
        return true;
    }

    public double GetBalance() => balance;

    public override string ToString() => $"[Account: {name}: {balance}]";
}

// ===== Savings Account =====
public class SavingsAccount : Account
{
    private double interestRate;

    public SavingsAccount(string name = "Unnamed Savings Account", double balance = 0.0, double interestRate = 0.0)
        : base(name, balance)
    {
        this.interestRate = interestRate;
    }

    public override bool Deposit(double amount)
    {
        if (!base.Deposit(amount)) return false;
        double interest = amount * (interestRate / 100.0);
        return base.Deposit(interest);
    }

    public override string ToString() => $"[SavingsAccount: {base.ToString()}, Interest Rate: {interestRate}%]";
}

// ===== Checking Account =====
public class CheckingAccount : Account
{
    private const double WithdrawFee = 1.50;

    public CheckingAccount(string name = "Unnamed Checking Account", double balance = 0.0)
        : base(name, balance)
    {
    }

    public override bool Withdraw(double amount) => base.Withdraw(amount + WithdrawFee);

    public override string ToString() => $"[CheckingAccount: {base.ToString()}, Fee: ${WithdrawFee}]";
}

// ===== Trust Account =====
public class TrustAccount : SavingsAccount
{
    private int withdrawalsCount = 0;
    private const int MaxWithdrawals = 3;
    private const double BonusAmount = 50.0;
    private const double MaxWithdrawPercent = 0.20; 

    public TrustAccount(string name = "Unnamed Trust Account", double balance = 0.0, double interestRate = 0.0)
        : base(name, balance, interestRate)
    {
    }

    // Like Savings deposit + $50 bonus on deposits >= 5000
    public override bool Deposit(double amount)
    {
        if (amount >= 5000.0) base.Deposit(BonusAmount);
        return base.Deposit(amount);
    }

    public override bool Withdraw(double amount)
    {
        if (withdrawalsCount >= MaxWithdrawals) return false;
        if (amount >= GetBalance() * MaxWithdrawPercent) return false; 

        if (!base.Withdraw(amount)) return false;

        withdrawalsCount++;
        return true;
    }

    public override string ToString() =>
        $"[TrustAccount: {base.ToString()}, Withdrawals: {withdrawalsCount}/{MaxWithdrawals}]";
}

// ===== Utilities (generic over any Account type) =====
public static class AccountUtil
{
    public static void Display<T>(List<T> accounts) where T : Account
    {
        Console.WriteLine("\n=== Accounts ==========================================");
        foreach (var acc in accounts) Console.WriteLine(acc);
    }

    public static void Deposit<T>(List<T> accounts, double amount) where T : Account
    {
        Console.WriteLine("\n=== Depositing to Accounts =================================");
        foreach (var acc in accounts)
        {
            if (acc.Deposit(amount))
                Console.WriteLine($"Deposited {amount} to {acc}");
            else
                Console.WriteLine($"Failed Deposit of {amount} to {acc}");
        }
    }

    public static void Withdraw<T>(List<T> accounts, double amount) where T : Account
    {
        Console.WriteLine("\n=== Withdrawing from Accounts ==============================");
        foreach (var acc in accounts)
        {
            if (acc.Withdraw(amount))
                Console.WriteLine($"Withdrew {amount} from {acc}");
            else
                Console.WriteLine($"Failed Withdrawal of {amount} from {acc}");
        }
    }
}

// ===== Program (Main) =====
class Program
{
    static void Main()
    {
        // Accounts
        var accounts = new List<Account>
        {
            new Account(),
            new Account("Larry"),
            new Account("Moe", 2000),
            new Account("Curly", 5000)
        };

        AccountUtil.Display(accounts);
        AccountUtil.Deposit(accounts, 1000);
        AccountUtil.Withdraw(accounts, 2000);

        // Savings
        var savAccounts = new List<SavingsAccount>
        {
            new SavingsAccount(),
            new SavingsAccount("Superman"),
            new SavingsAccount("Batman", 2000),
            new SavingsAccount("Wonderwoman", 5000, 5.0)
        };

        AccountUtil.Display(savAccounts);
        AccountUtil.Deposit(savAccounts, 1000);
        AccountUtil.Withdraw(savAccounts, 2000);

        // Checking
        var checAccounts = new List<CheckingAccount>
        {
            new CheckingAccount(),
            new CheckingAccount("Larry2"),
            new CheckingAccount("Moe2", 2000),
            new CheckingAccount("Curly2", 5000)
        };

        AccountUtil.Display(checAccounts);
        AccountUtil.Deposit(checAccounts, 1000);
        AccountUtil.Withdraw(checAccounts, 2000);
        AccountUtil.Withdraw(checAccounts, 2000);

        // Trust
        var trustAccounts = new List<TrustAccount>
        {
            new TrustAccount(),
            new TrustAccount("Superman2"),
            new TrustAccount("Batman2", 2000),
            new TrustAccount("Wonderwoman2", 5000, 5.0)
        };

        AccountUtil.Display(trustAccounts);
        AccountUtil.Deposit(trustAccounts, 1000);
        AccountUtil.Deposit(trustAccounts, 6000);
        AccountUtil.Withdraw(trustAccounts, 2000);
        AccountUtil.Withdraw(trustAccounts, 3000);
        AccountUtil.Withdraw(trustAccounts, 500);

        Console.WriteLine();
    }
}