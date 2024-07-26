document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchInput");
    const dataTable = document.getElementById("dataTable");
    const tbody = dataTable.querySelector("tbody");
    const originalRows = Array.from(tbody.querySelectorAll("tr"));

    function filterContacts() {
        const query = searchInput.value.toLowerCase();
        tbody.innerHTML = "";

        const filteredRows = originalRows.filter(row => {
            const cells = row.querySelectorAll("td");
            return Array.from(cells).some(cell =>
                cell.textContent.toLowerCase().includes(query)
            );
        });

        filteredRows.forEach(row => tbody.appendChild(row));
    }

    searchInput.addEventListener("input", filterContacts);
});