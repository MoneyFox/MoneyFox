namespace MoneyFox.Shared.Model {
    public struct NewPaymentParam {
        public PaymentType Type { get; set; }
    }

    public struct EditPaymentParam {
        public int Id { get; set; }
    }
}
