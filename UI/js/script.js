const api = 'https://localhost:7132';

document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('shortenForm');
    form.addEventListener('submit', handleFormSubmit);
    loadUrls();
});

async function handleFormSubmit(event) {
    event.preventDefault();
    const longUrl = document.getElementById('longUrl').value;
    await shortenUrl(longUrl);
    loadUrls();
}

async function shortenUrl(longUrl) {
    try {
        const response = await fetch(`${api}/shorten`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ longUrl })
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.detail);
        }

        const result = await response.json();
        alert(`Сокращенный URL: ${result.shortUrl}`);
    } catch (error) {
        displayError(error);
    }
}

async function loadUrls() {
    const urls = await fetch(`${api}/list`);
    renderTable(await urls.json());
}

function renderTable(urls) {
    const table = document.getElementById('urlTable').getElementsByTagName('tbody')[0];
    table.innerHTML = '';

    urls.forEach(url => {
        const row = table.insertRow();

        row.insertCell().textContent = url.id;
        addLinkCell(row, url.longUrl);
        addLinkCell(row, url.shortUrl);
        row.insertCell().textContent = new Date(url.createdAt).toLocaleString();
        row.insertCell().textContent = url.clickCount;

        const actionsCell = row.insertCell();
        const deleteButton = createDeleteButton(url.id);
        actionsCell.appendChild(deleteButton);

        const updateButton = createUpdateButton(url.id, url.longUrl);
        actionsCell.appendChild(updateButton);
    });
}

function addLinkCell(row, href) {
    const cell = row.insertCell();
    const link = document.createElement('a');
    link.href = href;
    link.textContent = href;
    cell.appendChild(link);
}

function createDeleteButton(id) {
    const button = document.createElement('button');
    button.textContent = 'Delete';
    button.addEventListener('click', async () => {
        await deleteUrl(id);
        loadUrls();
    });
    return button;
}

function createUpdateButton(id, longUrl) {
    const button = document.createElement('button');
    button.textContent = 'Update';
    button.addEventListener('click', () => {
        window.location.href = `update.html?id=${id}&longUrl=${encodeURIComponent(longUrl)}`;
    });
    return button;
}

async function deleteUrl(id) {
    await fetch(`${api}/${id}`, {
        method: 'DELETE'
    });
}

function displayError(error) {
    alert(`Error: ${error.message}`);
}
