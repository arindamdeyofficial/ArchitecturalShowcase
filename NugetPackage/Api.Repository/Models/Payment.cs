using System;
using System.Collections.Generic;

namespace Api.Repository.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string CardNumber { get; set; } = null!;

    public string ExpirationDate { get; set; } = null!;

    public string Cvc { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string PayerEmail { get; set; } = null!;

    public string? Description { get; set; }

    public string? PaymentGateway { get; set; }

    public string? InputToken { get; set; }

    public string? OutToken { get; set; }
}
