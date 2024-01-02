
document.addEventListener("DOMContentLoaded", function () {
    var button = document.getElementById("start-button");
    button.addEventListener("click", function () {
        // Redirect to the search page
        window.location.href = "/Search.html";
    });
});
const stateAbbreviationsToNames = {
    AL: "Alabama",
    AK: "Alaska",
    AZ: "Arizona",
    AR: "Arkansas",
    CA: "California",
    CO: "Colorado",
    CT: "Connecticut",
    DE: "Delaware",
    FL: "Florida",
    GA: "Georgia",
    HI: "Hawaii",
    ID: "Idaho",
    IL: "Illinois",
    IN: "Indiana",
    IA: "Iowa",
    KS: "Kansas",
    KY: "Kentucky",
    LA: "Louisiana",
    ME: "Maine",
    MD: "Maryland",
    MA: "Massachusetts",
    MI: "Michigan",
    MN: "Minnesota",
    MS: "Mississippi",
    MO: "Missouri",
    MT: "Montana",
    NE: "Nebraska",
    NV: "Nevada",
    NH: "New Hampshire",
    NJ: "New Jersey",
    NM: "New Mexico",
    NY: "New York",
    NC: "North Carolina",
    ND: "North Dakota",
    OH: "Ohio",
    OK: "Oklahoma",
    OR: "Oregon",
    PA: "Pennsylvania",
    RI: "Rhode Island",
    SC: "South Carolina",
    SD: "South Dakota",
    TN: "Tennessee",
    TX: "Texas",
    UT: "Utah",
    VT: "Vermont",
    VA: "Virginia",
    WA: "Washington",
    WV: "West Virginia",
    WI: "Wisconsin",
    WY: "Wyoming",
};

let chart; 
// Retrieve data from the HTML element
const dataElement = document.getElementById("stations-data");
const data1 = JSON.parse(dataElement.dataset.stations);
var data_details = null;

$.ajax({
    url: "/Home/Station_Details", // Update the URL with your actual controller and method
    type: "GET", // or "POST" depending on your server-side method
    dataType: "json",
    success: function (data) {
        //const data_details = JSON.parse(data.stations.stations);
        data_details = data.stations.stations;

        createFilteredStateStationsChart(data_details);

    },
    error: function (error) {
        console.error("Error", error);
        results.innerHTML = "<p>Sorry, something went wrong. Please try again later.</p>";
        // Handle errors
    }
});

function createFilteredStateStationsChart(stations) {
    const stateCounts = {};

    stations.forEach(station => {
        const state = station.state;
        const fullStateName = stateAbbreviationsToNames[state];
        if (fullStateName) {
            if (stateCounts[fullStateName]) {
                stateCounts[fullStateName]++;
            } else {
                stateCounts[fullStateName] = 1;
            }
        }
    });

    const filteredStateLabels = Object.keys(stateCounts);
    const filteredStateData = filteredStateLabels.map(state => stateCounts[state]);
    const colors = generateColors(filteredStateLabels.length);
    const borderColors = generateBorderColors(filteredStateLabels.length);

    const ctx = document.getElementById("stateStationsChart").getContext("2d");
    chart = new Chart(ctx, {
        type: "bar",
        data: {
            labels: filteredStateLabels,
            datasets: [
                {
                    label: "Number of Stations",
                    data: filteredStateData,
                    backgroundColor: colors,
                    borderColor: borderColors,
                    borderWidth: 1,
                },
            ],
        },
        options: {
            responsive: true,
            maintainAspectRatio: true,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        color: "black",
                    },
                    grid: {
                        display: false,
                    },
                },
                x: {
                    ticks: {
                        color: "black",
                    },
                    grid: {
                        display: false,
                    },
                },
            },
            plugins: {
                legend: {
                    labels: {
                        color: "black",
                        font: {
                            weight: "bold",
                        },
                    },
                },
            },
        },
        plugins: [
            {
                afterDraw: function (chart) {
                    if (chart.data.datasets.length > 0) {
                        document.getElementById("chart-loading").style.display = "none";
                    }
                },
            },
        ],
    });

    const fuelTypeSelect = document.getElementById("fuelTypeSelect");
    fuelTypeSelect.addEventListener("change", () => {
        const selectedFuelType = fuelTypeSelect.value;
        updateChartByFuelType(selectedFuelType);
    });

    window.addEventListener("resize", () => {
        if (chart) {
            chart.resize();
        }
    });
}

function updateChartByFuelType(selectedFuelType) { 
if (chart) {
    chart.destroy();
}
    if (selectedFuelType === "all") {
        createFilteredStateStationsChart(data_details);
    } else {
        const filteredData = data_details.filter((station) => station.fuel_type_code === selectedFuelType);
        createFilteredStateStationsChart(filteredData);
    }
}

function getRandomColor() {
    const r = Math.floor(Math.random() * 256);
    const g = Math.floor(Math.random() * 256);
    const b = Math.floor(Math.random() * 256);
    return `rgba(${r}, ${g}, ${b}, 0.2)`;
}

function getRandomBorderColor() {
    const r = Math.floor(Math.random() * 256);
    const g = Math.floor(Math.random() * 256);
    const b = Math.floor(Math.random() * 256);
    return `rgba(${r}, ${g}, ${b}, 1)`;
}

function generateColors(size) {
    return Array.from({ length: size }, getRandomColor);
}

function generateBorderColors(size) {
    return Array.from({ length: size }, getRandomBorderColor);
}


function deleteStation(stationId) {

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
}

