CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY, -- Auto-increment column
    CardNumber NVARCHAR(16) NOT NULL,
    ExpirationDate NVARCHAR(5) NOT NULL, -- Format: MM/YY
    Cvc NVARCHAR(4) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Currency NVARCHAR(3) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL,
    PayerEmail NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    PaymentGateway NVARCHAR(50) NULL,
    InputToken NVARCHAR(MAX) NULL,
    OutToken NVARCHAR(MAX) NULL
);
