import { api, displayError } from './script.js';

document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('updateForm');
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');
    const longUrl = urlParams.get('longUrl');

    document.getElementById('urlId').value = id;
    document.getElementById('longUrl').textContent = decodeURIComponent(longUrl);

    form.addEventListener('submit', handleFormSubmit);
});

async function handleFormSubmit(event) {
    event.preventDefault();
    const id = document.getElementById('urlId').value;
    const newLongUrl = document.getElementById('newLongUrl').value;

    try {
        await updateUrl(id, newLongUrl);
        window.location.href = 'index.html';
    } catch (error) {
        displayError(error);
    }
}

async function updateUrl(id, longUrl) {
    const response = await fetch(`${api}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ longUrl })
    });

    if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.detail);
    }
}