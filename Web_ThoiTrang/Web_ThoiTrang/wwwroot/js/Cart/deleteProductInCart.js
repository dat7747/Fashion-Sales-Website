$(document).ready(function () {
    // Xử lý sự kiện nhấn nút xóa sản phẩm
    $('.remove-btn').click(function () {
        var button = $(this);
        var productId = button.data('id'); // Lấy ID của sản phẩm

        // Gửi yêu cầu AJAX để xóa sản phẩm khỏi giỏ hàng
        $.ajax({
            url: '/Cart/RemoveItem',
            type: 'POST',
            data: {
                sanPhamId: productId
            },
            success: function (response) {
                if (response.total !== undefined) {
                    // Xóa sản phẩm khỏi giao diện
                    $('#cart-item-' + productId).remove();

                    // Cập nhật tổng giá tiền
                    $('#total-price').text(formatCurrency(response.total));
                }
            },
        });
    });

    // Hàm định dạng giá tiền
    function formatCurrency(amount) {
        return '$' + parseFloat(amount).toFixed(2);
    }
});
