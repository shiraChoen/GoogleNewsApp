// GET request to API
fetch("http://localhost:7034/News")
    .then(response => response.json())
    .then(data => {
        // Go over the data received from the API
        data.forEach(item => {
            document.getElementById("newsItems").innerHTML += `
                <div>
                    <h2>${item.title}</h2>
                    <p>${item.description}</p>
                    <a href="${item.link}">Read more</a>
                </div>
            `;
        });
    })
    .catch(error => console.error('Error fetching data:', error));
