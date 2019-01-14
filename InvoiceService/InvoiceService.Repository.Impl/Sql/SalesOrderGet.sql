select
	SalesOrderId,
	OrderDate,
	CompanyName,
	FirstName,
	LastName,
	DetailCount,
	TotalPrice
from
	Invoice.SalesOrder
order by
	OrderDate desc, SalesOrderId desc