function escapeHtml(value) {
    if (value === null || value === undefined) return '';
    return String(value)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;');
}

function setStatus(containerId, type, message) {
    const container = document.getElementById(containerId);
    if (!container) return;

    if (!message) {
        container.className = 'inline-status';
        container.textContent = '';
        return;
    }

    container.className = `inline-status ${type}`;
    container.textContent = message;
}

function formatMoney(value) {
    const num = Number(value);
    if (!Number.isFinite(num)) return '0.00';
    return num.toFixed(2);
}
