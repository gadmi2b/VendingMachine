# VendingMachine
For comments related directly to task please see readme.txt

An application was created as a test assignment by one of IT companies in a limited time frame.
- IDE: Visual Studio 2022
- Target platform: .NET6
- Backend:
	- ASP.NET Core MVC, Entity Framework Core v6.0.18
	- Microsoft SQL Server 2019 Express
	- AutoMapper v12.0.1, AutoMapper.Extensions.Microsoft.DependencyInjection v12.0.1
- Frontend:
	- HTML/CSS/Javascript
	- JQuery v3.5.1, Bootstrap v5.1.0
- Tests:
	- xUnit v2.4.2
- N-Layer architecture. 

It consits from 2 page and simulates work process of a Vending machine with drinks.

The first page is UI for following interactions:
- User can add concrete coins to the machine and increase his balance;
- User can buy one of presented on the screen drinks by clicking on its card;
- User can withdraw the rest balance and receive concrete coins back.
- Each drink has a name, cost and available quantity.
- Each coin has a nominal and could be jammed - means that it can't be inseted in the machine.
  
  [There are business rules around this interactions]
  - Unable to insert coin that is jammed;
  - Unable to purchase a drink if balance less than cost of drink;
  - Unable to purchase a drink if all drinks are sold out;
  - User balance will not disappear if user stops using machine.

The second page is administrative page for maintain purposes:
- User can see all drinks, add a new one, remove or update existing;
- User can set a concrete coin in a jammed/unjammed state;
- It also provides possibility to extract all information about drinks as a txt file.

  [There are business rules around this interactions]
    - Unable to add a drink with a cost <= 0;
    - Unable to add a drink with a quantity < 0;
    - Unable to add a drink with a name length <=0 and >50 chars.
    - to access maintain page a password should be provided as a parameter in the adress bar (?key=admin).

A database is a simple .mdf file (SQL Server 2019 Express).
