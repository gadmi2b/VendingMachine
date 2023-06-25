using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using VendingMachine.BLL.DTO;
using VendingMachine.BLL.Infrastructure;
using VendingMachine.BLL.Interfaces;
using VendingMachine.BLL.Services;
using VendingMachine.DAL.Entities;
using VendingMachine.Presentation.Models;

namespace VendingMachine.Presentation.Controllers
{
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> _logger;
        IVendingMachineService _vendingMachineService;
        readonly IMapper _mapper;


        public HomeController(IVendingMachineService vendingMachineService,
                              IMapper mapper)
        {
            _vendingMachineService = vendingMachineService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            IndexViewModel indexViewModel = _mapper.Map<IndexViewModel>(_vendingMachineService.GetIndexDTO());
            return View(indexViewModel);
        }

        public IActionResult Maintain(string secretKey)
        {
            // TODO:
            // check secretKey

            MaintainViewModel maintainViewModel = _mapper.Map<MaintainViewModel>(_vendingMachineService.GetMaintainDTO());
            return View(maintainViewModel);
        }


        public JsonResult AddCoin(long id)
        {
            #region check id
            if (id <= 0)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = "Bad request."
                });
            }
            #endregion

            try
            {
                _vendingMachineService.AddCoinToBalance(id);
                return new JsonResult(new
                {
                    status = "success",
                    message = "",
                    balance = _vendingMachineService.GetUserBalance(),
                });
            }
            catch (VendingMachineException ex)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = ex.Message,
                });
            }
        }

        public JsonResult PurchaseDrink(long id)
        {
            #region check id
            if (id <= 0)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = "Bad request."
                });
            }
            #endregion

            try
            {
                _vendingMachineService.PurchaseDrink(id);
                return new JsonResult(new
                {
                    status = "success",
                    message = "Thank you! Enjoy your drink.",
                    balance = _vendingMachineService.GetUserBalance(),
                    quantity = _vendingMachineService.GetDrinkQuantity(id),
                });
            }
            catch (VendingMachineException ex)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = ex.Message,
                });
            }
        }

        public JsonResult Withdraw()
        {
            try
            {
                Dictionary<CoinDTO, int> coinsQuantities = _vendingMachineService.Withdraw();
                string withdrawedCoins = " Following coins were withdrawed:";
                foreach(CoinDTO coin in coinsQuantities.Keys)
                {
                    string str = coinsQuantities.GetValueOrDefault(coin) == 1 ? " coin" : " coins";
                    withdrawedCoins = withdrawedCoins + "<br>" +
                                      coinsQuantities.GetValueOrDefault(coin) + str + " of nominal " + coin.Nominal.ToString();
                }

                return new JsonResult(new
                {
                    status = "success",
                    message = "Thank you!" + withdrawedCoins,
                    balance = _vendingMachineService.GetUserBalance(),
                });
            }
            catch (VendingMachineException ex)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = ex.Message,
                });
            }
        }
    }
}

//Person? user = users.FirstOrDefault(u => u.Id == id);
//// если не найден, отправляем статусный код и сообщение об ошибке
//if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

//// если пользователь найден, отправляем его
//return Results.Json(user);