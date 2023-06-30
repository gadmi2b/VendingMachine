Важно:
тестовое задание весьма объемно, при этом не понятен deadline - дата, после которой исчезнет интерес заказчика к работе,
поэтому, с одной стороны, задачи по заданию выполнены максимально полностью, в том числе продумана бизнес-логика,
а с другой, хотелось бы больше проработать UI, тесты, в том числе интеграционные, логирование и т.д. По этой же причине
в приложении практически не используется Model binding и Model validation - все проверки осуществляются на стороне сервера.
----------------------------------------------------------------------------------------------------------------------------
IDE: Visual Studio 2022
Target platform: .NET6
Backend:
	ASP.NET Core MVC, Entity Framework Core v6.0.18
	Microsoft SQL Server 2019 Express
	AutoMapper v12.0.1, AutoMapper.Extensions.Microsoft.DependencyInjection v12.0.1
Frontend:
	HTML/CSS/Javascript (Ajax requests)
	JQuery v3.5.1, Bootstrap v5.1.0
Tests:
	xUnit v2.4.2
----------------------------------------------------------------------------------------------------------------------------
Архитектура приложения построена по методологии N-Layer/Tear Architecture:
	Data Access Layer (DAL) - нижний слой (Class Library), отвечает за запросы к базе данных
	посредством реализации IUserRepository, ICoinRepository, IDrinkRepository
	
	Business Logic Layer (BLL) - средний слой (Class Library), отвечает за бизнес логику и связь
	между верхним и нижним слоями приложения, посредством реализации интерфейса IVendingMachineService

	Presentation Layer - верхний слой (WEB приложение), отвечает за представления данных пользователю.
	Релизован с помощью MVC патерна.

	Presentation Layer (program.cs) имеет связи с DAL сугубо для реализации Dependency Injection (DI).
----------------------------------------------------------------------------------------------------------------------------
Внедрение зависимостей (DI):
	Реализация в файле Program.cs с помощью стандартного иструментария ASP.NET Core (у AutoMapper свой)
	IUserRepository, ICoinRepository, IDrinkRepository, IMapper используются в конструкторе VendingMachineService
	IVendingMachineService, IMapper используется в конструкторе HomeController

База данных:
	Файл: VendingMachine.mdf
	Расположение: src/VendingMachine.Presentation/bin/Debug/net6.0/App_Data
	Строка подключения (смотри appsettings.json):
		"Data Source =.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\App_Data\\VendingMachine.mdf;Integrated Security=true"
	Структура: три таблицы (Users, Coins, Drinks) без отношений между собой. Code First/Database First approaches не применялись.
----------------------------------------------------------------------------------------------------------------------------
Выполненные необязательные требования:
	При возврате сдачи показывается количество и номинал монет. Сдача выдается монетами начиная с наибольшего номинала и вниз.
	Пока аппарат использует монеты номинала 1, сдача всегда будет выдаваться полностью. В противном случае пользователь будет предупрежден.

	На странице Maintain (готовая ссылка, принимающая параметр-ключ, в левом верхнем углу стартовой страницы) вверху есть кнопка Extract drinks.
	Позволяет скачать файл .txt с данными обо всех напитках в простейшем виде.
	!ВАЖНО:	по-хорошему такого рода export надо реализовывать следующим образом (это не сложно, но требует времени):
		вместе с запросом пользователь предоставляет свой Email adress (если его нет);
		далее на сервере в асинхронном виде формируются данные и сохраняются там же в виде файла в отдельную папку со спец. именем;
		по готовности файла пользователю высывается письмо со ссылкой на данный файл (через SMTP или свой почтовый сервис);
		на сервере крутится скрипт, который проверяет даты создания таких файлов и удаляет те, которые лежат дольше N дней/часов.
----------------------------------------------------------------------------------------------------------------------------
Описание назначений файлов:
src/VendingMachine.Presentation/Controllers/HomeController.cs - принимает запросы клиента соответствующими Actions в асинхронной манере;
src/VendingMachine.Presentation/Mapper/AppMappingProfile.cs - настройка маппинга между объектами;
src/VendingMachine.Presentation/Models/IndexViewModel.cs - модель, содержащая данные для Index view
src/VendingMachine.Presentation/Models/MaintainViewModel.cs - модель, содержащая данные для Maintain view
src/VendingMachine.Presentation/Views/Home/Index.cshtml - стартовая страница приложения с интерфейсом Vending Machine
src/VendingMachine.Presentation/Views/Home/Maintain.cshtml - страница администрирования Vending Machine
src/VendingMachine.Presentation/bin/Debug/net6.0/App_Data/VendingMachine.mdf - файл базы данных

src/VendingMachine.BLL/DTO/.. - набор классов DTO для обмена данными между слоями
src/VendingMachine.BLL/Handlers/DrinkChecker.cs - отвечает за проверку (Business Logic) объекта Drink
src/VendingMachine.BLL/Handlers/WithdrawCalculator.cs - отвечает за все расчеты, связанные с Withdraw операцией (Business Logic)
src/VendingMachine.BLL/Identity/UserIdentity.cs - отвечает за информацию о текущем пользователе приложения (Id, имя, права и т.д.)
src/VendingMachine.BLL/Infrastructure/VendingMachineException.cs - custom exception, которое выбрасывается на BLL, когда нарушаются Business Rules
src/VendingMachine.BLL/Interfaces/IVendingMachineService.cs - интерфейс сервиса VendingMachine
src/VendingMachine.BLL/Services/VendingMachineService.cs - сервис, реализующий IVendingMachineService и предоставляющий функциональную связь между
							Presentation Layer и Data Access Layer через слой бизнес правил
src/VendingMachine.DAL/EF/DataContext.cs - контекст базы данных с коллекциями объектов, сопоставленными с ее таблицами
src/VendingMachine.DAL/Entities/.. - набор классов сущностей в объектах которых будут храниться данные из базы данных
src/VendingMachine.DAL/Interfaces/.. - набор интерфейсов репозиториев для взамодействия с базой данных
src/VendingMachine.DAL/Repositories/.. - набор классов репозиториев, реализующие соответствующие интерфейсы и предоставляющие
					основной функционал взаимодействия с базой данных.

tests/VendingMachine.Tests - Unit тесты приложения.
