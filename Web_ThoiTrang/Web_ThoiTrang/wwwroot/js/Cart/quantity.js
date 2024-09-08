$(document).ready(function () {
    // Hàm định dạng tiền tệ (Ở đây sử dụng VND, bạn có thể thay đổi nếu sử dụng loại tiền khác)
    function formatCurrency(amount) {
        return amount.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
    }

    // Sự kiện khi nhấn nút tăng/giảm số lượng sản phẩm
    $('.quantity-btn').click(function () {
        var button = $(this);
        var action = button.data('action'); // Kiểm tra hành động (tăng/giảm)
        var productId = button.data('id'); // Lấy ID sản phẩm
        var quantityInput = $('#quantity-' + productId); // Lấy ô input số lượng
        var currentQuantity = parseInt(quantityInput.val()); // Lấy giá trị hiện tại của số lượng

        // Cập nhật số lượng dựa trên hành động (+ hoặc -)
        if (action === 'increase') {
            currentQuantity++;
        } else if (action === 'decrease' && currentQuantity > 1) {
            currentQuantity--;
        }

        // Gửi yêu cầu AJAX để cập nhật số lượng sản phẩm
        $.ajax({
            url: '/Cart/UpdateQuantity',
            type: 'POST',
            data: {
                sanPhamId: productId,
                newQuantity: currentQuantity
            },
            success: function (response) {
                // Cập nhật số lượng và tổng giá trị
                quantityInput.val(currentQuantity);
                $('#total-price').text(formatCurrency(response.total));  // Định dạng lại tổng giá trị
            },
            error: function () {
                alert('Error updating quantity');
            }
        });
    });

    // Sự kiện khi nhấn nút xóa sản phẩm khỏi giỏ hàng
    $('.remove-btn').click(function () {
        var button = $(this);
        var productId = button.data('id'); // Lấy ID sản phẩm

        // Gửi yêu cầu AJAX để xóa sản phẩm khỏi giỏ hàng
        $.ajax({
            url: '/Cart/RemoveItem',
            type: 'POST',
            data: {
                sanPhamId: productId
            },
            success: function (response) {
                // Xóa sản phẩm khỏi giao diện người dùng
                $('#cart-item-' + productId).remove();
                $('#total-price').text(formatCurrency(response.total)); // Định dạng lại tổng giá trị
            },
            error: function () {
                alert('Error removing item');
            }
        });
    });
});
