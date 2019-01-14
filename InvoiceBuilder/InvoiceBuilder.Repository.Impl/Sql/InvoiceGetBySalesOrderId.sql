SELECT
	SalesOrderId,
	OrderDate,
	CompanyName,
	FirstName,
	LastName,
	AddressLine1,
	AddressLine2,
	City,
	State,
	PostalCode
FROM
	Invoice.Invoice
WHERE
	SalesOrderId = @SalesOrderId