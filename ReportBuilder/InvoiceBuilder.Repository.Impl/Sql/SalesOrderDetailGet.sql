select
	SalesOrderDetailId,
	OrderQuantity,
	UnitPrice,
	ProductName
from
	Invoice.SalesOrderDetail
where
	SalesOrderId = @SalesOrderId
