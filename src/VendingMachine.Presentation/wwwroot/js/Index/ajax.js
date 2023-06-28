function addCoin(coinId) {
  $.ajax({
    type: "Get",
    url: "/Home/AddCoin",
    data: { id: coinId },
    dataType: "json",
    contentType: "application/json; charset=utf-8",

    success: function(response) {
      if (response.status == "success") {
        $("#js_balance").html(response.balance);
      }
      else {
        $("#js_information").html(response.message);
      }
    },
    failure: function(response) {
      console.log("Failure: " + response.status + " |  " + response.message);
    },
    error: function(response) {
      console.log("Error: " + response.status + " |  " + response.message);
    }
  });
}

function purchaseDrink(drinkId) {
  $.ajax({
    type: "Get",
    url: "/Home/PurchaseDrink",
    data: { id: drinkId },
    dataType: "json",
    contentType: "application/json; charset=utf-8",

    success: function(response) {
      if (response.status == "success") {
        $("#js_information").html(response.message);
        $("#js_balance").html(response.balance);
        $("#js_quantityDrink_" + drinkId).html(response.quantity);
      }
      else {
        $("#js_information").html(response.message);
      }
    },
    failure: function(response) {
      console.log("Failure: " + response.status + " |  " + response.message);
    },
    error: function(response) {
      console.log("Error: " + response.status + " |  " + response.message);
    }
  });
}

function withdraw() {
  $.ajax({
    type: "Get",
    url: "/Home/Withdraw",

    success: function(response) {
      if (response.status == "success") {
        $("#js_information").html(response.message);
        $("#js_balance").html(response.balance);
      }
      else {
        $("#js_information").html(response.message);
        $("#js_balance").html(response.balance);
      }
    },
    failure: function(response) {
      console.log("Failure: " + response.status + " |  " + response.message);
    },
    error: function(response) {
      console.log("Error: " + response.status + " |  " + response.message);
    }
  });
}