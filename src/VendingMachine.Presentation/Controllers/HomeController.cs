using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.BLL.DTO;
using VendingMachine.BLL.Infrastructure;
using VendingMachine.BLL.Interfaces;
using VendingMachine.Presentation.Models;

namespace VendingMachine.Presentation.Controllers
{
    public class HomeController : Controller
    {
        IVendingMachineService _vendingMachineService;
        readonly IMapper _mapper;
        const string _maintainKey = "admin";


        public HomeController(IVendingMachineService vendingMachineService,
                              IMapper mapper)
        {
            _vendingMachineService = vendingMachineService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel indexViewModel = _mapper.Map<IndexViewModel>(await _vendingMachineService.GetIndexDTOAsync());
            return View(indexViewModel);
        }

        public async Task<IActionResult> Maintain(string key)
        {
            if (string.IsNullOrEmpty(key) || key != _maintainKey)
            {
                return NotFound();
            }


            MaintainViewModel maintainViewModel = _mapper.Map<MaintainViewModel>(await _vendingMachineService.GetMaintainDTOAsync());

            string[] errors = (string[])TempData["errorMessages"]!;
            if (errors != null)
                maintainViewModel.ErrorMessages = errors.ToList();

            return View(maintainViewModel);
        }

        public async Task<JsonResult> AddCoin(long id)
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
                await _vendingMachineService.AddCoinToBalanceAsync(id);
                return new JsonResult(new
                {
                    status = "success",
                    message = "",
                    balance = await _vendingMachineService.GetUserBalanceAsync(),
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

        public async Task<JsonResult> PurchaseDrink(long id)
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
                await _vendingMachineService.PurchaseDrinkAsync(id);
                return new JsonResult(new
                {
                    status = "success",
                    message = "Thank you! Enjoy your drink.",
                    balance = await _vendingMachineService.GetUserBalanceAsync(),
                    quantity = await _vendingMachineService.GetDrinkQuantityAsync(id),
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

        public async Task<JsonResult> Withdraw()
        {
            try
            {
                Dictionary<CoinDTO, int> coinsQuantities = await _vendingMachineService.WithdrawAsync();
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
                    balance = await _vendingMachineService.GetUserBalanceAsync(),
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
    
        public async Task<JsonResult> UpdateDrink(DrinkDTO drink)
        {
            #region check id
            if (drink.Id <= 0)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = "Bad request. Drink was not updated."
                });
            }
            #endregion

            try
            {
                await _vendingMachineService.UpdateDrinkAsync(drink);

                return new JsonResult(new
                {
                    status = "success",
                    message = "Drink was successfully updated.",
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
        
        public async Task<JsonResult> ChangeIsJammedState(long coinId)
        {
            #region check id
            if (coinId <= 0)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = "Bad request. Coin state was not changed."
                });
            }
            #endregion

            try
            {
                bool isJammed = await _vendingMachineService.ChangeCoinIsJammedState(coinId);

                return new JsonResult(new
                {
                    status = "success",
                    message = "Coin state was successfully updated.",
                    isJammed = isJammed,
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
        
        public async Task<IActionResult> AddDrink(DrinkDTO drink)
        {
            try
            {
                await _vendingMachineService.AddDrinkAsync(drink);
            }
            catch (VendingMachineException ex)
            {
                TempData["errorMessages"] = new List<string> { ex.Message };
            }

            return RedirectToAction(nameof(Maintain), new { key = _maintainKey });
        }

        public async Task<IActionResult> DeleteDrinks(List<long> selectedDrinkIds)
        {
            List<string> errorMessages = new List<string>();
            try
            {
                foreach (long drinkId in selectedDrinkIds)
                {
                    string message = await _vendingMachineService.DeleteDrinkAsync(drinkId);
                    if (!string.IsNullOrEmpty(message))
                    {
                        errorMessages.Add(message);
                        message = string.Empty;
                    }
                    TempData["errorMessages"] = errorMessages;
                }
            }
            catch (VendingMachineException ex)
            {
                TempData["errorMessages"] = new List<string> { ex.Message };
            }

            return RedirectToAction(nameof(Maintain), new { key = _maintainKey });
        }

        public async Task<IActionResult> ExtractDrinks()
        {
            try
            {
                List<DrinkDTO> drinks =  await _vendingMachineService.GetDrinksAsync();

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "drinks.txt");

                string data = string.Empty;
                string delimiter = ";";
                foreach (DrinkDTO drink in drinks)
                {
                    data = data + $"{nameof(drink.Name)} = {drink.Name}{delimiter} " +
                                  $"{nameof(drink.Cost)} = {drink.Cost}{delimiter} " +
                                  $"{nameof(drink.Quantity)} = {drink.Quantity}\n";
                }
                byte[] bytes = Encoding.Default.GetBytes(data);
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                await fs.WriteAsync(bytes, 0, bytes.Length);
                fs.Position = 0;

                string userFilename = "drinks_extracted_" + DateTime.Today.ToString("dd/MM/yyyy") + ".txt";
                return File(fs, "text/plain", userFilename);

            }
            catch (VendingMachineException ex)
            {
                TempData["errorMessages"] = new List<string> { ex.Message };
                return RedirectToAction(nameof(Maintain), new { key = _maintainKey });
            }
        }
    }
}