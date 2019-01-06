drop view if exists Invoice.SalesOrderDetail;
drop view if exists Invoice.SalesOrder;
drop schema if exists Invoice;
go


create schema Invoice;
go


create view Invoice.SalesOrder
as
select
	SalesOrderHeader.SalesOrderID as SalesOrderId,
	SalesOrderHeader.OrderDate,
	Store.Name as StoreName,
	Person.FirstName,
	Person.LastName,
	AddressLine1,
	AddressLine2,
	City,
	StateProvince.Name as State,
	PostalCode
from
	Sales.SalesOrderHeader
	inner join Sales.Customer
		on SalesOrderHeader.CustomerID = Customer.CustomerID
	inner join Sales.Store
		on Sales.Customer.StoreID = Sales.Store.BusinessEntityID
	inner join Person.Person
		on Sales.Customer.PersonID = Person.Person.BusinessEntityID
	inner join Person.Address
		on SalesOrderHeader.BillToAddressID = Address.AddressID
	inner join Person.StateProvince
		on Address.StateProvinceID = StateProvince.StateProvinceID
go

create view Invoice.SalesOrderDetail
as
select
	SalesOrderDetail.SalesOrderID as SalesOrderId,
	SalesOrderDetail.SalesOrderDetailID as SalesOrderDetailId,
	SalesOrderDetail.OrderQty as OrderQuantity,
	SalesOrderDetail.UnitPrice as UnitPrice,
	Product.Name as ProductName
from
	Sales.SalesOrderDetail
	inner join Sales.SpecialOfferProduct
		on	SalesOrderDetail.SpecialOfferID =SpecialOfferProduct.SpecialOfferID
		and SalesOrderDetail.ProductID =SpecialOfferProduct.ProductID
	inner join Production.Product
		on	SpecialOfferProduct.ProductID = Product.ProductID	
go
