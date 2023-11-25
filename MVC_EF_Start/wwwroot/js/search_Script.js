document.addEventListener("DOMContentLoaded", function () {
  var form = document.getElementById("search-form");
  var button = document.getElementById("submit-button");
  var results = document.querySelector(".card-container");
  var apiKey = "hkZVQijE8ZvMpchuICzQYDI8gIyPjxVsGzt9h0oI"; // Replace with your API key
  var apiUrl = "https://developer.nrel.gov/api/alt-fuel-stations/v1/nearest.json?api_key=" + apiKey; // Base URL for the API

  button.addEventListener("click", function (event) {
    event.preventDefault(); // Prevent the default form submission behavior
      var location = document.getElementById("location").value;
      var fuelType = document.getElementById("fuel-type").value;

      if (location === "") {
          alert("Please enter a location or a Zip Code");
          return;
      }

      var requestUrl = apiUrl + "&location=" + location;

      if (fuelType !== "") {
          requestUrl += "&fuel_type=" + fuelType;
      }

      $.ajax({
          url: "/Home/Search", // Update the URL with your actual controller and method
          type: "GET", // or "POST" depending on your server-side method
          dataType: "json",
          data: { City: location, Fuel: fuelType },
          success: function (data) {
              if (data.length > 0) {
                  results.innerHTML = ""; // Clear previous results

                  data.forEach(function (station) {
                      
                      // Create and append station card, if needed
                      var stationCard = createStationCard(station);
                      results.appendChild(stationCard);
                  });

              } else {
                  results.innerHTML = "<p>No stations found for your search criteria.</p>";
              }
          },
          error: function (error) {
              console.error("Error", error);
              results.innerHTML = "<p>Sorry, something went wrong. Please try again later.</p>";
              // Handle errors
          }
      });
  });
    

  // Function to create a card for a station
  function createStationCard(station) {
    var stationCard = document.createElement("div");
      stationCard.className = "station card";
      var editUrl = '@Url.Action("Edit", "Home")';

      // Use the baseUrl variable to construct the URL
      // Use the editUrl variable to construct the URL
      stationCard.innerHTML = `
    <div class="card-body">
        <h5 class="card-title">${station.station_name}</h5>
        <p class="card-text">${station.street_address}, ${station.city}, ${station.state}, ${station.zip}</p>
        <p class="card-text">Phone: ${station.station_phone}</p>
        <p class="card-text">Fuel Type: ${station.fuel_type_code}</p>
        <a href="#"  class="btn btn-secondary edit-btn" data-station-id="${station.station_id}"> Edit </a>
        <a href="#" class="btn btn-secondary delete-btn" data-station-id="${station.station_id}">Delete</a>
    </div>
`;




    return stationCard;
  }

    $(document).on("click", ".delete-btn", function (event) {
        event.preventDefault();

        var stationId = $(this).data("station-id");

        $.ajax({
            url: "/Home/DeleteStation",
            type: "POST",
            data: { id: stationId },
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    alert("Station Deleted Successfully!")

                    window.location.href = '/Home/Index';

                } else {
                    // Handle failure
                   alert("Station deletion failed.");
                }
            },
            error: function (error) {
                alert("Station deletion failed.");
                // Handle errors
            }
        });
    });

    $(document).on("click", ".edit-btn", function (event) {
        event.preventDefault();

        var stationId = $(this).data("station-id");

        var url = '/Home/Edit?id=' + encodeURIComponent(stationId);

        // Redirect to the specified URL
        window.location.href = url;

       
    });
});
