use AdventureWorks;

declare @SalesOrderID int;

select
	@SalesOrderID = max(SalesOrderID)
from
	(
		select top 5
			SalesOrderHeader.SalesOrderID
		from
			Sales.SalesOrderHeader
			inner join 	Sales.SalesOrderDetail
				on	SalesOrderHeader.SalesOrderID = SalesOrderDetail.SalesOrderID

		group by
			SalesOrderHeader.SalesOrderID
		order by
			SalesOrderHeader.SalesOrderID
	) as Temp

select
	SalesOrderHeader.SalesOrderID,
	OrderDate,
	Store.Name as CompanyName,
	Person.FirstName + ' ' + Person.LastName as Name,
	Address.PostalCode,
	Address.AddressLine1 + ' ' + ISNULL(Address.AddressLine2, '') + ', ' + Address.City as Address,
	Product.Name as ProductName,
	CONVERT(float, UnitPrice) as UnitPrice,
	CONVERT(int, OrderQty) as OrderQuantity
from
	Sales.SalesOrderHeader
	inner join Sales.Customer
		on	SalesOrderHeader.CustomerID = Customer.CustomerID
	inner join Sales.Store
		on	Customer.StoreID = Store.BusinessEntityID
	inner join Person.Person
		on	Customer.PersonID = Person.BusinessEntityID
	inner join Person.Address
		on	SalesOrderHeader.BillToAddressID = Address.AddressID
	inner join 	Sales.SalesOrderDetail
		on	SalesOrderHeader.SalesOrderID = SalesOrderDetail.SalesOrderID
	inner join Production.Product
		on	SalesOrderDetail.ProductID = Product.ProductID
where
	SalesOrderHeader.SalesOrderID <= @SalesOrderID
order by
	SalesOrderHeader.SalesOrderID,
	SalesOrderDetail.SalesOrderDetailID