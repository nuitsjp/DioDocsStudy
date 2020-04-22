use AdventureWorks;

select
	Product.Name as ProductName,
	UnitPrice,
	OrderQty as OrderQuantity
from
	Sales.SalesOrderDetail
	inner join Production.Product
		on	SalesOrderDetail.ProductID = Product.ProductID
where
	SalesOrderID = 71936
order by
	SalesOrderDetailID