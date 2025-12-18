import { getProductById, getAllProducts, getTopProducts }
    from "../api/product/product-details.api.js";

import { addToCart }
    from "../api/cart/cart-details.api.js";

import { renderDetails }
    from "../ui/product/renderDetails.js";

import { initQuantity, getQuantity }
    from "../ui/product/quantity.handler.js";

import { handleBuyNow }
    from "../services/dondat/muangay.service.js";

const urlParams = new URLSearchParams(window.location.search);
let id = urlParams.get('id');

if (id) {

    getProductById(id).then(sanpham => {

        const data = sanpham.data;
        renderDetails(data);
        initQuantity();

        const btnAddToCart = document.querySelector('.btn-cart');
        if (btnAddToCart) {
            btnAddToCart.addEventListener("click", async () => {

                const res = await addToCart(id, getQuantity());

                if (res.status === 401) {
                    const returnUrl =
                        `/Customer/Home/Details?id=${encodeURIComponent(id)}`;
                    window.location.href =
                        `/Identity/Account/Login?returnUrl=${encodeURIComponent(returnUrl)}`;
                    return;
                }

                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Đã thêm vào giỏ hàng",
                    showConfirmButton: false,
                    timer: 1500
                });
            });
        }

        const btnBuyNow = document.querySelector('.btn-buy');
        if (btnBuyNow) {
            btnBuyNow.addEventListener("click", () => {
                handleBuyNow(id, getQuantity());
            });
        }
    });

    // sản phẩm liên quan
    getAllProducts().then(sanpham => {
        const data = sanpham.data;
        let sanPhamLQ = '';
        const randomSanPham =
            data.sort(() => 0.5 - Math.random()).slice(0, 3);

        randomSanPham.forEach(product => {
            sanPhamLQ += `
               <div class="product-card product-item-inner-item position-relative">
                        <div class="product-details">
                            <a href="/Customer/Home/Details?id=${product.idSanPham}" class="view-icon">
                                <img src="/images/${product.imageURL}" class="img-fluid w-100 rounded-top" alt="sản phẩm liên quan" style="height:180px">
                            </a>
                        </div>
                        <div class="product-card-info">
                            <h4 style="height:45px;overflow:hidden;-webkit-line-clamp:2;-webkit-box-orient:vertical;">${product.tenSanPham}</h4>
                            <p class="price">${Number(product.gia).toLocaleString('vi-VN')} ₫</p>
                            <button class="add-to-cart-small">Thêm vào giỏ</button>
                        </div>
                    </div>
                    `;
        });

        document.getElementById('sanPhamLQ').innerHTML = sanPhamLQ;
    });

    // top sản phẩm
    getTopProducts().then(result => {
        let html = '';
        result.data.forEach(sp => {
            const giaGiam = Number(sp.gia) * 0.86;
            html += `
                 <div class="featured-item">
                        <a href="/Customer/Home/Details?id=${sp.idSanPham}" class="view-icon">
                            <img src="/images/${sp.imageURL}" alt="${sp.tenSanPham}">
                        </a>
                        <div class="featured-info">
                            <h4>${sp.tenSanPham}</h4>
                            <p class="price">
                                ${giaGiam.toLocaleString('vi-VN')}₫ 
                                <span class="old-price">${Number(sp.gia).toLocaleString('vi-VN')}₫</span>
                            </p>
                            <p class="rating">★★★★☆</p>
                        </div>
                    </div>
            `;
        });
        document.getElementById('sanPhamNoiBat').innerHTML = html;
    });

} else {
    document.getElementById('sanPham').innerHTML =
        'Không tìm thấy ID sản phẩm.';
}
