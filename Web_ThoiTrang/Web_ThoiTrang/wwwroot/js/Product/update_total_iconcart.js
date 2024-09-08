function toggleMenu() {
    const menu = document.getElementById('menu');
    menu.classList.toggle('active');
}

// Update cart item count
document.addEventListener('DOMContentLoaded', function () {
    fetch('/Product/GetCartItemCount')
        .then(response => response.json())
        .then(data => {
            document.getElementById('cart-item-count').textContent = data.count;
        })
        .catch(error => console.error('Error fetching cart item count:', error));
});