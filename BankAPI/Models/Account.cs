using System;
using System.Collections.Generic;

namespace BankAPI.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int? UserId { get; set; }

    public string? AccountNumber { get; set; }

    public string AccountType { get; set; } = null!;

    public decimal? Balance { get; set; }

    public virtual ICollection<Transaction> TransactionFromAccounts { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionToAccounts { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
