INSERT INTO Payments (CardNumber, ExpirationDate, Cvc, Amount, Currency, PaymentMethod, PayerEmail, Description, PaymentGateway, InputToken, OutToken)
VALUES
('4111111111111111', '12/25', '123', 100.50, 'USD', 'CreditCard', 'payer1@example.com', 'Payment for order #1234', 'Stripe', 'InputToken123', 'OutToken123'),
('5555555555554444', '01/26', '456', 200.75, 'EUR', 'DebitCard', 'payer2@example.com', 'Payment for subscription', 'PayPal', 'InputToken456', 'OutToken456'),
('378282246310005',  '11/24', '789', 150.00, 'USD', 'CreditCard', 'payer3@example.com', NULL, 'Square', NULL, NULL);
