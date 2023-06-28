function updateDrink(drinkId) {
  $.ajax({
    type: "Get",
    url: "/Home/UpdateDrink",
    data: {
      Id: drinkId,
      Name: $("#js_drinkName_" + drinkId).val(),
      Cost: $("#js_drinkCost_" + drinkId).val(),
      Quantity: $("#js_drinkQuantity_" + drinkId).val(),
    },
    dataType: "json",
    contentType: "application/json; charset=utf-8",

    success: function(response) {
      if (response.status == "success") {
        $("#js_information").html(response.message);
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


function changeIsJammedState(coinId) {
  $.ajax({
    type: "Get",
    url: "/Home/ChangeIsJammedState",
    data: {
      coinId: coinId,
    },
    dataType: "json",
    contentType: "application/json; charset=utf-8",

    success: function (response) {
      if (response.status == "success") {
        $("#js_information").html(response.message);
        if (response.isJammed) {
          $("#" + coinId).removeClass("btn-outline-success");
          $("#" + coinId).addClass("btn-warning");
        }
        else {
          $("#" + coinId).removeClass("btn-warning");
          $("#" + coinId).addClass("btn-outline-success");
        }
      }
      else {
        $("#js_information").html(response.message);
      }
    },
    failure: function (response) {
      console.log("Failure: " + response.status + " |  " + response.message);
    },
    error: function (response) {
      console.log("Error: " + response.status + " |  " + response.message);
    }
  });
}
