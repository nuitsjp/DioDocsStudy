select
	SalesOrderId,
	OrderDate,
	StoreName,
	FirstName,
	LastName,
	AddressLine1,
	AddressLine2,
	City,
	State,
	PostalCode
from
	Invoice.SalesOrder
order by
	OrderDate desc