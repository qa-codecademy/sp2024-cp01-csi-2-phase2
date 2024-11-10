let myChart = null;
let cryptoDataAPI = [];
const monthNames = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

// Function to fetch Cryptocurrencies
const fetchCryptocurrencies = () => {
    return fetch('/api/StatsAPI/cryptocurrencies')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(cryptos => {
            cryptoDataAPI = cryptos;  // Store the cryptocurrencies data

            const cryptoSelector = document.getElementById('cryptoSelector');
            cryptoSelector.innerHTML = '';  // Clear any existing options

            // Check if there are any cryptocurrencies
            if (cryptos.length === 0) {
                const option = document.createElement('option');
                option.textContent = 'No cryptocurrencies available';
                option.disabled = true;
                cryptoSelector.appendChild(option);
            } else {
                // Populate the dropdown with available cryptocurrencies
                cryptos.forEach(crypto => {
                    const option = document.createElement('option');
                    option.value = crypto.name;
                    option.textContent = crypto.name;
                    cryptoSelector.appendChild(option);
                });
            }
            return cryptos;  // Return the list of cryptocurrencies after populating
        })
        .catch(error => {
            console.error('Error fetching cryptocurrencies:', error);
        });
};

// Function to fetch chart data for the selected cryptocurrency
const fetchChartData = (cryptocurrencyName) => {
    return fetch(`/api/StatsAPI/chartdata/${cryptocurrencyName}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(chartData => {
            if (chartData && chartData.length > 0) {
                chartData.sort((a, b) => a.month - b.month);

                const labels = chartData.map(data => monthNames[data.month - 1]);
                const values = chartData.map(data => data.value);

                const ctx = document.getElementById('cryptoChart').getContext('2d');
                if (myChart) {
                    myChart.destroy(); // Destroy the previous chart if it exists
                }

                myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: labels,
                        datasets: [{
                            label: `${cryptocurrencyName} Value`,
                            data: values,
                            borderColor: 'rgba(75, 192, 192, 1)',
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            borderWidth: 1,
                            fill: true
                        }]
                    },
                    options: {
                        responsive: true,
                        scales: {
                            x: {
                                title: {
                                    display: true,
                                    text: 'Month'
                                },
                                ticks: {
                                    autoSkip: true,
                                    maxTicksLimit: 12
                                }
                            },
                            y: {
                                beginAtZero: true,
                                title: {
                                    display: true,
                                    text: 'Value'
                                }
                            }
                        }
                    }
                });
            } else {
                console.error("No data available for chart rendering.");
            }
        })
        .catch(error => {
            console.error('Error fetching chart data:', error);
        });
};

// Update the display with the selected cryptocurrency's data
function CryptoDataSet(selectedCrypto) {
    const selectedData = cryptoDataAPI.find(crypto => crypto.name === selectedCrypto);
    const listContainer = document.getElementById("cryptoList");
    listContainer.innerHTML = ''; // Clear existing content

    if (selectedData) {
        const listItem = document.createElement("li");
        listItem.classList.add("list-group-item");
        listItem.innerHTML = `
            <strong>Rank:</strong> ${selectedData.rank} <br>
            <strong>Name:</strong> ${selectedData.name} <br>
            <strong>Symbol:</strong> ${selectedData.symbol} <br>
            <strong>Price (USD):</strong> $${parseFloat(selectedData.price_usd).toFixed(2)} <br>
            <strong>Market Cap (USD):</strong> $${parseFloat(selectedData.market_cap_usd).toFixed(2)} <br>
            <strong>Change (1h):</strong> ${parseFloat(selectedData.percent_change_1h).toFixed(2)}% <br>
            <strong>Change (24h):</strong> ${parseFloat(selectedData.percent_change_24h).toFixed(2)}% <br>
            <strong>Change (7d):</strong> ${parseFloat(selectedData.percent_change_7d).toFixed(2)}% <br>
        `;
        listContainer.appendChild(listItem);
    } else {
        const listItem = document.createElement("li");
        listItem.classList.add("list-group-item");
        listItem.innerHTML = "Crypto data not found.";
        listContainer.appendChild(listItem);
    }
}

// Initialize the page when DOM is loaded
document.addEventListener('DOMContentLoaded', function () {
    // Step 1: Fetch cryptocurrencies
    fetchCryptocurrencies().then(cryptos => {
        // Step 2: After fetching cryptocurrencies, check for localStorage
        const selectedCrypto = localStorage.getItem("selectedCrypto");

        // Step 3: If a selected cryptocurrency is found in localStorage
        if (selectedCrypto) {
            const cryptoSelector = document.getElementById("cryptoSelector");
            const existingOption = Array.from(cryptoSelector.options).find(option => option.value === selectedCrypto);

            if (existingOption) {
                cryptoSelector.value = selectedCrypto; // Set the dropdown to the selected value

                // Step 4: Fetch and update the chart and data for the stored cryptocurrency
                fetchChartData(selectedCrypto);
                CryptoDataSet(selectedCrypto);
            }
        } else {
            // Step 5: If no selection in localStorage, select the first available cryptocurrency
            const firstCrypto = cryptos[0];
            if (firstCrypto) {
                const cryptoSelector = document.getElementById("cryptoSelector");
                cryptoSelector.value = firstCrypto.name; // Set the first cryptocurrency as the default

                // Step 6: Fetch and update the chart and data for the default cryptocurrency
                fetchChartData(firstCrypto.name);
                CryptoDataSet(firstCrypto.name);
            }
        }

        // Step 7: Handle dropdown change event
        document.getElementById('cryptoSelector').addEventListener('change', (event) => {
            const selectedCrypto = event.target.value;

            // Save the selected cryptocurrency to localStorage
            localStorage.setItem("selectedCrypto", selectedCrypto);

            // Fetch data and update the chart for the selected cryptocurrency
            fetchChartData(selectedCrypto);
            CryptoDataSet(selectedCrypto);
        });
    });
});
