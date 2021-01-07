using System;
using System.Threading;

namespace ThreadDeadlockControl
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Main Thread Started");
            Account A = new Account(10000, 101);
            Account B = new Account(5000, 102);

            AccountManager managerFromAtoB = new AccountManager(A, B, 3000);
            Thread firstThread = new Thread(managerFromAtoB.Transfer);
            firstThread.Name = "T1";

            AccountManager managerFromBtoA = new AccountManager(B, A, 4000);
            Thread secondThread = new Thread(managerFromBtoA.Transfer);
            secondThread.Name = "T2";

            firstThread.Start(); secondThread.Start();
            firstThread.Join(); secondThread.Join();

            Console.WriteLine($"Balance A:{A.Balance}");
            Console.WriteLine($"Balance B:{B.Balance}");
            Console.WriteLine("Main Thread Ended");

        }
    }

    class Account
    {
        public double Balance { get; set; }
        public long ID { get; set; }

        public Account(double balance, int id)
        {
            this.Balance = balance;
            this.ID = id;
        }

        public long GetID => ID;

        public void WithDraw(double amount)
        {
            Balance -= amount;
        }

        public void Deposit(double amount)
        {
            Balance += amount;
        }
    }

    class AccountManager
    {
        private Account _fromAccount;
        private Account _toAccount;
        private double amountToTransfer;
        public AccountManager(Account fromAccount, Account toAccount, double amountToTransfer)
        {
            this._fromAccount = fromAccount;
            this._toAccount = toAccount;
            this.amountToTransfer = amountToTransfer;
        }

        public void Transfer()
        {
            object lock1, lock2;
            if (_fromAccount.ID > _toAccount.ID)
            {
                lock1 = _fromAccount;
                lock2 = _toAccount;
            }
            else
            {
                lock1 = _toAccount;
                lock2 = _fromAccount;
            }

            lock (lock1)
            {
                Console.WriteLine($"In _fromAccount lock, for :{Thread.CurrentThread.Name}");
                Thread.Sleep(1000);

                lock (lock2)
                {
                    Console.WriteLine($"Thread:{Thread.CurrentThread.Name}!");
                    _fromAccount.WithDraw(amountToTransfer);
                    _toAccount.Deposit(amountToTransfer);

                }

            }

        }

    }

}
