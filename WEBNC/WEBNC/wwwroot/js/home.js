// Hàm render sản phẩm chung (giữ nguyên code gốc của bạn, chỉ đổi tên hàm để dễ gọi lại)
function renderSanPham(data) {
    if (!data || data.length === 0) {
        document.getElementById('ourSanPham').innerHTML = '<div class="col-12 text-center py-5"><p class="fs-3 text-muted">Không có sản phẩm nào.</p></div>';
        document.getElementById('newSanPham').innerHTML = '';
        document.getElementById('listStart').innerHTML = '';
        return;
    }

    let htmls = '';
    let listStart = '';

    data.forEach(product => {
        htmls += `
            <div class="col-md-6 col-lg-4 col-xl-3">
                <div class="product-item rounded wow fadeInUp" data-wow-delay="0.3s">
                    <div class="product-item-inner border rounded">
                        <div class="product-item-inner-item">
                            <img src="/images/${product.imageURL}" class="img-fluid w-100 rounded-top" alt="ảnh" style="width:auto;height:250px">
                            <div class="product-new">New</div>
                            <div class="product-details">
                                <a href="/Customer/Home/Details?id=${product.idSanPham}"><i class="fa fa-eye fa-1x"></i></a>
                            </div>
                        </div>
                        <div class="text-center rounded-bottom p-4">
                            <a href="#" class="d-block mb-2">${product.loaiSanPham.tenLoaiSanPham}</a>
                            <a href="#" class="d-block h4">${product.tenSanPham}</a>
                            <del class="me-2 fs-5">${product.gia} </del>
                            <span class="text-primary fs-5">${product.gia} VND</span>
                        </div>
                    </div>
                    <div class="product-item-add border border-top-0 rounded-bottom text-center p-4 pt-0">
                        <button class="btn btn-primary border-secondary rounded-pill py-2 px-4 mb-4 btnAddToCart" data-id="${product.idSanPham}">
                            <i class="fas fa-shopping-cart me-2"></i> Thêm vào giỏ hàng
                        </button>
                        <div class="d-flex justify-content-between align-items-center">
                            <div class="d-flex">
                                <i class="fas fa-star text-primary"></i>
                                <i class="fas fa-star text-primary"></i>
                                <i class="fas fa-star text-primary"></i>
                                <i class="fas fa-star text-primary"></i>
                                <i class="fas fa-star"></i>
                            </div>
                            <div class="d-flex">
                                <span class="text-primary d-flex align-items-center justify-content-center me-3">
                                    <span class="rounded-circle btn-sm-square border"><i class="fas fa-random"></i></span>
                                </span>
                                <span class="text-primary d-flex align-items-center justify-content-center me-0">
                                    <span class="rounded-circle btn-sm-square border"><i class="fas fa-heart"></i></span>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;

        listStart += `
            <div class="productImg-item products-mini-item border" style="background: #fff;">
                <div class="row g-0">
                    <div class="col-5">
                        <div class="products-mini-img border-end h-100">
                            <img src="/images/${product.imageURL}" class="img-fluid w-100 h-100" alt="Image" style="object-fit: contain;">
                            <div class="products-mini-icon rounded-circle bg-primary">
                                <a href="/Customer/Home/Details?id=${product.idSanPham}">
                                    <i class="fa fa-eye fa-1x text-white"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                    <div class="col-7">
                        <div class="products-mini-content p-3">
                            <a href="#" class="d-block mb-2 text-muted" style="font-size: 0.9rem;">${product.loaiSanPham.tenLoaiSanPham}</a>
                            <a href="/Customer/Home/Details?id=${product.idSanPham}" class="d-block h5 text-truncate">${product.tenSanPham}</a>
                            <span class="text-primary fw-bold">${product.gia.toLocaleString()} VND</span>
                        </div>
                    </div>
                </div>
                <div class="products-mini-add border p-3">
                    <a href="#" class="btn btn-primary border-secondary rounded-pill py-2 px-4 btnAddToCart" data-id="${product.idSanPham}">
                        <i class="fas fa-shopping-cart me-2"></i> Thêm vào giỏ
                    </a>
                </div>
            </div>
        `;
    });

    document.getElementById('ourSanPham').innerHTML = htmls;
    document.getElementById('newSanPham').innerHTML = htmls;

    const listStartElement = document.getElementById('listStart');
    if (listStartElement) {
        listStartElement.innerHTML = listStart;

        // Re-initialize Owl Carousel (giữ nguyên code gốc của bạn)
        var $carousel = $("#listStart");
        $carousel.trigger('destroy.owl.carousel');
        $carousel.removeClass("owl-loaded owl-drag owl-hidden");
        $carousel.find('.owl-stage-outer').children().unwrap();

        $carousel.owlCarousel({
            autoplay: true,
            smartSpeed: 1000,
            margin: 25,
            loop: true,
            center: false,
            dots: false,
            nav: true,
            navText: [
                '<i class="bi bi-arrow-left" style="font-size: 2rem;"></i>',
                '<i class="bi bi-arrow-right" style="font-size: 2rem;"></i>'
            ],
            items: 4,
            responsive: {
                0: { items: 1 },
                576: { items: 2 },
                768: { items: 3 },
                992: { items: 4 }
            }
        });
    }
}

// Load tất cả sản phẩm khi trang chủ load (giữ nguyên code gốc)
fetch('https://localhost:7047/api/customer/products')
    .then(response => response.json())
    .then(sanpham => renderSanPham(sanpham.data))
    .catch(error => console.error('Lỗi:', error));

// Xử lý click vào dropdown Danh mục (THÊM CHỨC NĂNG MỚI)
document.addEventListener("click", function (e) {
    const item = e.target.closest(".loai-item"); // class bạn dùng cho item loại
    if (!item) return;

    e.preventDefault();
    const tenLoai = item.dataset.loai || '';

    // Gọi API lọc theo loại
    const url = tenLoai
        ? `https://localhost:7047/api/customer/products?loai=${encodeURIComponent(tenLoai)}`
        : 'https://localhost:7047/api/customer/products';

    fetch(url)
        .then(res => res.json())
        .then(result => renderSanPham(result.data))
        .catch(err => console.error('Lỗi lọc sản phẩm:', err));

    // Ẩn/hiện slider
    const sliderWrapper = document.querySelector('.slider-wrapper');
    if (sliderWrapper) {
        sliderWrapper.style.display = tenLoai ? 'none' : 'block';
    }

    // Đổi tiêu đề thành tên loại vừa chọn
    const titleElement = document.querySelector('.tab-class h1');
    if (titleElement) {
        titleElement.textContent = tenLoai ? tenLoai : 'Sản phẩm';
    }

    // ẨN/HIỆN PHẦN "TẤT CẢ SẢN PHẨM" Ở DƯỚI (carousel + tiêu đề)
    const allProductsSection = document.querySelector('.container-fluid.products.productList.overflow-hidden');
    if (allProductsSection) {
        allProductsSection.style.display = tenLoai ? 'none' : 'block';
    }

    // Đóng dropdown
    const openDropdown = document.querySelector('.dropdown-menu.show');
    if (openDropdown) openDropdown.classList.remove('show');
});

// ====== ADD TO CART (giữ nguyên code gốc của bạn) ======
document.addEventListener("click", async (e) => {
    const btn = e.target.closest(".btnAddToCart");
    if (!btn) return;
    e.preventDefault();
    const productId = btn.dataset.id;
    const quantity = 1;
    if (!productId) {
        console.error("Không có productId trong nút AddToCart");
        return;
    }
    const res = await fetch("https://localhost:7047/api/cart/add", {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            idSanPham: productId,
            soLuongTrongGio: quantity
        })
    });
    if (res.status === 401) {
        const returnUrl = location.pathname + location.search;
        window.location.href =
            `/Identity/Account/Login?returnUrl=${encodeURIComponent(returnUrl)}`;
        return;
    }
    if (!res.ok) {
        Swal.fire({
            icon: "error",
            title: "Thêm giỏ hàng thất bại",
            text: "Vui lòng thử lại!",
        });
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