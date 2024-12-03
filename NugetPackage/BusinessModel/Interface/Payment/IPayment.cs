namespace BusinessModel.Payment.Interface
{
    public interface IPayment
    {
        public int Id { get; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string Cvc { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }
        public string PayerEmail { get; set; }
        public string Description { get; set; }
        public string PaymentGateway { get; set; }
        public string InputToken { get; set; }
        public string OutToken { get; set; }
    }
}
