declare @MaxBusinessEntitiID int;

select
	@MaxBusinessEntitiID = max(BusinessEntityID)
from
	AdventureWorks.Person.Person

select top 5
	BusinessEntityID,
	FirstName,
	LastName
from
	AdventureWorks.Person.Person