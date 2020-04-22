use AdventureWorks;

select
	SalesOrderID,
	OrderDate,
	Store.Name as CompanyName,
	Person.FirstName + ' ' + Person.LastName as Name,
	Address.PostalCode,
	Address.AddressLine1 + ' ' + ISNULL(Address.AddressLine2, '') + ', ' + Address.City as Address

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
where
	SalesOrderID = 71936
