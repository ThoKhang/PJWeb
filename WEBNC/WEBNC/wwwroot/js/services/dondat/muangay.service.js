import { addToCart } from "../../api/cart/cart-details.api.js";

export async function handleBuyNow(id, quantity = 1) {
    try {
        const res = await addToCart(id, quantity);

        if (res.status === 401) {
            // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
            const returnUrl = window.location.href;
            window.location.href = `/Identity/Account/Login?returnUrl=${encodeURIComponent(returnUrl)}`;
            return;
        }

        if (res.ok) {
            // Thêm vào giỏ thành công, chuyển hướng đến trang chọn phương thức thanh toán
            window.location.href = "/Customer/Checkout/PaymentMethod";
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Không thể thêm sản phẩm vào giỏ hàng.'
            });
        }
    } catch (error) {
        console.error("Lỗi mua ngay:", error);
        Swal.fire({
            icon: 'error',
            title: 'Lỗi',
            text: 'Đã xảy ra lỗi khi xử lý yêu cầu.'
        });
    }
}
